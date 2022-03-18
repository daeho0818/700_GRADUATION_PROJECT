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
    bool isGround;
    
    [SerializeField] Transform feetPos;
    [SerializeField] float checkRadius;
    [SerializeField] LayerMask whatIsGround;
    [SerializeField] float jumpTime;
    float jumpTimeCounter;
    bool isJumping;
    bool doubleJumpAble;
    [SerializeField] CameraFollow cam;

    [HideInInspector] public JewelryBase[] CurJewelry = new JewelryBase[2];

    private void Awake()
    {
        GetDataInManager();
        JewelryAwake();
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
        maxHp = InGameManager.Instance.Hp;
        maxMp = InGameManager.Instance.Mp;
        moveSpeed = InGameManager.Instance.MoveSpeed;
        jumpForce = InGameManager.Instance.JumpForce;
        defence = InGameManager.Instance.Defence;
        damage = InGameManager.Instance.Damage;
        skillDamage = InGameManager.Instance.SkillDamage;
        attackSpeed = InGameManager.Instance.AttackSpeed;

        Hp = maxHp;
        Mp = maxMp;
    }

    void Update()
    {
        JewelryUpdate(); //항상 먼저 실행됨


        JumpLogic();
        JumpHold();
        CheckGround();
    }

    void FixedUpdate()
    {
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
        if(ground != isGround)
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
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.rotation = Quaternion.Euler(0, -180, 0);
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("CameraRoom"))
        {
            cam.followType = collision.GetComponent<RoomTrigger>().type;

            if(cam.followType == CameraFollowType.FollowArena && !InGameManager.Instance.GameActive)
            {
                InGameManager.Instance.GameStart();
            }
        }
    }
}
