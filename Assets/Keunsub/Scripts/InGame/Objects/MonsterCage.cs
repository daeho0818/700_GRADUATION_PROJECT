using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;


public enum EntitySize
{
    Small,
    Medium,
    Big
}

public class MonsterCage : MonoBehaviour
{

    public List<SpriteRenderer> CageSprites;

    public void Init()
    {
        CageSprites.ForEach(item => item.gameObject.SetActive(false));
        transform.position = new Vector2(0, -10f);
    }

    public Entity Appear(Vector2 spawnPos, Entity spawnEnemy, EntitySize size)
    {

        CageSprites[(int)size].gameObject.SetActive(true);
        Vector2 temp = new Vector2();
        temp.x = spawnPos.x;
        temp.y = -10f;

        transform.position = temp;
        Vector3 originPos = temp;

        Entity entity = Instantiate(spawnEnemy, transform.position, Quaternion.identity, transform.parent);
        entity.gameObject.SetActive(false);

        transform.DOMoveY(spawnPos.y, 1f).SetEase(Ease.Linear).OnComplete(() =>
        {
            entity.transform.position = transform.position;
            entity.gameObject.SetActive(true);
            transform.DOMoveY(-10f, 1f).SetEase(Ease.Linear).SetDelay(2f).OnComplete(() =>
            {
                //Init();
                Destroy(gameObject);
            });
        });

        return entity;
    }
}
