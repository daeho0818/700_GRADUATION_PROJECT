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
    [Tooltip("애니메이션 공격 해당 프레임 (공격 패턴이 여러개일 경우)")]
    public int[] attack_frames;
    [Tooltip("애니메이션 움직임 해당 프레임")]
    public int walk_frame;
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
    public abstract class EnemyState
    {
        protected Enemy enemy = null;
        protected Entity player = null;

        abstract public void Update();
        abstract public void Release();
    }

    public class IdleState : EnemyState
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

    public class WalkState : EnemyState
    {
        Coroutine walk_process = null;
        Coroutine ai_moving = null;
        Coroutine state_change = null;

        Timer attack_coolTimer = new Timer();
        bool attackable = false;
        public WalkState(Enemy enemy)
        {
            this.enemy = enemy;
            player = enemy.player;

            attack_coolTimer.TimerStart(enemy, enemy.attack_coolTime, () => { attackable = true; });

            enemy.animation.SetState("Walk");

            enemy.animation.GetState().frames_actions[enemy.walk_frame] = () => { walk_process = enemy.StartCoroutine(Walking()); };
        }

        public override void Update()
        {
            // 애니메이션 종료 중일 경우
            if (state_change != null) return;

            // 플레이어 탐색과 이동 가능한 경우
            if (enemy.search_player && enemy.movable)
            {
                // 플레이어를 놓쳤을 경우
                if (enemy.find_player == true && enemy.FindPlayer() == false)
                {
                    enemy.find_player = false;

                    EnemyAnimation animation = enemy.animation;

                    if (animation.GetState().wait)
                    {
                        state_change = enemy.StartCoroutine(StateChange("Idle"));
                        return;
                    }
                    else
                    {
                        enemy.ChangeState("Idle");
                        return;
                    }
                }

                // 플레이어를 발견했을 경우
                else if (enemy.FindPlayer() == true)
                {
                    enemy.find_player = true;

                    StopWalking();

                    if (enemy.animation.GetState().index >= enemy.walk_frame)
                        enemy.MoveToPlayer();
                }
            }

            // 플레이어를 공격 가능한 경우
            if (enemy.AttackCheck() && attackable == true)
            {
                enemy.attack_check = true;

                EnemyAnimation animation = enemy.animation;

                if (animation.GetState().wait)
                {
                    state_change = enemy.StartCoroutine(StateChange("Attack"));
                    return;
                }
                else
                {
                    enemy.ChangeState("Attack");
                    return;
                }
            }
        }

        IEnumerator StateChange(string name)
        {
            enemy.animation.AnimEnd();

            // 반복 중이던 애니메이션이 끝난 뒤에 애니메이션을 변경하도록 설정
            var state = enemy.animation.GetState();
            state.OnAnimationEnd = () => { enemy.ChangeState(name); };

            yield return null;
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
            if (enemy.animation.GetState().wait)
                state_change = enemy.StartCoroutine(StateChange("Idle"));
            else
                enemy.ChangeState("Idle");
        }
    }

    public class AttackState : EnemyState
    {
        Coroutine attack = null;
        public AttackState(Enemy enemy)
        {
            this.enemy = enemy;
            player = enemy.player;

            enemy.animation.SetState("Attack");

            enemy.FlipSprite();

            EnemyAnimation.AnimState state = enemy.animation.GetState();
            System.Action attack = () =>
            {
                this.attack = enemy.StartCoroutine(enemy.BaseAttack());

                if (enemy.attack_particle)
                    enemy.attack_particle.Play();
            };

            if (state.frames_actions.Length > 0)
            {
                state.frames_actions[enemy.enemy.attack_frame] = attack;
            }
            state.OnAnimationEnd = () => enemy.ChangeState("Idle");
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

    #region 공격 패턴이 여러개일 때
    public class Attack1State : EnemyState
    {
        public Attack1State(Enemy enemy)
        {
        }

        public override void Update()
        {
        }

        public override void Release()
        {
        }
    }
    public class Attack2State : EnemyState
    {
        public Attack2State(Enemy enemy)
        {
        }

        public override void Update()
        {
        }

        public override void Release()
        {
        }
    }
    public class Attack3State : EnemyState
    {
        public Attack3State(Enemy enemy)
        {
        }

        public override void Update()
        {
        }

        public override void Release()
        {
        }
    }
    public class Attack4State : EnemyState
    {
        public Attack4State(Enemy enemy)
        {
        }

        public override void Update()
        {
        }

        public override void Release()
        {
        }
    }
    public class Attack5State : EnemyState
    {
        public Attack5State(Enemy enemy)
        {
        }

        public override void Update()
        {
        }

        public override void Release()
        {
        }
    }
    #endregion

    public class HitState : EnemyState
    {
        public HitState(Enemy enemy)
        {
            this.enemy = enemy;
            player = enemy.player;

            enemy.animation.SetState("Hit");

            EnemyAnimation.AnimState state = enemy.animation.GetState();
            state.OnAnimationEnd = () => enemy.ChangeState("Idle");
        }

        public override void Update()
        {
        }

        public override void Release()
        {
        }
    }

    public class DeadState : EnemyState
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

    protected EnemyState enemy_state = null;

    protected virtual void ChangeState(string state)
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
            #region 공격 패턴이 여러개일 때
            case string s when nameof(Attack1State).Contains(s):
                enemy_state?.Release();
                enemy_state = new Attack1State(this);
                break;
            case string s when nameof(Attack2State).Contains(s):
                enemy_state?.Release();
                enemy_state = new Attack2State(this);
                break;
            case string s when nameof(Attack3State).Contains(s):
                enemy_state?.Release();
                enemy_state = new Attack3State(this);
                break;
            case string s when nameof(Attack4State).Contains(s):
                enemy_state?.Release();
                enemy_state = new Attack4State(this);
                break;
            case string s when nameof(Attack5State).Contains(s):
                enemy_state?.Release();
                enemy_state = new Attack5State(this);
                break;
            #endregion
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

        enemyStateName = state;
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
    /// 애니메이션 공격 해당 프레임 (공격 패턴이 여러개일 경우)
    /// </summary>
    public int[] attack_frames { get => enemy.attack_frames; set => enemy.attack_frames = value; }
    /// <summary>
    /// 애니메이션 움직임 해당 프레임
    /// </summary>
    public int walk_frame { get => enemy.walk_frame; set => enemy.walk_frame = value; }
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

    [SerializeField] protected ParticleSystem attack_particle;
    [SerializeField] protected ParticleSystem[] attack_particles;

    [Space(10)]
    [SerializeField] protected string enemyStateName;
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

        OnHit += (int damage) =>
        {
            if (super_armor == false)
            {
                KnockBack(damage);
            }
        };

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
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Obstruction"))
        {
            InvokeRepeating(nameof(DotDamage),0, 1);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Obstruction"))
        {
            CancelInvoke(nameof(DotDamage));
        }
    }

    /// <summary>
    /// 가시를 밟았을 때 일정 시간마다 데미지를 주는 함수
    /// </summary>
    void DotDamage()
    {
        hp -= 10;
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
            Debug.DrawRay(target, Vector2.down, Color.red, 3);
            FlipSprite(target.x > transform.position.x);
        } while (long_attack == true && hits.Length == 0);

        while (true)
        {
            yield return null;

            if (movable == false)
            {
                continue;
            }

            vec = ((target - transform.position).normalized * move_speed * Time.deltaTime);
            transform.position += (vec);

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
        transform.position += move_speed * (new Vector3(player.transform.position.x > transform.position.x ? 1 : -1, 0) * Time.deltaTime);
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

        var hits = Physics2D.BoxCastAll(position + (offset * size), collider.size * size, rot, Vector2.zero, 0, LayerMask.GetMask("Entity"));

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

    /// <summary>
    /// 현재 플레이어가 바라보고 있는 방향을 구하는 함수
    /// </summary>
    /// <returns></returns>
    // protected int GetDirX()
    // {
    //     return transform.rotation.y != 0 ? 1 : -1;
    // }

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