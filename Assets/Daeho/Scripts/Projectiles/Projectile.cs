
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// πﬂªÁ√º
public class Projectile : MonoBehaviour
{
    public Vector2 fire_direction;
    public float move_speed;

    protected virtual void Update()
    {
        transform.Translate(fire_direction * move_speed * Time.deltaTime);
    }
}
