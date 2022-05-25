using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orc_0002 : GroundObject
{
    [SerializeField] Projectile_Arc bullet_prefab;
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
    /// �÷��̾ ���� �������� �׸��� ��ô���� ������ ���� �Լ�
    /// </summary>
    protected override IEnumerator BaseAttack()
    {
        Projectile_Arc bullet;

        yield return null;

        renderer.flipX = transform.position.x < target_move_position.x;

        bullet = Instantiate(bullet_prefab);
        bullet.transform.position = transform.position;
        bullet.move_speed = bullet_speed;
        bullet.SetArc(player.transform.position);
    }

    Vector2 target_move_position;

    /// <summary>
    /// �÷��̾���� �Ÿ��� �����ϸ� �÷��̾ ���� �̵��ϴ� �Լ�
    /// </summary>
    protected override void MoveToPlayer()
    {
        if (player)
        {
            int dir_x = (player.transform.position.x > transform.position.x ? -1 : 1);

            target_move_position = player.transform.position + new Vector3(distance_with_player * dir_x, 0);
            target_move_position.y = transform.position.y;

            renderer.flipX = transform.position.x < target_move_position.x;
            transform.position = Vector2.Lerp(transform.position, target_move_position, Time.deltaTime * move_speed);
        }
    }
}
