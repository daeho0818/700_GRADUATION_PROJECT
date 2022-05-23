using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Guided : Projectile
{
    public Transform target { get; private set; }

    event System.Action onCollision = null;

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

        if (shooting != null) StopCoroutine(shooting);
        shooting = StartCoroutine(Shooting());
    }

    /// <summary>
    /// �浹 �� ������ ������ �����ϴ� �Լ�
    /// </summary>
    /// <param name="action"></param>
    public void SetCollision(System.Action action) => onCollision = action;

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
            if (Time.time - time < 2 && target)
                fire_direction = (target.position - transform.position).normalized;

            transform.Translate(fire_direction * Time.deltaTime * move_speed);

            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            onCollision?.Invoke();
            Destroy(gameObject);
        }
    }
}
