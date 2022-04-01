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
        int cnt = 15;
        GameObject testMonster = new GameObject("monster (clone)");
        Entity entity = testMonster.AddComponent<Monster>();

        Entity flying = SummonMonster(MonsterSize.MIDIUM, Vector3.zero, entity);
        for (int i = 0; i < cnt; i++)
        {
            SummonScaffold(new Vector3(i - 10, 0, 0));
        }

        while (!flying.IsDestroy)
            yield return null;

        yield return new WaitForSeconds(4f);

        DeactiveAllScafold(3f);

        yield return new WaitForSeconds(5f);
        NextWave();
    }

    void SecondWave()
    {
        StartCoroutine(SecondWaveCoroutine());
    }

    IEnumerator SecondWaveCoroutine()
    {
        yield return null;
        int count = 4;
        for (int i = 0; i < count; i++)
        {
            SummonScaffold(new Vector3(i, i - 2, 0));
            yield return new WaitForSeconds(0.2f);
        }

        yield return new WaitForSeconds(4f);
        DeactiveAllScafold(3f);
        yield return new WaitForSeconds(5f);
        
        NextWave();
    }
}
