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
    /// '����' ���� ���� (��ü �÷��� �� ������ ��ġ�� �ڷ���Ʈ �� ���� ����)
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

        // �÷��� ���� ��ġ�ϵ��� �ϱ� ���� ����
        transform.position = platforms[target_platform_index].position + // �÷��� ��ġ - - - ��
                                                  Vector3.up * platforms[target_platform_index].localScale.y / 2 + // �÷����� ���� ���� ũ�� - - - ��
                                                  Vector3.up * transform.localScale.y / 2; // ������ ���� ���� ũ�� - - - ��

        // ���� �տ��� �������� ������ ������� ����
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
    /// '����' ���� ���� (�÷��̾ ���� ����ź�� ���� �� ���Ľ�Ŵ)
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
