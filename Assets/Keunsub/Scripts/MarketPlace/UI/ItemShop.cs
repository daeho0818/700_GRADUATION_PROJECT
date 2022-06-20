using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemShop : MonoBehaviour
{
    public ItemBuyButton itemBtn;
    public Transform btnContainer;
    public List<ItemBuyButton> itemBtns = new List<ItemBuyButton>();
    MarketPlaceManager manager;

    public void Init(MarketPlaceManager _manager)
    {
        manager = _manager;

        foreach(var item in manager.StoreItems)
        {
            ItemBuyButton buttonTmp = Instantiate(itemBtn, btnContainer);
            buttonTmp.Init(item);
            itemBtns.Add(buttonTmp);
        }
    }

    public void RemoveButton(ItemBuyButton button)
    {
        if (itemBtns.Contains(button))
        {
            itemBtns.Remove(button);
            Destroy(button.gameObject);
        }
    }
}
