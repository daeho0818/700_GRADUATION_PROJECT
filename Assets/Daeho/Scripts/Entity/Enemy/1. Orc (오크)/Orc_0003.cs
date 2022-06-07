using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orc_0003 : GroundObject
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

    /// <summary>
    /// ���� ��Ÿ�� ������ �����ϴ� ���� ����
    /// </summary>
    protected override IEnumerator BaseAttack()
    {
        float dir_x;

        dir_x = (player.transform.position - transform.position).normalized.x;
        yield return StartCoroutine(_BaseAttack(dir_x));
    }
    IEnumerator _BaseAttack(float dir_x)
    {
        StopCoroutine(ai_moving);

        RaycastHit2D[] hits;
        Vector2 dir = new Vector2(dir_x, 0);

        FlipSprite();
        do
        {
            hits = Physics2D.RaycastAll(transform.position, dir, 2f, LayerMask.GetMask("Wall"));
            Debug.DrawRay(transform.position, dir * 2f, Color.red, 0.1f);

            // ���� ������ ��
            if (hits.Length > 0) break;

            hits = Physics2D.RaycastAll((Vector2)transform.position + (dir * 1), Vector2.down, 5f, LayerMask.GetMask("Ground"));
            Debug.DrawRay((Vector2)transform.position + (dir * 1), Vector2.down * 5f, Color.red, 0.1f);

            Player p = CheckCollision(transform.position, (BoxCollider2D)colliders[0], 0);
            p?.OnHit?.Invoke(1);

            // �÷��� ���� �������� ��
            if (hits.Length == 0) break;

            transform.Translate(dir * move_speed * 2 * Time.deltaTime);
            yield return null;

        } while (true);

        ai_moving = StartCoroutine(AIMoving());
    }


    protected override void MoveToPlayer() { }
}
