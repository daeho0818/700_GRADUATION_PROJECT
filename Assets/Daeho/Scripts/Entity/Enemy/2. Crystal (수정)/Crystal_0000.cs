using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal_0000 : GroundObject
{
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }

    Player p;
    protected override void Update()
    {
        base.Update();

        if (p == null && animation.GetStateName() == "Attack")
        {
            p = CheckCollision(transform.position, (BoxCollider2D)colliders[0], 0);
            p?.OnHit?.Invoke(1);
        }
    }

    protected override bool AttackCheck() => true;

    /// <summary>
    /// 플레이어를 향해 할퀴거나 돌진하는 공격 스킬
    /// </summary>
    protected override IEnumerator BaseAttack()
    {
        float distance = Vector2.Distance(transform.position, player.transform.position);
        IEnumerator enumerator;

        if (distance <= attack_distance)
            enumerator = SmashAttack();
        else
            enumerator = DashAttack();

        p = null;

        yield return StartCoroutine(enumerator);
    }
    IEnumerator DashAttack()
    {
        yield return null;

        Vector3 force = (player.transform.position.x > transform.position.x ? Vector2.right : Vector2.left) * 10; // 바라보고 있는 방향으로 돌진
        force.y = 2;
        rigid.AddForce(force, ForceMode2D.Impulse);
        FlipSprite(force.x > 0);

        Player p = null;

        while (rigid.velocity.x != 0)
        {
            if (p == null)
            {
                var collider = (BoxCollider2D)colliders[0];

                p = CheckCollision(transform.position, collider, 0);
                p?.OnHit?.Invoke(1);
            }

            yield return null;
        }
    }

    /// <summary>
    /// 할퀴기 공격
    /// </summary>
    IEnumerator SmashAttack()
    {
        yield return null;

        var collider = (BoxCollider2D)colliders[1];

        Player p = CheckCollision(transform.position, collider, 0);
        p?.OnHit?.Invoke(1);
    }

    protected override IEnumerator AIMoving()
    {
        yield return null;
    }
}
