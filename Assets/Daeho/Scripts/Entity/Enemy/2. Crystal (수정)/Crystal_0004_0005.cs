using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal_0004_0005 : FlyingObject
{
    [SerializeField] Projectile proj_prefab;
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();

        // 정지 오브젝트.
        move_speed = 0;
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override IEnumerator AIMoving()
    {
        yield return null;
    }

    protected override void MoveToPlayer()
    {
    }

    protected override bool AttackCheck()
    {
        float distance = Vector2.Distance(player.transform.position, transform.position);
        return search_distance >= distance;
    }

    protected override void BaseAttack()
    {
        // 자폭 애니메이션

        const int FIRE_COUNT = 8;

        Projectile proj;
        Vector2 fire_direction;
        float rad;

        for (int i = 0; i <= 360; i += 360 / FIRE_COUNT)
        {
            rad = i * Mathf.Deg2Rad;

            fire_direction = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));

            proj = Instantiate(proj_prefab);
            proj.transform.position = transform.position;
            proj.fire_direction = fire_direction;
            proj.SetCollision((p) => { p?.OnHit(1); });
        }

        Destroy(gameObject);
    }
}
