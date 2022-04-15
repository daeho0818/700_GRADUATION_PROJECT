using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    [Header("AI Moving Information")]
    [SerializeField] bool search_player = true;
    [SerializeField] float ai_moving_range;
    [SerializeField] float delay_min;
    [SerializeField] float delay_max;

    bool find_player = false;

    Player player;
    protected override void Awake()
    {
    }

    protected override void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        renderer = GetComponent<SpriteRenderer>();

        player = FindObjectOfType<Player>();
    }

    protected override void Update()
    {
        OnDestroy?.Invoke();

        find_player = Mathf.Abs(player.transform.position.y - transform.position.y) <= 0.3f;
        if (find_player) MoveToPlayer();
    }

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
        transform.Translate(move_speed * (new Vector2(player.transform.position.x, 0) - new Vector2(transform.position.x, 0)).normalized * Time.deltaTime);
    }
}
