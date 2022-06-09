using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EInfo = EnemyInformation;
using AIInfo = AIInformation;

[System.Serializable]
public struct EnemyInformation
{
    // Enemy �ִϸ��̼� ������
    public EnemyAnimation animation;
    // ���� ���� ���� ����
    public bool attack_check;
    // ���� ��/�� ������
    public float attack_beforeDelay;
    public float attack_afterDelay;
    // ���� ��Ÿ��
    public float attack_coolTime;
    // �Ѿ� �ӵ�
    public float bullet_speed;
}

[System.Serializable]
public struct AIInformation
{
    // �÷��̾� Ž�� ����
    public bool search_player;
    // AI �̵� �ݰ�
    public float ai_moving_range;
    // �̵� �� �ּ� ���ð�
    public float delay_min;
    // �̵� �� �ִ� ���ð�
    public float delay_max;
    [Tooltip("�÷��̾ Ž���ϴ� ���� (�Ÿ�)")]
    public float search_distance;
    [Tooltip("�÷��̾� �߰��� �����Ǵ� �Ÿ�")]
    public float unSearch_distance;
    [Tooltip("�߰� �� �÷��̾�� ������ �Ÿ�")]
    public float distance_with_player;
}

public class Enemy : Entity
{
    [Header("Enemy Information")]
    [SerializeField] EInfo enemy;
    #region Properties
    /// <summary>
    /// Enemy �ִϸ��̼� ������
    /// </summary>
    public new EnemyAnimation animation { get => enemy.animation; set => enemy.animation = value; }
    /// <summary>
    /// ���� ���� ���� ����
    /// </summary>
    public bool attack_check { get => enemy.attack_check; set => enemy.attack_check = value; }
    /// <summary>
    /// ���� �� ������
    /// </summary>
    public float attack_beforeDelay { get => enemy.attack_beforeDelay; set => enemy.attack_beforeDelay = value; }
    /// <summary>
    /// ���� �� ������
    /// </summary>
    public float attack_afterDelay { get => enemy.attack_afterDelay; set => enemy.attack_afterDelay = value; }
    /// <summary>
    /// ���� ��Ÿ��
    /// </summary>
    public float attack_coolTime { get => enemy.attack_coolTime; set => enemy.attack_coolTime = value; }
    /// <summary>
    /// �Ѿ� �ӵ�
    /// </summary>
    public float bullet_speed { get => enemy.bullet_speed; set => enemy.bullet_speed = value; }
    #endregion

    [Header("AI Moving Information")]
    [SerializeField] protected AIInfo ai;
    #region Properties
    /// <summary>
    /// �÷��̾� Ž�� ����
    /// </summary>
    public bool search_player { get => ai.search_player; set => ai.search_player = value; }
    /// <summary>
    /// AI �̵� �ݰ�
    /// </summary>
    public float ai_moving_range { get => ai.ai_moving_range; set => ai.ai_moving_range = value; }
    /// <summary>
    /// �̵� �� �ּ� ���ð�
    /// </summary>
    public float delay_min { get => ai.delay_min; set => ai.delay_min = value; }
    /// <summary>
    /// �̵� �� �ִ� ���ð�
    /// </summary>
    public float delay_max { get => ai.delay_max; set => ai.delay_max = value; }
    /// <summary>
    /// �÷��̾ Ž���ϴ� ���� (�Ÿ�)
    /// </summary>
    public float search_distance { get => ai.search_distance; set => ai.search_distance = value; }
    /// <summary>
    /// �÷��̾� �߰��� �����Ǵ� �Ÿ�
    /// </summary>
    public float unSearch_distance { get => ai.unSearch_distance; set => ai.unSearch_distance = value; }
    // �߰� �� �÷��̾�� ������ �Ÿ�
    /// <summary>
    /// �߰� �� �÷��̾�� ������ �Ÿ�
    /// </summary>
    public float distance_with_player { get => ai.distance_with_player; set => ai.distance_with_player = value; }
    #endregion

    protected Coroutine ai_moving = null;

    // �÷��̾� �߰� ����
    public bool find_player { get; set; }

    protected Player player;
    protected override void Awake()
    {
    }

    protected override void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        renderer = GetComponent<SpriteRenderer>();
        animation = GetComponent<EnemyAnimation>();

        player = FindObjectOfType<Player>();

        ai_moving = StartCoroutine(AIMoving());

        OnHit += (int damage) =>
        {
            animation.SetState("Hit");
            hp -= damage;
        };
        OnHit += KnockBack;

        OnDestroy += () => animation.SetState("Dead");
        OnDestroy += () => Destroy(gameObject, 1);
    }

    protected override void Update()
    {
        if (IsDestroy)
            OnDestroy?.Invoke();

        if (search_player && movable)
        {
            bool tempFInd = FindPlayer();
            if (tempFInd)
            {
                MoveToPlayer();

                // �÷��̾ ���� ������ : �ʷϻ�
                renderer.color = Color.green;

                animation.SetState("Walk");
            }
            find_player = tempFInd;
        }

        if (!attack_check && player != null)
        {
            bool tempAtk = AttackCheck();
            if (tempAtk)
            {
                StartCoroutine(Attack());
                movable = false;
            }
            attack_check = tempAtk;
        }
    }

    protected virtual void KnockBack(int damage)
    {
        Vector2 dir;
        dir.x = player.transform.position.x > transform.position.x ? -1 : 1;
        dir.y = 1;

        rigid.AddForce(dir * (damage * 2), ForceMode2D.Impulse);
    }

    /// <summary>
    /// �÷��̾ Ž���ϴ� �Լ�
    /// </summary>
    /// <returns>�÷��̾� Ž�� ����</returns>
    protected virtual bool FindPlayer()
    {
        float distance = Vector2.Distance(player.transform.position, transform.position);

        if (distance > ai.search_distance && find_player)
            return distance <= ai.unSearch_distance;

        return distance <= ai.search_distance;
    }

    /// <summary>
    /// ���� ������ ��Ȳ���� Ȯ���ϴ� �Լ�
    /// </summary>
    /// <returns>���� ���� ����</returns>
    protected virtual bool AttackCheck() => find_player;

    Timer coolTime_timer = new Timer();
    Timer delay_timer = new Timer();
    /// <summary>
    /// ���� ��/�� ������, ��Ÿ�� �� ���� ��Ŀ���� ���� �Լ�
    /// </summary>
    /// <returns></returns>
    private IEnumerator Attack()
    {
        animation.SetState("Attack");

        #region �ִϸ��̼� ������ �� ������ �������� ��.

        // ��� �ð� : �����
        renderer.color = Color.yellow;

        yield return new WaitForSeconds(attack_beforeDelay);

        // ���� �� : ������
        renderer.color = Color.red;

        yield return StartCoroutine(BaseAttack());

        // ��� �ð� : �����
        renderer.color = Color.yellow;

        yield return new WaitForSeconds(attack_afterDelay);

        if (!delay_timer.Processing())
            delay_timer.TimerStart(this, attack_afterDelay, () => { movable = true; });

        #endregion

        // �⺻ ����
        renderer.color = Color.white;
        animation.SetState("Idle");

        if (!coolTime_timer.Processing())
            coolTime_timer.TimerStart(this, attack_coolTime,
                () =>
                {
                    animation.SetState("Idle");
                    attack_check = false;
                });
    }
    /// <summary>
    /// �⺻���� ������ �����ϴ� �����Լ�
    /// </summary>
    protected virtual IEnumerator BaseAttack() { yield return null; }

    /// <summary>
    /// �÷��̾ Ž������ ���� ������ AI ��� �������� ������ �Լ�
    /// </summary>
    /// <returns></returns>
    protected virtual IEnumerator AIMoving()
    {
        Vector3 target;
        Vector3 vec;
        RaycastHit2D[] hits;

        do
            yield return null;
        while (rigid.velocity.y != 0);

        target = transform.position + new Vector3(Random.Range(-ai_moving_range, ai_moving_range), 0);

        while (true)
        {
            yield return null;
            if (find_player || !movable) continue;

            vec = ((target - transform.position).normalized * move_speed * Time.deltaTime);
            transform.Translate(vec);
            FlipSprite();

            hits = Physics2D.RaycastAll(transform.position, (target - transform.position).normalized, 2, LayerMask.GetMask("Wall"));

            // ���� �ε����ų�, Ÿ�� ��ġ���� �̵��� �Ϸ���� ��
            if (Vector2.Distance(target, transform.position) <= vec.x || hits.Length > 0)
            {
                yield return new WaitForSeconds(Random.Range(delay_min, delay_max));

                target = transform.position + new Vector3(Random.Range(-ai_moving_range, ai_moving_range), 0);
            }
        }
    }

    /// <summary>
    /// �÷��̾� �߰� �� �÷��̾ ���� �̵�
    /// </summary>
    protected virtual void MoveToPlayer()
    {
        transform.Translate(move_speed * ((new Vector2(player.transform.position.x, 0) - new Vector2(transform.position.x, 0)).normalized) * Time.deltaTime);
        renderer.flipX = player.transform.position.x > transform.position.x;
    }

    /// <summary>
    /// Collider ���� ��ȯ
    /// </summary>
    /// <param name="collider"></param>
    /// <param name="dir_x"></param>
    protected void SetColliderDirection(Collider2D collider, float dir_x)
    {
        Vector2 offset = collider.offset;

        if (offset.x > 0)
            collider.offset = offset * new Vector2(dir_x, 1);
        else
            collider.offset = offset * new Vector2(-dir_x, 1);
    }

    /// <summary>
    /// BoxCollider�� Player�� �浹�� Ȯ���ϴ� �Լ�
    /// </summary>
    /// <param name="position">BoxCollider2D ��ġ</param>
    /// <param name="collider">BoxCollider2D</param>
    /// <param name="rot">ȸ��</param>
    /// <returns>�浹�ߴٸ� player, �ƴ϶�� null</returns>
    protected Player CheckCollision(Vector2 position, BoxCollider2D collider, float rot)
    {
        var hits = Physics2D.BoxCastAll(position + collider.offset, collider.size, rot, Vector2.zero, 0, LayerMask.GetMask("Entity"));

        foreach (var hit in hits)
        {
            if (hit.transform.TryGetComponent(out Player p))
                return p;
        }

        return null;
    }

    /// <summary>
    /// CapshuleCollider�� Player�� �浹�� Ȯ���ϴ� �Լ�
    /// </summary>
    /// <param name="position">CapsuleCollider2D ��ġ</param>
    /// <param name="collider">CapsuleCollider2D</param>
    /// <param name="capshule_dir">CapsuleCollider2D ����</param>
    /// <param name="rot">ȸ��</param>
    /// <returns>�浹�ߴٸ� player, �ƴ϶�� null</returns>
    protected Player CheckCollision(Vector2 position, CapsuleCollider2D collider, CapsuleDirection2D capshule_dir, float rot)
    {
        var hits = Physics2D.CapsuleCastAll(position + collider.offset, collider.size, capshule_dir, 0, Vector2.zero, 0, LayerMask.GetMask("Entity"));

        foreach (var hit in hits)
        {
            if (TryGetComponent(out Player p))
                return p;
        }

        return null;
    }

    /// <summary>
    /// CircleCollider2D�� Player�� �浹�� Ȯ���ϴ� �Լ�
    /// </summary>
    /// <param name="position">CircleCollider2D ��ġ</param>
    /// <param name="collider">CircleCollider2D</param>
    /// <returns>�浹�ߴٸ� player, �ƴ϶�� null</returns>
    protected Player CheckCollision(Vector2 position, CircleCollider2D collider)
    {
        var hits = Physics2D.CircleCastAll(position + collider.offset, collider.radius, Vector2.zero, 0, LayerMask.GetMask("Entity"));

        foreach (var hit in hits)
        {
            if (TryGetComponent(out Player p))
                return p;
        }

        return null;
    }

    /// <summary>
    /// �÷��̾ �������� ĳ���� �� / �� ȸ���ϴ� �Լ�
    /// </summary>
    protected void FlipSprite()
    {
        Quaternion rot = transform.rotation;

        if (player.transform.position.x > transform.position.x)
            transform.rotation = Quaternion.Euler(rot.x, 180, rot.y);
        else
            transform.rotation = Quaternion.Euler(rot.x, 0, rot.y);
    }
}
