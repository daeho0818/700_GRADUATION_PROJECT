using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class UpgradeClass
{
    public Player player;
    public int level;
    public abstract int maxLevel { get; }
    public abstract string Desc { get; }


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

    public override string Desc => "공격 속도가 0.05초 만큼 감소합니다";

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

    public override string Desc => "최대 체력이 20만큼 증가합니다";

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

    public override string Desc => "공격력이 5만큼 증가합니다";

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

    public override string Desc => "치명타 확률이 15% 만큼 증가합니다";

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

    public override string Desc => "대쉬 지속시간이 0.5초만큼 늘어납니다";

    public override UpgradeClass Upgrade()
    {
        if(level < maxLevel)
        {
            player.dashDelay += 0.05f;
            level++;
        }
        return this;
    }
}

public class UpgradeDashCool : UpgradeClass
{
    public override int maxLevel => 5;

    public override string Desc => "대쉬 쿨타임이 0.15초만큼 감소합니다";

    public override UpgradeClass Upgrade()
    {
        if (level < maxLevel)
        {
            level++;
            player.dashCool -= 0.15f;
        }
        return this;
    }
}

public class UpgradeEXP : UpgradeClass
{
    public override int maxLevel => 10;

    public override string Desc => "경험치 획득량이 0.15% 더 증가합니다";

    public override UpgradeClass Upgrade()
    {
        if(level < maxLevel)
        {
            level++;
            player.ExpAmount += 0.15f;
        }
        return this;
    }
}

public class UpgradeMP : UpgradeClass
{
    public override int maxLevel => 5;

    public override string Desc => "MP 회복 쿨타임이 0.25초 감소합니다";

    public override UpgradeClass Upgrade()
    {
        if(level < maxLevel)
        {
            level++;
            player.MpCool -= 0.25f;
        }

        return this;
    }
}

public class UpgradeHP : UpgradeClass
{
    public override int maxLevel => 5;

    public override string Desc => "체력 회복량이 0.15%만큼 더 증가합니다";

    public override UpgradeClass Upgrade()
    {
        if(level < maxLevel)
        {
            player.HpAmount += 0.15f;
            level++;
        }
        return this;
    }
}

public class UpgradeMoveSpeed : UpgradeClass
{
    public override int maxLevel => 5;

    public override string Desc => "이동 속도가 1만큼 더 증가합니다";

    public override UpgradeClass Upgrade()
    {
        if(level < maxLevel)
        {
            player.moveSpeed += 1f;
            level++;
        }
        return this;
    }
}

