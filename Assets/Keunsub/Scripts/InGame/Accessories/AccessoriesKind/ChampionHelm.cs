using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChampionHelm : JewelryBase
{
    public override void AtAttackEnd()
    {
    }

    public override void AtAttackStart(Entity enemy)
    {
    }

    public override void AtAwake()
    {
        player.damage = player.damage + (player.damage * 0.05f);
        player.defence -= 0.05f;
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
    }

    public override void AtUseButton()
    {
    }
}
