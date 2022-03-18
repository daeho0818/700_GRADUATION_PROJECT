using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerCenser : JewelryBase
{

    float delay;
    float defaultDamage;
    bool skillActive;

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
        defaultDamage = player.damage;
    }

    public override void AtUpdate()
    {
        if (skillActive)
        {
            if (delay > 10f)
            {
                player.damage = defaultDamage;
                skillActive = false;
                delay = 0f;
            }
            else
            {
                player.damage = defaultDamage + (defaultDamage * 0.05f);
                delay += Time.deltaTime;
            }
        }
    }

    public override void AtUseButton()
    {
        skillActive = true;
    }
}
