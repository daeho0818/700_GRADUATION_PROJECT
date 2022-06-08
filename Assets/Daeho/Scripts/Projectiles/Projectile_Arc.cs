using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Arc : Projectile
{
    Vector2 target_position;
    /// <summary>
    /// 포물선을 그리며 발사할 타겟 위치 설정
    /// </summary>
    /// <param name="target">타겟 위치</param>
    public void SetArc(Vector2 target)
    {
        target_position = target;

        StartCoroutine(MoveToTarget());
    }

    protected override void Update()
    {
        if (Input.GetKeyDown(KeyCode.T)) Time.timeScale = 0.1f;
    }

    const float y = 3;
    /// <summary>
    /// 포물선을 그리며 타겟을 향해 날아가는 함수
    /// </summary>
    /// <returns></returns>
    IEnumerator MoveToTarget()
    {
        float progress = 0;

        Vector2 center = ((Vector2)transform.position + target_position) * 0.5f;
        center.y -= 3;

        Vector2 pos1 = (Vector2)transform.position - center;
        Vector2 pos2 = target_position - center;

        while (progress < 1)
        {
            transform.position = Vector3.Slerp(pos1, pos2, progress);
            transform.position += (Vector3)center;

            progress += Time.deltaTime;
            yield return null;
        }
    }
}
