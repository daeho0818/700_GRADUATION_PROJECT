using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeCream : JewelryBase
{
    float delay;
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
        if (skillActive)
        {
            int randChance = Random.Range(0, 100);

            if (randChance < 15)
            {
                player.miss = true;
            }
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
        if (skillActive)
        {
            if (delay >= 15f)
            {
                skillActive = false;
                delay = 0f;
            }
            else
                delay += Time.deltaTime;
        }
    }

    public override void AtUseButton()
    {
        skillActive = true;
    }
}
