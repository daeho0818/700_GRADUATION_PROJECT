using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orc_0003 : GroundObject
{

    Coroutine AttackCoroutine;

    protected override void Awake()
    {
        base.Awake();
        OnHit += DashKnockBack;
    }
    protected override void Start()
    {
        base.Start();

        ChangeState("Attack");
    }

    protected override void Update()
    {
        base.Update();
    }


    void DashKnockBack(int damage)
    {
        if (AttackCoroutine != null)
        {
            StopCoroutine(AttackCoroutine);
            StartCoroutine(animation.AnimEnd());
        }
    }

    /// <summary>
    /// ���� ��Ÿ�� ������ �����ϴ� ���� ����
    /// </summary>
    protected override IEnumerator BaseAttack()
    {
        float dir_x;

        dir_x = player.transform.position.x > transform.position.x ? 1 : -1;
        yield return AttackCoroutine = StartCoroutine(_BaseAttack(dir_x));
    }
    IEnumerator _BaseAttack(float dir_x)
    {
        RaycastHit2D[] hits;
        Vector2 dir = new Vector2(dir_x, 0);
        Player p = null;

        do
        {
            hits = Physics2D.RaycastAll(transform.position, dir, 2f, LayerMask.GetMask("Wall"));
            Debug.DrawRay(transform.position, dir * 2f, Color.red, 0.1f);

            // ���� ������ ��
            if (hits.Length > 0) break;

            hits = Physics2D.RaycastAll((Vector2)transform.position + (dir * 1), Vector2.down, 5f, LayerMask.GetMask("Ground"));
            Debug.DrawRay((Vector2)transform.position + (dir * 1), Vector2.down * 5f, Color.red, 0.1f);

            if (p == null)
            {
                p = CheckCollision(transform.position, (BoxCollider2D)colliders[0], 0);
                p?.OnHit?.Invoke(1);
                if (p != null)
                    Debug.Log(p);
            }

            // �÷��� ���� �������� ��
            if (hits.Length == 0) break;

            transform.position += (Vector3)(dir * move_speed * 2 * Time.deltaTime);
            yield return null;

        } while (true);

        StartCoroutine(animation.AnimEnd());
    }

    protected override bool AttackCheck()
    {
        float distance_y = Mathf.Abs(player.transform.position.y - transform.position.y);

        // �������� ��ġ���� ��� ����
        return distance_y <= 1;
    }
    protected override void MoveToPlayer() { }
}
