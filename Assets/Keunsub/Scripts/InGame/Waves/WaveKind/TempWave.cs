using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempWave : WaveBase
{

    public override void InitWaves(InGameManager _manager)
    {
        manager = _manager;
        InitWavesFunc(FirstWave, SecondWave);
    }

    void FirstWave()
    {
        StartCoroutine(FirstWaveCoroutine());
    }

    IEnumerator FirstWaveCoroutine()
    {
        List<Entity> curEnemy = new List<Entity>();
        int nextWaveMonsterCount = 5;
        yield return null;

        int monsterCount = 15;
        for (int i = 0; i < monsterCount; i++)
        {
            Entity temp = SummonMonster(MonsterSize.SMALL, Vector3.zero, null);
            curEnemy.Add(temp);
            yield return new WaitForSeconds(0.2f);
        }

        while (curEnemy.Count > nextWaveMonsterCount) yield return null;
        NextWave();
    }

    void SecondWave()
    {

    }
}
