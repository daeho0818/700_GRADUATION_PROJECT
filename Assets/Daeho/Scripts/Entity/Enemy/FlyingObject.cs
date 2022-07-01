using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingObject : Enemy
{
    protected Coroutine fly_animation;
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();

        fly_animation = StartCoroutine(FlyAnimation());
    }

    protected override void Update()
    {
        base.Update();
    }

    /// <summary>
    /// Flying Object 부유 애니메이션
    /// </summary>
    /// <returns></returns>
    protected virtual IEnumerator FlyAnimation()
    {
        float value = Random.Range(0, 2);
        value *= 180;

        while (true)
        {
            transform.localPosition += new Vector3(0, Mathf.Sin(value += 0.7f * Mathf.Deg2Rad) * 0.005f);
            yield return null;
        }
    }

    protected override void KnockBack(int damage)
    {
        Vector2 dir;
        dir.x = player.transform.position.x > transform.position.x ? -1 : 1;
        dir.y = 0;

        rigid.AddForce(dir * (damage * 0.5f), ForceMode2D.Impulse);
    }
}