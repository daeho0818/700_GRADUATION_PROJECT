using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour
{
    [SerializeField]
    public UpgradeClass thisUpgrade;

    public Image Icon;
    public Text Level;
    public Text Desc;

    void Start()
    {

    }

    void Update()
    {

    }

    public void InitIcon(Sprite icon)
    {
        Icon.sprite = icon;
    }

    public void InitUpgrade(UpgradeClass upgrade)
    {
        thisUpgrade = upgrade;
        // to do
        // connect it to UI objects

        Level.text = "Lv. " + thisUpgrade.level.ToString();
        Desc.text = thisUpgrade.Desc;
    }

    public void Upgrade()
    {
        if (!InGameManager.Instance.upgradeTrigger)
        {
            thisUpgrade.Upgrade();
            Level.text = "Lv. " + thisUpgrade.level.ToString();
            InGameManager.Instance.UpgradeEnd();
        }
    }
}
