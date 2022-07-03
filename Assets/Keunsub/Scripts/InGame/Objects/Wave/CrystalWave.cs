using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalWave : WaveBase
{

    [Header("Crystal Monsters")]
    public Entity WolfCrystal;
    public Entity FistCrystal;
    public Entity[] MagicianCrystal;
    public Entity[] CubeCrystal;
    public Entity GolemCrystal;

    private void Awake()
    {
        Init(Wave1, Wave2, Wave3, Wave4, Wave5, Wave6, Wave7, Wave8, Wave9, Wave10);
    }

    void Wave1()
    {
        StartCoroutine(Wave1Coroutine());
    }

    IEnumerator Wave1Coroutine()
    {
        SoundManager.Instance.PlayBackground("Crystal_Battle");
        Monsters.Add(SpawnMonster(MagicianCrystal, new Vector2(-10f, -3f), EntitySize.Medium));
        Monsters.Add(SpawnMonster(MagicianCrystal, new Vector2(10f, -3f), EntitySize.Medium));

        yield return StartCoroutine(WaitUntilMonsterDie(Monsters));
        waveEnd = true;
    }

    void Wave2()
    {
        StartCoroutine(Wave2Coroutine());
    }

    IEnumerator Wave2Coroutine()
    {
        Platforms.Add(SpawnPlatform(new Vector2(-7f, -2f)));
        Platforms.Add(SpawnPlatform(new Vector2(-4.5f, -2f)));
        Platforms.Add(SpawnPlatform(new Vector2(7f, -2f)));
        Platforms.Add(SpawnPlatform(new Vector2(4.5f, -2f)));

        yield return new WaitForSeconds(3f);

        Monsters.Add(SpawnMonster(MagicianCrystal, new Vector2(-11f, -3f), EntitySize.Medium));
        Monsters.Add(SpawnMonster(MagicianCrystal, new Vector2(-9f, 0), EntitySize.Medium));
        Monsters.Add(SpawnMonster(MagicianCrystal, new Vector2(11f, -3f), EntitySize.Medium));
        Monsters.Add(SpawnMonster(MagicianCrystal, new Vector2(9f, 0), EntitySize.Medium));



        yield return StartCoroutine(WaitUntilMonsterDie(Monsters));
        waveEnd = true;
    }

    void Wave3()
    {
        StartCoroutine(Wave3Coroutine());
    }

    IEnumerator Wave3Coroutine()
    {
        Monsters.Add(SpawnMonster(CubeCrystal, new Vector2(10f, -6.5f), EntitySize.Small));
        Monsters.Add(SpawnMonster(CubeCrystal, new Vector2(-10f, -6.5f), EntitySize.Small));    

        yield return StartCoroutine(WaitUntilMonsterDie(Monsters));
        waveEnd = true;
    }

    void Wave4()
    {
        StartCoroutine(Wave4Coroutine());
    }

    IEnumerator Wave4Coroutine()
    {
        Platforms.Add(SpawnPlatform(new Vector2(-10f, 1f)));
        Platforms.Add(SpawnPlatform(new Vector2(10f, 1f)));

        yield return new WaitForSeconds(3f);

        Monsters.Add(SpawnMonster(CubeCrystal, new Vector2(-11f, 2.5f), EntitySize.Small));
        Monsters.Add(SpawnMonster(CubeCrystal, new Vector2(11f, 2.5f), EntitySize.Small));

        Monsters.Add(SpawnMonster(CubeCrystal, new Vector2(-9f, -0.5f), EntitySize.Small));
        Monsters.Add(SpawnMonster(CubeCrystal, new Vector2(9f, -0.5f), EntitySize.Small));

        yield return StartCoroutine(WaitUntilMonsterDie(Monsters));

        RemovePlatform(Platforms);
        waveEnd = true;
    }

    void Wave5()
    {
        StartCoroutine(Wave5Coroutine());
    }

    IEnumerator Wave5Coroutine()
    {
        Monsters.Add(SpawnMonster(WolfCrystal, new Vector2(10f, -6.5f), EntitySize.Medium));

        yield return StartCoroutine(WaitUntilMonsterDie(Monsters));
        waveEnd = true;
    }

    void Wave6()
    {
        StartCoroutine(Wave6Coroutine());
    }

    IEnumerator Wave6Coroutine()
    {
        SoundManager.Instance.PlayBackground("Crystal_Climax");
        Platforms.Add(SpawnPlatform(new Vector2(-3.75f, -3f)));
        Platforms.Add(SpawnPlatform(new Vector2(-1.25f, -3f)));
        Platforms.Add(SpawnPlatform(new Vector2(1.25f, -3f)));
        Platforms.Add(SpawnPlatform(new Vector2(3.75f, -3f)));
        Platforms.Add(SpawnPlatform(new Vector2(-3.75f, 0.5f)));
        Platforms.Add(SpawnPlatform(new Vector2(-1.25f, 0.5f)));
        Platforms.Add(SpawnPlatform(new Vector2(1.25f, 0.5f)));
        Platforms.Add(SpawnPlatform(new Vector2(3.75f, 0.5f)));
        Platforms.Add(SpawnPlatform(new Vector2(-3.75f, 4f)));
        Platforms.Add(SpawnPlatform(new Vector2(-1.25f, 4f)));
        Platforms.Add(SpawnPlatform(new Vector2(1.25f, 4f)));
        Platforms.Add(SpawnPlatform(new Vector2(3.75f, 4f)));
        yield return new WaitForSeconds(3f);

        SpawnSharp();
        yield return new WaitForSeconds(3f);

        Monsters.Add(SpawnMonster(CubeCrystal, new Vector2(0, -1.2f), EntitySize.Small));

        yield return StartCoroutine(WaitUntilMonsterDie(Monsters));
        waveEnd = true;
    }

    void Wave7()
    {
        StartCoroutine(Wave7Coroutine());
    }

    IEnumerator Wave7Coroutine()
    {
        SpawnWall(14f);
        yield return new WaitForSeconds(2f);

        Monsters.Add(SpawnMonster(CubeCrystal, new Vector2(0, 2.5f), EntitySize.Small));
        Monsters.Add(SpawnMonster(MagicianCrystal, new Vector2(6f, 6f), EntitySize.Medium));
        Monsters.Add(SpawnMonster(MagicianCrystal, new Vector2(-6f, -2.5f), EntitySize.Medium));

        yield return StartCoroutine(WaitUntilMonsterDie(Monsters));
        RemovePlatform(Platforms);
        RemoveSharp();
        RemoveWall();
        waveEnd = true;
    }

    void Wave8()
    {
        StartCoroutine(Wave8Coroutine());
    }

    IEnumerator Wave8Coroutine()
    {
        Platforms.Add(SpawnPlatform(new Vector2(0, -4f)));
        Platforms.Add(SpawnPlatform(new Vector2(5.5f, -2.5f)));
        Platforms.Add(SpawnPlatform(new Vector2(10.5f, 1.5f)));
        Platforms.Add(SpawnPlatform(new Vector2(5.5f, 5.5f)));
        Platforms.Add(SpawnPlatform(new Vector2(0, 2f)));
        Platforms.Add(SpawnPlatform(new Vector2(-6.5f, 3f)));
        Platforms.Add(SpawnPlatform(new Vector2(-10.5f, -2f)));

        yield return new WaitForSeconds(5f);

        Monsters.Add(SpawnMonster(MagicianCrystal, new Vector2(-4.5f, -1.5f), EntitySize.Medium));
        yield return new WaitForSeconds(1f);
        Monsters.Add(SpawnMonster(MagicianCrystal, new Vector2(8.6f, 3.5f), EntitySize.Medium));
        yield return new WaitForSeconds(1f);
        Monsters.Add(SpawnMonster(MagicianCrystal, new Vector2(-11f, 3f), EntitySize.Medium));
        yield return new WaitForSeconds(1f);
        Monsters.Add(SpawnMonster(MagicianCrystal, new Vector2(10f, -3.5f), EntitySize.Medium));
        yield return new WaitForSeconds(1f);


        yield return StartCoroutine(WaitUntilMonsterDie(Monsters));
        waveEnd = true;
    }

    void Wave9()
    {
        StartCoroutine(Wave9Coroutine());
    }

    IEnumerator Wave9Coroutine()
    {
        Monsters.Add(SpawnMonster(CubeCrystal, new Vector2(0, -2.5f), EntitySize.Small));
        Monsters.Add(SpawnMonster(CubeCrystal, new Vector2(5.5f, -1f), EntitySize.Small));
        Monsters.Add(SpawnMonster(CubeCrystal, new Vector2(10.5f, 3f), EntitySize.Small));
        Monsters.Add(SpawnMonster(CubeCrystal, new Vector2(5.5f, 7f), EntitySize.Small));
        Monsters.Add(SpawnMonster(CubeCrystal, new Vector2(0, 3.5f), EntitySize.Small));
        Monsters.Add(SpawnMonster(CubeCrystal, new Vector2(-6.5f, 4.5f), EntitySize.Small));
        Monsters.Add(SpawnMonster(CubeCrystal, new Vector2(-10.5f, -0.5f), EntitySize.Small));

        yield return StartCoroutine(WaitUntilMonsterDie(Monsters));
        
        RemovePlatform(Platforms);
        yield return new WaitForSeconds(3f);
        waveEnd = true;
    }

    void Wave10()
    {
        StartCoroutine(Wave10Coroutine());
    }

    IEnumerator Wave10Coroutine()
    {
        SpawnRoof(3.5f);
        SpawnWall(7f);
        Monsters.Add(SpawnMonster(FistCrystal, new Vector2(-6f, -5f), EntitySize.Medium));
        Monsters.Add(SpawnMonster(WolfCrystal, new Vector2(6f, -5.5f), EntitySize.Medium));

        yield return StartCoroutine(WaitUntilMonsterDie(Monsters));
        waveEnd = true;

        RemoveRoof();
        RemoveWall();
    }

    void BossWave()
    {
        StartCoroutine(BossWaveCoroutine());
    }

    IEnumerator BossWaveCoroutine()
    {



        yield return StartCoroutine(WaitUntilMonsterDie(Monsters));
    }
}
