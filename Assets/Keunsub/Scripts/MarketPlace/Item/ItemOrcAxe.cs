using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Item/OrcAxe", order = 0)]
public class ItemOrcAxe : ItemBase
{
    public override void AtAttack(Entity monster)
    {
    }

    public override void AtButtonClick()
    {
    }

    public override void AtGameInit()
    {
        player.damageIncrease += 0.5f;
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
    }
}