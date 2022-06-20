using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemBase : MonoBehaviour
{
    [Header("State")]
    public int cost;
    public bool purchased = false; // 구매 여부
    public bool equiped = false;

    [Header("Etc")]
    public Sprite iconSprite;
    public Player player;

    public virtual void Init(Player _player)
    {
        player = _player;
    }

    public abstract void AtGameInit();
    public abstract void AtAttack(Entity monster);
    public abstract void AtKill();
    public abstract void AtOnDamage();
    public abstract void AtButtonClick();
    public abstract void AtUpdate();
}
