using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlessedDice : JewelryBase
{
    public override void AtAttackEnd()
    {
    }

    public override void AtAttackStart(Entity enemy)
    {
    }

    public override void AtAwake()
    {
    }

    public override void AtDamaged()
    {
        int randChance = Random.Range(0, 5);
        int rand = Random.Range(0, 100);

        if(rand < randChance)
        {
            player.miss = true;
        }
    }

    public override void AtEnd()
    {
    }

    public override void AtStart()
    {
    }

    public override void AtUpdate()
    {
    }

    public override void AtUseButton()
    {
    }
}
