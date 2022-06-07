using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Platform : MonoBehaviour
{

    public Transform L_Platform;
    public Transform R_Platform;

    Collider2D L_col;
    Collider2D R_col;

    public void Init()
    {
        L_col = L_Platform.GetComponent<Collider2D>();
        R_col = R_Platform.GetComponent<Collider2D>();

        L_col.enabled = false;
        R_col.enabled = false;

        transform.position = new Vector3(0, -10f, 0);

        L_Platform.rotation = Quaternion.Euler(0, 0, 90f);
        R_Platform.rotation = Quaternion.Euler(0, 0, -90f);
    }

    public void Appear(Vector3 pos)
    {
        L_Platform.rotation = Quaternion.Euler(0, 0, 90f);
        R_Platform.rotation = Quaternion.Euler(0, 0, -90f);

        L_col.enabled = false;
        R_col.enabled = false;

        Vector2 temp = new Vector2();
        temp.x = pos.x;
        temp.y = -10f;
        transform.position = temp;

        transform.DOMoveY(pos.y, 1f).SetEase(Ease.Linear).OnComplete(() =>
        {
            L_Platform.DOLocalRotate(new Vector3(0, 0, 0f), 0.2f);
            R_Platform.DOLocalRotate(new Vector3(0, 0, 0f), 0.2f).OnComplete(() =>
            {
                L_col.enabled = true;
                R_col.enabled = true;
            });
        });
    }

    public void Disappear()
    {
        L_col.enabled = false;
        R_col.enabled = false;

        R_Platform.DOShakePosition(3f, 0.2f).SetEase(Ease.Linear);
        L_Platform.DOShakePosition(3f, 0.2f).SetEase(Ease.Linear).OnComplete(() =>
        {

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
