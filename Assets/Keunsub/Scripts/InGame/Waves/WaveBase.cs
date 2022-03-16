using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WaveBase : MonoBehaviour
{
    public List<Action> waves = new List<Action>();
    protected InGameManager manager;

    protected void InitWavesFunc(params Action[] _waves)
    {
        foreach (var wave in _waves)
        {
            waves.Add(wave);
        }
    }

    protected void SummonMonster(MonsterSize size, Vector3 position, Entity entity)
    {
        manager.SummonMonster(size, position, entity);
    }

    protected void SummonScaffol(Vector3 position)
    {
        manager.SummonScaffold(position);
    }

    protected void SummonGroundTrap()
    {
        manager.SummonGroundTrap();
    }

    public abstract void InitWaves(InGameManager _manager);
}
