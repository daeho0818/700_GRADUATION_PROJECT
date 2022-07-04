using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//사랑해요 사랑해요
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

        attack = StartCoroutine(Attack());
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override IEnumerator AIMoving()
    {
        yield return null;
    }

    protected override bool FindPlayer() => false;

    protected override void MoveToPlayer()
    {
    }

    protected override bool AttackCheck()
    {
        float distance = Vector2.Distance(player.transform.position, transform.position);
        return attack_distance >= distance;
    }

    [SerializeField] int bullet_damage = 1;
    Coroutine attack = null;
    /// <summary>
    /// 사방으로 가시를 날리며 대기하는 패턴  
    /// </summary>
    /// <returns></returns>
    IEnumerator Attack()
    {
        yield return new WaitForSeconds(1);

        float deg;
        Vector2 dir;
        Projectile proj;

        while (true)
        {
            deg = Random.Range(0, 360);
            deg = deg * Mathf.Deg2Rad;

            dir = new Vector2(Mathf.Cos(deg), Mathf.Sin(deg));

            proj = Instantiate(proj_prefab);
            proj.transform.position = transform.position;
            proj.move_speed = bullet_speed;
            proj.fire_direction = dir;
            proj.SetCollision((p) => { p?.OnHit?.Invoke(bullet_damage); });

            yield return new WaitForSeconds(1);
        }
    }

    /// <summary>
    /// 자폭 공격 패턴
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator BaseAttack()
    {
        StopCoroutine(attack);
        StopCoroutine(fly_animation);

        const int FIRE_COUNT = 8;

        Projectile proj;
        Vector2 fire_direction;
        float rad;

        float delay = animation.GetState().GetDelay();
        float time = Time.time;

        float rand_x;
        float rand_y;
        float force = 1;
        do
        {
            rand_x = Random.Range(-0.1f, 0.1f) * force;
            rand_y = Random.Range(-0.1f, 0.1f) * force;

            force += 0.1f;

            transform.position += new Vector3(rand_x, rand_y) * Time.deltaTime;
            yield return null;
            transform.position -= new Vector3(rand_x, rand_y) * Time.deltaTime;
        }
        while (Time.time - time <= delay);

        attack_particles[0].Play();
        for (int i = 0; i <= 360; i += 360 / FIRE_COUNT)
        {
            rad = i * Mathf.Deg2Rad;

            fire_direction = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));

            proj = Instantiate(proj_prefab);
            proj.transform.position = transform.position;
            proj.fire_direction = fire_direction;
            proj.move_speed = bullet_speed;
            proj.SetCollision((p) => { p?.OnHit?.Invoke(bullet_damage); });
        }

        hp = 0;
    }
}
