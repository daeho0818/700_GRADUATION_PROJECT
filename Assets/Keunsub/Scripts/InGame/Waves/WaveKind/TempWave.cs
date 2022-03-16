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

    }

    void SecondWave()
    {

    }
}
