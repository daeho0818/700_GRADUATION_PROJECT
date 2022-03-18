using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Devilnomicon : JewelryBase
{
    public override void AtAttackEnd()
    {
    }

    public override void AtAttackStart(Entity enemy)
    {
        //skill cool time reduce
    }

    public override void AtAwake()
    {
        player.skillDamage += player.skillDamage * 0.15f;
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
