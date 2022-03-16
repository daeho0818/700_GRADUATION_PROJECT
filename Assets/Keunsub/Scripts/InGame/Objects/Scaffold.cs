using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class Scaffold : MonoBehaviour
{

    [SerializeField] Transform[] Ground;
    Vector3 DefaultRot = new Vector3(0, 0, -90);
    Vector3 ReverseRot = new Vector3(0, 180, 0);
    Vector3 DefaultPos = Vector3.zero;
    Collider2D groundCollider;

    void Start()
    {
        DefaultPos = transform.position;
        groundCollider = GetComponent<Collider2D>();
        groundCollider.enabled = false;

        SetDefaultRot();
        MoveDebug();
    }

    void Update()
    {
        
    }
    
    void MoveDebug()
    {
        StartCoroutine(DebugCoroutine());
    }

    IEnumerator DebugCoroutine()
    {
        while (true)
        {
            Appear(0.5f, Vector3.zero);
            yield return new WaitForSeconds(2f);
            Disappear(3f);
            yield return new WaitForSeconds(4f);

        }
    }

    void SetDefaultRot()
    {
        Ground[0].transform.rotation = Quaternion.Euler(DefaultRot);
        Ground[1].transform.rotation = Quaternion.Euler(DefaultRot - ReverseRot);
    }

    public void Appear(float duration, Vector3 position)
    {
        position.y = DefaultPos.y;
        transform.position = position;
        transform.DOMoveY(position.y, duration).SetEase(Ease.Linear).OnComplete(()=> {
            ScaffoldAppear();
        });
    }

    public void Disappear(float duration)
    {
        StartCoroutine(ScaffoldShake(duration));
    }

    IEnumerator ScaffoldShake(float duration)
    {
        float time = 0f;
        while (time <= duration)
        {

            foreach (var item in Ground)
            {
                item.transform.localPosition = UnityEngine.Random.insideUnitSphere / 10f;
            }

            time += Time.deltaTime;
            yield return null;
        }
        foreach (var item in Ground)
        {
            item.transform.localPosition = Vector3.zero;
        }

        ScaffoldDisappear(() => {
            transform.DOMoveY(DefaultPos.y, 0.5f).SetEase(Ease.Linear);
        });
    }

    public void ScaffoldAppear()
    {
        Ground[0].transform.DOLocalRotate(Vector3.zero, 0.2f);
        Ground[1].transform.DOLocalRotate(Vector3.zero - ReverseRot, 0.2f).OnComplete(()=> {
            groundCollider.enabled = true;
        });
    }

    public void ScaffoldDisappear(Action endAction)
    {
        groundCollider.enabled = false;
        Ground[0].transform.DOLocalRotate(DefaultRot, 0.2f);
        Ground[1].transform.DOLocalRotate(DefaultRot - ReverseRot, 0.2f).OnComplete(()=> {
            endAction();
        });
    }
}
