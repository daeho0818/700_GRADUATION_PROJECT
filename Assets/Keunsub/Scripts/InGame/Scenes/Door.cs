using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public int thisIdx;
    public int NextSceneIdx;
    public int NextDoorIdx;

    SceneContainer parent;

    public void Init(int idx, SceneContainer _parant)
    {
        thisIdx = idx;
        parent = _parant;
    }

    public void NextScene()
    {
        GameManager.Instance.MoveToScene(NextSceneIdx, NextDoorIdx);
    }
}
