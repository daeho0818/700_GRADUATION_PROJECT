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

    protected override void Update()
    {
        base.Update();
    }

    protected override bool FindPlayer() => base_attack == null;

    protected override bool AttackCheck() => smash_attack == null;

    /// <summary>
    /// 플레이어를 향해 돌진하는 공격 스킬
    /// </summary>
    protected override IEnumerator BaseAttack()
    {
        if (base_attack != null) StopCoroutine(base_attack);
        base_attack = StartCoroutine(_BaseAttack());

        yield return base_attack;
    }
    Coroutine base_attack = null;
    IEnumerator _BaseAttack()
    {
        yield return new WaitForSeconds(2);

        Vector3 force = (renderer.flipX ? Vector2.right : Vector2.left) * 10; // 바라보고 있는 방향으로 돌진
        force.y = 2;
        rigid.AddForce(force, ForceMode2D.Impulse);
        FlipSprite(force.x > 0);

        yield return new WaitForSeconds(0.5f);
        base_attack = null;
    }

    /// <summary>
    /// 플레이어를 향해 이동하며 가까이 도착했을 경우 할퀴기
    /// </summary>
    protected override void MoveToPlayer()
    {
        if (smash_attack != null) return;

        if (Vector2.Distance(player.transform.position, transform.position) <= distance_with_player)
        {
            smash_attack = StartCoroutine(SmashAttack());
            return;
        }

        base.MoveToPlayer();
    }

    Coroutine smash_attack = null;
    /// <summary>
    /// 할퀴기 공격
    /// </summary>
    IEnumerator SmashAttack()
    {
        yield return new WaitForSeconds(0.5f);

        BoxCollider2D collider = (BoxCollider2D)colliders[1];

        Player p = CheckCollision(transform.position, collider, 0);
        p?.OnHit?.Invoke(1);
        yield return new WaitForSeconds(2f);

        smash_attack = null;
    }

    protected override IEnumerator AIMoving()
    {
        yield return null;
    }
}
