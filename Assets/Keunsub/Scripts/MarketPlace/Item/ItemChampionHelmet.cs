using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemChampionHelmet : ItemBase
{
    public override void AtAttack(Entity monster)
    {
    }

    public override void AtButtonClick()
    {
    }

    public override void AtGameInit()
    {
        player.damageIncrease += 0.25f;
        player.defenseIncrease -= 0.15f;
    }

    public override void AtKill()
    {
    }

    public override void AtOnDamage()
    {
    }

    public override void AtUpdate()
    {
    }

    public override void EndAttack()
    {
    }
}
