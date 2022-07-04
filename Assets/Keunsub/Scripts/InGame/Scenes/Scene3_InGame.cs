using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene3_InGame : SceneContainer
{

    InGameManager manager;

    public override void Init()
    {
        GameManager.Instance.player.StateInit();
        manager = GetComponent<InGameManager>();
        manager.GameInit();
        Bound = new CameraBound(Vector3.zero, 13.5f, 7.8f, 5f);
    }
}
