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

    protected override IEnumerator BaseAttack()
    {
        BoxCollider2D collider;
        int dir_x;
        Player p;

        yield return null;

        FlipSprite();

        collider = (BoxCollider2D)colliders[1];
        dir_x = player.transform.position.x > transform.position.x ? 1 : -1;

        p = CheckCollision((Vector2)transform.position + collider.offset, collider, 0);
        p?.OnHit?.Invoke(1);
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
