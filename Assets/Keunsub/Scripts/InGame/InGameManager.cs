using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameManager : MonoBehaviour
{
    public Transform Pivot;
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
                nowWave.WaveStart();
            }
        }


    }
}
