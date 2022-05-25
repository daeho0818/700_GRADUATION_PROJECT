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
    /// 플레이어와의 거리를 유지하며 플레이어를 향해 이동하는 함수
    /// </summary>
    protected override void MoveToPlayer()
    {
        Debug.Log("응애");

        int dir_x = (player.transform.position.x > transform.position.x ? -1 : 1);

        target_move_position = player.transform.position + new Vector3(distance_with_player * dir_x, 0);
        target_move_position.y = transform.position.y;

        renderer.flipX = transform.position.x < player.transform.position.x;
        transform.position = Vector2.Lerp(transform.position, target_move_position, Time.deltaTime * move_speed);
    }

    /// <summary>
    /// 플레이어를 향해 유도탄 발사
    /// </summary>
    protected override void BaseAttack()
    {
        Projectile_Guided proj = Instantiate(projectile_prefab);
        proj.transform.position = transform.position;
        proj.move_speed = bullet_speed;
        proj.SetTarget(player.transform);
    }
}