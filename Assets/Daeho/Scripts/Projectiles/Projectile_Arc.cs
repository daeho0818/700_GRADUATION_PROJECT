using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Arc : Projectile
{
    Vector3 target_position;
    public void SetArc(Vector2 target)
    {
        target_position = target;
        target_position.y = transform.position.y;

        StartCoroutine(MoveToTarget());
    }

    protected override void Update()
    {
    }

    IEnumerator MoveToTarget()
    {
        Vector3 start_position = transform.position;

        float distance = Mathf.Abs(target_position.x - start_position.x);
        float gone_distance;
        float will_distance;

        while (true)
        {
            gone_distance = Mathf.Abs(transform.position.x - start_position.x);
            will_distance = distance - gone_distance;

            transform.position = new Vector2(transform.position.x, (start_position.y + gone_distance * will_distance / distance * 2));
            target_position.y = transform.position.y;

            transform.position = Vector2.MoveTowards(transform.position, target_position, Time.deltaTime * move_speed);

            yield return null;
        }
    }
}
