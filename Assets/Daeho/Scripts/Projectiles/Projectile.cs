
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �߻�ü
public class Projectile : MonoBehaviour
{
    public Vector2 fire_direction { get; set; }
    public float move_speed { get; set; }

    protected event System.Action<Player> onCollision = null;

    protected virtual void Update()
    {
        transform.position += (Vector3)fire_direction * move_speed * Time.deltaTime;
    }

    /// <summary>
    /// �浹 �� ������ ������ �����ϴ� �Լ�
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

        else if (collision.CompareTag("Platform")) Destroy(gameObject);
    }
}
