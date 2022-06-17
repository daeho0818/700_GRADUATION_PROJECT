using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class InGameManager : MonoBehaviour
{
    public Transform Pivot;
    public Transform Door;
    public bool isGameActive;
    WaveBase nowWave;

    bool upgradeTrigger;

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

        Upgrade();

        while (!upgradeTrigger) yield return null;


    }

    public void Upgrade()
    {
        // make three random choose
        // show it at ui
        // each button will upgrade player's state 

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

    }

    public void GameEnd()
    {
        // -3.5,  -11

        Door.DOMoveY(-11f, 5f).SetEase(Ease.Linear);
    }
}
