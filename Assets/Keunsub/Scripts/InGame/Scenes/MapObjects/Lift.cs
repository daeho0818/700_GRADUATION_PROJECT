using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Lift : MonoBehaviour
{
    public bool isDown = true;
    public bool isClose = false;
    public bool isInteracted = false;
    public bool isMoving = false;
    public Scene2_Ready Scene2;

    private void Update()
    {
        Collider2D cd = Physics2D.OverlapCircle(transform.position, 5f, LayerMask.GetMask("Entity"));
        if (cd != null)
            isClose = cd.CompareTag("Player");

        if (isClose && isInteracted && !isMoving)
        {
            if (isDown)
            {
                MoveUp();
            }
            else
            {
                MoveDown();
            }

            isDown = !isDown;
            isMoving = true;
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isInteracted = true;
            isMoving = false;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isInteracted = false;
            isMoving = false;
        }
    }

    void MoveUp()
    {
        Scene2.MoveUp();
        transform.DOMoveY(4.7f, 2f).SetEase(Ease.InOutBack);
    }

    void MoveDown()
    {
        Scene2.MoveDown();
        transform.DOMoveY(-10.31f, 2f).SetEase(Ease.InOutBack);
    }
}
