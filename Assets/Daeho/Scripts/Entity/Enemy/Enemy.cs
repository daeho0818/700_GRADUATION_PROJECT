using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EInfo = EnemyInformation;
using AIInfo = AIInformation;

[System.Serializable]
public struct EnemyInformation
{
    // Enemy 애니메이션 관리자
    public EnemyAnimation animation;
    // 공격 패턴 유무 여부
    public bool attack_check;
    // 공격 전/후 딜레이
    public float attack_beforeDelay;
    public float attack_afterDelay;
    // 공격 쿨타임
    public float attack_coolTime;
    // 총알 속도
    public float bullet_speed;
}

[System.Serializable]
public struct AIInformation
{
    // 플레이어 탐색 여부
    public bool search_player;
    // AI 이동 반경
    public float ai_moving_range;
    // 이동 간 최소 대기시간
    public float delay_min;
    // 이동 간 최대 대기시간
    public float delay_max;
    [Tooltip("플레이어를 탐색하는 범위 (거리)")]
    public float search_distance;
    [Tooltip("플레이어 추격이 해제되는 거리")]
    public float unSearch_distance;
    [Tooltip("추격 중 플레이어와 유지할 거리")]
    public float distance_with_player;
}

public class Enemy : Entity
{
    [Header("Enemy Information")]
    [SerializeField] EInfo enemy;
    #region Properties
    /// <summary>
    /// Enemy 애니메이션 관리자
    /// </summary>
    public new EnemyAnimation animation { get => enemy.animation; set => enemy.animation = value; }
    /// <summary>
    /// 공격 패턴 유무 여부
    /// </summary>
    public bool attack_check { get => enemy.attack_check; set => enemy.attack_check = value; }
    /// <summary>
    /// 공격 전 딜레이
    /// </summary>
    public float attack_beforeDelay { get => enemy.attack_beforeDelay; set => enemy.attack_beforeDelay = value; }
    /// <summary>
    /// 공격 후 딜레이
    /// </summary>
    public float attack_afterDelay { get => enemy.attack_afterDelay; set => enemy.attack_afterDelay = value; }
    /// <summary>
    /// 공격 쿨타임
    /// </summary>
    public float attack_coolTime { get => enemy.attack_coolTime; set => enemy.attack_coolTime = value; }
    /// <summary>
    /// 총알 속도
    /// </summary>
    public float bullet_speed { get => enemy.bullet_speed; set => enemy.bullet_speed = value; }
    #endregion

    [Header("AI Moving Information")]
    [SerializeField] protected AIInfo ai;
    #region Properties
    /// <summary>
    /// 플레이어 탐색 여부
    /// </summary>
    public bool search_player { get => ai.search_player; set => ai.search_player = value; }
    /// <summary>
    /// AI 이동 반경
    /// </summary>
    public float ai_moving_range { get => ai.ai_moving_range; set => ai.ai_moving_range = value; }
    /// <summary>
    /// 이동 간 최소 대기시간
    /// </summary>
    public float delay_min { get => ai.delay_min; set => ai.delay_min = value; }
    /// <summary>
    /// 이동 간 최대 대기시간
    /// </summary>
    public float delay_max { get => ai.delay_max; set => ai.delay_max = value; }
    /// <summary>
    /// 플레이어를 탐색하는 범위 (거리)
    /// </summary>
    public float search_distance { get => ai.search_distance; set => ai.search_distance = value; }
    /// <summary>
    /// 플레이어 추격이 해제되는 거리
    /// </summary>
    public float unSearch_distance { get => ai.unSearch_distance; set => ai.unSearch_distance = value; }
    // 추격 중 플레이어와 유지할 거리
    /// <summary>
    /// 추격 중 플레이어와 유지할 거리
    /// </summary>
    public float distance_with_player { get => ai.distance_with_player; set => ai.distance_with_player = value; }
    #endregion

    protected Coroutine ai_moving = null;

    // 플레이어 발견 여부
    public bool find_player { get; set; }

    protected Player player;
    protected override void Awake()
    {
    }

    protected override void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        renderer = GetComponent<SpriteRenderer>();
        animation = GetComponent<EnemyAnimation>();

        player = FindObjectOfType<Player>();

        ai_moving = StartCoroutine(AIMoving());

        OnHit += (int damage) =>
        {
            animation.SetState("Hit");
            hp -= damage;
        };
        OnHit += KnockBack;

        OnDestroy += () => animation.SetState("Dead");
        OnDestroy += () => Destroy(gameObject, 1);
    }

    protected override void Update()
    {
        if (IsDestroy)
            OnDestroy?.Invoke();

        if (search_player && movable)
        {
            bool tempFInd = FindPlayer();
            if (tempFInd)
            {
                MoveToPlayer();

                // 플레이어를 향해 움직임 : 초록색
                renderer.color = Color.green;

                animation.SetState("Walk");
            }
            find_player = tempFInd;
        }

        if (!attack_check && player != null)
        {
            bool tempAtk = AttackCheck();
            if (tempAtk)
            {
                StartCoroutine(Attack());
                movable = false;
            }
            attack_check = tempAtk;
        }
    }

    protected virtual void KnockBack(int damage)
    {
        Vector2 dir;
        dir.x = player.transform.position.x > transform.position.x ? -1 : 1;
        dir.y = 1;

        rigid.AddForce(dir * (damage * 2), ForceMode2D.Impulse);
    }

    /// <summary>
    /// 플레이어를 탐색하는 함수
    /// </summary>
    /// <returns>플레이어 탐색 여부</returns>
    protected virtual bool FindPlayer()
    {
        float distance = Vector2.Distance(player.transform.position, transform.position);

        if (distance > ai.search_distance && find_player)
            return distance <= ai.unSearch_distance;

        return distance <= ai.search_distance;
    }

    /// <summary>
    /// 공격 가능한 상황인지 확인하는 함수
    /// </summary>
    /// <returns>공격 가능 여부</returns>
    protected virtual bool AttackCheck() => find_player;

    Timer coolTime_timer = new Timer();
    Timer delay_timer = new Timer();
    /// <summary>
    /// 공격 전/후 딜레이, 쿨타임 등 공격 매커니즘 구현 함수
    /// </summary>
    /// <returns></returns>
    private IEnumerator Attack()
    {
        animation.SetState("Attack");

        #region 애니메이션 프레임 다 들어오면 지워버릴 것.

        // 대기 시간 : 노란색
        renderer.color = Color.yellow;

        yield return new WaitForSeconds(attack_beforeDelay);

        // 공격 중 : 빨간색
        renderer.color = Color.red;

        yield return StartCoroutine(BaseAttack());

        // 대기 시간 : 노란색
        renderer.color = Color.yellow;

        yield return new WaitForSeconds(attack_afterDelay);

        if (!delay_timer.Processing())
            delay_timer.TimerStart(this, attack_afterDelay, () => { movable = true; });

        #endregion

        // 기본 상태
        renderer.color = Color.white;
        animation.SetState("Idle");

        if (!coolTime_timer.Processing())
            coolTime_timer.TimerStart(this, attack_coolTime,
                () =>
                {
                    animation.SetState("Idle");
                    attack_check = false;
                });
    }
    /// <summary>
    /// 기본적인 공격을 구현하는 가상함수
    /// </summary>
    protected virtual IEnumerator BaseAttack() { yield return null; }

    /// <summary>
    /// 플레이어를 탐색하지 못할 동안의 AI 기반 움직임을 구현한 함수
    /// </summary>
    /// <returns></returns>
    protected virtual IEnumerator AIMoving()
    {
        Vector3 target;
        Vector3 vec;
        RaycastHit2D[] hits;

        do
            yield return null;
        while (rigid.velocity.y != 0);

        target = transform.position + new Vector3(Random.Range(-ai_moving_range, ai_moving_range), 0);

        while (true)
        {
            yield return null;
            if (find_player || !movable) continue;

            vec = ((target - transform.position).normalized * move_speed * Time.deltaTime);
            transform.Translate(vec);
            FlipSprite();

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

    /// <summary>
    /// Collider 방향 전환
    /// </summary>
    /// <param name="collider"></param>
    /// <param name="dir_x"></param>
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

    /// <summary>
    /// 플레이어를 기준으로 캐릭터 좌 / 우 회전하는 함수
    /// </summary>
    protected void FlipSprite()
    {
        Quaternion rot = transform.rotation;

        if (player.transform.position.x > transform.position.x)
            transform.rotation = Quaternion.Euler(rot.x, 180, rot.y);
        else
            transform.rotation = Quaternion.Euler(rot.x, 0, rot.y);
    }
}
