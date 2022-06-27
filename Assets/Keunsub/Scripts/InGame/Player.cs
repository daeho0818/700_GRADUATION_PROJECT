using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Random = UnityEngine.Random;

public class Player : Entity
{
    [Header("Player")]

    #region BehaviourState
    public float moveSpeed;
    public float attackDelay;
    public float jumpHoldTime = 2.5f;
    public float dashSpeed;
    public float dashDelay;
    public float dashCool;
    float curDash;
    float curCool;
    float curJumpTime;
    #endregion
    public float damage;
    public float criticalChance;
    public float Exp;
    public float MaxExp => GetMaxExp();
    public int level = 1;
    public float ExpAmount = 1f;
    public float Mp;
    public float MaxMp = 100f;
    public float MpCool = 3f;
    public float HpAmount = 1f;

    public float damageIncrease = 1f;
    public float defenseIncrease = 1f;
    public float skillDamageIncrease = 1f;
    public float moneyIncrease = 1f;
    public float dodge = 10f; // 0~100

    public Vector2 Nockback;

    [SerializeField] Vector2 JumpForce;

    [Header("Effect")]
    [SerializeField] ParticleSystem DashEffect;

    [Header("Attack")]
    [SerializeField] BoxCollider2D[] AttackColliders;

    [Header("Check Ground")]
    [SerializeField] Transform FeetPos;
    [SerializeField] float checkRadius;
    [SerializeField] LayerMask GroundMask;

    [Header("Skill")]
    public float SkillDamage;
    [SerializeField] Transform skillAtkPos;
    [SerializeField] Transform skillGroundPos;

    #region AnimatorState
    bool isRunning;
    bool isGround;
    bool isAttack;
    bool isFalling;
    bool isJumping;
    bool isRunAble = true;
    bool isDash;
    [SerializeField] int attackState;
    #endregion

    bool gameOver;
    bool isCombo;
    bool isSkill;
    bool doubleJumpAble = true;
    bool obstructionCool = false;

    bool orcDashSkillActive;
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

    void OrcDashSkill()
    {
        if (Input.GetKeyDown(KeyCode.A) && !isSkill && isGround && !orcDashSkillActive)
        {
            isSkill = true;
            orcDashSkillActive = true;
            StartCoroutine(OrcDashSkillCoroutine());
        }
    }

    IEnumerator OrcDashSkillCoroutine()
    {
        ANIM.SetInteger("SkillKind", 0);
        ANIM.SetTrigger("SkillTrigger");
        ANIM.SetBool("IsSkill", isSkill);

        yield return new WaitForSeconds(1.5f);

        ANIM.SetTrigger("SkillActive");
        do
        {
            RaycastHit2D[] hit = Physics2D.RaycastAll(skillGroundPos.position, Vector3.down, 2f, LayerMask.GetMask("Ground"));
            if (hit.Length <= 0)
            {
                Debug.Log("Out of ground");
                break;
            }

            hit = Physics2D.RaycastAll(transform.position, transform.right, 0.5f, LayerMask.GetMask("Wall"));
            if (hit.Length > 0)
            {
                Debug.Log("Hit at wall");
                break;
            }

            transform.Translate(Vector3.right * Time.deltaTime * dashSpeed);

            Physics2D.OverlapCircle(skillAtkPos.position, 0.4f, LayerMask.GetMask("Entity"))?.GetComponent<Entity>()?.OnHit((int)(SkillDamage * skillDamageIncrease));
            yield return null;

        } while (true);


        Physics2D.OverlapCircleAll(skillAtkPos.position, 0.4f, LayerMask.GetMask("Entity")).ToList().ForEach(item => item.GetComponent<Entity>().OnHit((int)(SkillDamage * skillDamageIncrease * 2f)));
        yield return new WaitForSeconds(0.5f); // 공격 후 반동
        
        isSkill = false;
        ANIM.SetBool("IsSkill", isSkill);

        yield return new WaitForSeconds(20f); // skill delay
        orcDashSkillActive = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(skillAtkPos.position, 0.4f);
    }

    void GameOverFunc()
    {
        RB.AddForce(Nockback);
        Time.timeScale = 0.5f;
        ANIM.SetTrigger("GameOver");

        GameManager.Instance.GameOver();
    }

    public int ReturnDamage(Entity monster)
    {
        float _damage = (damage + Random.Range(0, 3)) * damageIncrease;

        if (Random.Range(0, 100) < criticalChance)
        {
            _damage *= 2f;
        }
        Exp += _damage * ExpAmount;
        return (int)_damage;
    }

    float GetMaxExp()
    {
        return level * (level + 1) * 25 - 50;
    }

    void SetExpUI()
    {
        InGameUIManager.Instance.SetExpUI(Exp, MaxExp);
    }

    public void StateInit()
    {
        max_hp = GameManager.Instance.DefaultHp;
        hp = max_hp;
        damage = GameManager.Instance.DefaultDamage;
        criticalChance = GameManager.Instance.DefaultCritical;

        Exp = 0;
        level = 1;
        ExpAmount = 1f;
        HpAmount = 1f;

        Mp = MaxMp;
    }

    void OnHitAction(int damage)
    {
        if (!isDash)
        {
            int rand = Random.Range(0, 100);

            if (rand > dodge)
            {
                hp -= damage * defenseIncrease;
                GameManager.Instance.PrintDamage(damage, transform.position, Color.red);
            }


            if (hp <= 0 && !gameOver)
            {
                gameOver = true;
                GameOverFunc();
            }
        }
    }

    protected override void Start()
    {
        hp = max_hp;
    }

    protected override void Update()
    {
        if (!gameOver)
        {
            if (!isSkill)
            {
                JumpHolding();
                AttackLogic();
                DashLogic();
            }

            JumpLogic();
            OrcDashSkill();
            AnimatorLogic();
        }

        SetExpUI();
        InGameUIManager.Instance.SetHpBar(hp, max_hp);
    }

    void DashLogic()
    {
        if (curCool <= dashCool) curCool += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.C) && !isDash && curCool > dashCool)
        {
            OffAllCollider();
            isDash = true;
            isAttack = false;
            curCool = 0f;

            int dir = transform.localEulerAngles.y == 0f ? 0 : 1;
            DashEffect.GetComponent<ParticleSystemRenderer>().flip = new Vector3(dir, 0, 0);
            DashEffect.Play();
            NoGravity();
        }

    }

    void NoGravity()
    {
        RB.gravityScale = 0f;
        RB.velocity = Vector2.zero;
    }

    void YesGravity()
    {
        RB.gravityScale = 5f;
    }

    private void FixedUpdate()
    {
        if (!gameOver && !isSkill)
        {
            RunningLogic();
            FixedDash();
        }
    }

    void FixedDash()
    {
        if (isDash && curDash < dashDelay)
        {
            transform.Translate(Vector3.right * dashSpeed * Time.deltaTime);
            curDash += Time.deltaTime;
        }
        else
        {
            DashEffect.Stop();
            isDash = false;
            curDash = 0f;
            YesGravity();
        }
    }

    void RunningLogic()
    {
        if (Input.GetKey(KeyCode.LeftArrow) && !isDash && (isRunAble || (!isGround && isAttack)))
        {
            if (isGround)
                OffAllCollider();
            transform.rotation = Quaternion.Euler(0, 180, 0);
            transform.Translate(Vector3.right * Time.deltaTime * moveSpeed);
            isRunning = true;

        }
        else if (Input.GetKey(KeyCode.RightArrow) && !isDash && (isRunAble || (!isGround && isAttack)))
        {
            if (isGround)
                OffAllCollider();
            transform.rotation = Quaternion.Euler(0, 0, 0);
            transform.Translate(Vector3.right * Time.deltaTime * moveSpeed);
            isRunning = true;

        }
        else
        {
            isRunning = false;
        }
    }

    void OffAllCollider()
    {
        AttackColliderOff(0);
        AttackColliderOff(1);
        AttackColliderOff(2);
    }

    void JumpLogic()
    {
        if (Input.GetKeyDown(KeyCode.Z) && !isJumping && isGround && !isSkill)
        {
            OffAllCollider();
            JumpFunc();
            isDash = false;
            YesGravity();
            curDash = 0f;

            if (curCoroutine != null)
                StopCoroutine(curCoroutine);

            isCombo = false;
            isAttack = false;
            isRunAble = true;
            attackState = 0;
        }

        if (Input.GetKeyUp(KeyCode.Z) && isJumping)
        {
            isJumping = false;
        }

        if (Input.GetKeyDown(KeyCode.Z) && !isGround && doubleJumpAble)
        {
            OffAllCollider();
            YesGravity();
            isDash = false;
            curDash = 0f;

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
        OffAllCollider();
        isJumping = true;
        curJumpTime = jumpHoldTime;
        RB.AddForce(JumpForce, ForceMode2D.Impulse);
    }

    void JumpHolding()
    {
        if (Input.GetKey(KeyCode.Z) && isJumping && curJumpTime > 0f)
        {
            RB.AddForce(JumpForce * Time.deltaTime * 4f, ForceMode2D.Impulse);
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

        if (Input.GetKeyDown(KeyCode.X) && !isDash)
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

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Obstruction") && !obstructionCool)
        {
            obstructionCool = true;
            OnHit(10);
            Invoke("ObstructionCoolFalse", 2f);
        }
    }

    void ObstructionCoolFalse()
    {
        obstructionCool = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("CameraRoom"))
        {
            Door door = collision.GetComponent<Door>();
            door.NextScene();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Monster"))
        {

        }
    }
}
