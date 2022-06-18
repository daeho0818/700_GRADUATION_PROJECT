using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UpgradeUI : MonoBehaviour
{

    public UpgradeButton[] Buttons = new UpgradeButton[3];

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void InitButtons(params UpgradeClass[] upgrades)
    {
        for (int i = 0; i < Buttons.Length; i++)
        {
            Buttons[i].InitUpgrade(upgrades[i]);
            Buttons[i].GetComponent<RectTransform>().anchoredPosition = new Vector2((i - 1) * 900f, -1600);
            Buttons[i].GetComponent<RectTransform>().DOAnchorPosY(0f, 0.5f).SetDelay(i / 3f).SetEase(Ease.OutBack);
        }
    }

    public void RemoveButtons()
    {
        for (int i = 0; i < Buttons.Length; i++)
        {
            Buttons[i].GetComponent<RectTransform>().DOAnchorPosY(-1600f, 0.5f).SetEase(Ease.InBack);
        }
    }
}
