using System;
using System.Linq;
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


    [SerializeField] Transform initPos;
    [SerializeField] int allocSize;

    #region Scaffold
    [SerializeField] Scaffold scaffoldPrefab;
    Stack<Scaffold> DeactiveScaffold = new Stack<Scaffold>();
    Stack<Scaffold> ActiveScaffold = new Stack<Scaffold>();
    #endregion

    #region Cage
    [SerializeField] MonsterCage bigCagePrefab;
    [SerializeField] MonsterCage midiumCagePrefab;
    [SerializeField] MonsterCage smallCagePrefab;
    Stack<MonsterCage> BigCage = new Stack<MonsterCage>();
    Stack<MonsterCage> MidiumCage = new Stack<MonsterCage>();
    Stack<MonsterCage> SmallCage = new Stack<MonsterCage>();
    #endregion

    Coroutine combatCoroutine;

    [HideInInspector] public bool GameActive;


    void Start()
    {
        AllocCage();
        AllocScaffold();
        waveIdx = 0;
        InitWave();
    }

    void Update()
    {

    }

    public List<Scaffold> GetActiveScaffold()
    {
        var platformList = ActiveScaffold.ToList();
        return platformList;
    }

    void AllocCage()
    {
        for (int i = 0; i < allocSize; i++)
        {
            MonsterCage bigTmp = Instantiate(bigCagePrefab, initPos.position, Quaternion.identity, transform);
            MonsterCage midiumTmp = Instantiate(midiumCagePrefab, initPos.position, Quaternion.identity, transform);
            MonsterCage smallTmp = Instantiate(smallCagePrefab, initPos.position, Quaternion.identity, transform);

            bigTmp.Init();
            midiumTmp.Init();
            smallTmp.Init();

            bigTmp.gameObject.SetActive(false);
            midiumTmp.gameObject.SetActive(false);
            smallTmp.gameObject.SetActive(false);

            BigCage.Push(bigTmp);
            MidiumCage.Push(midiumTmp);
            SmallCage.Push(smallTmp);

        }
    }

    void AllocScaffold()
    {
        for (int i = 0; i < allocSize; i++)
        {
            Scaffold temp = Instantiate(scaffoldPrefab, initPos.position, Quaternion.identity, transform);
            temp.Init();
            temp.gameObject.SetActive(false);
            DeactiveScaffold.Push(temp);
        }
    }

    public void DeactiveAllScaffold(float duration)
    {
        int count = ActiveScaffold.Count;
        for (int i = 0; i < count; i++)
        {
            Scaffold temp = ActiveScaffold.Pop();
            temp.Disappear(duration, () => { temp.gameObject.SetActive(false); });
            DeactiveScaffold.Push(temp);
        }
    }

    public void InitWave()
    {
        Waves[waveIdx].InitWaves(this);
        WaveFunc = Waves[waveIdx].waves;
    }

    public Entity SummonMonster(MonsterSize size, Vector3 position, Entity entity)
    {
        MonsterCage temp = null;
        switch (size)
        {
            case MonsterSize.SMALL:
                temp = SmallCage.Pop();
                break;
            case MonsterSize.MIDIUM:
                temp = MidiumCage.Pop();
                break;
            case MonsterSize.BIG:
                temp = BigCage.Pop();
                break;
        }
        temp.gameObject.SetActive(true);
        Entity retMonster = temp.Appear(entity, position, 0.5f, () => { temp.DisAppear(1f, ()=> { temp.gameObject.SetActive(false); }); });
        return retMonster;
    }

    public void SummonScaffold(Vector3 position)
    {
        Scaffold temp = DeactiveScaffold.Pop();
        temp.gameObject.SetActive(true);
        temp.Appear(0.5f, position);
        ActiveScaffold.Push(temp);
    }

    public void SummonGroundTrap()
    {

    }

    public void GameStart()
    {
        combatCoroutine = StartCoroutine(CombatCoroutine());
        GameActive = true;
    }

    public void GameOver()
    {
        StopCoroutine(combatCoroutine);
    }

    IEnumerator CombatCoroutine()
    {
        //door close
        //intro
        //delay

        yield return new WaitForSeconds(2f);

        foreach (var wave in WaveFunc)
        {
            wave();

            while (!waveEnd)
            {
                yield return null;
            }
        }

        //game finished
    }
}