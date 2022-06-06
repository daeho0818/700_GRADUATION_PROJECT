using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Guided : Projectile
{
    public Transform target { get; private set; }


    protected override void Update()
    {
    }

    /// <summary>
    /// 유도 대상을 설정하는 함수
    /// </summary>
    /// <param name="target">유도 대상</param>
    public void SetTarget(Transform target)
    {
        this.target = target;

        if (shooting != null) StopCoroutine(shooting);
        shooting = StartCoroutine(Shooting());
    }

    Coroutine shooting = null;
    /// <summary>
    /// 유도 대상을 따라 총알을 발사하는 함수
    /// </summary>
    /// <returns></returns>
    IEnumerator Shooting()
    {
        float time = Time.time;

        while (true)
        {
            if (Time.time - time < 2 && target)
                fire_direction = (target.position - transform.position).normalized;

            transform.position += (Vector3)(fire_direction * Time.deltaTime * move_speed);
            transform.rotation = Quaternion.Euler(0, 0, 180 + (Mathf.Atan2(fire_direction.y, fire_direction.x) * Mathf.Rad2Deg));

            yield return null;
        }
    }
}
