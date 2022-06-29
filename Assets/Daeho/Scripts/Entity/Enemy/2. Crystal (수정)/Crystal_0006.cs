using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal_0006 : GroundObject
{
    public new class AttackState : EnemyState
    {
        Crystal_0006 crystal;
        public AttackState(Enemy enemy)
        {
            crystal = (Crystal_0006)enemy;

            float distance = Vector2.Distance(crystal.transform.position, crystal.player.transform.position);

            if (distance > crystal.attack_distance)
                crystal.ChangeState("Attack1");
            else
                crystal.ChangeState("Attack2");

        }

        public override void Update()
        {
        }

        public override void Release()
        {
        }
    }

    public new class Attack1State : EnemyState
    {
        Crystal_0006 crystal;
        public Attack1State(Enemy enemy)
        {
            crystal = (Crystal_0006)enemy;

            int attack_frame = crystal.attack_frames[0];
            System.Action action = () => { crystal.StartCoroutine(crystal.AttackPattern1()); };

            var state = crystal.animation.GetState();
            state.frames_actions[attack_frame] = action;
            state.OnAnimationEnd = () => { crystal.ChangeState("Idle"); };
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
        Crystal_0006 crystal;
        public Attack2State(Enemy enemy)
        {
            crystal = (Crystal_0006)enemy;

            int attack_frame = crystal.attack_frames[1];
            System.Action action = () => { crystal.StartCoroutine(crystal.AttackPattern2()); };

            var state = crystal.animation.GetState();
            state.frames_actions[attack_frame] = action;
            state.OnAnimationEnd = () => { crystal.ChangeState("Idle"); };
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
            case string s when nameof(AttackState).Contains(s):
                enemy_state?.Release();
                enemy_state = new AttackState(this);
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

        enemyStateName = state;
    }

    [SerializeField] GameObject lazer_prefab;
    [SerializeField] Transform lazer_position;
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

    protected override void MoveToPlayer()
    {
    }

    protected override bool AttackCheck()
    {
        return true;
    }

    /// <summary>
    /// 레이저 공격 패턴
    /// </summary>
    /// <returns></returns>
    IEnumerator AttackPattern1()
    {
        yield return null;

        GameObject lazer = Instantiate(lazer_prefab, transform);
        lazer.transform.position = lazer_position.position;

        float time = Time.time;

        Player p;
        BoxCollider2D box = lazer.GetComponent<BoxCollider2D>();

        while (Time.time - time <= 3)
        {
            lazer.transform.localScale += Vector3.right * 6 * Time.deltaTime;
            lazer.transform.Translate(Vector3.right * 3 * Time.deltaTime);

            p = CheckCollision(lazer.transform.position, box, 0);
            p?.OnHit?.Invoke(1);

            yield return null;
        }

        animation.AnimEnd();
    }

    /// <summary>
    /// 정권지르기 공격 패턴
    /// </summary>
    /// <returns></returns>
    IEnumerator AttackPattern2()
    {
        yield return null;

        BoxCollider2D collider = (BoxCollider2D)colliders[1];
        Player p = CheckCollision(transform.position, collider, 0);
        p?.OnHit?.Invoke(1);
    }
}
