using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrimsonFlower : JewelryBase
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
        player.Hp += 20f;
    }
}
