using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene2_Ready : SceneContainer
{
    public override void Init()
    {
        Bound = new CameraBound(Vector3.zero, 0, 5.5f);
    }
}
