using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Monster : Monster
{
    [SerializeField] Transform[] warp_positions;

    [Header("Prefabs")]
    [SerializeField] BloodClot blood_prefab;
    [SerializeField] BloodCrack crack_prefab;
    [SerializeField] GameObject blood_wind_prefab;
    [SerializeField] GameObject hole_prefab;
    [SerializeField] GameObject monster_mouth_prefab;

    [Header("Skill Positions")]
    [SerializeField] Transform blood_wind_position;
    [SerializeField] Transform hole_position;
    [SerializeField] Transform monster_mouth_position;
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

        if (Input.GetKeyDown(KeyCode.Return))
        {
            StartCoroutine(Pattern1());
        }
    }

    /// <summary>
    /// '����' �̵� ����
    /// </summary>
    /// <returns></returns>
    IEnumerator Pattern1()
    {
        int warp_index = 1;//Random.Range(0, 3);
        Vector2 warp_position = warp_positions[warp_index].position;

        // ���� ����Ʈ

        yield return new WaitForSeconds(2);

        transform.position = warp_position;

        if (warp_index == 0 || warp_index == 2)
        {
            StartCoroutine(Pattern3_1(warp_index));
            StartCoroutine(Pattern3_2(warp_index));
        }
        else
        {
            StartCoroutine(Pattern2_1());
        }
    }

    bool[] pattern2 = new bool[2] { false, false };
    /// <summary>
    /// '��Ѹ���' ���� ���� (�Ʒ��� ���ϴ� �͵��� �ٷ� �߻�)
    /// </summary>
    /// <returns></returns>
    IEnumerator Pattern2_1()
    {
        pattern2[0] = true;
        rigid.gravityScale = 0;

        Vector2 direction;
        int count = 11;
        BloodClot blood;

        // �͵��� �Ѹ���
        colliders[1].enabled = true;

        for (int i = 0; i < 180; i += 180 / count)
        {
            direction = new Vector2(Mathf.Cos(i * Mathf.Deg2Rad), -Mathf.Sin(i * Mathf.Deg2Rad));
            blood = Instantiate(blood_prefab);
            blood.fire_direction = direction;
            blood.move_speed = 10;
        }

        yield return new WaitForSeconds(3);
        colliders[1].enabled = false;


        if (pattern2[1])
        {
            pattern2[0] = pattern2[1] = false;
            StartCoroutine(Pattern1());
            rigid.gravityScale = 1;
        }
        else
        {
            StartCoroutine(Pattern2_2());
        }
    }


    /// <summary>
    /// '�����' ���� ���� (���� �ֵθ� �� �տ� ���� �� ����)
    /// </summary>
    /// <returns></returns>
    IEnumerator Pattern2_2()
    {
        pattern2[1] = true;
        rigid.gravityScale = 0;

        // �ִϸ��̼�
        // ���� ���ø� �� ���
        yield return new WaitForSeconds(2);

        // �� �ֵθ�
        colliders[1].enabled = true;

        BloodCrack crack = Instantiate(crack_prefab);

        // �տ��� ���� �� ���
        yield return new WaitForSeconds(1);

        crack.Explosion();

        yield return new WaitForSeconds(0.5f);

        Destroy(crack.gameObject);

        if (pattern2[0])
        {
            pattern2[0] = pattern2[1] = false;
            StartCoroutine(Pattern1());
            rigid.gravityScale = 1;
        }
        else
        {
            StartCoroutine(Pattern2_1());
        }
    }

    /// <summary>
    /// '�ǹٶ�' ���� ���� (�������� �ǹٶ��� �Ҹ� �÷��̾ �о�� ��Ʈ ������ �� ����)
    /// </summary>
    /// <returns></returns>
    IEnumerator Pattern3_1(int warp_index)
    {
        // ���ʵ��� ���� ����
        yield return new WaitForSeconds(1);

        GameObject blood_wind_obj = Instantiate(blood_wind_prefab);
        blood_wind_obj.transform.position = blood_wind_position.position * new Vector2((warp_index == 0 ? 1 : -1), 1);

        yield return new WaitForSeconds(3);

        Destroy(blood_wind_obj);
    }

    /// <summary>
    /// '����' ���� ���� (������ ������ �� ���ۿ��� �̹��� ���� ���� ����)
    /// </summary>
    /// <returns></returns>
    IEnumerator Pattern3_2(int warp_index)
    {
        GameObject hole = Instantiate(hole_prefab);
        hole.transform.position = hole_position.position * new Vector2((warp_index == 0 ? 1 : -1), 1);

        // �̹��� �� ���� ���ð�
        yield return new WaitForSeconds(2);

        GameObject monster_mouth = Instantiate(monster_mouth_prefab);

        Vector2 mouth_position = monster_mouth_position.position * new Vector2((warp_index == 0 ? 1 : -1), 1);
        monster_mouth.transform.position = mouth_position;

        // Sprite Renderer Tiled Size �����Ͽ� �̹� �� �ִϸ��̼� �����

        // while ((Vector2)monster_mouth.transform.position != mouth_position)
        // {
        //     monster_mouth.transform.position = Vector2.MoveTowards(monster_mouth.transform.position, mouth_position, Time.deltaTime * 50);
        //     yield return null;
        // }

        yield return new WaitForSeconds(0.5f);
        Destroy(hole);
        Destroy(monster_mouth);
    }
}
