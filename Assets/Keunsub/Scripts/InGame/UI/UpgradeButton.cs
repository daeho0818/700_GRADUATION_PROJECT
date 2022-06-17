using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeButton : MonoBehaviour
{

    public UpgradeClass thisUpgrade;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void InitUpgrade(UpgradeClass upgrade)
    {
        thisUpgrade = upgrade;
        // to do
        // connect it to UI objects
    }

    public void Upgrade()
    {
        thisUpgrade.Upgrade();
    }
}
