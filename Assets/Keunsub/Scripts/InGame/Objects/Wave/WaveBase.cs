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

    protected void SpawnSharp()
    {
        // 바닥 가시 소환
    }

    protected void RemoveSharp()
    {
        // 바닥 가시 제거
    }
        
    protected void RemovePlatform(List<Platform> platforms)
    {
        foreach (var item in platforms)
        {
            item.Disappear();
        }

        platforms.Clear();
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
        int curWaveIdx = 0;

        foreach (var item in WaveFuncList)
        {
            item?.Invoke();

            waveEnd = false;
            while (!waveEnd) yield return null;
            waveEnd = false;

            bool isUpgrade = GameManager.Instance.player.Exp > GameManager.Instance.player.MaxExp;

            if (isUpgrade && curWaveIdx != WaveFuncList.Count - 1) yield return StartCoroutine(manager.UpgradePause());

            yield return new WaitForSeconds(2f);
            curWaveIdx++;
        }

        GameManager.Instance.player.StateInit();
        manager.GameEnd();

    }

    protected IEnumerator WaitUntilMonsterDie(List<Entity> EntityList, int remainCnt = 0)
    {
        while (EntityList.Count > remainCnt)
        {
            for (int i = 0; i < EntityList.Count; i++)
                if (EntityList[i].IsDestroy)
                {
                    StartCoroutine(DestroyMonster(EntityList[i]));
                    EntityList.Remove(EntityList[i]);
                }

            yield return null;
        }
    }

    protected IEnumerator DestroyMonster(Entity monster)
    {
        yield return new WaitForSeconds(3f);

        monster.GetComponentInChildren<Collider2D>().isTrigger = true;

        while (monster.transform.position.y > -10f) yield return null;

        Destroy(monster.gameObject);
    }

}
