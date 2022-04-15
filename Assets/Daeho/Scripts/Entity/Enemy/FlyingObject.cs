using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingObject : Enemy
{
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();

        StartCoroutine(FlyAnimation());
    }

    protected override void Update()
    {
        base.Update();
    }

    protected virtual IEnumerator FlyAnimation()
    {
        float value = 0;

        while (true)
        {
            transform.localPosition += new Vector3(0, Mathf.Sin(value += 0.7f * Mathf.Deg2Rad) * 0.005f);
            yield return null;
        }
    }
}
