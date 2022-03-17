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
        for (int i = 0; i < cnt; i++)
        {
            SummonScaffold(new Vector3(i - 10, 0, 0));
        }

        yield return new WaitForSeconds(4f);

        DeactiveAllScafold(0.5f);

        yield return new WaitForSeconds(3f);
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
        NextWave();
    }
}
