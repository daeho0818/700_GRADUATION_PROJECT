using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingCrystal : MonoBehaviour
{
    [SerializeField] ParticleSystem VFX_Explosion;
    Vector3[] points = new Vector3[4];

    float timer = 0f;
    float curTime = 0f;
    float speed;

    Entity TargetInfo;
    Player player;

    public void Init(Transform _start, Transform _target, float _speed, float _distanceStart, float _distanceEnd, Player player)
    {
        speed = _speed;
        TargetInfo = _target.GetComponent<Entity>();
        this.player = player;
        timer = Random.Range(1.0f, 2.0f);

        points[0] = _start.position;
        points[1] = _start.position +
            (_distanceStart * Random.Range(-1.0f, 1.0f) * _start.right) +
            (_distanceStart * Random.Range(-0.15f, 1.0f) * _start.up) +
            (_distanceStart * Random.Range(-1.0f, -0.8f) * _start.forward);
        points[2] = _target.position +
            (_distanceEnd * Random.Range(-1.0f, 1.0f) * _target.right) +
            (_distanceEnd * Random.Range(-1.0f, 1.0f) * _target.up) +
            (_distanceEnd * Random.Range(0.8f, 1.0f) * _target.forward);
        points[3] = _target.position;

        transform.position = points[0];
    }

    void Update()
    {
        try
        {
            TargetInfo.GetComponent<Transform>();
        }
        catch (MissingReferenceException e)
        {
            ParticleSystem temp = Instantiate(VFX_Explosion, transform.position, Quaternion.identity);

            Destroy(temp.gameObject, 1f);
            Destroy(gameObject);
            return;
        }

        if(curTime > timer)
        {
            TargetInfo?.OnHit(player.ReturnSkillDamage());


            ParticleSystem temp = Instantiate(VFX_Explosion, TargetInfo.transform.position, Quaternion.identity);
            Destroy(temp.gameObject, 1f);

            Destroy(gameObject);
            return;
        }

        points[3] = TargetInfo.transform.position;
        curTime += Time.deltaTime * speed;

        transform.position = new Vector3(
            CubicBezierCurve(points[0].x, points[1].x, points[2].x, points[3].x),
            CubicBezierCurve(points[0].y, points[1].y, points[2].y, points[3].y),
            CubicBezierCurve(points[0].z, points[1].z, points[2].z, points[3].z)
            );
    }


    float CubicBezierCurve(float a, float b, float c, float d)
    {
        float t = curTime / timer;

        float ab = Mathf.Lerp(a, b, t);
        float bc = Mathf.Lerp(b, c, t);
        float cd = Mathf.Lerp(c, d, t);

        float abbc = Mathf.Lerp(ab, bc, t);
        float bccd = Mathf.Lerp(bc, cd, t);

        return Mathf.Lerp(abbc, bccd, t);
    }
}
