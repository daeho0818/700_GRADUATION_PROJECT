using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orc_0004 : GroundObject
{
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override bool AttackCheck()
    {
        float distance = Vector2.Distance(transform.position, player.transform.position);
        return distance <= distance_with_player;
    }

    [SerializeField] int attack_damage = 1;
    /// <summary>
    /// 땅을 내려치며 공격
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator BaseAttack()
    {
        BoxCollider2D collider;
        int dir_x;
        Player p;

        yield return null;

        collider = (BoxCollider2D)colliders[1];
        dir_x = player.transform.position.x > transform.position.x ? 1 : -1;

        p = CheckCollision(transform.position, collider, 0);
        p?.OnHit?.Invoke(attack_damage);
    }

    protected override IEnumerator AIMoving()
    {
        yield return null;
    }

    protected override bool FindPlayer()
    {
        float distance = Vector2.Distance(transform.position, player.transform.position);
        return distance > distance_with_player;
    }
}
