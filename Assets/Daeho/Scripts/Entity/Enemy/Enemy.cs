using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EInfo = EnemyInformation;
using AIInfo = AIInformation;

[System.Serializable]
public struct EnemyInformation
{
    [Tooltip("Enemy 애니메이션 관리자")]
    public EnemyAnimation animation;
    [Tooltip("공격 패턴 유무 여부")]
    public bool attack_check;
    [Tooltip("애니메이션 공격 해당 프레임")]
    public int attack_frame;
    [Tooltip("공격 대기 시간")]
    public int attack_coolTime;
    [Tooltip("총알 속도")]
    public float bullet_speed;
    [Tooltip("공격 범위")]
    public float attack_distance;
    [Tooltip("원거리 공격 여부")]
    public bool long_attack;
    [Tooltip("피격 시 넉백 여부")]
    public bool super_armor;
}

[System.Serializable]
public struct AIInformation
{
    [Tooltip("플레이어 탐색 여부")]
    public bool search_player;
    [Tooltip("AI 이동 반경")]
    public float ai_moving_range;
    [Tooltip("이동 간 최소 대기시간")]
    public float delay_min;
    [Tooltip("이동 간 최대 대기시간")]
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
    #region Enemy States
    abstract class EnemyState
    {
        protected Enemy enemy = null;
        protected Entity player = null;

        abstract public void Update();
        abstract public void Release();
    }

    class IdleState : EnemyState
    {
        public IdleState(Enemy enemy)
        {
            this.enemy = enemy;
            player = enemy.player;

            enemy.animation.SetState("Idle");

            enemy.StartCoroutine(ChangeToWalk());
        }

        public override void Update()
        {
        }

        public override void Release()
        {
            enemy.CancelInvoke(nameof(ChangeToWalk));
        }

        IEnumerator ChangeToWalk()
        {
            yield return new WaitForSeconds(Random.Range(enemy.delay_min, enemy.delay_max));

            enemy.ChangeState("Walk");
        }
    }

    class WalkState : EnemyState
    {
        Coroutine walk_process = null;
        Coroutine ai_moving = null;

        Timer attack_coolTimer = new Timer();
        bool attackable = false;
        public WalkState(Enemy enemy)
        {
            this.enemy = enemy;
            player = enemy.player;

            attack_coolTimer.TimerStart(enemy, enemy.attack_coolTime, () => { attackable = true; });

            enemy.animation.SetState("Walk");

            walk_process = enemy.StartCoroutine(Walking());
        }

        public override void Update()
        {
            // 플레이어 탐색과 이동 가능한 경우
            if (enemy.search_player && enemy.movable)
            {
                // 플레이어를 놓쳤을 경우
                if (enemy.find_player == true && enemy.FindPlayer() == false)
                {
                    enemy.find_player = false;

                    enemy.ChangeState("Idle");
                }

                // 플레이어를 발견했을 경우
                else if (enemy.FindPlayer() == true)
                {
                    enemy.find_player = true;

                    StopWalking();

                    enemy.MoveToPlayer();
                }
            }

            // 플레이어를 공격 가능한 경우
            if (enemy.AttackCheck() && attackable == true)
            {
                enemy.ChangeState("Attack");
            }
        }

        public override void Release()
        {
            StopWalking();
        }

        /// <summary>
        /// AI 기반 움직임을 멈추는 함수
        /// </summary>
        void StopWalking()
        {
            if (walk_process != null)
            {
                enemy.StopCoroutine(walk_process);
            }

            if (ai_moving != null)
            {
                enemy.StopCoroutine(ai_moving);
            }
        }

        /// <summary>
        /// AI 기반 움직임 프로세스를 관리하는 함수
        /// </summary>
        /// <returns></returns>
        IEnumerator Walking()
        {
            ai_moving = enemy.StartCoroutine(enemy.AIMoving());
            yield return ai_moving;

            // AI 기반 움직임을 마쳤을 경우 대기 상태
            enemy.ChangeState("Idle");
        }
    }

    class AttackState : EnemyState
    {
        Coroutine attack = null;
        public AttackState(Enemy enemy)
        {
            this.enemy = enemy;
            player = enemy.player;

            enemy.animation.SetState("Attack");

            enemy.FlipSprite();

            EnemyAnimation.AnimState state = enemy.animation.GetState();
            System.Action attack = () => { this.attack = enemy.StartCoroutine(enemy.BaseAttack()); };

            if (state.frames_actions.Length > 0)
            {
                state.frames_actions[enemy.enemy.attack_frame] = attack;
                state.frames_actions[state.frames_actions.Length - 1] = () => enemy.ChangeState("Idle");
            }
        }

        public override void Update()
        {
        }

        public override void Release()
        {
            StopAttack();
        }

        /// <summary>
        /// 공격 상태를 멈추는 함수
        /// </summary>
        void StopAttack()
        {
            if (attack != null)
            {
                enemy.StopCoroutine(attack);
            }
        }
    }

    class HitState : EnemyState
    {
        public HitState(Enemy enemy)
        {
            this.enemy = enemy;
            player = enemy.player;

            enemy.animation.SetState("Hit");

            EnemyAnimation.AnimState state = enemy.animation.GetState();
            state.frames_actions[state.frames_actions.Length - 1] = () => enemy.ChangeState("Idle");
        }

        public override void Update()
        {
        }

        public override void Release()
        {
        }
    }

    class DeadState : EnemyState
    {
        public DeadState(Enemy enemy)
        {
            this.enemy = enemy;
            player = enemy.player;

            enemy.animation.SetState("Dead");

            EnemyAnimation.AnimState state = enemy.animation.GetState();

            state.frames_actions[state.frames_actions.Length - 1] = enemy.Release;
        }

        public override void Update()
        {
        }

        public override void Release()
        {
        }
    }

    EnemyState enemy_state = null;

    protected void ChangeState(string state)
    {
        switch (state)
        {
            case string s when nameof(IdleState).Contains(s):
                enemy_state?.Release();
                enemy_state = new IdleState(this);
                break;
            case string s when nameof(WalkState).Contains(s):
                enemy_state?.Release();
                enemy_state = new WalkState(this);
                break;
            case string s when nameof(AttackState).Contains(s):
                enemy_state?.Release();
                enemy_state = new AttackState(this);
                break;
            case string s when nameof(HitState).Contains(s):
                enemy_state?.Release();
                enemy_state = new HitState(this);
                break;
            case string s when nameof(DeadState).Contains(s):
                enemy_state?.Release();
                enemy_state = new DeadState(this);
                break;
            default:
                Debug.Assert(false);
                return;
        }
    }
    #endregion

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
    /// 애니메이션 공격 해당 프레임
    /// </summary>
    public int attack_frame { get => enemy.attack_frame; set => enemy.attack_frame = value; }
    /// <summary>
    /// 공격 대기 시간
    /// </summary>
    public int attack_coolTime { get => enemy.attack_coolTime; set => enemy.attack_coolTime = value; }
    /// <summary>
    /// 총알 속도
    /// </summary>
    public float bullet_speed { get => enemy.bullet_speed; set => enemy.bullet_speed = value; }
    /// <summary>
    /// 공격 범위
    /// </summary>
    public float attack_distance { get => enemy.attack_distance; set => enemy.attack_distance = value; }
    /// <summary>
    /// 원거리 공격 여부
    /// </summary>
    public bool long_attack { get => enemy.long_attack; set => enemy.long_attack = value; }
    /// <summary>
    /// 피격 시 넉백 여부
    /// </summary>
    public bool super_armor { get => enemy.super_armor; set => enemy.super_armor = value; }
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

    /// <summary>
    /// 플레이어 발견 여부
    /// </summary>
    public bool find_player { get; set; }

    protected Player player;

    [Space(10)]
    [SerializeField] string enemyStateName;
    protected override void Awake()
    {
        hp = max_hp;
    }

    protected override void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        renderer = GetComponent<SpriteRenderer>();
        animation = GetComponent<EnemyAnimation>();

        player = FindObjectOfType<Player>();

        OnHit += KnockBack;
        OnHit += (int damage) =>
        {
            if (super_armor == false)
            {
                ChangeState("Hit");
            }

            hp -= damage;
        };
        OnHit += (d) => GameManager.Instance.PrintDamage(d, transform.position, Color.yellow);

        OnDestroy += () => ChangeState("Dead");
        // 코인 떨구는 내용 OnDestroy += () => { };

        distance_with_player = Random.Range(distance_with_player - 1, distance_with_player + 2);

        ChangeState("Walk");
    }

    protected override void Update()
    {
        if (IsDestroy == true && OnDestroy != null)
        {
            OnDestroy();
            OnDestroy = null;
        }

        enemy_state?.Update();
        enemyStateName = enemy_state.GetType().Name;
    }

    /// <summary>
    /// 피격 시 넉백당하는 함수
    /// </summary>
    /// <param name="damage">피격 데미지</param>
    protected virtual void KnockBack(int damage)
    {
        Vector2 dir;
        dir.x = player.transform.position.x > transform.position.x ? -1 : 1;
        dir.y = 1f;

        rigid.velocity = Vector2.zero;
        rigid.AddForce(dir * 2, ForceMode2D.Impulse);
    }

    /// <summary>
    /// 플레이어를 탐색하는 함수
    /// </summary>
    /// <returns>플레이어 탐색 여부</returns>
    protected virtual bool FindPlayer()
    {
        float distance = Vector2.Distance(player.transform.position, transform.position);

        // 플레이어가 탐색 범위를 벗어났을 때
        if (find_player == true && distance > ai.search_distance)
            return distance <= ai.unSearch_distance;

        // else case : 플레이어가 탐색 범위 안에 있을 때
        return distance <= ai.search_distance;
    }

    /// <summary>
    /// 공격 가능한 상황인지 확인하는 함수
    /// </summary>
    /// <returns>공격 가능 여부</returns>
    protected virtual bool AttackCheck() => find_player;

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
        while (rigid.velocity.y != 0); // 바닥에 떨어질 때까지 대기

        do
        {
            target = transform.position + new Vector3(Random.Range(-ai_moving_range, ai_moving_range), 0);
            hits = Physics2D.RaycastAll(target, Vector2.down, 1, LayerMask.GetMask("Ground"));
            Debug.DrawRay(target, Vector2.down, Color.red, 0.5f);
        } while (long_attack == true && hits.Length == 0);

        while (true)
        {
            yield return null;

            vec = ((target - transform.position).normalized * move_speed * Time.deltaTime);
            transform.position += (vec);
            FlipSprite(vec.x > 0);

            hits = Physics2D.RaycastAll(transform.position, (target - transform.position).normalized, 2, LayerMask.GetMask("Wall"));

            // 벽에 부딛혔거나, 타겟 위치로의 이동이 완료됐을 때
            if (Vector2.Distance(target, transform.position) <= vec.x || hits.Length > 0)
            {
                yield break;
            }
        }
    }

    /// <summary>
    /// 플레이어 발견 시 플레이어를 향해 이동
    /// </summary>
    protected virtual void MoveToPlayer()
    {
        transform.position += (Vector3)(move_speed * ((new Vector2(player.transform.position.x, 0) - new Vector2(transform.position.x, 0)).normalized) * Time.deltaTime);
        FlipSprite();
    }

    /// <summary>
    /// BoxCollider와 Player의 충돌을 확인하는 함수
    /// </summary>
    /// <param name="position">BoxCollider2D 위치</param>
    /// <param name="collider">BoxCollider2D</param>
    /// <param name="rot">회전</param>
    /// <returns>충돌했다면 player, 아니라면 null</returns>
    protected Player CheckCollision(Vector2 position, BoxCollider2D collider, float rot, float size = 1)
    {
        Vector2 offset = collider.offset;
        if (transform.rotation.y != 0) offset *= Vector2.left;

        var hits = Physics2D.BoxCastAll(position + (offset * size), collider.size, rot, Vector2.zero, 0, LayerMask.GetMask("Entity"));

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
        Vector2 offset = collider.offset;
        if (transform.rotation.y != 0) offset *= Vector2.left;

        var hits = Physics2D.CapsuleCastAll(position + offset, collider.size, capshule_dir, 0, Vector2.zero, 0, LayerMask.GetMask("Entity"));

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
        Vector2 offset = collider.offset;
        if (transform.rotation.y != 0) offset *= Vector2.left;

        var hits = Physics2D.CircleCastAll(position + offset, collider.radius, Vector2.zero, 0, LayerMask.GetMask("Entity"));

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
    protected void FlipSprite(bool flipX)
    {
        Quaternion rot = transform.rotation;

        if (flipX)
            transform.rotation = Quaternion.Euler(rot.x, 180, rot.y);
        else
            transform.rotation = Quaternion.Euler(rot.x, 0, rot.y);
    }

    public void Release()
    {
        StopAllCoroutines();

        enabled = false;
        animation.enabled = false;

        renderer.color = Color.red;
        rigid.gravityScale = 1;
        OnHit = (d) => { };
    }
}