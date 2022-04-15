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
    [Tooltip("플레이어를 탐색하는 범위 (거리)")]
    [SerializeField] protected float search_distance;
    [Tooltip("플레이어 추격이 해제되는 거리")]
    [SerializeField] protected float unSearch_distance;
    [Tooltip("추격 중 플레이어와 유지할 거리")]
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
    /// 플레이어를 탐색하는 함수
    /// </summary>
    /// <returns>플레이어 탐색 여부</returns>
    protected virtual bool FindPlayer()
    {
        float distance = Vector2.Distance(player.transform.position, transform.position);

        if (distance > search_distance && find_player)
            return distance <= unSearch_distance;

        return distance <= search_distance;
    }
    /// <summary>
    /// 공격 가능한 상황인지 확인하는 함수
    /// </summary>
    /// <returns>공격 가능 여부</returns>
    protected virtual bool AttackCheck() => find_player;

    /// <summary>
    /// 기본적인 공격을 구현하는 가상함수
    /// </summary>
    protected virtual void BaseAttack() { }

    /// <summary>
    /// 플레이어를 탐색하지 못할 동안의 AI 기반 움직임을 구현한 함수
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
    /// 플레이어 발견 시 플레이어를 향해 이동
    /// </summary>
    protected virtual void MoveToPlayer()
    {
        transform.Translate(move_speed * ((new Vector2(player.transform.position.x, 0) - new Vector2(transform.position.x, 0)).normalized) * Time.deltaTime);
    }
}
