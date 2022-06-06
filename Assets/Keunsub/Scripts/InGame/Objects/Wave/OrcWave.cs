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
        Init(FirstWave);
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
            foreach (var item in RemainEnemy) if (item.hp <= 0f) RemainEnemy.Remove(item);
            yield return null;
        }

        waveEnd = true;
    }
}
