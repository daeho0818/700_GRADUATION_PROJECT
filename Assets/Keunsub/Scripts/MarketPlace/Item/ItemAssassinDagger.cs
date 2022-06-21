using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAssassinDagger : ItemBase
{

    bool eventActive = false;

    public override void AtAttack(Entity monster)
    {
        if(monster.transform.localEulerAngles.y == -180f)
        {
            if(monster.transform.position.x > player.transform.position.x)
            {
                eventActive = true;
                player.damageIncrease += 0.8f;
            }
        }
        else
        {
            if(monster.transform.position.x < player.transform.position.x)
            {
                eventActive = true;
                player.damageIncrease += 0.8f;
            }
        }
    }

    public override void AtButtonClick()
    {
    }

    public override void AtGameInit()
    {
    }

    public override void AtKill()
    {
    }

    public override void AtOnDamage()
    {
    }

    public override void AtUpdate()
    {
    }

    public override void EndAttack()
    {
        if (eventActive) player.damageIncrease -= 0.8f;
    }
}
