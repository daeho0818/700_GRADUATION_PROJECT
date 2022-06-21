using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBerserkerGloves : ItemBase
{
    public override void AtAttack(Entity monster)
    {
    }

    public override void AtButtonClick()
    {
    }

    public override void AtGameInit()
    {
    }

    public override void AtKill()
    {
    }

    public override void AtOnDamage()
    {
    }

    public override void AtUpdate()
    {
        int count = (int)(player.max_hp - player.hp) / 5;
    }

    public override void EndAttack()
    {
    }
}
