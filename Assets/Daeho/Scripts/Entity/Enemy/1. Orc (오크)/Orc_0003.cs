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
            hits = Physics2D.RaycastAll(transform.position, dir, 2, LayerMask.NameToLayer("Wall"));

            if (hits.Length > 0) break;

            transform.Translate(dir * move_speed * 2 * Time.deltaTime);
            yield return null;

        } while (true);

        ai_moving = StartCoroutine(AIMoving());
    }


    protected override void MoveToPlayer()
    {
        base.MoveToPlayer();
    }
}
