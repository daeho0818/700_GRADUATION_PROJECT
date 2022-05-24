
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 발사체
public class Projectile : MonoBehaviour
{
    public Vector2 fire_direction;
    public float move_speed;

    protected event System.Action<Player> onCollision = null;

    protected virtual void Update()
    {
        transform.Translate(fire_direction * move_speed * Time.deltaTime);
    }

    /// <summary>
    /// 충돌 시 실행할 내용을 저장하는 함수
    /// </summary>
    /// <param name="action"></param>
    public void SetCollision(System.Action<Player> action) => onCollision = action;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            onCollision?.Invoke(collision.GetComponent<Player>());
            Destroy(gameObject);
        }
    }
}
