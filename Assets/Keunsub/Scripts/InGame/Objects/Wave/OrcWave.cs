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
        Init(Wave1, Wave2, Wave3, Wave4, Wave5, Wave6, Wave7, Wave8);
    }

    void Wave1()
    {
        StartCoroutine(Wave1Coroutine());
    }

    IEnumerator Wave1Coroutine()
    {
        Monsters.Add(SpawnMonster(BigOrc, new Vector2(-10f, -5f), EntitySize.Medium));


        while (Monsters.Count > 0)
        {
            for (int i = 0; i < Monsters.Count; i++)
                if (Monsters[i].IsDestroy) Monsters.Remove(Monsters[i]);

            yield return null;
        }
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


        while (Monsters.Count > 0)
        {
            for (int i = 0; i < Monsters.Count; i++)
                if (Monsters[i].IsDestroy) Monsters.Remove(Monsters[i]);

            yield return null;
        }
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

        while (Monsters.Count > 0)
        {
            for (int i = 0; i < Monsters.Count; i++)
                if (Monsters[i].IsDestroy) Monsters.Remove(Monsters[i]);

            yield return null;
        }
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

        while (Monsters.Count > 0)
        {
            for (int i = 0; i < Monsters.Count; i++)
                if (Monsters[i].IsDestroy) Monsters.Remove(Monsters[i]);

            yield return null;
        }

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


        while (Monsters.Count > 0)
        {
            for (int i = 0; i < Monsters.Count; i++)
                if (Monsters[i].IsDestroy) Monsters.Remove(Monsters[i]);

            yield return null;
        }
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


        while (Monsters.Count > 0)
        {
            for (int i = 0; i < Monsters.Count; i++)
                if (Monsters[i].IsDestroy) Monsters.Remove(Monsters[i]);

            yield return null;
        }
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


        while (Monsters.Count > 0)
        {
            for (int i = 0; i < Monsters.Count; i++)
                if (Monsters[i].IsDestroy) Monsters.Remove(Monsters[i]);

            yield return null;
        }
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

        while (Monsters.Count > 0)
        {
            for (int i = 0; i < Monsters.Count; i++)
                if (Monsters[i].IsDestroy) Monsters.Remove(Monsters[i]);

            yield return null;
        }

        RemovePlatform(Platforms);
        yield return new WaitForSeconds(3f);

        waveEnd = true;
    }
}
