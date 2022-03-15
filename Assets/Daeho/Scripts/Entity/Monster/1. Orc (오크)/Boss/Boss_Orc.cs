using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Orc : Monster
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

        if (Input.GetKeyDown(KeyCode.Space))
            StartCoroutine(AttackPattern1(GameObject.Find("Player").transform.position));

        if (Input.GetKeyDown(KeyCode.RightArrow))
            StartCoroutine(AttackPattern2(10, Vector3.right));

        else if (Input.GetKeyDown(KeyCode.LeftArrow))
            StartCoroutine(AttackPattern2(10, Vector3.left));
    }

    /// <summary>
    /// '추격' 공격 패턴
    /// </summary>
    /// <param name="target">추격 위치</param>
    /// <returns></returns>
    IEnumerator AttackPattern1(Vector2 target)
    {
        while (true)
        {
            yield return null;

        }
    }

    /// <summary>
    /// '돌격' 공격 패턴
    /// </summary>
    /// <param name="move_speed">돌격 속도</param>
    /// <param name="direction">돌격 방향</param>
    /// <returns></returns>
    IEnumerator AttackPattern2(float move_speed, Vector3 direction)
    {
        RaycastHit2D[] hits;
        bool loop;
        Vector2 origin;

        // 공격 대기 시간
        yield return new WaitForSeconds(2);

        do
        {
            origin = transform.position + new Vector3(direction.x * transform.localScale.x / 2, 0) + direction * 2;

            hits = Physics2D.RaycastAll(origin, Vector2.down, 5, LayerMask.GetMask("Platform"));
            Debug.DrawRay(origin, Vector2.down * 5, Color.red, 0.1f);

            loop = hits.Length > 0;

            transform.Translate(direction * move_speed * Time.deltaTime);
            yield return null;
        }
        while (loop);

        colliders[2].offset = new Vector2(Mathf.Abs(colliders[2].offset.x) * direction.x, colliders[2].offset.y);
        colliders[2].enabled = true;

        yield return new WaitForSeconds(0.5f);

        colliders[2].enabled = false;
    }
}
