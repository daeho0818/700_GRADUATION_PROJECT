using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class InGameManager : Singleton<InGameManager>
{
    public Transform Pivot;
    public Transform Door;
    public bool isGameActive;
    public WaveBase nowWave;
    WaveBase[] Waves;

    public bool upgradeTrigger;
    public List<UpgradeClass> Upgrades = new List<UpgradeClass>();
    public UpgradeUI upgradeUI;

    void Start()
    {
        Waves = GetComponents<WaveBase>();
    }

    void Update()
    {
        if (!isGameActive)
        {
            Collider2D col = Physics2D.OverlapCircle(Pivot.position, 5f, LayerMask.GetMask("Entity"));
            if (col != null && col.CompareTag("Player"))
            {
                isGameActive = true;
                StartCoroutine(WaveCoroutine());
            }
        }
    }

    IEnumerator WaveCoroutine()
    {
        for (int i = 0; i < Waves.Length; i++)
        {
            nowWave = Waves[i];
            nowWave = Waves[1]; //remove when after debug
            yield return StartCoroutine(nowWave.WaveStart(this));
        }

        // wait

        GameManager.Instance.player.StateInit();
        GameEnd();
    }

    public IEnumerator UpgradePause()
    {
        upgradeTrigger = false;
        GameManager.Instance.player.Exp = 0f;
        GameManager.Instance.player.level++;

        Upgrade();

        while (!upgradeTrigger) yield return null;

        upgradeUI.RemoveButtons();

    }

    public void Upgrade()
    {
        // make three random choose
        // show it at ui
        // each button will upgrade player's state 

        upgradeUI.gameObject.SetActive(true);

        List<int> idxList = new List<int>();
        for (int i = 0; i < Upgrades.Count; i++)
        {
            if (Upgrades[i].level < Upgrades[i].maxLevel)
                idxList.Add(i);
        }

        int[] rand = new int[3];

        for (int i = 0; i < rand.Length; i++)
        {
            rand[i] = idxList[Random.Range(0, idxList.Count)];
            idxList.Remove(rand[i]);
        }

        upgradeUI.InitButtons(Upgrades[rand[0]], Upgrades[rand[1]], Upgrades[rand[2]]);
    }

    public void UpgradeEnd()
    {
        upgradeTrigger = true;
        upgradeUI.gameObject.SetActive(false);
    }

    public void GameInit()
    {
        isGameActive = false;
        var enemies = FindObjectsOfType<Enemy>();
        var platforms = FindObjectsOfType<Platform>();

        for (int i = 0; i < enemies.Length; i++)
            Destroy(enemies[i].gameObject);

        for (int i = 0; i < platforms.Length; i++)
            Destroy(platforms[i].gameObject);

        Door.position = new Vector2(-14f, -3.5f);

        Upgrades.Clear();

        Upgrades.Add(new UpgradeATKSpeed());
        Upgrades.Add(new UpgradeMaxHp());
        Upgrades.Add(new UpgradeATKDMG());
        Upgrades.Add(new UpgradeCritical());
        Upgrades.Add(new UpgradeDashDelay());
        Upgrades.Add(new UpgradeDashCool());
        Upgrades.Add(new UpgradeEXP());
        Upgrades.Add(new UpgradeMP());
        Upgrades.Add(new UpgradeHP());
        Upgrades.Add(new UpgradeMoveSpeed());

        Upgrades.ForEach(item => item.Init(GameManager.Instance.player));
    }

    public void GameEnd()
    {
        // -3.5,  -11

        Door.DOMoveY(-11f, 5f).SetEase(Ease.Linear);
    }
}
