using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SorcererRobe : JewelryBase
{
    int count;
    bool skillAble = true;

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
        if(!skillAble && count > 0)
        {
            count--;
            player.miss = true;
        }
    }

    public override void AtEnd()
    {
    }

    public override void AtStart()
    {
        count = 2;
    }

    public override void AtUpdate()
    {
        if(player.Hp <= 10 && skillAble)
        {
            skillAble = false;
        }
    }

    public override void AtUseButton()
    {
    }
}
