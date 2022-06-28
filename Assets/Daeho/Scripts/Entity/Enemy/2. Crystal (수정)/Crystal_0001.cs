using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal_0001 : GroundObject
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
    /// 3��Ÿ �̵� ��ġ (��Ÿ�� ������)
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator BaseAttack()
    {
        float delay;
        EnemyAnimation.AnimState state = animation.GetState();

        Vector2 force;
        float count;
        BoxCollider2D collider;
        Player p;

        super_armor = true;

        for (int i = 0; i < 3; i++)
        {
            force = new Vector2(player.transform.position.x > transform.position.x ? 1 : -1, 0) * 5;

            // 3Ÿ�� ������
            if (i == 2) force = new Vector2(0, 5);

            rigid.velocity = Vector2.zero;

            rigid.AddForce(force, ForceMode2D.Impulse);
            FlipSprite();

            count = Time.time;

            collider = (BoxCollider2D)colliders[i];

            delay = state.GetDelay();

            // ���� ���ݱ��� ������
            while (Time.time - count < delay)
            {
                p = CheckCollision(transform.position, collider, 0);
                p?.OnHit?.Invoke(1);
                yield return null;
            }
        }

        // �ٴڿ� ������ ������ ���
        while (rigid.velocity.y != 0)
            yield return null;

        super_armor = false;
        animation.AnimEnd();
    }

    protected override IEnumerator AIMoving()
    {
        return base.AIMoving();
    }
}
