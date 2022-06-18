using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUIManager : Singleton<InGameUIManager>
{

    public Image ExpImage;
    public Image HpBarContainer;
    public Image HpBar;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void SetHpBar(float hp, float maxHp)
    {
        // 1025 + 20 * add

        int add = (int)hp - 100;
        int cnt = add / 20;

        HpBarContainer.rectTransform.sizeDelta = new Vector2(1025 + cnt * 100, 70);
        HpBar.rectTransform.sizeDelta = new Vector2(990 + cnt * 100, 60);

        HpBar.fillAmount = hp / maxHp;
    }

    public void SetExpUI(float exp, float max)
    {
        ExpImage.fillAmount = exp / max;
    }
}
