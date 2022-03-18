using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevilChalice : JewelryBase
{

    bool skillActive;
    float defaultDamage;
    float defaultDefence;

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
        defaultDamage = player.damage;
        defaultDefence = player.defence;
    }

    public override void AtUpdate()
    {
        if (skillActive)
        {
            if (delay >= 10f)
            {
                player.damage = defaultDamage;
                player.defence = defaultDefence;
                delay = 0f;
            }
            else
            {
                delay += Time.deltaTime;
                player.damage = defaultDamage + (defaultDamage * 0.1f);
                player.defence = defaultDefence + (defaultDefence * 0.2f);
            }
        }
    }

    public override void AtUseButton()
    {
        skillActive = true;
    }
}
