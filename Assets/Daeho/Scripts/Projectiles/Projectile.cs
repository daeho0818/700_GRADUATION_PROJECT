
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 발사체
public class Projectile : MonoBehaviour
{
    public Vector2 fire_direction { get; set; }
    public float move_speed { get; set; }

    protected System.Action<Player> onCollision = null;

    protected virtual void Start()
    {
        Invoke(nameof(Destroy), 10);

        transform.rotation = Quaternion.Euler(0, 0, 180 + Mathf.Atan2(fire_direction.y, fire_direction.x) * Mathf.Rad2Deg);
    }

    protected virtual void Update()
    {
        transform.position += (Vector3)fire_direction * move_speed * Time.deltaTime;
    }

    /// <summary>
    /// 충돌 시 실행할 내용을 저장하는 함수
    /// </summary>
    /// <param name="action"></param>
    public void SetCollision(System.Action<Player> action) => onCollision = action;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.TryGetComponent(out Player p))
        {
            onCollision?.Invoke(p);
            Destroy(gameObject);
        }

        // else if (collision.CompareTag("Platform")) Destroy(gameObject);
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }
}
