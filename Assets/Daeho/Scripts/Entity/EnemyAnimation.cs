using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SB = System.SerializableAttribute;

abstract class AnimState
{
    public Sprite[] frame_sprites;
    public float delay;

    private int index = 0;

    IEnumerator AnimUpdate()
    {
        yield return null;
    }
}

// 기본 상태
[SB]
class IdleState : AnimState
{
    public IdleState(Enemy model)
    {
    }
}

// 움직임 상태
[SB]
class WalkState : AnimState
{
    public WalkState(Enemy model)
    {
    }
}

// 피격 상태
[SB]
class HitState : AnimState
{
    public HitState(Enemy model)
    {
    }
}

// 공격 상태
[SB]
class AttackState : AnimState
{
    [SerializeField] (int, int) attack_frame_range;
    public AttackState(Enemy model)
    {
    }
}

// 죽음 상태
[SB]
class DeadState : AnimState
{
    public DeadState(Enemy model)
    {
    }
}

public class EnemyAnimation : MonoBehaviour
{
    Enemy model;

    [SerializeField] IdleState idle;
    [SerializeField] WalkState walk;
    [SerializeField] HitState hit;
    [SerializeField] AttackState attack;
    [SerializeField] DeadState dead;

    private AnimState state;
    private int index;

    void Start()
    {
        model = GetComponent<Enemy>();
    }

    public void SetState(string name)
    {
        switch (name)
        {
            case nameof(IdleState):
                state = new IdleState(model);
                break;
            case nameof(WalkState):
                state = new WalkState(model);
                break;
            case nameof(HitState):
                state = new HitState(model);
                break;
            case nameof(AttackState):
                state = new AttackState(model);
                break;
            case nameof(DeadState):
                state = new DeadState(model);
                break;
            default:
                Debug.Assert(false);
                break;
        }
    }
}
