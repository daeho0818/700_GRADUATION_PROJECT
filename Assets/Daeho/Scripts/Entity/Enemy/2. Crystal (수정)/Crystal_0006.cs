using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal_0006 : GroundObject
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
    /// �÷��̾ ���� �����ϴ� ���� ����
    /// </summary>
    /// <returns></returns>
    IEnumerator AttackPattern1()
    {
        Vector2 dir = player.transform.position.x > transform.position.x ? Vector2.right : Vector2.left;
        RaycastHit2D[] hits;
        do
        {
            yield return null;
            hits = Physics2D.RaycastAll(transform.position, dir, 3, LayerMask.GetMask("Wall"));
            transform.Translate(dir * move_speed * 2 * Time.deltaTime);
        } while (hits.Length > 0);

        yield return new WaitForSeconds(2);
    }

    /// <summary>
    /// ������ ���� ����
    /// </summary>
    /// <returns></returns>
    IEnumerator AttackPattern2()
    {
        yield return null;
    }

    /// <summary>
    /// ���������� ���� ����
    /// </summary>
    /// <returns></returns>
    IEnumerator AttackPattern3()
    {
        yield return null;
    }
}
