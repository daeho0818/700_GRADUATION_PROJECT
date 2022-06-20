using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemBuyButton : MonoBehaviour
{
    public Image ItemIconImg;
    public Text ItemCostTxt;
    public ItemBase thisItem;

    public void Init(ItemBase _item)
    {
        thisItem = _item;

        ItemIconImg.sprite = thisItem.iconSprite;
        ItemIconImg.SetNativeSize();
        ItemIconImg.rectTransform.sizeDelta *= 3f;

        ItemCostTxt.text = string.Format("{0:#,##0}", thisItem.cost);
    } 

    public void BuyButton()
    {
        if(thisItem.cost <= GameManager.Instance.money)
        {
            GameManager.Instance.money -= thisItem.cost;
            thisItem.purchased = true;

        }
    }
}
