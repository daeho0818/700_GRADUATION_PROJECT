using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    [Header("Enemy Information")]
    [SerializeField] bool attack_check;
    [SerializeField] float attack_coolTime;
    [SerializeField] float cur_attack_coolTime;
    [SerializeField] protected float bullet_speed;

    [Header("AI Moving Information")]
    [SerializeField] bool search_player = true;
    [SerializeField] float ai_moving_range;
    [SerializeField] float delay_min;
    [SerializeField] float delay_max;
    [Tooltip("�÷��̾ Ž���ϴ� ���� (�Ÿ�)")]
    [SerializeField] protected float search_distance;
    [Tooltip("�÷��̾� �߰��� �����Ǵ� �Ÿ�")]
    [SerializeField] protected float unSearch_distance;
    [Tooltip("�߰� �� �÷��̾�� ������ �Ÿ�")]
    [SerializeField] protected float distance_with_player;

    public bool find_player;

    protected Player player;
    protected override void Awake()
    {
    }

    protected override void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        renderer = GetComponent<SpriteRenderer>();

        player = FindObjectOfType<Player>();

        StartCoroutine(AIMoving());
    }

    protected override void Update()
    {
        OnDestroy?.Invoke();

        if (search_player)
        {
            bool tempFInd = FindPlayer();
            if (tempFInd) MoveToPlayer();
            find_player = tempFInd;
        }

        if (!attack_check)
        {
            bool tempAtk = AttackCheck();
            if (tempAtk) BaseAttack();
            attack_check = tempAtk;
            cur_attack_coolTime = 0;
        }
        else
        {
            if (cur_attack_coolTime >= attack_coolTime)
            {
                attack_check = false;
                cur_attack_coolTime = 0;
            }

            else
                cur_attack_coolTime += Time.deltaTime;
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

    /// <summary>
    /// �⺻���� ������ �����ϴ� �����Լ�
    /// </summary>
    protected virtual void BaseAttack() { }

    /// <summary>
    /// �÷��̾ Ž������ ���� ������ AI ��� �������� ������ �Լ�
    /// </summary>
    /// <returns></returns>
    protected virtual IEnumerator AIMoving()
    {
        Vector3 target = transform.position + new Vector3(Random.Range(-ai_moving_range, ai_moving_range), 0);
        Vector3 vec;

        while (true)
        {
            yield return null;
            if (find_player) continue;

            vec = ((target - transform.position).normalized * move_speed * Time.deltaTime);
            Debug.Log(vec);
            transform.Translate(vec);

            if (Vector2.Distance(target, transform.position) <= vec.x)
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
    }
}
