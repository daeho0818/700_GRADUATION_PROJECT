using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SB = System.SerializableAttribute;

public class BossAnimation : EnemyAnimation
{
    [SB]
    public class Attack1State : AnimState
    {
    }

    [SB]
    public class Attack2State : AnimState
    {
    }

    [SB]
    public class Attack3State : AnimState
    {
    }

    [SB]
    public class Attack4State : AnimState
    {
    }

    [SB]
    public class Attack5State : AnimState
    {
    }

    [Space(10)]
    [SerializeField] Attack1State attack1;
    [Space(10)]
    [SerializeField] Attack2State attack2;
    [Space(10)]
    [SerializeField] Attack3State attack3;
    [Space(10)]
    [SerializeField] Attack4State attack4;
    [Space(10)]
    [SerializeField] Attack5State attack5;

    public override void SetState(string name)
    {
        switch (name)
        {
            case string n when nameof(IdleState).Contains(n):
                state = idle;
                break;
            case string n when nameof(WalkState).Contains(n):
                state = walk;
                break;
            case string n when nameof(HitState).Contains(n):
                state = hit;
                break;
            case string n when nameof(Attack1State).Contains(n):
                state = attack1;
                break;
            case string n when nameof(Attack2State).Contains(n):
                state = attack2;
                break;
            case string n when nameof(Attack3State).Contains(n):
                state = attack3;
                break;
            case string n when nameof(Attack4State).Contains(n):
                state = attack4;
                break;
            case string n when nameof(Attack5State).Contains(n):
                state = attack5;
                break;
            case string n when nameof(DeadState).Contains(n):
                state = dead;
                break;
            default:
                Debug.Assert(false);
                return;
        }

        s_state = name;
        state.update = StartCoroutine(state.Update());
    }
}
