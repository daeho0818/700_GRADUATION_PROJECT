using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterSize
{
    SMALL,
    MIDIUM,
    BIG
}

public class InGameManager : Singleton<InGameManager>
{
    [SerializeField] List<WaveBase> Waves = new List<WaveBase>();
    public List<Action> WaveFunc = new List<Action>();
    public bool waveEnd;
    public int waveIdx;

    void Start()
    {
        waveIdx = 0;
        InitWave();
    }

    void Update()
    {

    }

    public void InitWave()
    {
        Waves[waveIdx].InitWaves(this);
        WaveFunc = Waves[waveIdx].waves;
    }

    public void SummonMonster(MonsterSize size, Vector3 position, Entity entity)
    {

    }

    public void SummonScaffol(Vector3 position)
    {

    }

    public void SummonGroundTrap()
    {

    }

    IEnumerator CombatCoroutine()
    {
        foreach (var item in WaveFunc)
        {
            item();

            while (!waveEnd)
            {
                yield return null;
            }
        }

        //game finished
    }
}