using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SB = System.SerializableAttribute;


public class EnemyAnimation : MonoBehaviour
{
    #region Animation states
    abstract class AnimState
    {
        public Sprite[] frame_sprites;
        public float delay;
        public bool loop;

        protected Enemy model;

        private int index = 0;

        public Coroutine update { get; set; } = null;

        public void SetModel(Enemy model)
        {
            index = 0;

            if (this.model != null) return;

            this.model = model;
        }

        /// <summary>
        /// 애니메이션 진행
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerator Update()
        {
            if (delay == 0)
                delay = 0.01f;

            // 애니메이션 프레임이 없을 경우 대기
            if (frame_sprites == null || frame_sprites.Length == 0)
                yield break;

            while (true)
            {
                if (index >= frame_sprites.Length)
                {
                    if (loop == false)
                        yield break;
                    else index = 0;
                }

                model.renderer.sprite = frame_sprites[index++];

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

    Enemy model;

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
        model = GetComponent<Enemy>();
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
    /// 현재 Animation State를 받아오는 함수
    /// - Idle
    /// - Walk
    /// - Hit
    /// - Attack
    /// - Dead
    /// </summary>
    /// <returns></returns>
    public string GetState()
    {
        return s_state;
    }
}
