using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene3_InGame : SceneContainer
{

    InGameManager manager;

    public override void Init()
    {

        manager = GetComponent<InGameManager>();
        manager.GameInit();
        Bound = new CameraBound(Vector3.zero, 5f, 3f);
    }
}
