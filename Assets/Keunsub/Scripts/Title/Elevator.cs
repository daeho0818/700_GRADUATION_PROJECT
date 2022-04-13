using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    [SerializeField] GameObject Wall;
    [SerializeField] Player player;
    [SerializeField] Transform pos;
    Animator anim;
    bool moving;
    bool isStop = true;
    int cnt = 0;

    void Start()
    {
        anim = GetComponent<Animator>();
        Init();
    }

    public void Init()
    {
        Wall.SetActive(false);
    }

    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !moving && isStop)
        {
            if(cnt % 2 == 0)
            {
                moving = true;
                isStop = false;
                Wall.SetActive(true);
                anim.SetTrigger("0");
            }
            else
            {
                moving = true;
                isStop = false;
                Wall.SetActive(true);
                anim.SetTrigger("1");
            }

            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !moving && isStop)
        {
            cnt++;
            collision.transform.SetParent(null);
        }
    }

    void EndDown()
    {
        Wall.SetActive(false);
        moving = false;
        isStop = true;
    }

    void EndUp()
    {
        moving = false;
        isStop = true;
    }
}
