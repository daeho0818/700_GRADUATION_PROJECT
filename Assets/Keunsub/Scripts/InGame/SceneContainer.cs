using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SceneContainer : MonoBehaviour
{
    public CameraBound Bound;
    public Transform[] EnterPoses;

    public abstract void Init();

    public void OnEnter(CameraFollow cam, Transform player, int posIdx)
    {
        cam.Bound = Bound;
        player.transform.position = EnterPoses[posIdx].position;
    }
}
