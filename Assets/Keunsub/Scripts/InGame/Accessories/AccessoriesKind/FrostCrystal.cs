using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrostCrystal : JewelryBase
{
    public override void AtAttackEnd()
    {
    }

    public override void AtAttackStart(Entity enemy)
    {
        int randChance = Random.Range(0, 100);
        if(randChance < 5)
        {
            //enemy frozen
        }
    }

    public override void AtAwake()
    {
    }

    public override void AtDamaged()
    {
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
