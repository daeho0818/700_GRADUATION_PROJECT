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
        Init(FirstWave, SecondWave);
    }

    void FirstWave()
    {
        StartCoroutine(FirstWaveCoroutine());
    }

    IEnumerator FirstWaveCoroutine()
    {
        List<Entity> RemainEnemy = new List<Entity>();

        RemainEnemy.Add(SpawnMonster(BigOrc, new Vector2(-10f, -5f), EntitySize.Medium));
        RemainEnemy.Add(SpawnMonster(BigOrc, new Vector2(10f, -5f), EntitySize.Medium));


        while (RemainEnemy.Count > 0)
        {
            for (int i = 0; i < RemainEnemy.Count; i++)
                if (RemainEnemy[i].IsDestroy) RemainEnemy.Remove(RemainEnemy[i]);

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
        List<Platform> platforms = new List<Platform>();
        List<Entity> monsters = new List<Entity>();

        platforms.Add(SpawnPlatform(new Vector2(-10f, -2f)));
        platforms.Add(SpawnPlatform(new Vector2(-7.5f, -2f)));
        platforms.Add(SpawnPlatform(new Vector2(-5f, -2f)));
        platforms.Add(SpawnPlatform(new Vector2(10f, -2f)));
        platforms.Add(SpawnPlatform(new Vector2(7.5f, -2f)));
        platforms.Add(SpawnPlatform(new Vector2(5f, -2f)));

        yield return new WaitForSeconds(3f);

        monsters.Add(SpawnMonster(ThrowOrc, new Vector2(-7.5f, -1f), EntitySize.Medium));
        monsters.Add(SpawnMonster(ThrowOrc, new Vector2(7.5f, -1f), EntitySize.Medium));

        yield return new WaitForSeconds(3f);

        monsters.Add(SpawnMonster(DashOrc, new Vector2(-11f, -6f), EntitySize.Medium));
        monsters.Add(SpawnMonster(DashOrc, new Vector2(11f, -6f), EntitySize.Medium));


        while (monsters.Count > 0)
        {
            for (int i = 0; i < monsters.Count; i++)
            {
                if (monsters[i].IsDestroy) monsters.Remove(monsters[i]);
            }
            yield return null;
        }

        Debug.Log("wave end");
        waveEnd = true;
    }
}
