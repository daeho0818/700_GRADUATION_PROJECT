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
    /// 3연타 이동 펀치 (막타는 어퍼컷)
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator BaseAttack()
    {
        float delay = animation.GetState().delay;

        Vector2 force;
        float count;
        BoxCollider2D collider;
        Player p;

        super_armor = true;

        for (int i = 0; i < 3; i++)
        {
            force = new Vector2(player.transform.position.x > transform.position.x ? 1 : -1, 0) * 5;
            if (i == 2) force = new Vector2(0, 5);

            rigid.AddForce(force, ForceMode2D.Impulse);
            FlipSprite();

            count = Time.time;

            collider = (BoxCollider2D)colliders[i];

            while (Time.time - count < delay)
            {
                p = CheckCollision(transform.position, collider, 0);
                p?.OnHit?.Invoke(1);
                yield return null;
            }
        }

        super_armor = false;
    }

    protected override void MoveToPlayer()
    {
    }
}
