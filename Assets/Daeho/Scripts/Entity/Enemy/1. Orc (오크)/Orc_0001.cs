using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orc_0001 : GroundObject
{
    [SerializeField] Projectile bullet_prefab;
    [SerializeField] Transform shoot_position;
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

    int loop_count = 0;
    /// <summary>
    /// �÷��̾ ���� 3�� ���� �Ѿ��� �߻��ϴ� ���� �Լ�
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator BaseAttack()
    {
        System.Action action;

        // 3�� �߻縦 ���� ����
        if (++loop_count <= 2)
            action = () => ChangeState("Attack");
        else
        {
            action = () => ChangeState("Idle");
            loop_count = 0;
        }

        EnemyAnimation.AnimState state = animation.GetState();
        state.frames_actions[state.frames_actions.Length - 1] = action;

        WaitForSeconds second = new WaitForSeconds(0.85f);
        Projectile bullet;
        Vector2 direction = (player.transform.position - transform.position).normalized;
        direction.y = 0;

        yield return null;

        bullet = Instantiate(bullet_prefab);
        bullet.transform.position = shoot_position.position;
        bullet.fire_direction = direction;
        bullet.move_speed = bullet_speed;
        bullet.GetComponent<SpriteRenderer>().flipX = direction.x > 0;
        bullet.SetCollision((p) => { p.OnHit?.Invoke(1); });
    }

    protected override bool AttackCheck()
    {
        float y = Mathf.Abs(transform.position.y - transform.position.y);
        float dis = Vector2.Distance(transform.position, player.transform.position);

        // �÷��̾ �������� ���� �Ÿ� ���Ͽ� ���� ���
        return y <= 1 && dis <= attack_distance;
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

            FlipSprite();

            Vector2 vec = Vector2.Lerp(transform.position, target_move_position, Time.deltaTime * move_speed);

            // ���Ÿ� ���� �÷������� �ȶ������� ����
            if (long_attack && Physics2D.RaycastAll(vec, Vector2.down, 2, LayerMask.GetMask("Ground")).Length == 0) return;

            transform.position = vec;
        }
    }
}
