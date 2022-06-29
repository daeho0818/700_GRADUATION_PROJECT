using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal_0000 : GroundObject
{
    public new class AttackState : EnemyState
    {
        Crystal_0000 crystal;
        public AttackState(Enemy enemy)
        {
            crystal = ((Crystal_0000)enemy);

            float distance = Vector2.Distance(crystal.transform.position, crystal.player.transform.position);

            if (distance <= crystal.attack_distance)
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
        Crystal_0000 crystal;
        public Attack1State(Enemy enemy)
        {
            crystal = ((Crystal_0000)enemy);

            crystal.animation.SetState("Attack1");

            int attack_frame = crystal.attack_frames[0];
            System.Action action = () => { crystal.StartCoroutine(crystal.SmashAttack()); };

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
        Crystal_0000 crystal;
        public Attack2State(Enemy enemy)
        {
            crystal = ((Crystal_0000)enemy);

            crystal.animation.SetState("Attack2");

            int attack_frame = crystal.attack_frames[1];
            System.Action action = () => { crystal.StartCoroutine(crystal.DashAttack()); };

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

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }

    Player p;
    protected override void Update()
    {
        base.Update();

        if (p == null && animation.GetStateName() == "Attack")
        {
            p = CheckCollision(transform.position, (BoxCollider2D)colliders[0], 0);
            p?.OnHit?.Invoke(1);
        }
    }

    protected override bool AttackCheck() => true;

    protected override IEnumerator BaseAttack()
    {
        yield return null;
    }

    /// <summary>
    /// 플레이어를 향해 돌진하는 공격 패턴
    /// </summary>
    /// <returns></returns>
    IEnumerator DashAttack()
    {
        yield return null;

        Vector3 force = (player.transform.position.x > transform.position.x ? Vector2.right : Vector2.left) * 10; // 바라보고 있는 방향으로 돌진
        force.y = 2;
        rigid.AddForce(force, ForceMode2D.Impulse);
        FlipSprite(force.x > 0);

        Player p = null;

        super_armor = true;

        while (rigid.velocity.x != 0)
        {
            if (p == null)
            {
                var collider = (BoxCollider2D)colliders[0];

                p = CheckCollision(transform.position, collider, 0);
                p?.OnHit?.Invoke(1);
            }

            yield return null;
        }

        super_armor = false;
    }

    /// <summary>
    /// 플레이어를 할퀴는 공격 패턴
    /// </summary>
    IEnumerator SmashAttack()
    {
        yield return null;

        var collider = (BoxCollider2D)colliders[1];

        Player p = CheckCollision(transform.position, collider, 0, 0.6f);
        p?.OnHit?.Invoke(1);
    }

    protected override IEnumerator AIMoving()
    {
        yield return null;
    }
}
