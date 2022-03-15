using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Entity
{
    protected override void Awake()
    {
    }

    protected override void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    protected override void Update()
    {
    }
}
