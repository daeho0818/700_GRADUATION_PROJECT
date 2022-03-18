using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CeremonialDagger : JewelryBase
{

    float damageTmp;

    public override void AtAttackEnd()
    {
        player.damage = damageTmp;
    }

    public override void AtAttackStart(Entity enemy)
    {
        if(enemy.transform.rotation == player.transform.rotation)
        {
            damageTmp = player.damage;
            player.damage = damageTmp + (damageTmp * 0.1f);
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
