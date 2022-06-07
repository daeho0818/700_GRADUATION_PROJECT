using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SB = System.SerializableAttribute;

abstract class AnimState
{
    public Sprite[] frame_sprites;
    public float delay;

    protected Enemy model;

    private int index = 0;

    public Coroutine update { get; set; } = null;

    public void SetModel(Enemy model)
    {
        if (this.model != null) return;

        this.model = model;
    }
    /// <summary>
    /// 애니메이션 진행
    /// </summary>
    /// <returns></returns>
    public virtual IEnumerator Update()
    {
        if (delay == 0) delay = 0.01f;

        while (true)
        {
            if (frame_sprites == null || frame_sprites.Length == 0)
            {
                yield return null;
                continue;
            }

            model.renderer.sprite = frame_sprites[index++];

            if (index >= frame_sprites.Length) index = 0;

            yield return delay;
        }
    }
}

// 기본 상태
[SB]
class IdleState : AnimState
{
}

// 움직임 상태
[SB]
class WalkState : AnimState
{
}

// 피격 상태
[SB]
class HitState : AnimState
{
}

// 공격 상태
[SB]
class AttackState : AnimState
{
    [SerializeField] (int, int) attack_frame_range;
}

// 죽음 상태
[SB]
class DeadState : AnimState
{
}

public class EnemyAnimation : MonoBehaviour
{
    Enemy model;

    [SerializeField] IdleState idle;
    [Space(10)]
    [SerializeField] WalkState walk;
    [Space(10)]
    [SerializeField] HitState hit;
    [Space(10)]
    [SerializeField] AttackState attack;
    [Space(10)]
    [SerializeField] DeadState dead;

    private AnimState _state;
    private AnimState state
    {
        get => _state;
        set
        {
            _state = value;
            _state.SetModel(model);
        }
    }

    private string s_state = "";

    void Start()
    {
        model = GetComponent<Enemy>();
    }

    /// <summary>
    /// Animation 현재 State 변경 함수
    /// </summary>
    /// <param name="name">변경할 State 이름</param>
    public void SetState(string name)
    {
        // 이전 애니메이션이 진행 중이라면 중단
        if (state != null && state.update != null) StopCoroutine(state.update);

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
            case string n when nameof(AttackState).Contains(n):
                state = attack;
                break;
            case string n when nameof(DeadState).Contains(n):
                state = dead;
                break;
            default:
                Debug.Assert(false);
                return;
        }

        state.update = StartCoroutine(state.Update());
        s_state = name;
    }

    public string GetState()
    {
        return s_state;
    }
}
