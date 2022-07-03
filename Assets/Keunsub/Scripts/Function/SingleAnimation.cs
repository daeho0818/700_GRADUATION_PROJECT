using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleAnimation : MonoBehaviour
{

    public float frameDelay;
    public Sprite[] frame;
    SpriteRenderer SR;

    void Start()
    {
        SR = GetComponent<SpriteRenderer>();
        StartCoroutine(MainAnimation());
    }

    IEnumerator MainAnimation()
    {
        int idx = 0;
        while (true)
        {
            SR.sprite = frame[idx];
            yield return new WaitForSeconds(frameDelay);

            if (idx < frame.Length - 1) idx++;
            else idx = 0;
        }
    }
}
