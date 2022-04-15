using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Monster : GroundObject
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

    int current_index = -1;
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
    /// '워프' 이동 패턴
    /// </summary>
    /// <returns></returns>
    IEnumerator Pattern1()
    {
        Debug.Log("워프 이동 패턴!");


        int warp_index;

        do
        {
            warp_index = 1;//Random.Range(0, 3);
        } while (current_index == warp_index);
        //current_index = warp_index;

        Vector2 warp_position = warp_positions[warp_index].position;

        // 워프 이펙트

        yield return new WaitForSeconds(2);

        transform.position = warp_position;

        if (warp_index == 0 || warp_index == 2)
        {
            StartCoroutine(Pattern3_1(warp_index));
            StartCoroutine(Pattern3_2(warp_index));
        }
        else
        {
            if (Random.Range(0, 2) == 0)
                StartCoroutine(Pattern2_1());
            else
                StartCoroutine(Pattern2_2());
        }
    }

    bool[] pattern2 = new bool[2] { false, false };
    /// <summary>
    /// '흩뿌리기' 공격 패턴 (아래를 향하는 핏덩이 다량 발사)
    /// </summary>
    /// <returns></returns>
    IEnumerator Pattern2_1()
    {
        Debug.Log("흩뿌리기 공격 패턴!");

        pattern2[0] = true;
        rigid.gravityScale = 0;

        Vector2 direction;
        int count = 11;
        BloodClot blood;

        // 핏덩이 뿌리기
        colliders[1].enabled = true;

        for (int i = 20; i < 160; i += 180 / count)
        {
            direction = new Vector2(Mathf.Cos(i * Mathf.Deg2Rad), -Mathf.Sin(i * Mathf.Deg2Rad));
            blood = Instantiate(blood_prefab);
            blood.transform.position = transform.position;
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
    /// '베어내기' 공격 패턴 (낫을 휘두른 뒤 균열 생성 및 폭발)
    /// </summary>
    /// <returns></returns>
    IEnumerator Pattern2_2()
    {
        Debug.Log("베어내기 공격 패턴!");

        pattern2[1] = true;
        rigid.gravityScale = 0;

        // 애니메이션
        // 낫을 들어올린 후 대기
        yield return new WaitForSeconds(2);

        // 낫 휘두름
        colliders[1].enabled = true;

        BloodCrack crack = Instantiate(crack_prefab);

        // 균열이 생긴 뒤 대기
        yield return new WaitForSeconds(1);

        colliders[1].enabled = false;

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
    /// '피바람' 공격 패턴 (전방으로 피바람을 불며 플레이어를 밀어내고 도트 데미지 및 감속)
    /// </summary>
    /// <returns></returns>
    IEnumerator Pattern3_1(int warp_index)
    {
        Debug.Log("피바람 공격 패턴!");

        // 몇초동안 힘을 모음
        yield return new WaitForSeconds(1);

        GameObject blood_wind_obj = Instantiate(blood_wind_prefab);
        blood_wind_obj.transform.position = blood_wind_position.position * new Vector2((warp_index == 0 ? 1 : -1), 1);

        yield return new WaitForSeconds(3);

        Destroy(blood_wind_obj);

        StartCoroutine(Pattern1());
    }

    /// <summary>
    /// '구멍' 공격 패턴 (구멍을 생성한 뒤 구멍에서 이물의 입이 나와 공격)
    /// </summary>
    /// <returns></returns>
    IEnumerator Pattern3_2(int warp_index)
    {
        Debug.Log("구멍 공격 패턴!");

        GameObject hole = Instantiate(hole_prefab);
        hole.transform.position = hole_position.position * new Vector2((warp_index == 0 ? 1 : -1), 1);

        // 이물의 입 생성 대기시간
        yield return new WaitForSeconds(4);

        GameObject monster_mouth = Instantiate(monster_mouth_prefab);

        Vector2 mouth_position = monster_mouth_position.position * new Vector2((warp_index == 0 ? 1 : -1), 1);
        monster_mouth.transform.position = mouth_position;

        // Sprite Renderer Tiled Size 조정하여 이물 입 애니메이션 만들기

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
