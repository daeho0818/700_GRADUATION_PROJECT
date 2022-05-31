using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Scene2_Ready : SceneContainer
{

    [Header("Scene2")]
    public Transform OutDoor;

    public override void Init()
    {
        Bound = new CameraBound(new Vector3(0, 1.5f, 0), 0, 5.5f);
    }

    public void MoveDown()
    {
        OutDoor.DOMoveY(-12.5f, 0.3f);
    }

    public void MoveUp()
    {
        OutDoor.DOMoveY(-4.4f, 0.3f);
    }
}
