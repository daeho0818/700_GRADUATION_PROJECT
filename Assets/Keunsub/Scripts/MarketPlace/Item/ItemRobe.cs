using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRobe : ItemBase
{

    int count = 2;
    float coolTime = 120f;
    float curCool = 0f;
    bool active;

    public override void AtAttack(Entity monster)
    {
    }

    public override void AtButtonClick()
    {
    }

    public override void AtGameInit()
    {
        count = 2;
        curCool = 0f;
        active = false;
    }

    public override void AtKill()
    {
    }

    public override void AtOnDamage()
    {
        if (active)
        {
            count--;

        }
    }

    public override void AtUpdate()
    {
        if (player.hp / player.max_hp < 0.1f && !active && curCool == 0f)
        {
            active = true;
        }

        if (active)
        {
            curCool += Time.deltaTime;
            if (curCool >= Time.deltaTime) active = false;
        }
    }

    public override void EndAttack()
    {
    }
}
