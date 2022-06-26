using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene1_Market : SceneContainer 
{
    public override void Init()
    {
        Bound = new CameraBound(Vector3.zero, 20f, 2f, 5f);
    }

}
