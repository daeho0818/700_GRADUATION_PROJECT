using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Platform : MonoBehaviour
{

    public Transform L_Platform;
    public Transform R_Platform;
    public Collider2D col;

    public void Init()
    {
        col.enabled = false;

        transform.position = new Vector3(0, -10f, 0);

        L_Platform.rotation = Quaternion.Euler(0, 0, 90f);
        R_Platform.rotation = Quaternion.Euler(0, 0, -90f);
    }

    public void Appear(Vector3 pos)
    {
        L_Platform.rotation = Quaternion.Euler(0, 0, 90f);
        R_Platform.rotation = Quaternion.Euler(0, 0, -90f);

        col.enabled = false;
        Vector2 temp = new Vector2();
        temp.x = pos.x;
        temp.y = -10f;
        transform.position = temp;

        transform.DOMoveY(pos.y, 1f).SetEase(Ease.Linear).OnComplete(() =>
        {
            L_Platform.DOLocalRotate(new Vector3(0, 0, 0f), 0.2f);
            R_Platform.DOLocalRotate(new Vector3(0, 0, 0f), 0.2f).OnComplete(() =>
            {
                col.enabled = true;
            });
        });
    }

    public void Disappear()
    {
        R_Platform.DOShakePosition(3f, 0.2f, 30, 90, false, false);
        L_Platform.DOShakePosition(3f, 0.2f, 30, 90, false, false).OnComplete(() =>
        {
            col.enabled = false;

            L_Platform.localPosition = Vector3.zero;
            R_Platform.localPosition = Vector3.zero;

            L_Platform.DOLocalRotate(new Vector3(0, 0, 90f), 0.2f);
            R_Platform.DOLocalRotate(new Vector3(0, 0, -90f), 0.2f).OnComplete(() =>
            {
                transform.DOMoveY(-10f, 1f).SetDelay(0.5f).OnComplete(() =>
                {
                    //gameObject.SetActive(false);
                    Destroy(gameObject);
                });

            });
        });
    }
}
