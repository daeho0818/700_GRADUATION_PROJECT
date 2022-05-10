using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    #region BehaviourState
    public float moveSpeed;
    public float attackDelay;
    #endregion

    #region AnimatorState
    bool isRunning;
    bool isGround;
    bool isAttack;
    bool isFalling;
    bool isJumping;
    int attackState;
    #endregion

    #region Component
    Rigidbody2D RB;
    Animator ANIM;
    #endregion

    protected override void Awake()
    {
        RB = GetComponent<Rigidbody2D>();
        ANIM = GetComponent<Animator>();
    }

    protected override void Start()
    {
    }

    protected override void Update()
    {
    }

    void RunningLogic()
    {

    }

    void JumpLogic()
    {

    }

    void AttackLogic()
    {

    }

    void ETC()
    {
        isFalling = RB.velocity.y < 0f;

        ANIM.SetBool("isFalling", isFalling);
    }
}
