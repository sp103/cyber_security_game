using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OwnedBlackMarketItems : MonoBehaviour
{
    public GameObject Item;
    List<GameObject> PlayerItemsList = new List<GameObject>();
    List<GameObject> EnemyItemsList = new List<GameObject>();
    public Sprite[] images = new Sprite[16];
    int PlayerHeight = 0;
    int EnemyHeight = 0;
    public GameObject PlayerItems;
    public GameObject EnemyItems;
    public GameManager manager;

    public void OnEnable()
    {
        manager = FindObjectOfType<GameManager>();
        if (manager.PlayerTurn)
        {
            EnemyItems.SetActive(false);
            PlayerItems.SetActive(true);
            transform.parent.GetComponent<ScrollRect>().content = PlayerItems.GetComponent<RectTransform>();
        }
        else
        {
            PlayerItems.SetActive(false);
            EnemyItems.SetActive(true);
            transform.parent.GetComponent<ScrollRect>().content = EnemyItems.GetComponent<RectTransform>();
        }

    }

    public void AddItem(int ItemID)
    {
        manager = FindObjectOfType<GameManager>();
        if (!manager.PlayerTurn)
        {
            PlayerHeight += 210;
            PlayerItems.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, PlayerHeight);
            GameObject item = Instantiate(Item);
            item.transform.SetParent(PlayerItems.transform);
            item.transform.localPosition = new Vector3(0, -(PlayerHeight - 420), 0);
            item.GetComponent<PlayBlackMarketItem>().card = ItemID;
            item.GetComponent<PlayBlackMarketItem>().owner = "Player";
            item.transform.GetChild(0).GetComponent<Image>().sprite = images[ItemID];
            PlayerItemsList.Add(item);
            AlignElements(PlayerItemsList);
        }
        else
        {
            EnemyHeight += 210;
            EnemyItems.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, EnemyHeight);
            GameObject item = Instantiate(Item);
            item.transform.SetParent(EnemyItems.transform);
            item.transform.localPosition = new Vector3(0, -(EnemyHeight - 420), 0);
            item.GetComponent<PlayBlackMarketItem>().card = ItemID;
            item.GetComponent<PlayBlackMarketItem>().owner = "Enemy";
            item.transform.GetChild(0).GetComponent<Image>().sprite = images[ItemID];
            EnemyItemsList.Add(item);
            AlignElements(EnemyItemsList);
        }
    }

    List<GameObject> AlignElements(List<GameObject> list)
    {
        int offset = Mathf.Clamp(list.Count - 3, 0, list.Count) * 105;
        for (int i = 0; i < list.Count; i++)
            list[i].transform.position = new Vector3(list[i].transform.position.x, list[i].transform.position.y + offset, list[i].transform.position.z);
        return list;
    }

    public void RemoveItem(GameObject RemoveItem)
    {
        if (manager.PlayerTurn)
        {
            PlayerItemsList.Remove(RemoveItem);
            int y = PlayerHeight / 2 - 210;
            for (int i = 0; i < PlayerItemsList.Count; i++)
            {
                PlayerItemsList[i].transform.localPosition = new Vector3(0, y, 0);
                y -= 210;
            }
            PlayerHeight -= 210;
            GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, PlayerHeight);
        }
        else
        {
            EnemyItemsList.Remove(RemoveItem);
            int y = EnemyHeight / 2 - 210;
            for (int i = 0; i < EnemyItemsList.Count; i++)
            {
                EnemyItemsList[i].transform.localPosition = new Vector3(0, y, 0);
                y -= 210;
            }
            EnemyHeight -= 210;
            GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, EnemyHeight);
        }
    }
}
