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
    /// �÷��̾ ���� �����ϴ� ���� ��ų
    /// </summary>
    protected override void BaseAttack()
    {
        return;

        if (base_attack != null) StopCoroutine(base_attack);
        base_attack = StartCoroutine(_BaseAttack());
    }
    Coroutine base_attack = null;
    IEnumerator _BaseAttack()
    {
        yield return new WaitForSeconds(2);

        Vector3 target = player.transform.position;
        Vector3 force = (target - transform.position);
        force.y = 2;
        rigid.AddForce(force, ForceMode2D.Impulse);
        renderer.flipX = force.x > 0;

        yield return new WaitForSeconds(0.5f);
        base_attack = null;
    }

    /// <summary>
    /// �÷��̾ ���� �̵��ϸ� ������ �������� ��� ������
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
    /// ������ ����
    /// </summary>
    IEnumerator SmashAttack()
    {
        yield return new WaitForSeconds(0.5f);

        BoxCollider2D collider = (BoxCollider2D)colliders[1];

        int dir_x = player.transform.position.x > transform.position.x ? 1 : -1;
        SetColliderDirection(collider, dir_x);

        Player p = CheckCollision(transform.position, collider, 0);
        Debug.Log(p);
        // p?.OnHit();
        yield return new WaitForSeconds(2f);

        smash_attack = null;
    }

    protected override IEnumerator AIMoving()
    {
        yield return null;
    }
}
