using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BerserkerGloves : JewelryBase
{

    float defaultSpeed;

    public override void AtAttackEnd()
    {
    }

    public override void AtAttackStart(Entity enemy)
    {
    }

    public override void AtAwake()
    {
        defaultSpeed = player.attackSpeed;
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
        float reduce = player.maxHp - player.Hp;
        float increaseAmount = reduce / 5f;

        player.attackSpeed = defaultSpeed + (defaultSpeed * (100 / increaseAmount));
    }

    public override void AtUseButton()
    {
    }
}
