using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollider : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster"))
        {
            if (!collision.GetComponent<Enemy>().IsDestroy)
                collision.GetComponent<Enemy>().OnHit(GameManager.Instance.player.ReturnDamage());
        }
    }
}
