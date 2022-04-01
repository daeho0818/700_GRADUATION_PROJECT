using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class MonsterCage : MonoBehaviour
{
    Animator anim;
    [SerializeField] Vector3 endPos;
    Vector3 defaultPos;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void DebugCode()
    {
        StartCoroutine(DebugCoroutine());
    }

    public void Init()
    {
        defaultPos = transform.position;
        anim = GetComponent<Animator>();
    }

    IEnumerator DebugCoroutine()
    {
        while (true)
        {
            Appear(null, endPos, 0.5f);
            yield return new WaitForSeconds(2f);
            DisAppear();
            yield return new WaitForSeconds(2f);
        }
    }

    public Entity Appear(Entity monster, Vector3 position, float duration, Action endAction = null)
    {
        Vector3 temp = position;
        temp.y = defaultPos.y;

        Entity monsterTmp = null;
        
        if(monster != null)
        {
            monsterTmp = Instantiate(monster, transform.position, Quaternion.identity);
            monsterTmp.gameObject.SetActive(false);
        }

        transform.DOMoveY(position.y, duration).SetEase(Ease.OutBack).OnComplete(() => {
            SummonMonster(monsterTmp);
            endAction?.Invoke();
        });

        return monsterTmp;
    }

    public void DisAppear(float delay = 0f, Action endAction = null)
    {
        transform.DOMoveY(defaultPos.y, 0.5f).SetDelay(delay).SetEase(Ease.Linear).OnComplete(() =>
        {
            endAction?.Invoke();
        });
    }

    void SummonMonster(Entity monster)
    {
        monster?.gameObject.SetActive(true);
        //cage door open animation;
        //monster spawn
    }
}
