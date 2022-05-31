using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene3_InGame : SceneContainer
{
    public override void Init()
    {
        Bound = new CameraBound(Vector3.zero, 5f, 3f);
    }
}
