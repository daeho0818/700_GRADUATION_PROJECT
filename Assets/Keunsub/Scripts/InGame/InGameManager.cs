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

    void Start()
    {
        int waveIdx = 0; //get from game manager
        nowWave = GetComponents<WaveBase>()[waveIdx];
    }

    void Update()
    {
        if (!isGameActive)
        {
            if(Physics2D.OverlapCircle(Pivot.position, 5f).CompareTag("Player"))
            {
                isGameActive = true;
                nowWave.WaveStart(this);
            }
        }


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
