using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAngelNecklace : ItemBase
{
    public override void AtAttack(Entity monster)
    {
    }

    public override void AtButtonClick()
    {
    }

    public override void AtGameInit()
    {
        player.moneyIncrease += 0.5f;
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
