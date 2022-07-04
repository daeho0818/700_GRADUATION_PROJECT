using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using Random = UnityEngine.Random;

public class GameManager : Singleton<GameManager>
{
    [Header("Player")]
    public Player player;
    public CameraFollow cam;

    [Header("Default State")]
    public float DefaultDamage;
    public int DefaultHp;
    public int DefaultCritical; //critical hit

    [Header("Items")] // 재화, 장신구
    public int money; // 파이트머니
    public List<ItemBase> ItemList = new List<ItemBase>();

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
        ItemList.ForEach(item => item.Init(player));

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

    public void GameOver()
    {
        StartCoroutine(GameOverCoroutine(3f));
    }

    IEnumerator GameOverCoroutine(float duration)
    {
        float timer = duration;
        Camera.main.DOOrthoSize(1f, duration);
        while (timer >= 0)
        {
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, player.transform.position - new Vector3(0, 1f, 10f), timer / duration * Time.deltaTime);
            timer -= Time.deltaTime;
            yield return null;
        }
        
        yield return new WaitForSeconds(3f);

        yield return StartCoroutine(FadeIn(0.5f, () =>
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }));
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

    public void PrintDamage(int damage, Vector3 pos, Color color)
    {
        GameObject damageTxt = PrintTM(damage, pos);
        damageTxt.name = "DamageTxt: " + damage.ToString();
        damageTxt.GetComponent<TextMesh>().color = color;
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
