using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UpgradeUI : MonoBehaviour
{

    public UpgradeButton[] Buttons = new UpgradeButton[3];
    int buttonIdx;

    void Start()
    {

    }

    void Update()
    {
        for (int i = 0; i < Buttons.Length; i++)
        {
            if (i == buttonIdx)
            {
                Buttons[i].GetComponent<Image>().color = new Color(1, 1, 1, 1);
            }
            else
            {
                Buttons[i].GetComponent<Image>().color = new Color(0, 0, 0, 0.5f);
            }
        }

        ButtonInput();
    }

    void ButtonInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Buttons[buttonIdx].Upgrade();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (buttonIdx < Buttons.Length - 1)
                buttonIdx++;
            else buttonIdx = 0;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (buttonIdx > 0) buttonIdx--;
            else buttonIdx = Buttons.Length - 1;
        }
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

    public void InitButtonsIcon(params Sprite[] icons)
    {
        for (int i = 0; i < icons.Length; i++)
        {
            Buttons[i].InitIcon(icons[i]);
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
