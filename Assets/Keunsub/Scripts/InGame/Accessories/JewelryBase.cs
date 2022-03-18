using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class JewelryBase : MonoBehaviour
{
    protected Player player;

    public void Init(Player _player)
    {
        player = _player;
    }

    public abstract void AtAwake();
    public abstract void AtStart();
    public abstract void AtUpdate();
    public abstract void AtAttackStart(Entity enemy);
    public abstract void AtAttackEnd();
    public abstract void AtDamaged();
    public abstract void AtEnd();
    public abstract void AtUseButton();
}
