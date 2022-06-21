using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPowerStone : ItemBase //������ ��
{
    public override void AtAttack(Entity monster)
    {
        int chance = Random.Range(0, 100);
        if(chance < 10)
        {
            player.OnHit(10);
        }
    }

    public override void AtButtonClick()
    {
    }

    public override void AtGameInit()
    {
        player.damageIncrease += 0.8f;
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
