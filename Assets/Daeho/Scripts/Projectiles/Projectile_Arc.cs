using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Arc : Projectile
{
    Vector3 target_position;
    /// <summary>
    /// �������� �׸��� �߻��� Ÿ�� ��ġ ����
    /// </summary>
    /// <param name="target">Ÿ�� ��ġ</param>
    public void SetArc(Vector2 target)
    {
        target_position = target;

        StartCoroutine(MoveToTarget());
    }

    protected override void Update()
    {
    }

    const float y = 3;
    /// <summary>
    /// �������� �׸��� Ÿ���� ���� ���ư��� �Լ�
    /// </summary>
    /// <returns></returns>
    IEnumerator MoveToTarget()
    {
        float progress = 0;

        Vector3 center = (transform.position + target_position) * 0.5f;
        center.y -= 3;

        Vector3 pos1 = transform.position - center;
        Vector3 pos2 = target_position - center;

        while (progress < 1)
        {
            transform.position = Vector3.Slerp(pos1, pos2, progress);
            transform.position += center;

            progress += Time.deltaTime;
            yield return null;
        }

        onCollision = (p) => { };

        progress = 0;

        var renderer = GetComponent<SpriteRenderer>();
        var color = renderer.color;
        var scale = transform.localScale;

        while (progress < 1)
        {
            renderer.color = Color.Lerp(color, new Color(1, 1, 1, 0), progress);
            transform.localScale = Vector2.Lerp(scale, Vector2.zero, progress);
            
            progress += Time.deltaTime;

            yield return null;
        }

        Destroy(gameObject);
    }
}
