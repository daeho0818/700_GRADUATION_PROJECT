using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EInfo = EnemyInformation;
using AIInfo = AIInformation;

[System.Serializable]
public struct EnemyInformation
{
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
    #region trash
    // ���� ���� ���� ����
    [SerializeField] protected bool attack_check;
    // ���� ��/�� ������
    [SerializeField] protected float attack_beforeDelay;
    [SerializeField] protected float attack_afterDelay;
    // ���� ��Ÿ��
    [SerializeField] protected float attack_coolTime;
    // �Ѿ� �ӵ�
    [SerializeField] protected float bullet_speed;
    #endregion

    [Header("AI Moving Information")]
    [SerializeField] AIInfo ai;
    #region trash
    // �÷��̾� Ž�� ����
    [SerializeField] protected bool search_player = true;
    // AI �̵� �ݰ�
    [SerializeField] protected float ai_moving_range;
    // �̵� �� �ּ� ���ð�
    [SerializeField] protected float delay_min;
    // �̵� �� �ִ� ���ð�
    [SerializeField] protected float delay_max;
    [Tooltip("�÷��̾ Ž���ϴ� ���� (�Ÿ�)")]
    [SerializeField] protected float search_distance;
    [Tooltip("�÷��̾� �߰��� �����Ǵ� �Ÿ�")]
    [SerializeField] protected float unSearch_distance;
    [Tooltip("�߰� �� �÷��̾�� ������ �Ÿ�")]
    [SerializeField] protected float distance_with_player;
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

        player = FindObjectOfType<Player>();

        ai_moving = StartCoroutine(AIMoving());
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

    /// <summary>
    /// �÷��̾ Ž���ϴ� �Լ�
    /// </summary>
    /// <returns>�÷��̾� Ž�� ����</returns>
    protected virtual bool FindPlayer()
    {
        float distance = Vector2.Distance(player.transform.position, transform.position);

        if (distance > search_distance && find_player)
            return distance <= unSearch_distance;

        return distance <= search_distance;
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

        if (!coolTime_timer.Processing())
            coolTime_timer.TimerStart(this, attack_coolTime, () => { attack_check = false; });
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
        Vector3 target = transform.position + new Vector3(Random.Range(-ai_moving_range, ai_moving_range), 0);
        Vector3 vec;
        RaycastHit2D[] hits;

        while (true)
        {
            yield return null;
            if (find_player || !movable) continue;

            vec = ((target - transform.position).normalized * move_speed * Time.deltaTime);
            transform.Translate(vec);

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
}
