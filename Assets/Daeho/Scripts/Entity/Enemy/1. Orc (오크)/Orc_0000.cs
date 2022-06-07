using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orc_0000 : FlyingObject
{
    [SerializeField] Projectile bullet_prefab;
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

    Vector3 target_fire_position;

    /// <summary>
    /// �÷��̾ ���� ��ź�� �߻��ϴ� �⺻ ���� �Լ�
    /// </summary>
    protected override IEnumerator BaseAttack()
    {
        yield return null;

        target_fire_position = player.transform.position;
        Vector2 dir;
        float value;
        Projectile bullet;

        for (int i = -1; i < 2; i++)
        {
            value = Mathf.Sin(i * Mathf.Deg2Rad) * 50;
            dir = ((target_fire_position + new Vector3(value, value)) - transform.position).normalized;

            bullet = Instantiate(bullet_prefab);
            bullet.transform.position = transform.position;
            bullet.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);
            bullet.fire_direction = dir;
            bullet.move_speed = bullet_speed;
            bullet.SetCollision((p) => { p?.OnHit?.Invoke(1); });
        }
    }

    Vector2 target_move_position;

    /// <summary>
    /// �÷��̾���� �Ÿ��� �����ϸ� �÷��̾ ���� �̵��ϴ� �Լ�
    /// </summary>
    protected override void MoveToPlayer()
    {
        int dir_x = (player.transform.position.x > transform.position.x ? -1 : 1);

        target_move_position = player.transform.position + new Vector3(distance_with_player * dir_x, 0);
        target_move_position.y = transform.position.y;

        FlipSprite();
        transform.position = Vector2.Lerp(transform.position, target_move_position, Time.deltaTime * move_speed);
    }
}
