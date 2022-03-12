using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackMarketItem : MonoBehaviour
{
    // Game manager GameObject
    GameManager manager;
    // Current highest bid
    int BidAmount;
    // The turn the highest bid was placed
    int BidTurn;
    // Is the item actively being bid on
    public bool active;
    // Who bid last
    string LastBid;
    // id to store item effects
    public int id;
    Announcement text;
    Text BidText;
    public int MinBid;

    private void Start()
    {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        text = transform.GetChild(3).GetComponent<Announcement>();
        BidText = transform.GetChild(4).GetComponent<Text>();
        BidText.text = ("Min bid: " + MinBid);
        BidAmount = MinBid - 1;
    }

    public void TurnUpdate()
    {
        int turn = manager.Turns;
        if (active & (turn != BidTurn))
        {
            if (LastBid == "Player")
            {
                Debug.Log("Player won");
                Destroy(gameObject);
                transform.parent.GetComponent<BlackMarket>().BuyItem(gameObject);
            }
            else
            {
                Debug.Log("Enemy won");
                Destroy(gameObject);
                transform.parent.GetComponent<BlackMarket>().BuyItem(gameObject);
            }
        }
    }

    public void Bid()
    {
        int NewBid = int.Parse(transform.GetChild(0).GetComponent<InputField>().text);
        if (NewBid > BidAmount)
        {
            if (manager.PlayerTurn)
            {
                if (GameObject.Find("GCHQ").GetComponent<Entity>().Resources >= NewBid)
                {
                    active = true;
                    LastBid = "Player";
                    BidAmount = NewBid;
                    BidTurn = manager.Turns;
                    transform.GetChild(0).GetComponent<InputField>().text = "";
                    text.SetText("You are the highest bidder");
                    BidText.text = ("Current bid " + BidAmount);
                }
                else
                    text.SetText("Not enough resources to bid");
            }
            else
            {
                if (GameObject.Find("SCS").GetComponent<Entity>().Resources >= NewBid)
                {
                    active = true;
                    LastBid = "Enemy";
                    BidAmount = NewBid;
                    BidTurn = manager.Turns;
                    transform.GetChild(0).GetComponent<InputField>().text = "";
                    text.SetText("You are the highest bidder");
                    BidText.text = "No Current Bids";
                    BidText.text = ("Current bid " + BidAmount);
                }
                else
                    text.SetText("Not enough resources to bid");
            }
        }
        else
            text.SetText("You need to bid more than " + BidAmount);
    }
}
