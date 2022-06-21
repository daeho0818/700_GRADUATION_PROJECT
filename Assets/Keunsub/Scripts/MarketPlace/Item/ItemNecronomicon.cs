using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemNecronomicon : ItemBase
{
    public override void AtAttack(Entity monster)
    {
    }

    public override void AtButtonClick()
    {
    }

    public override void AtGameInit()
    {
        player.skillDamageIncrease += 0.25f;
    }

    public override void AtKill()
    {
        // 스킬 쿨타임 1초 감소
    }

    public override void AtOnDamage()
    {
    }

    public override void AtUpdate()
    {
    }

    public override void EndAttack()
    {
    }
}
