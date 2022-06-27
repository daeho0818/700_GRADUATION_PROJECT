using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal_0002_0003 : FlyingObject
{
    [SerializeField] Projectile_Guided projectile_prefab;

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

    Vector2 target_move_position;
    /// <summary>
    /// �÷��̾���� �Ÿ��� �����ϸ� �÷��̾ ���� �̵��ϴ� �Լ�
    /// </summary>
    protected override void MoveToPlayer()
    {
        int dir_x = (player.transform.position.x > transform.position.x ? -1 : 1);

        target_move_position = player.transform.position + new Vector3(distance_with_player * dir_x, distance_with_player / 2);

        if (Physics2D.RaycastAll(transform.position, new Vector2(dir_x, 0), 2, LayerMask.GetMask("Wall")).Length > 0)
        {
            return;
        }

        FlipSprite();
        transform.position = Vector2.Lerp(transform.position, target_move_position, Time.deltaTime * move_speed);
    }

    /// <summary>
    /// �÷��̾ ���� ����ź �߻�
    /// </summary>
    protected override IEnumerator BaseAttack()
    {
        Projectile_Guided proj;

        yield return null;

        proj = Instantiate(projectile_prefab);
        proj.transform.position = transform.position;
        proj.move_speed = bullet_speed;
        proj.SetTarget(player.transform);
        proj.SetCollision((p) => { p?.OnHit?.Invoke(1); });
    }
}