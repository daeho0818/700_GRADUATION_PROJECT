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
    /// 벽이 나타날 때까지 돌진하는 공격 패턴
    /// </summary>
    protected override void BaseAttack()
    {
        float dir_x = (player.transform.position - transform.position).normalized.x;
        StartCoroutine(_BaseAttack(dir_x));
    }
    IEnumerator _BaseAttack(float dir_x)
    {
        StopCoroutine(ai_moving);

        RaycastHit2D[] hits;
        Vector2 dir = new Vector2(dir_x, 0);

        yield return new WaitForSeconds(1.5f);
        do
        {
            hits = Physics2D.RaycastAll(transform.position, dir, 2f, LayerMask.GetMask("Wall"));
            Debug.DrawRay(transform.position, dir * 2f, Color.red, 0.1f);

            // 벽을 만났을 때
            if (hits.Length > 0) break;

            hits = Physics2D.RaycastAll((Vector2)transform.position + (dir * 3), Vector2.down, 1f, LayerMask.GetMask("Wall"));
            Debug.DrawRay(transform.position, dir * 2f, Color.red, 0.1f);

            // 플랫폼 끝에 도달했을 때
            if (hits.Length == 0) break;

            transform.Translate(dir * move_speed * 2 * Time.deltaTime);
            yield return null;

        } while (true);

        ai_moving = StartCoroutine(AIMoving());
    }


    protected override void MoveToPlayer() { }
}
