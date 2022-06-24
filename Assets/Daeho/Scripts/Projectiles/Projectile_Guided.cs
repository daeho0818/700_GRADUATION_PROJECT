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
    /// ���� ����� �����ϴ� �Լ�
    /// </summary>
    /// <param name="target">���� ���</param>
    public void SetTarget(Transform target)
    {
        this.target = target;

        fire_direction = target.position.x > transform.position.x ? Vector2.right : Vector2.left;

        if (shooting != null) StopCoroutine(shooting);
        shooting = StartCoroutine(Shooting());
    }

    Coroutine shooting = null;
    /// <summary>
    /// ���� ����� ���� �Ѿ��� �߻��ϴ� �Լ�
    /// </summary>
    /// <returns></returns>
    IEnumerator Shooting()
    {
        float time = Time.time;

        while (true)
        {
            // ���� �ð��� ������ ������ Ÿ���� ����
            if (Time.time - time > 1 && Time.time - time < 3 && target)
                fire_direction = (target.position - transform.position).normalized;

            transform.position += (Vector3)(fire_direction * Time.deltaTime * move_speed);
            transform.rotation = Quaternion.Euler(0, 0, 180 + (Mathf.Atan2(fire_direction.y, fire_direction.x) * Mathf.Rad2Deg));

            yield return null;
        }
    }
}
