using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HourGlass : JewelryBase
{

    bool skillAble;

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
        skillAble = true;
    }

    public override void AtUpdate()
    {
        if(player.Hp <= 0 && skillAble) 
        {
            skillAble = false;
            player.Hp = player.maxHp;
        }
    }

    public override void AtUseButton()
    {
    }
}
