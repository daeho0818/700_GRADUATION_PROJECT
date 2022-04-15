using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float maxHp;
    public float maxMp;
    public float defence; //0~1, 데미지에 곱하기
    public float damage;
    public float skillDamage;
    public float moveSpeed;
    public float jumpForce;
    public bool invincible;
    public bool miss; //공격 받을 때 장신구로부터 검사를 받고 공격 끝날 때 false로 바꿈
    public float attackSpeed;
    public float Hp;
    public float Mp;

    Rigidbody2D RB;

    [SerializeField] Transform feetPos;
    [SerializeField] float checkRadius;
    [SerializeField] LayerMask whatIsGround;
    [SerializeField] float jumpTime;
    float jumpTimeCounter;

    bool isGround;
    bool isJumping;
    bool isRunning;
    bool isAttack;
    bool comboAble = true;
    int attackState = 0; //1: atk1, 2: atk2, 3: atk3
    public bool isShop;

    Coroutine attackCoroutine;
    bool doubleJumpAble;
    Animator anim;
    [SerializeField] TitleCameraFollow cam;

    [HideInInspector] public JewelryBase[] CurJewelry = new JewelryBase[2];

    private void Awake()
    {
        GetDataInManager();
        JewelryAwake();
        anim = GetComponent<Animator>();
    }

    #region JewelryFunc
    void JewelryAwake()
    {
        foreach (var item in CurJewelry)
        {
            item?.AtAwake();
        }
    }

    void JewelryStart()
    {
        foreach (var item in CurJewelry)
        {
            item?.AtStart();
        }
    }

    void JewelryUpdate()
    {
        foreach (var item in CurJewelry)
        {
            item?.AtUpdate();
        }
    }

    void JewelryEnd()
    {
        foreach (var item in CurJewelry)
        {
            item?.AtEnd();
        }
    }
    #endregion

    void Start()
    {
        RB = GetComponent<Rigidbody2D>();

        JewelryStart();
    }

    void GetDataInManager()
    {
        maxHp = StatusManager.Instance.Hp;
        maxMp = StatusManager.Instance.Mp;
        moveSpeed = StatusManager.Instance.MoveSpeed;
        jumpForce = StatusManager.Instance.JumpForce;
        defence = StatusManager.Instance.Defence;
        damage = StatusManager.Instance.Damage;
        skillDamage = StatusManager.Instance.SkillDamage;
        attackSpeed = StatusManager.Instance.AttackSpeed;

        Hp = maxHp;
        Mp = maxMp;
    }

    void Update()
    {
        JewelryUpdate(); //항상 먼저 실행됨

        AttackLogic();

        if (!isAttack)
        {
            JumpLogic();
            JumpHold();
        }
        CheckGround();

        SetAnimatorState();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("CameraRoom") && GameManager.Instance.curMap == 0)
        {
            cam.SetFollowTarget(transform, collision.transform, collision.GetComponent<TitleRoom>().thisType, collision.GetComponent<TitleRoom>().clamp);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Store") && GameManager.Instance.curMap == 0)
        {
            InGameUIManager.Instance.Interaction.SetActive(true);

            if (!isShop && Input.GetKeyDown(KeyCode.F))
            {
                isShop = true;
                int index = int.Parse(collision.name);
                InGameUIManager.Instance.StatusUpgradeOn(index);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Store") && GameManager.Instance.curMap == 0)
        {
            InGameUIManager.Instance.Interaction.SetActive(false);
        }
    }

    void AttackLogic()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (!isAttack)
            {
                attackState = 1;
                isAttack = true;
                attackCoroutine = StartCoroutine(AttackCoroutine());
                StartCoroutine(ComboCoroutine());
            }
            else if (isAttack && attackState < 3 && attackCoroutine != null && comboAble && isGround)
            {
                attackState++;
                StopCoroutine(attackCoroutine);
                StartCoroutine(ComboCoroutine());
                attackCoroutine = StartCoroutine(AttackCoroutine());
            }
        }
    }

    IEnumerator ComboCoroutine()
    {
        comboAble = false;
        yield return new WaitForSeconds(attackSpeed / 2f);
        comboAble = true;
    }

    IEnumerator AttackCoroutine()
    {
        yield return new WaitForSeconds(attackSpeed);
        isAttack = false;
        attackState = 0;
        attackCoroutine = null;
    }

    void SetAnimatorState()
    {
        anim.SetBool("IsRunning", isRunning);
        anim.SetBool("IsAttack", isAttack);
        anim.SetInteger("attackState", attackState);
    }

    void FixedUpdate()
    {
        if (!isAttack && !isShop)
            MoveLogic();
    }

    void JumpLogic()
    {
        if (Input.GetKeyDown(KeyCode.Z) && isGround && !isJumping)
        {
            RB.velocity = Vector2.up * jumpForce;
            jumpTimeCounter = jumpTime;
            isJumping = true;
        }
        else if (Input.GetKeyDown(KeyCode.Z) && !isGround && doubleJumpAble)
        {
            RB.velocity = Vector2.up * jumpForce * 1.5f;
            jumpTimeCounter = 0f;
            isJumping = false;
            doubleJumpAble = false;
        }
    }

    void JumpHold()
    {
        if (Input.GetKey(KeyCode.Z))
        {
            if (jumpTimeCounter > 0 && isJumping && RB.velocity != Vector2.zero)
            {
                //RB.AddForce(Vector2.up * jumpForce, ForceMode2D.Force);
                RB.velocity = Vector2.up * jumpForce;
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }

        if (Input.GetKeyUp(KeyCode.Z))
        {
            jumpTimeCounter = 0f;
            isJumping = false;
        }
    }

    void CheckGround()
    {
        bool ground = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround);
        if (ground != isGround)
        {
            doubleJumpAble = true;
            isGround = ground;
        }
        else
        {
            isGround = ground;
        }
    }

    void MoveLogic()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
            isRunning = true;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.rotation = Quaternion.Euler(0, -180, 0);
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }
    }
}
