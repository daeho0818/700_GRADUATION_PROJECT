using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : Singleton<GameManager>
{
    [Header("Player")]
    public Player player;
    public CameraFollow cam;

    [Header("Scenes")]
    public List<SceneContainer> Scenes = new List<SceneContainer>();
    public int CurSceneIdx;
    public int NextRoomIdx;

    [Header("Damage Text")]
    public GameObject DamageTxt;

    [Header("UI")]
    public Image Blur;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        MoveToScene(0, 0);
    }

    public void MoveToScene(int nextScene, int nextDoor)
    {
        StartCoroutine(
        FadeIn(0.5f, () =>
        {
            player.transform.SetParent(null);

            Scenes[CurSceneIdx].gameObject.SetActive(false);

            CurSceneIdx = nextScene;
            Scenes[CurSceneIdx].gameObject.SetActive(true);
            Scenes[CurSceneIdx].Init();
            Scenes[CurSceneIdx].OnEnter(cam, player.transform, nextDoor);

            StartCoroutine(FadeOut(0.5f, ()=>
            {
                player.transform.SetParent(null);
            }));
        }
        ));
        
    }

    IEnumerator FadeOut(float duration, Action action = null)
    {
        Blur.gameObject.SetActive(true);
        float time = duration;
        while (time > 0f)
        {
            Blur.color = new Color(0, 0, 0, time / duration);
            time -= Time.deltaTime;
            yield return null;
        }
        Blur.color = new Color(0, 0, 0, 0);
        Blur.gameObject.SetActive(false);
        action?.Invoke();
    }

    IEnumerator FadeIn(float duration, Action action = null)
    {
        Blur.gameObject.SetActive(true);
        float time = 0f;
        while (time < duration)
        {
            Blur.color = new Color(0, 0, 0, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        Blur.color = new Color(0, 0, 0, 1);
        Blur.gameObject.SetActive(false);
        action?.Invoke();
    }

    public void PrintHeal(int heal, Vector3 pos)
    {
        GameObject healTxt = PrintTM(heal, pos);
        healTxt.name = "HealTxt: " + heal.ToString();
        healTxt.GetComponent<TextMesh>().color = Color.green;
    }

    public void PrintDamage(int damage, Vector3 pos)
    {
        GameObject damageTxt = PrintTM(damage, pos);
        damageTxt.name = "DamageTxt: " + damage.ToString();
        damageTxt.GetComponent<TextMesh>().color = Color.red;
    }

    GameObject PrintTM(int amount, Vector3 pos)
    {
        GameObject damageTxt = Instantiate(DamageTxt);
        Rigidbody2D damageRB = damageTxt.GetComponent<Rigidbody2D>();
        TextMesh damageTM = damageTxt.GetComponent<TextMesh>();

        damageTxt.transform.position = pos;
        damageRB.AddForce(new Vector2(Random.Range(-5f, 5f), Random.Range(3f, 5f)), ForceMode2D.Impulse);
        damageTM.text = amount.ToString();
        Destroy(damageTxt, 0.5f);

        return damageTxt;
    }
}