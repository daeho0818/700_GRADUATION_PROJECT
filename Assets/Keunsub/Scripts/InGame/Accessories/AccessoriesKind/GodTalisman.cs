using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodTalisman : JewelryBase
{

    float delay;

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
        if (delay >= 10f)
        {
            delay = 0f;
            player.Hp += 1f;
        }
        else
            delay += Time.deltaTime;
    }

    public override void AtUseButton()
    {
    }
}
