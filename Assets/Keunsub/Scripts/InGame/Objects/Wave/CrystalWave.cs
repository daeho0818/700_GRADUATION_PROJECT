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

    void Wave4()
    {

    }

    void Wave5()
    {

    }

    void Wave6()
    {

    }

    void Wave7()
    {

    }

    void Wave8()
    {

    }

    void Wave9()
    {

    }

    void Wave10()
    {

    }
}
