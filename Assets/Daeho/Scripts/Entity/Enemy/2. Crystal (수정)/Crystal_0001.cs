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

    protected override void BaseAttack()
    {
        StartCoroutine(_BaseAttack());
    }

    /// <summary>
    /// 3연타 이동 펀치 (막타는 어퍼컷)
    /// </summary>
    /// <returns></returns>
    IEnumerator _BaseAttack()
    {
        yield return new WaitForSeconds(1f);

        for (float i = 0f; i < 1.5f; i += 0.5f)
        {
            rigid.AddForce((player.transform.position - transform.position).normalized * 5, ForceMode2D.Impulse);

            float count = Time.time;

            BoxCollider2D collider = (BoxCollider2D)colliders[(int)i + 1];
            SetColliderDirection(collider, player.transform.position.x > transform.position.x ? 1 : -1);

            renderer.flipX = player.transform.position.x > transform.position.x;

            Player p;
            while (Time.time - count < 1f)
            {
                p = CheckCollision(transform.position, collider, 0);
                // p?.OnHit();
                yield return null;
            }
        }

        yield return new WaitForSeconds(1f);
    }

    protected override void MoveToPlayer()
    {
    }
}
