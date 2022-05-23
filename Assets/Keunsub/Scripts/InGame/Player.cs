using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    [Header("Player")]

    #region BehaviourState
    public float moveSpeed;
    public float attackDelay;
    public float jumpHoldTime = 2.5f;
    float curJumpTime;
    #endregion

    [SerializeField] Vector2 JumpForce;

    [Header("Attack")]
    [SerializeField] BoxCollider2D[] AttackColliders;

    [Header("Check Ground")]
    [SerializeField] Transform FeetPos;
    [SerializeField] float checkRadius;
    [SerializeField] LayerMask GroundMask;

    #region AnimatorState
    bool isRunning;
    bool isGround;
    bool isAttack;
    bool isFalling;
    bool isJumping;
    [SerializeField] int attackState;
    #endregion

    bool isCombo;
    Coroutine activeCoroutine;

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
        if (!isAttack)
        {
            JumpLogic();
            JumpHolding();
            RunningLogic();
        }
        AttackLogic();
        AnimatorLogic();
    }

    private void FixedUpdate()
    {
    }

    void RunningLogic()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
            transform.Translate(Vector3.right * Time.deltaTime * moveSpeed);
            isRunning = true;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            transform.Translate(Vector3.right * Time.deltaTime * moveSpeed);
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }
    }

    void JumpLogic()
    {
        if (Input.GetKeyDown(KeyCode.Z) && !isJumping && isGround)
        {
            isJumping = true;
            curJumpTime = jumpHoldTime;
            RB.AddForce(JumpForce, ForceMode2D.Impulse);
        }

        if (Input.GetKeyUp(KeyCode.Z) && isJumping)
        {
            isJumping = false;
        }

        isGround = Physics2D.OverlapCircle(FeetPos.position, checkRadius, GroundMask);
    }

    void JumpHolding()
    {
        if (Input.GetKey(KeyCode.Z) && isJumping && curJumpTime > 0f)
        {
            RB.AddForce(JumpForce / 2);
            curJumpTime -= Time.deltaTime;
        }

    }

    void AttackColliderOn(int idx)
    {
        AttackColliders[idx].gameObject.SetActive(true);
    }

    void AttackColliderOff(int idx)
    {
        AttackColliders[idx].gameObject.SetActive(false);
    }

    Coroutine curCoroutine;
    void AttackLogic()
    {
        // 지상일때는 콤보 가능, 공중에서는 콤보 불가능
        // 콤보에서 다음 콤보로 이어지기 전 짧은 시간동안 isAttack 풀어짐

        if (Input.GetKeyDown(KeyCode.X))
        {
            if (isGround)
            {
                if (!isCombo && !isAttack)
                {
                    curCoroutine = StartCoroutine(AttackCoroutine());
                }
                else if(isCombo && isAttack && attackState < 3)
                {
                    StopCoroutine(curCoroutine);
                    isCombo = false;
                    curCoroutine = StartCoroutine(AttackCoroutine());
                }
            }
            else
            {
                //공중공격
            }
        }
    }

    IEnumerator AttackCoroutine()
    {
        attackState++;
        isAttack = true;

        yield return new WaitForSeconds(attackDelay / 2);

        isCombo = true;
        float timer = attackDelay / 2;

        while (timer > 0f)
        {
            timer -= Time.deltaTime;
            yield return null;
        }

        isCombo = false;
        isAttack = false;
        attackState = 0;
    }

    void AnimatorLogic()
    {
        isFalling = RB.velocity.y <= 0.5f;

        ANIM.SetBool("IsFalling", isFalling);
        ANIM.SetBool("IsRunning", isRunning);
        ANIM.SetBool("IsJumping", isJumping);
        ANIM.SetBool("IsGround", isGround);
        ANIM.SetBool("IsAttack", isAttack);
        ANIM.SetInteger("attackState", attackState);
    }
}
