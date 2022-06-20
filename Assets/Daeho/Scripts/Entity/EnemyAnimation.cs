using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SB = System.SerializableAttribute;


public class EnemyAnimation : MonoBehaviour
{
    #region Animation states
    public abstract class AnimState
    {
        [Tooltip("애니메이션 프레임")]
        public Sprite[] frame_sprites;
        public System.Action[] frames_actions;
        [Tooltip("애니메이션 딜레이")]
        public float delay;
        [Tooltip("애니메이션 반복")]
        public bool loop;
        [Tooltip("대기 후 애니메이션이 끝난 후 마지막 프레임 실행")]
        public bool wait;

        protected Enemy model;

        internal int index = 0;

        public Coroutine update { get; set; } = null;

        public void SetModel(Enemy model)
        {
            index = 0;
            frames_actions = new System.Action[frame_sprites.Length];

            if (this.model != null) return;

            this.model = model;
        }

        /// <summary>
        /// 애니메이션 진행
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerator Update()
        {
            yield return null;

            if (delay == 0)
                delay = 0.01f;

            // 애니메이션 프레임이 없을 경우 종료
            if (frame_sprites == null || frame_sprites.Length == 0)
                yield break;

            while (true)
            {
                // 마지막 애니메이션 프레임에 도달했을 경우
                if (index >= frame_sprites.Length)
                {
                    if (loop == false)
                        yield break;
                    else index = 0;
                }
                else if (index == frame_sprites.Length - 1)
                {
                    yield return null;
                    continue;
                }

                model.renderer.sprite = frame_sprites[index++];

                if (frames_actions[index - 1] != null)
                {
                    frames_actions[index - 1]();
                }

                yield return new WaitForSeconds(delay);
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

    // 공격 상태
    [SB]
    class AttackState : AnimState
    {
    }

    // 피격 상태
    [SB]
    class HitState : AnimState
    {
    }

    // 죽음 상태
    [SB]
    class DeadState : AnimState
    {
    }
    #endregion

    Enemy _model;
    Enemy model
    {
        get
        {
            if (_model == null)
            {
                _model = GetComponent<Enemy>();
            }

            return _model;
        }
    }

    [SerializeField] IdleState idle;
    [Space(10)]
    [SerializeField] WalkState walk;
    [Space(10)]
    [SerializeField] AttackState attack;
    [Space(10)]
    [SerializeField] HitState hit;
    [Space(10)]
    [SerializeField] DeadState dead;

    private AnimState _state;
    private AnimState state
    {
        get => _state;
        set
        {
            // 이전 애니메이션이 진행 중이라면 중단
            if (_state != null && _state.update != null)
            {
                StopCoroutine(_state.update);
            }

            _state = value;
            _state.SetModel(model);
        }
    }

    private string s_state = "";

    void Start()
    {
    }

    /// <summary>
    /// Animation 현재 State 변경 함수
    /// </summary>
    /// <param name="name">변경할 State 이름</param>
    public void SetState(string name)
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

        s_state = name;
        state.update = StartCoroutine(state.Update());
    }

    /// <summary>
    /// wait이 true로 설정되었을 경우 호출 가능
    /// </summary>
    /// <returns></returns>
    public IEnumerator AnimEnd()
    {
        if (state.wait == false) yield break;

        model.renderer.sprite = state.frame_sprites[state.index];

        if (state.frames_actions[state.index] != null)
        {
            state.frames_actions[state.index]();
        }

        yield return new WaitForSeconds(state.delay);

        StopCoroutine(state.update);
    }

    /// <summary>
    /// 현재 Animation State를 받아오는 함수
    /// - Idle
    /// - Walk
    /// - Hit
    /// - Attack
    /// - Dead
    /// </summary>
    /// <returns></returns>
    public string GetStateName()
    {
        return s_state;
    }

    public AnimState GetState()
    {
        return state;
    }
}
