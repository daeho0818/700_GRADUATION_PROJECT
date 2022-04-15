using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundObject : Enemy
{


    Player player;
    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Start()
    {
        base.Start();

        player = FindObjectOfType<Player>();
        StartCoroutine(AIMoving());
    }

    protected override void Update()
    {
        base.Update();
    }
}
