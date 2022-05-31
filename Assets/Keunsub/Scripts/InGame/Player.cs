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
    bool isRunAble = true;
    [SerializeField] int attackState;
    #endregion

    bool isCombo;
    bool doubleJumpAble = true;
    Coroutine activeCoroutine;

    #region Component
    public Camera mainCam;
    public CameraFollow camFollow;
    Rigidbody2D RB;
    Animator ANIM;
    #endregion

    protected override void Awake()
    {
        camFollow = mainCam.GetComponent<CameraFollow>();
        RB = GetComponent<Rigidbody2D>();
        ANIM = GetComponent<Animator>();

        OnHit += OnHitAction;
        OnDestroy += OnDestroyAction;
    }

    void OnDestroyAction()
    {

    }

    void OnHitAction(int damage)
    {
        GameManager.Instance.PrintDamage(damage, transform.position);
    }

    protected override void Start()
    {
    }

    protected override void Update()
    {
        RunningLogic();
        JumpLogic();
        JumpHolding();
        AttackLogic();
        AnimatorLogic();
    }

    private void FixedUpdate()
    {
    }

    void RunningLogic()
    {
        if (Input.GetKey(KeyCode.LeftArrow) && (isRunAble || (!isGround && isAttack)))
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
            transform.Translate(Vector3.right * Time.deltaTime * moveSpeed);
            isRunning = true;
        }
        else if (Input.GetKey(KeyCode.RightArrow) && (isRunAble || (!isGround && isAttack)))
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
            JumpFunc();
            isAttack = false;
        }

        if (Input.GetKeyUp(KeyCode.Z) && isJumping)
        {
            isJumping = false;
        }

        if (Input.GetKeyDown(KeyCode.Z) && !isGround && doubleJumpAble)
        {
            doubleJumpAble = false;
            RB.velocity = Vector2.zero;
            RB.AddForce(JumpForce * 2, ForceMode2D.Impulse);
        }

        isGround = Physics2D.OverlapCircle(FeetPos.position, checkRadius, GroundMask);

        if (isGround)
            transform.SetParent(Physics2D.OverlapCircle(FeetPos.position, checkRadius, GroundMask)!.transform);
        else
            transform.SetParent(null);

        if (isGround && !doubleJumpAble) doubleJumpAble = true;
    }

    void JumpFunc()
    {
        isJumping = true;
        curJumpTime = jumpHoldTime;
        RB.AddForce(JumpForce, ForceMode2D.Impulse);
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
                else if (isCombo && isAttack && attackState < 3)
                {
                    StopCoroutine(curCoroutine);
                    isCombo = false;
                    isRunAble = true;
                    curCoroutine = StartCoroutine(AttackCoroutine());
                }
            }
            else
            {
                if (!isAttack)
                    StartCoroutine(OverGroundAttackCoroutine());
                //공중공격
            }
        }
    }

    IEnumerator OverGroundAttackCoroutine()
    {
        isAttack = true;
        yield return new WaitForSeconds(attackDelay / 2.5f);
        isAttack = false;
    }

    IEnumerator AttackCoroutine()
    {
        attackState++;
        isAttack = true;
        yield return new WaitForSeconds(attackDelay / 2 / 2);
        isRunAble = false;
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
        isRunAble = true;
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("CameraRoom"))
        {
            Door door = collision.GetComponent<Door>();
            door.NextScene();
        }
    }
}
