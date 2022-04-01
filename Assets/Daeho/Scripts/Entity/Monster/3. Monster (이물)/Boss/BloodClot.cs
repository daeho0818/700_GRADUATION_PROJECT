using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodClot : Projectile
{
    void Start()
    {

    }

    void Update()
    {
        base.Update();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Contains("Platform"))
        {
            Destroy(gameObject);
        }
    }
}
