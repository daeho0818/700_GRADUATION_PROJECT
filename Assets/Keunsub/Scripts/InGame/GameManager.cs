using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void MoveToScene(int nextScene, int nextDoor)
    {
        Scenes[CurSceneIdx].gameObject.SetActive(false);

        CurSceneIdx = nextScene;
        Scenes[CurSceneIdx].gameObject.SetActive(true);
        Scenes[CurSceneIdx].Init();
        Scenes[CurSceneIdx].OnEnter(cam, player.transform, nextDoor);
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
