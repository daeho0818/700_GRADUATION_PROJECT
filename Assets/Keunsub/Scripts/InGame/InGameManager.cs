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

    public bool upgradeTrigger;
    public List<UpgradeClass> Upgrades = new List<UpgradeClass>();
    public UpgradeUI upgradeUI;

    void Start()
    {
        int waveIdx = 0; //get from game manager
        nowWave = GetComponents<WaveBase>()[waveIdx];

    }

    void Update()
    {
        if (!isGameActive)
        {
            Collider2D col = Physics2D.OverlapCircle(Pivot.position, 5f, LayerMask.GetMask("Entity"));
            if (col != null && col.CompareTag("Player"))
            {
                isGameActive = true;
                nowWave.WaveStart(this);
            }
        }


    }

    public IEnumerator UpgradePause()
    {
        upgradeTrigger = false;
        GameManager.Instance.player.Exp -= GameManager.Instance.player.MaxExp;

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

        int[] rand = new int[3] { Random.Range(0, Upgrades.Count), Random.Range(0, Upgrades.Count), Random.Range(0, Upgrades.Count) };
        upgradeUI.InitButtons(Upgrades[rand[0]], Upgrades[rand[1]], Upgrades[rand[2]]);
    }

    public void UpgradeEnd()
    {
        upgradeTrigger = true;
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

        Upgrades.ForEach(item => item.Init(GameManager.Instance.player));
    }

    public void GameEnd()
    {
        // -3.5,  -11

        Door.DOMoveY(-11f, 5f).SetEase(Ease.Linear);
    }
}
