using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class BlackMarket : MonoBehaviour
{
    public GameObject Item;
    public List<BlackMarketItem> BlackMarketItems = new List<BlackMarketItem>();
    public Sprite[] images = new Sprite[16];
    int height = 16 * 210;
    public OwnedBlackMarketItems OwnedItems;

    [Server]
    public void Load()
    {
        for (int i = 0; i < images.Length; i++)
        {
            GameObject item = Instantiate(Item);
            item.GetComponent<BlackMarketItem>().id = i;
            switch (i)
            {
                case 4:
                case 5:
                case 7:
                case 11:
                    item.GetComponent<BlackMarketItem>().MinBid = 2;
                    item.GetComponent<BlackMarketItem>().BidAmount = 1;
                    break;
                case 2:
                case 6:
                case 12:
                case 13:
                case 14:
                case 15:
                    item.GetComponent<BlackMarketItem>().MinBid = 3;
                    item.GetComponent<BlackMarketItem>().BidAmount = 2;
                    break;
                case 3:
                case 8:
                case 9:
                case 10:
                    item.GetComponent<BlackMarketItem>().MinBid = 4;
                    item.GetComponent<BlackMarketItem>().BidAmount = 3;
                    break;
                case 0:
                case 1:
                    item.GetComponent<BlackMarketItem>().MinBid = 5;
                    item.GetComponent<BlackMarketItem>().BidAmount = 4;
                    break;
            }
            if (item.GetComponent<BlackMarketItem>().id != 11 && item.GetComponent<BlackMarketItem>().id != 12 && item.GetComponent<BlackMarketItem>().id != 13 && item.GetComponent<BlackMarketItem>().id != 7)
                NetworkServer.Spawn(item);
            //BlackMarketItems.Add(item);
        }
    }

    public void LoadItems()
    {
        BlackMarketItems = new List<BlackMarketItem> (FindObjectsOfType<BlackMarketItem>());
        BlackMarketItems.Reverse();
        Debug.Log(BlackMarketItems.Count);
        int y = BlackMarketItems.Count * 98;
        int i = 0;
        foreach (BlackMarketItem item in BlackMarketItems)
        {
            item.transform.GetChild(1).GetComponent<Image>().sprite = images[item.id];
            item.transform.SetParent(transform);
            item.transform.localPosition = new Vector3(0, y, 0);
            item.Load();
            y -= 210;
            i++;
        }
    }

    public void MonthlyUpdate()
    {
        for(int i = 0; i < BlackMarketItems.Count; i++)
        {
            if (BlackMarketItems[i].active == true)
                BlackMarketItems[i].TurnUpdate();
        }
    }

    public void BuyItem(GameObject RemoveItem)
    {
        Debug.Log(RemoveItem);
        BlackMarketItems.Remove(RemoveItem.GetComponent<BlackMarketItem>());
        int y = height / 2 - 210;
        for (int i = 0; i < BlackMarketItems.Count; i++)
        {
            BlackMarketItems[i].transform.localPosition = new Vector3(0, y, 0);
            y -= 210;
        }
        height -= 210;
        GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
        OwnedItems.AddItem(RemoveItem.GetComponent<BlackMarketItem>().id);
    }
}
