using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarketPlaceManager : MonoBehaviour
{

    public List<ItemBase> StoreItems = new List<ItemBase>();
    public List<ItemBase> InventoryItems = new List<ItemBase>();

    public ItemShop itemShop;

    void Start()
    {
        InitItems();
        itemShop.Init(this);
    }

    public void InitItems()
    {
        // 아이템 정리

        StoreItems.Clear();
        InventoryItems.Clear();

        foreach (var item in GameManager.Instance.ItemList)
        {
            if (item.purchased)
                InventoryItems.Add(item);
            else
                StoreItems.Add(item);
        }
    }
}
