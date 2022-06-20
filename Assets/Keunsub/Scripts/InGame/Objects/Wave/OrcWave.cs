using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrcWave : WaveBase
{

    [Header("Orc Monsters")]
    public Entity BigOrc;
    public Entity DashOrc;
    public Entity ThrowOrc;
    public Entity PistolOrc;
    public Entity ShotgunOrc;
    public Entity OrcBoss;

    private void Awake()
    {
        Init(Wave1, Wave2, Wave3, Wave4, Wave5, Wave6, Wave7, Wave8, Wave9, Wave10, Wave11, Wave12);
    }

    void Wave1()
    {
        StartCoroutine(Wave1Coroutine());
    }

    IEnumerator Wave1Coroutine()
    {
        Monsters.Add(SpawnMonster(BigOrc, new Vector2(-10f, -5f), EntitySize.Medium));

        yield return StartCoroutine(WaitUntilMonsterDie(Monsters));
        waveEnd = true;
    }

    void Wave2()
    {
        StartCoroutine(Wave2Coroutine());
    }

    IEnumerator Wave2Coroutine()
    {
        Monsters.Add(SpawnMonster(BigOrc, new Vector2(-10f, -5f), EntitySize.Medium));
        Monsters.Add(SpawnMonster(BigOrc, new Vector2(10f, -5f), EntitySize.Medium));

        yield return StartCoroutine(WaitUntilMonsterDie(Monsters));
        waveEnd = true;
    }

    void Wave3()
    {
        StartCoroutine(Wave3Coroutine());
    }

    IEnumerator Wave3Coroutine()
    {
        Platforms.Add(SpawnPlatform(new Vector2(-5, -4)));
        Platforms.Add(SpawnPlatform(new Vector2(5, -4)));

        yield return new WaitForSeconds(3f);

        Monsters.Add(SpawnMonster(ShotgunOrc, new Vector2(9, -2), EntitySize.Small));

        yield return StartCoroutine(WaitUntilMonsterDie(Monsters));

        waveEnd = true;
    }

    void Wave4()
    {
        StartCoroutine(Wave4Coroutine());
    }

    IEnumerator Wave4Coroutine()
    {
        Platforms.Add(SpawnPlatform(new Vector2(0, -2)));

        yield return new WaitForSeconds(3f);

        Monsters.Add(SpawnMonster(ShotgunOrc, new Vector2(9, -2), EntitySize.Small));
        Monsters.Add(SpawnMonster(ShotgunOrc, new Vector2(-9, -2), EntitySize.Small));

        yield return StartCoroutine(WaitUntilMonsterDie(Monsters));

        RemovePlatform(Platforms);
        yield return new WaitForSeconds(3f);

        waveEnd = true;
    }

    void Wave5()
    {
        StartCoroutine(Wave5Coroutine());
    }

    IEnumerator Wave5Coroutine()
    {
        Monsters.Add(SpawnMonster(DashOrc, new Vector2(-10f, -5f), EntitySize.Medium));


        yield return StartCoroutine(WaitUntilMonsterDie(Monsters));
        waveEnd = true;
    }

    void Wave6()
    {
        StartCoroutine(Wave6Coroutine());
    }

    IEnumerator Wave6Coroutine()
    {
        Monsters.Add(SpawnMonster(DashOrc, new Vector2(-10f, -5f), EntitySize.Medium));
        Monsters.Add(SpawnMonster(DashOrc, new Vector2(10f, -5f), EntitySize.Medium));

        yield return StartCoroutine(WaitUntilMonsterDie(Monsters));
        waveEnd = true;
    }

    void Wave7()
    {
        StartCoroutine(Wave7Coroutine());
    }

    IEnumerator Wave7Coroutine()
    {
        Monsters.Add(SpawnMonster(ThrowOrc, new Vector2(-10f, -5f), EntitySize.Small));
        Monsters.Add(SpawnMonster(ThrowOrc, new Vector2(10f, -5f), EntitySize.Small));

        yield return StartCoroutine(WaitUntilMonsterDie(Monsters));

        waveEnd = true;
    }

    void Wave8()
    {
        StartCoroutine(Wave8Coroutine());
    }

    IEnumerator Wave8Coroutine()
    {
        Platforms.Add(SpawnPlatform(new Vector2(-5, -4)));
        Platforms.Add(SpawnPlatform(new Vector2(0, -2)));
        Platforms.Add(SpawnPlatform(new Vector2(6, 0)));
        Platforms.Add(SpawnPlatform(new Vector2(0, 3)));
        Platforms.Add(SpawnPlatform(new Vector2(-5, 5)));

        yield return new WaitForSeconds(3f);

        Monsters.Add(SpawnMonster(ThrowOrc, new Vector2(-5, -3.5f), EntitySize.Small));
        Monsters.Add(SpawnMonster(ThrowOrc, new Vector2(0, -1.5f), EntitySize.Small));
        Monsters.Add(SpawnMonster(ThrowOrc, new Vector2(6, 0.5f), EntitySize.Small));
        Monsters.Add(SpawnMonster(ThrowOrc, new Vector2(0, 3.5f), EntitySize.Small));
        Monsters.Add(SpawnMonster(ThrowOrc, new Vector2(-5, 5.5f), EntitySize.Small));

        yield return StartCoroutine(WaitUntilMonsterDie(Monsters));

        RemovePlatform(Platforms);
        yield return new WaitForSeconds(3f);

        waveEnd = true;
    }

    void Wave9()
    {
        StartCoroutine(Wave9Coroutine());
    }

    IEnumerator Wave9Coroutine()
    {

        Platforms.Add(SpawnPlatform(new Vector2(7, 3)));
        Platforms.Add(SpawnPlatform(new Vector2(-7, 3)));

        yield return new WaitForSeconds(2f);

        Monsters.Add(SpawnMonster(ThrowOrc, new Vector2(7, 3.5f), EntitySize.Small));
        Monsters.Add(SpawnMonster(ThrowOrc, new Vector2(-7, 3.5f), EntitySize.Small));

        yield return new WaitForSeconds(2f);

        List<Entity> temp = new List<Entity>();

        temp.Add(SpawnMonster(DashOrc, new Vector2(-10, -6.5f), EntitySize.Medium));
        temp.Add(SpawnMonster(DashOrc, new Vector2(-8, -6.5f), EntitySize.Medium));
        temp.Add(SpawnMonster(DashOrc, new Vector2(10, -6.5f), EntitySize.Medium));
        temp.Add(SpawnMonster(DashOrc, new Vector2(8, -6.5f), EntitySize.Medium));

        yield return new WaitForSeconds(1f);

        yield return StartCoroutine(WaitUntilMonsterDie(temp));

        Platforms.Add(SpawnPlatform(new Vector2(1.25f, -1)));
        Platforms.Add(SpawnPlatform(new Vector2(-1.25f, -1)));

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

        Platforms.Add(SpawnPlatform(new Vector2(0, -3.5f)));
        Platforms.Add(SpawnPlatform(new Vector2(6, -1)));
        Platforms.Add(SpawnPlatform(new Vector2(-6, -1)));

        yield return new WaitForSeconds(3f);

        Monsters.Add(SpawnMonster(PistolOrc, new Vector2(10, -6.5f), EntitySize.Small));
        Monsters.Add(SpawnMonster(PistolOrc, new Vector2(-10, -6.5f), EntitySize.Small));
        Monsters.Add(SpawnMonster(ThrowOrc, new Vector2(-6, -0.5f), EntitySize.Small));
        Monsters.Add(SpawnMonster(ThrowOrc, new Vector2(6, -0.5f), EntitySize.Small));
        Monsters.Add(SpawnMonster(ShotgunOrc, new Vector2(11, 1), EntitySize.Small));
        Monsters.Add(SpawnMonster(ShotgunOrc, new Vector2(-11, 1), EntitySize.Small));

        yield return StartCoroutine(WaitUntilMonsterDie(Monsters));
        waveEnd = true;

    }

    void Wave11()
    {
        StartCoroutine(Wave11Coroutine());
    }

    IEnumerator Wave11Coroutine()
    {
        Platforms.Add(SpawnPlatform(new Vector2(3, 3)));
        Platforms.Add(SpawnPlatform(new Vector2(-3, 3)));

        yield return new WaitForSeconds(1f);

        SpawnSharp();

        Monsters.Add(SpawnMonster(ThrowOrc, new Vector2(3, 3.5f), EntitySize.Small));
        Monsters.Add(SpawnMonster(ThrowOrc, new Vector2(-3, 3.5f), EntitySize.Small));
        Monsters.Add(SpawnMonster(ShotgunOrc, new Vector2(11, 0), EntitySize.Small));
        Monsters.Add(SpawnMonster(ShotgunOrc, new Vector2(-11, 0), EntitySize.Small));

        yield return StartCoroutine(WaitUntilMonsterDie(Monsters));
    }

    void Wave12()
    {
        StartCoroutine(Wave12Coroutine());
    }

    IEnumerator Wave12Coroutine()
    {
        RemovePlatform(Platforms);

        Platforms.Add(SpawnPlatform(new Vector2(-3.75f, -3)));
        Platforms.Add(SpawnPlatform(new Vector2(-1.25f, -3)));
        Platforms.Add(SpawnPlatform(new Vector2(1.25f, -3)));
        Platforms.Add(SpawnPlatform(new Vector2(3.75f, -3)));
        Platforms.Add(SpawnPlatform(new Vector2(10, -3)));
        Platforms.Add(SpawnPlatform(new Vector2(-10, -3)));

        yield return new WaitForSeconds(3f);

        Monsters.Add(SpawnMonster(PistolOrc, new Vector2(10, -2.5f), EntitySize.Small));
        Monsters.Add(SpawnMonster(PistolOrc, new Vector2(-10, -2.5f), EntitySize.Small));
        Monsters.Add(SpawnMonster(BigOrc, new Vector2(3.75f, -2.5f), EntitySize.Medium));
        Monsters.Add(SpawnMonster(BigOrc, new Vector2(-3.75f, -2.5f), EntitySize.Medium));
    }
}
