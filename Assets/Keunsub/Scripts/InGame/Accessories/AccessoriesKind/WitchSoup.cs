using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitchSoup : JewelryBase
{
    float delay;
    float defaultSpeed;

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
        defaultSpeed = player.attackSpeed;
    }

    public override void AtUpdate()
    {
        if (skillActive)
        {
            if (delay >= 5f)
            {
                player.attackSpeed = defaultSpeed;
                skillActive = false;
                delay = 0f;
            }
            else
            {
                player.attackSpeed = defaultSpeed + (defaultSpeed * 0.1f);
                delay += Time.deltaTime;
            }
        }
    }

    public override void AtUseButton()
    {
        skillActive = true;
    }
}
