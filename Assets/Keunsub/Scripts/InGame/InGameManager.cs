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

    [SerializeField] int allocSize;
    [SerializeField] Scaffold scaffoldPrefab;
    Stack<Scaffold> DeactiveScaffold = new Stack<Scaffold>();
    Stack<Scaffold> ActiveScaffold = new Stack<Scaffold>();

    void Start()
    {
        waveIdx = 0;
        InitWave();
    }

    void Update()
    {

    }

    void AllocScaffold()
    {
        for (int i = 0; i < allocSize; i++)
        {
            Scaffold temp = Instantiate(scaffoldPrefab);
            temp.gameObject.SetActive(false);
            DeactiveScaffold.Push(temp);
        }
    }

    public void DeactiveAllScaffold(float duration)
    {
        foreach (var item in ActiveScaffold)
        {
            item.Disappear(duration, () => { item.gameObject.SetActive(false); });
        }
    }

    public void InitWave()
    {
        Waves[waveIdx].InitWaves(this);
        WaveFunc = Waves[waveIdx].waves;
    }

    public void SummonMonster(MonsterSize size, Vector3 position, Entity entity)
    {

    }

    public void SummonScaffold(Vector3 position)
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