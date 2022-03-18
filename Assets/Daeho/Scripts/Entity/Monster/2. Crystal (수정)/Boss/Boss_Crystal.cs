using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Crystal : Monster
{
    [SerializeField] Transform[] platforms;
    [SerializeField] GameObject crystal_prefab;
    [SerializeField] GameObject crystal_bullet_prefab;

    int current_platform_index;
    protected override void Awake()
    {

    }
    protected override void Start()
    {

    }
    protected override void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            StartCoroutine(Pattern1());
        if (Input.GetKeyDown(KeyCode.Alpha2))
            StartCoroutine(Pattern2());
    }

    /// <summary>
    /// '워프' 공격 패턴 (전체 플랫폼 중 랜덤한 위치로 텔레포트 후 수정 공격)
    /// </summary>
    /// <returns></returns>
    IEnumerator Pattern1()
    {
        List<int> indexes = new List<int>();

        for (int i = 0; i < platforms.Length; i++)
            if (i != current_platform_index)
                indexes.Add(i);

        int target_platform_index = indexes[Random.Range(0, indexes.Count)];

        yield return new WaitForSeconds(3);

        // 플랫폼 위에 위치하도록 하기 위한 연산
        transform.position = platforms[target_platform_index].position + // 플랫폼 위치 - - - ①
                                                  Vector3.up * platforms[target_platform_index].localScale.y / 2 + // 플랫폼의 세로 절반 크기 - - - ②
                                                  Vector3.up * transform.localScale.y / 2; // 보스의 세로 절반 크기 - - - ③

        // 모은 손에서 양쪽으로 수정이 길어지며 공격
        var crystal_left = Instantiate(crystal_prefab, transform);
        var crystal_right = Instantiate(crystal_prefab, transform);

        crystal_left.transform.localPosition = Vector2.left;
        crystal_right.transform.localPosition = Vector2.right;

        float start_time = Time.time;
        while (Time.time - start_time <= 0.5f)
        {
            crystal_left.transform.localPosition += Vector3.left * 10 * Time.deltaTime;
            crystal_left.transform.localScale += Vector3.right * 20 * Time.deltaTime;

            crystal_right.transform.localPosition += Vector3.right * 10 * Time.deltaTime;
            crystal_right.transform.localScale += Vector3.right * 20 * Time.deltaTime;

            yield return null;
        }

        yield return new WaitForSeconds(0.5f);
        Destroy(crystal_left);
        Destroy(crystal_right);
    }

    /// <summary>
    /// '응답' 공격 패턴 (플레이어를 향해 수정탄을 날린 뒤 폭파시킴)
    /// </summary>
    /// <returns></returns>
    IEnumerator Pattern2()
    {
        var crystal_bullet = Instantiate(crystal_bullet_prefab, transform);
        crystal_bullet.transform.localPosition = Vector2.zero;
        var collider = crystal_bullet.GetComponentInChildren<CircleCollider2D>();

        collider.radius = 3;
        yield return new WaitForSeconds(0.5f);
        collider.radius = 0.5f;

        Transform player = GameObject.FindWithTag("Player").transform;
        Vector3 target_position = player.position;

        while (crystal_bullet.transform.position != target_position)
        {
            crystal_bullet.transform.position = Vector2.MoveTowards(crystal_bullet.transform.position, target_position, 0.1f);
            yield return null;
        }

        collider.radius = 3;
        yield return new WaitForSeconds(0.5f);
        collider.radius = 0.5f;
        Destroy(crystal_bullet);

        yield return new WaitForSeconds(3);
    }
}
