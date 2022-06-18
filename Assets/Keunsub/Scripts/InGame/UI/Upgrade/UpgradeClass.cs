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

    public override string Desc => "���� �ӵ��� 0.05�� ��ŭ �����մϴ�";

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

    public override string Desc => "�ִ� ü���� 20��ŭ �����մϴ�";

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

    public override string Desc => "���ݷ��� 5��ŭ �����մϴ�";

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

    public override string Desc => "ġ��Ÿ Ȯ���� 15% ��ŭ �����մϴ�";

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

    public override string Desc => "�뽬 ���ӽð��� 0.5�ʸ�ŭ �þ�ϴ�";

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

    public override string Desc => "�뽬 ��Ÿ���� 0.15�ʸ�ŭ �����մϴ�";

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

    public override string Desc => "����ġ ȹ�淮�� 0.15% �� �����մϴ�";

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

    public override string Desc => "MP ȸ�� ��Ÿ���� 0.25�� �����մϴ�";

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

    public override string Desc => "ü�� ȸ������ 0.15%��ŭ �� �����մϴ�";

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

    public override string Desc => "�̵� �ӵ��� 1��ŭ �� �����մϴ�";

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

