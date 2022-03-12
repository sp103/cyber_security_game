using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackMarket : MonoBehaviour
{
    public GameObject Item;
    List<GameObject> BlackMarketItems = new List<GameObject>();
    public Sprite[] images = new Sprite[16];
    int height = 16 * 210;
    public OwnedBlackMarketItems OwnedItems;

    // Start is called before the first frame update
    void Start()
    {
        int y = 1565;
        for (int i = 0; i < images.Length; i++)
        { 
            GameObject item = Instantiate(Item);
            item.transform.SetParent(transform);
            item.transform.localPosition = new Vector3(0, y, 0);
            item.GetComponent<BlackMarketItem>().id = i;
            item.transform.GetChild(1).GetComponent<Image>().sprite = images[i];
            switch (i)   
            {
                case 4:
                case 5:
                case 7:
                case 11:
                    item.GetComponent<BlackMarketItem>().MinBid = 2;
                break;
                case 2:
                case 6:
                case 12:
                case 13:
                case 14:
                case 15:
                    item.GetComponent<BlackMarketItem>().MinBid = 3;
                break;
                case 3:
                case 8:
                case 9:
                case 10:
                    item.GetComponent<BlackMarketItem>().MinBid = 4;
                break;
                case 0:
                case 1:
                    item.GetComponent<BlackMarketItem>().MinBid = 5;
                break;
            }

            BlackMarketItems.Add(item);
            y -= 210;
        }
    }

    public void MonthlyUpdate()
    {
        for(int i = 0; i < BlackMarketItems.Count; i++)
        {
            if (BlackMarketItems[i].GetComponent<BlackMarketItem>().active == true)
                BlackMarketItems[i].GetComponent<BlackMarketItem>().TurnUpdate();
        }
    }

    public void BuyItem(GameObject RemoveItem)
    {
        BlackMarketItems.Remove(RemoveItem);
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
