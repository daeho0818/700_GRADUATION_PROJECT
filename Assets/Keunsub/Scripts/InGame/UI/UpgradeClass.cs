using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UpgradeClass
{
    public Player player;
    public int level;
    public abstract int maxLevel { get; }


    public void Init(Player _player)
    {
        level = 0;
        player = _player;
    }

    public abstract UpgradeClass Upgrade();
}

public class UpgradeATKSpeed : UpgradeClass
{
    public override int maxLevel => 5;

    public override UpgradeClass Upgrade()
    {
        if (level < maxLevel)
        {
            player.attackDelay -= 0.05f;
            level++;
        }
        return this;
    }
}

public class UpgradeMaxHp : UpgradeClass
{
    public override int maxLevel => 10;

    public override UpgradeClass Upgrade()
    {
        if (level < maxLevel)
        {
            player.max_hp += 20;
            player.hp += 20;
            level++;
        }
        return this;
    }
}

public class UpgradeATKDMG : UpgradeClass
{
    public override int maxLevel => 10;

    public override UpgradeClass Upgrade()
    {
        if (level < maxLevel)
        {
            player.damage += 5;
            level++;
        }
        return this;
    }
}

public class UpgradeCritical : UpgradeClass
{
    public override int maxLevel => 10;

    public override UpgradeClass Upgrade()
    {
        if(level < maxLevel)
        {
            player.criticalChance += 15f;
            level++;
        }
        return this;
    }
}

public class UpgradeDashDelay : UpgradeClass
{
    public override int maxLevel => 10;

    public override UpgradeClass Upgrade()
    {
        if(level < maxLevel)
        {
            player.dashDelay += 0.1f;
            level++;
        }
        return this;
    }
}

public class UpgradeDashCool : UpgradeClass
{
    public override int maxLevel => 5;

    public override UpgradeClass Upgrade()
    {
        if (level < maxLevel)
        {
            level++;
            player.dashCool -= 0.25f;
        }
        return this;
    }
}

public class UpgradeEXP : UpgradeClass
{
    public override int maxLevel => throw new System.NotImplementedException();

    public override UpgradeClass Upgrade()
    {
        throw new System.NotImplementedException();
    }
}

public class UpgradeMP : UpgradeClass
{
    public override int maxLevel => throw new System.NotImplementedException();

    public override UpgradeClass Upgrade()
    {
        throw new System.NotImplementedException();
    }
}

public class UpgradeHP : UpgradeClass
{
    public override int maxLevel => throw new System.NotImplementedException();

    public override UpgradeClass Upgrade()
    {
        throw new System.NotImplementedException();
    }
}

public class UpgradeMoveSpeed : UpgradeClass
{
    public override int maxLevel => 5;

    public override UpgradeClass Upgrade()
    {
        throw new System.NotImplementedException();
    }
}

