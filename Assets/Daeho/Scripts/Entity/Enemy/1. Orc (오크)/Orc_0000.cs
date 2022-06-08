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

        bool player_right = player.transform.position.x > transform.position.x;
        bool player_down = player.transform.position.y < transform.position.y;

        // ��ź ���� (�� �� ����)
        Vector2 dir1 = (player.transform.position - transform.position).normalized;
        Vector3 point1 = player.transform.position + new Vector3(player_right ? 1 : -1, player_down ? 1 : -1);
        Vector3 point2 = player.transform.position + new Vector3(player_right ? -1 : 1, player_down ? -1 : 1);
        var dirs = new Vector2[3] { dir1, (point1 - transform.position).normalized, (point2 - transform.position).normalized };

        Projectile bullet;

        FlipSprite();

        foreach (var dir in dirs)
        {
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
