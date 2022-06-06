using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orc_0001 : GroundObject
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

    /// <summary>
    /// 플레이어를 향해 3번 연속 총알을 발사하는 공격 함수
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator BaseAttack()
    {
        WaitForSeconds second = new WaitForSeconds(0.85f);
        Projectile bullet;
        Vector2 direction = (player.transform.position - transform.position).normalized;
        direction.y = 0;

        yield return null;

        renderer.flipX = transform.position.x < player.transform.position.x;

        for (int i = 0; i < 3; i++)
        {
            bullet = Instantiate(bullet_prefab);
            bullet.transform.position = transform.position;
            bullet.fire_direction = direction;
            bullet.move_speed = bullet_speed;
            bullet.GetComponent<SpriteRenderer>().flipX = direction.x < 0;

            yield return second;
        }
    }

    Vector2 target_move_position;

    /// <summary>
    /// 플레이어와의 거리를 유지하며 플레이어를 향해 이동하는 함수
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
