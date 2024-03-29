using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Orc : GroundObject
{
    [SerializeField] GameObject rock_prefab;
    [SerializeField] GameObject fragment_prefab;

    public new class WalkState : EnemyState
    {
        Boss_Orc boss;

        public WalkState(Enemy enemy)
        {
            this.enemy = enemy;

            boss = ((Boss_Orc)enemy);
        }

        public override void Update()
        {
            if (boss.AttackCheck())
            {
                boss.ChangeState("Attack2");
            }
        }

        public override void Release()
        {
        }
    }

    public new class Attack1State : EnemyState
    {
        Boss_Orc boss;

        public Attack1State(Enemy enemy)
        {
            this.enemy = enemy;

            boss = ((Boss_Orc)enemy);

            enemy.animation.SetState("Attack1");

            boss.FlipSprite();

            var state = enemy.animation.GetState();
            System.Action action = () => { enemy.StartCoroutine(boss.AttackPattern1(20)); };

            state.frames_actions[boss.attack_frames[0]] = action;
            state.OnAnimationEnd = () => { boss.ChangeState("Idle"); };
        }

        public override void Update()
        {
        }

        public override void Release()
        {
        }
    }

    public new class Attack2State : EnemyState
    {
        Boss_Orc boss;

        public Attack2State(Enemy enemy)
        {
            this.enemy = enemy;

            boss = ((Boss_Orc)enemy);

            boss.animation.SetState("Attack2");

            Vector3 dir = boss.player.transform.position.x > boss.transform.position.x ? Vector3.right : Vector3.left;
            boss.FlipSprite();

            var state = enemy.animation.GetState();
            System.Action action = () => { enemy.StartCoroutine(boss.AttackPattern2(20, dir)); };

            state.frames_actions[boss.attack_frames[1]] = action;
            state.OnAnimationEnd = () => { boss.ChangeState("Attack1"); };
        }

        public override void Update()
        {
        }

        public override void Release()
        {
        }
    }

    protected override void ChangeState(string state)
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
            case string s when nameof(Attack1State).Contains(s):
                enemy_state?.Release();
                enemy_state = new Attack1State(this);
                break;
            case string s when nameof(Attack2State).Contains(s):
                enemy_state?.Release();
                enemy_state = new Attack2State(this);
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

    protected override bool AttackCheck()
    {
        return true;
    }

    protected override IEnumerator AIMoving()
    {
        yield return null;
    }

    [SerializeField] int jump_damage = 1;
    [SerializeField] int fragment_damage = 1;
    /// <summary>
    /// '추격' 공격 패턴 (플레이어가 위치한 플랫폼의 끝으로 도약)
    /// </summary>
    /// <param name="fragment_speed">암석 패턴 후 솟아난 파편 속도</param>
    /// <returns></returns>
    IEnumerator AttackPattern1(float fragment_speed)
    {
        GameObject[] platforms = GameObject.FindGameObjectsWithTag("Platform");
        Transform target_platform = null;
        float min_distance = 9999;

        // 이동할 플랫폼 선택
        foreach (var plat in platforms)
        {
            if (Vector2.Distance(player.transform.position, plat.transform.position) < min_distance)
            {
                min_distance = Vector2.Distance(player.transform.position, plat.transform.position);
                target_platform = plat.transform;
            }
        }

        bool dir_is_right = target_platform.position.x > transform.position.x;

        // 이동 방향에 따른 이동 위치 설정
        Vector2 target = new Vector2(target_platform.position.x, player.transform.position.y) + // 플랫폼 위치 - - - ①
            new Vector2(target_platform.localScale.x / 2 * (dir_is_right ? -1 : 1), 0) + // ① 에서 구한 플랫폼 위치의 맨 끝 - - - ②
            new Vector2(transform.localScale.x / 2 * (dir_is_right ? 4 : -4), 0); // ② 에서 구한 맨 끝에서 조금 안쪽으로 이동 (플랫폼에 절반만 걸친 상태이기 때문) - - - ③

        Vector2 start_position = transform.position;
        float distance =
            // 시작 지점과 목표 지점 x 죄표 사이의 거리
            Vector2.Distance(transform.position * Vector2.right, target * Vector2.right);

        // 포물선 그리며 점프
        #region
        float progress = 0;

        Vector3 center = (transform.position + player.transform.position) * 0.5f;
        center.y -= 3;

        Vector3 pos1 = transform.position - center;
        Vector3 pos2 = player.transform.position - center;

        colliders[0].isTrigger = true;
        attack_particles[0].Play();
        while (progress < 1)
        {
            transform.position = Vector3.Slerp(pos1, pos2, progress);
            transform.position += center;

            progress += Time.deltaTime;
            yield return null;
        }
        attack_particles[1].Play();
        colliders[0].isTrigger = false;


        Player p = CheckCollision(transform.position, (CapsuleCollider2D)colliders[1], CapsuleDirection2D.Horizontal, 0);
        p?.OnHit?.Invoke(jump_damage);

        animation.AnimEnd();
        yield return new WaitForSeconds(0.5f);

        #endregion

        // 추격 후 바위 솟아오름
        #region
        var rock = Instantiate(rock_prefab);

        Vector2 spawn_position = (Vector2)target_platform.position + // 플랫폼 위치 - - - ①
            new Vector2(target_platform.localScale.x / 2 * (dir_is_right ? 1 : -1), 0) + // ①에서 구한 플랫폼 위치의 맨 끝 - - - ②
            new Vector2(rock.transform.localScale.x / 2 * (dir_is_right ? -1 : 1), 0); // ②에서 구한 맨 끝에서 조금 안쪽으로 이동 (화면을 벗어나기 때문) - - - ③

        rock.transform.position = spawn_position;

        Vector2 rock_target_position = new Vector2(rock.transform.position.x, transform.position.y);
        while (Vector2.Distance(rock.transform.position, rock_target_position) >= 0.01f)
        {
            rock.transform.position = Vector2.MoveTowards(rock.transform.position, rock_target_position, 0.05f);
            yield return null;
        }
        #endregion

        // 바위 솟아오른 후 맵 끝으로 파동 날아감
        #region
        var fragment = Instantiate(fragment_prefab);
        fragment.transform.position = rock_target_position;
        var collider = fragment.GetComponent<BoxCollider2D>();

        RaycastHit2D[] hits;
        Vector2 origin;
        Vector3 direction = dir_is_right ? Vector3.left : Vector3.right;

        attack_particles[2].Play();
        do
        {
            origin = fragment.transform.position + new Vector3(fragment.transform.localScale.x * direction.x, 0);
            hits = Physics2D.RaycastAll(origin, Vector2.down, 3, LayerMask.GetMask("Ground"));

            if (hits.Length == 0)
                break;

            hits = Physics2D.RaycastAll(fragment.transform.position, direction, 1, LayerMask.GetMask("Wall"));

            if (hits.Length > 0)
                break;

            if (p == null)
            {
                p = CheckCollision(fragment.transform.position, collider, 0);
                p?.OnHit.Invoke(fragment_damage);
            }

            fragment.transform.position += direction * fragment_speed * Time.deltaTime;
            yield return null;
        }
        while (true);
        #endregion

        Destroy(fragment);

        Destroy(rock, 2);
    }

    [SerializeField] int dash_damage = 1;
    [SerializeField] int smash_damage = 1;
    /// <summary>
    /// '돌격' 공격 패턴 (플랫폼의 끝까지 돌진하며 공격, 돌진을 마친 후 무기를 휘두름)
    /// </summary>
    /// <param name="move_speed">돌격 속도</param>
    /// <param name="direction">돌격 방향</param>
    /// <returns></returns>
    IEnumerator AttackPattern2(float move_speed, Vector3 direction)
    {
        RaycastHit2D[] hits;
        Vector2 origin;

        Player p = null;

        do
        {
            origin = transform.position + direction * 2;

            hits = Physics2D.RaycastAll(origin, Vector2.down, 2, LayerMask.GetMask("Ground"));
            Debug.DrawRay(origin, Vector2.down * 2, Color.red, 0.1f);

            if (hits.Length == 0) break; // 낭떠러지일 때

            hits = Physics2D.RaycastAll(transform.position, direction, 2, LayerMask.GetMask("Wall"));
            Debug.DrawRay(origin, direction * 2, Color.red, 0.1f);

            if (hits.Length > 0) break; // 앞에 벽이 있을 때

            transform.position += direction * move_speed * Time.deltaTime;

            if (p == null)
            {
                p = CheckCollision(transform.position, (CapsuleCollider2D)colliders[2], CapsuleDirection2D.Horizontal, 0);
                p?.OnHit?.Invoke(dash_damage);
            }

            yield return null;
        } while (true);

        animation.AnimEnd();

        p = CheckCollision(transform.position, (CapsuleCollider2D)colliders[3], CapsuleDirection2D.Vertical, 0);
        p?.OnHit?.Invoke(smash_damage);
    }
}
