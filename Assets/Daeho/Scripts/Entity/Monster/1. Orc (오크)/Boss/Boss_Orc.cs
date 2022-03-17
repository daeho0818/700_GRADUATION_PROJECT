using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Orc : Monster
{
    [SerializeField] GameObject rock_prefab;
    [SerializeField] GameObject fragment_prefab;
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.Space))
            StartCoroutine(AttackPattern1(20));

        if (Input.GetKeyDown(KeyCode.RightArrow))
            StartCoroutine(AttackPattern2(10, Vector3.right));

        else if (Input.GetKeyDown(KeyCode.LeftArrow))
            StartCoroutine(AttackPattern2(10, Vector3.left));
    }

    /// <summary>
    /// '�߰�' ���� ���� (�÷��̾ ��ġ�� �÷����� ������ ����)
    /// </summary>
    /// <param name="fragment_speed">�ϼ� ���� �� �ھƳ� ���� �ӵ�</param>
    /// <returns></returns>
    IEnumerator AttackPattern1(float fragment_speed)
    {
        Transform player = GameObject.FindWithTag("Player").transform;
        GameObject[] platforms = GameObject.FindGameObjectsWithTag("Platform");
        Transform target_platform = null;
        float min_distance = 9999;

        // �̵��� �÷��� ����
        foreach (var plat in platforms)
        {
            if (Vector2.Distance(player.position, plat.transform.position) < min_distance)
            {
                min_distance = Vector2.Distance(player.position, plat.transform.position);
                target_platform = plat.transform;
            }
        }

        bool dir_is_right = target_platform.position.x > transform.position.x;

        // �̵� ���⿡ ���� �̵� ��ġ ����
        Vector2 target = new Vector2(target_platform.position.x, player.position.y) + // �÷��� ��ġ - - - ��
            new Vector2(target_platform.localScale.x / 2 * (dir_is_right ? -1 : 1), 0) + // �翡�� ���� �÷��� ��ġ�� �� �� - - - ��
            new Vector2(transform.localScale.x / 2 * (dir_is_right ? 1 : -1), 0); // �迡�� ���� �� ������ ���� �������� �̵� (�÷����� ���ݸ� ��ģ �����̱� ����) - - - ��

        Vector2 start_position = transform.position;
        float moved_distance;
        float move_distance;
        float height;
        float distance =
            // ���� ������ ��ǥ ���� x ��ǥ ������ �Ÿ�
            Vector2.Distance(transform.position * Vector2.right, target * Vector2.right);

        while (transform.position.x != target.x)
        {
            yield return null;

            transform.position = Vector2.MoveTowards(transform.position, new Vector2(target.x, transform.position.y), 0.1f);

            moved_distance = Mathf.Abs(start_position.x - transform.position.x);
            move_distance = new Vector2(distance, 0).x - Mathf.Abs(start_position.x - transform.position.x);
            height = moved_distance * move_distance;

            transform.position = new Vector2(transform.position.x, start_position.y + height / (distance / 2));
        }

        colliders[1].offset = new Vector2(Mathf.Abs(colliders[1].offset.x) * (dir_is_right ? 1 : -1), 0);
        colliders[1].enabled = true;

        yield return new WaitForSeconds(0.5f);

        colliders[1].enabled = false;

        var rock = Instantiate(rock_prefab);
        var fragment = Instantiate(fragment_prefab);

        Vector2 spawn_position = new Vector2(target_platform.position.x, player.position.y) + // �÷��� ��ġ - - - ��
            new Vector2(target_platform.localScale.x / 2 * (dir_is_right ? 1 : -1), 0) + // �翡�� ���� �÷��� ��ġ�� �� �� - - - ��
            new Vector2(rock.transform.localScale.x / 2 * (dir_is_right ? -1 : 1), 0); // �迡�� ���� �� ������ ���� �������� �̵� (ȭ���� ����� ����) - - - ��

        rock.transform.position = fragment.transform.position = spawn_position;

        RaycastHit2D[] hits;
        Vector2 origin;
        Vector2 direction = dir_is_right ? Vector2.left : Vector2.right;
        do
        {
            origin = fragment.transform.position + new Vector3(fragment.transform.localScale.x * direction.x, 0);
            hits = Physics2D.RaycastAll(origin, Vector2.down, 3, LayerMask.GetMask("Platform"));
            Debug.DrawRay(origin, Vector2.down * 3, Color.red, 0.1f);

            fragment.transform.Translate(direction * fragment_speed * Time.deltaTime);
            yield return null;
        }
        while (hits.Length > 0);
    }

    /// <summary>
    /// '����' ���� ���� (�÷����� ������ �����ϸ� ����, ������ ��ģ �� ���⸦ �ֵθ�)
    /// </summary>
    /// <param name="move_speed">���� �ӵ�</param>
    /// <param name="direction">���� ����</param>
    /// <returns></returns>
    IEnumerator AttackPattern2(float move_speed, Vector3 direction)
    {
        RaycastHit2D[] hits;
        Vector2 origin;

        // ���� ��� �ð�
        yield return new WaitForSeconds(2);

        colliders[2].enabled = true;
        do
        {
            origin = transform.position + new Vector3(direction.x * transform.localScale.x / 2, 0) + direction * 2;

            hits = Physics2D.RaycastAll(origin, Vector2.down, 5, LayerMask.GetMask("Platform"));
            Debug.DrawRay(origin, Vector2.down * 5, Color.red, 0.1f);

            transform.Translate(direction * move_speed * Time.deltaTime);
            yield return null;
        }
        while (hits.Length > 0);
        colliders[2].enabled = false;

        colliders[3].offset = new Vector2(Mathf.Abs(colliders[2].offset.x) * direction.x, colliders[2].offset.y);
        colliders[3].enabled = true;

        yield return new WaitForSeconds(0.5f);

        colliders[3].enabled = false;
    }
}
