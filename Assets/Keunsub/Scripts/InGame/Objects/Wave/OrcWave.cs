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
        Init(FirstWave, SecondWave, ThirdWave);
    }

    void FirstWave()
    {
        StartCoroutine(FirstWaveCoroutine());
    }

    IEnumerator FirstWaveCoroutine()
    {
        Monsters.Add(SpawnMonster(BigOrc, new Vector2(-10f, -5f), EntitySize.Medium));
        Monsters.Add(SpawnMonster(BigOrc, new Vector2(10f, -5f), EntitySize.Medium));


        while (Monsters.Count > 0)
        {
            for (int i = 0; i < Monsters.Count; i++)
                if (Monsters[i].IsDestroy) Monsters.Remove(Monsters[i]);

            yield return null;
        }
        Debug.Log("wave end");
        waveEnd = true;
    }

    void SecondWave()
    {
        StartCoroutine(SecondWaveCoroutine());
    }

    IEnumerator SecondWaveCoroutine()
    {

        Platforms.Add(SpawnPlatform(new Vector2(-10f, -2f)));
        Platforms.Add(SpawnPlatform(new Vector2(-7.5f, -2f)));
        Platforms.Add(SpawnPlatform(new Vector2(-5f, -2f)));
        Platforms.Add(SpawnPlatform(new Vector2(10f, -2f)));
        Platforms.Add(SpawnPlatform(new Vector2(7.5f, -2f)));
        Platforms.Add(SpawnPlatform(new Vector2(5f, -2f)));

        yield return new WaitForSeconds(3f);

        Monsters.Add(SpawnMonster(ThrowOrc, new Vector2(-7.5f, -1f), EntitySize.Medium));
        Monsters.Add(SpawnMonster(ThrowOrc, new Vector2(7.5f, -1f), EntitySize.Medium));

        yield return new WaitForSeconds(3f);

        Monsters.Add(SpawnMonster(DashOrc, new Vector2(-11f, -6f), EntitySize.Medium));
        Monsters.Add(SpawnMonster(DashOrc, new Vector2(11f, -6f), EntitySize.Medium));


        while (Monsters.Count > 0)
        {
            for (int i = 0; i < Monsters.Count; i++)
            {
                if (Monsters[i].IsDestroy) Monsters.Remove(Monsters[i]);
            }
            yield return null;
        }

        Debug.Log("wave end");
        waveEnd = true;
    }

    void ThirdWave()
    {
        StartCoroutine(ThirdWaveCoroutine());
    }

    IEnumerator ThirdWaveCoroutine()
    {
        Platforms.Add(SpawnPlatform(new Vector2(-2.5f, 2f)));
        Platforms.Add(SpawnPlatform(new Vector2(0f, 2f)));
        Platforms.Add(SpawnPlatform(new Vector2(2.5f, 2f)));

        yield return new WaitForSeconds(3f);

        Monsters.Add(SpawnMonster(ShotgunOrc, new Vector2(0, 5f), EntitySize.Medium));
        Monsters.Add(SpawnMonster(BigOrc, new Vector2(-7.5f, -1f), EntitySize.Medium));
        Monsters.Add(SpawnMonster(DashOrc, new Vector2(7.5f, -1f), EntitySize.Medium));

        while (Monsters.Count > 0)
        {
            for (int i = 0; i < Monsters.Count; i++)
            {
                if (Monsters[i].IsDestroy) Monsters.Remove(Monsters[i]);
            }
            yield return null;
        }

        Debug.Log("wave end");
        RemovePlatform(Platforms.ToArray());
        waveEnd = true;
    }
}
