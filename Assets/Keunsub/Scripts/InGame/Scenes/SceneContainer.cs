using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SceneContainer : MonoBehaviour
{
    public CameraBound Bound;
    public Door[] Doors;
    public Transform[] ExitPoses;

    public abstract void Init();

    private void Start()
    {
        int i = 0;
        foreach (var item in Doors)
        {
            item.Init(i, this);
            i++;
        }
    }

    public void OnEnter(CameraFollow cam, Transform player, int posIdx)
    {
        cam.Bound = Bound;
        cam.transform.position = ExitPoses[posIdx].transform.position;
        player.transform.position = ExitPoses[posIdx].transform.position;
    }
}