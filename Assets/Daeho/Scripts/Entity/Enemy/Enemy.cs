using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    [Header("Enemy Information")]
    [SerializeField] protected bool attack_check;
    [SerializeField] protected float attack_coolTime;
    [SerializeField] protected float cur_attack_coolTime;
    [SerializeField] protected float bullet_speed;

    [Header("AI Moving Information")]
    [SerializeField] protected bool search_player = true;
    [SerializeField] protected float ai_moving_range;
    [SerializeField] protected float delay_min;
    [SerializeField] protected float delay_max;
    [Tooltip("플레이어를 탐색하는 범위 (거리)")]
    [SerializeField] protected float search_distance;
    [Tooltip("플레이어 추격이 해제되는 거리")]
    [SerializeField] protected float unSearch_distance;
    [Tooltip("추격 중 플레이어와 유지할 거리")]
    [SerializeField] protected float distance_with_player;
    protected Coroutine ai_moving = null;

    public bool find_player;

    protected Player player;
    protected override void Awake()
    {
    }

    protected override void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        renderer = GetComponent<SpriteRenderer>();

        player = FindObjectOfType<Player>();

        ai_moving = StartCoroutine(AIMoving());
    }

    protected override void Update()
    {
        OnDestroy?.Invoke();

        if (search_player && player)
        {
            bool tempFInd = FindPlayer();
            if (tempFInd) MoveToPlayer();
            find_player = tempFInd;
        }

        if (!attack_check && player)
        {
            bool tempAtk = AttackCheck();
            if (tempAtk) BaseAttack();
            attack_check = tempAtk;
            cur_attack_coolTime = 0;
        }
        else
        {
            if (cur_attack_coolTime >= attack_coolTime)
            {
                attack_check = false;
                cur_attack_coolTime = 0;
            }

            else
                cur_attack_coolTime += Time.deltaTime;
        }
    }

    /// <summary>
    /// 플레이어를 탐색하는 함수
    /// </summary>
    /// <returns>플레이어 탐색 여부</returns>
    protected virtual bool FindPlayer()
    {
        float distance = Vector2.Distance(player.transform.position, transform.position);

        if (distance > search_distance && find_player)
            return distance <= unSearch_distance;

        return distance <= search_distance;
    }
    /// <summary>
    /// 공격 가능한 상황인지 확인하는 함수
    /// </summary>
    /// <returns>공격 가능 여부</returns>
    protected virtual bool AttackCheck() => find_player;

    /// <summary>
    /// 기본적인 공격을 구현하는 가상함수
    /// </summary>
    protected virtual void BaseAttack() { }

    /// <summary>
    /// 플레이어를 탐색하지 못할 동안의 AI 기반 움직임을 구현한 함수
    /// </summary>
    /// <returns></returns>
    protected virtual IEnumerator AIMoving()
    {
        Vector3 target = transform.position + new Vector3(Random.Range(-ai_moving_range, ai_moving_range), 0);
        Vector3 vec;
        RaycastHit2D[] hits;

        while (true)
        {
            yield return null;
            if (find_player) continue;

            vec = ((target - transform.position).normalized * move_speed * Time.deltaTime);
            transform.Translate(vec);

            hits = Physics2D.RaycastAll(transform.position, (target - transform.position).normalized, 2, LayerMask.GetMask("Wall"));

            // 벽에 부딛혔거나, 타겟 위치로의 이동이 완료됐을 때
            if (Vector2.Distance(target, transform.position) <= vec.x || hits.Length > 0)
            {
                yield return new WaitForSeconds(Random.Range(delay_min, delay_max));

                target = transform.position + new Vector3(Random.Range(-ai_moving_range, ai_moving_range), 0);
            }
        }
    }

    /// <summary>
    /// 플레이어 발견 시 플레이어를 향해 이동
    /// </summary>
    protected virtual void MoveToPlayer()
    {
        transform.Translate(move_speed * ((new Vector2(player.transform.position.x, 0) - new Vector2(transform.position.x, 0)).normalized) * Time.deltaTime);
        renderer.flipX = player.transform.position.x > transform.position.x;
    }

    protected void SetColliderDirection(Collider2D collider, float dir_x)
    {
        Vector2 offset = collider.offset;

        if (offset.x > 0)
            collider.offset = offset * new Vector2(dir_x, 1);
        else
            collider.offset = offset * new Vector2(-dir_x, 1);
    }

    /// <summary>
    /// BoxCollider와 Player의 충돌을 확인하는 함수
    /// </summary>
    /// <param name="position">BoxCollider2D 위치</param>
    /// <param name="collider">BoxCollider2D</param>
    /// <param name="rot">회전</param>
    /// <returns>충돌했다면 player, 아니라면 null</returns>
    protected Player CheckCollision(Vector2 position, BoxCollider2D collider, float rot)
    {
        var hits = Physics2D.BoxCastAll(position + collider.offset, collider.size, rot, Vector2.zero, 0, LayerMask.GetMask("Entity"));

        foreach (var hit in hits)
        {
            if (hit.transform.TryGetComponent(out Player p))
                return p;
        }

        return null;
    }

    /// <summary>
    /// CapshuleCollider와 Player의 충돌을 확인하는 함수
    /// </summary>
    /// <param name="position">CapsuleCollider2D 위치</param>
    /// <param name="collider">CapsuleCollider2D</param>
    /// <param name="capshule_dir">CapsuleCollider2D 방향</param>
    /// <param name="rot">회전</param>
    /// <returns>충돌했다면 player, 아니라면 null</returns>
    protected Player CheckCollision(Vector2 position, CapsuleCollider2D collider, CapsuleDirection2D capshule_dir, float rot)
    {
        var hits = Physics2D.CapsuleCastAll(position + collider.offset, collider.size, capshule_dir, 0, Vector2.zero, 0, LayerMask.GetMask("Entity"));

        foreach (var hit in hits)
        {
            if (TryGetComponent(out Player p))
                return p;
        }

        return null;
    }

    /// <summary>
    /// CircleCollider2D와 Player의 충돌을 확인하는 함수
    /// </summary>
    /// <param name="position">CircleCollider2D 위치</param>
    /// <param name="collider">CircleCollider2D</param>
    /// <returns>충돌했다면 player, 아니라면 null</returns>
    protected Player CheckCollision(Vector2 position, CircleCollider2D collider)
    {
        var hits = Physics2D.CircleCastAll(position + collider.offset, collider.radius, Vector2.zero, 0, LayerMask.GetMask("Entity"));

        foreach (var hit in hits)
        {
            if (TryGetComponent(out Player p))
                return p;
        }

        return null;
    }
}
