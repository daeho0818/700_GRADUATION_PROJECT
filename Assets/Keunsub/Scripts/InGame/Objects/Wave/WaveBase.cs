using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WaveBase : MonoBehaviour
{
    public List<Action> WaveFuncList = new List<Action>();
    protected bool waveEnd;
    Coroutine waveCoroutine;

    [Header("Wave Entities")]
    public List<Entity> Monsters = new List<Entity>();
    public List<Platform> Platforms = new List<Platform>();

    [Header("Wave Base")]
    public MonsterCage monsterCage;
    public Platform platform;
    /*
        #region debug
        private void Start()
        {
            WaveStart();
        }
        #endregion*/

    InGameManager manager;

    public virtual void WaveStart(InGameManager _manager)
    {
        manager = _manager;
        waveCoroutine = StartCoroutine(WaveCoroutine());
    }

    protected Entity SpawnMonster(Entity monster, Vector2 spawnPos, EntitySize size)
    {
        MonsterCage cageTemp = Instantiate(monsterCage, transform);
        cageTemp.Init();
        Entity result = cageTemp.Appear(spawnPos, monster, size);

        return result;
    }

    protected Platform SpawnPlatform(Vector2 spawnPos)
    {
        Platform temp = Instantiate(platform, transform);
        temp.Init();
        temp.Appear(spawnPos);

        return temp;
    }

    protected void RemovePlatform(params Platform[] platforms)
    {
        foreach (var item in platforms)
        {
            item.Disappear();
        }
    }

    protected void Init(params Action[] waves)
    {
        foreach (var item in waves)
        {
            WaveFuncList.Add(item);
        }
    }

    IEnumerator WaveCoroutine()
    {
        foreach (var item in WaveFuncList)
        {
            item?.Invoke();

            waveEnd = false;
            while (!waveEnd) yield return null;
            waveEnd = false;
        }

        manager.GameEnd();

    }

}