using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneMove : MonoBehaviour
{

    public Vector3 movePos;
    public GameObject PrevScene;
    public GameObject NextScene;
    public int moveMap;

    void Start()
    {

    }

    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            InGameUIManager.Instance.SceneMoveFade(() =>
            {
                PrevScene.SetActive(false);
                NextScene.SetActive(true);
                collision.transform.position = movePos;
                GameManager.Instance.curMap = moveMap;
            });
        }
    }
}
