using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class BlackMarketItem : NetworkBehaviour
{
    // Game manager GameObject
    GameManager manager;
    // Current highest bid
    [SyncVar]
    public int BidAmount;
    // The turn the highest bid was placed
    [SyncVar]
    int BidTurn;
    // Is the item actively being bid on
    [SyncVar]
    public bool active;
    // Who bid last
    [SyncVar]
    string LastBid;
    // id to store item effects
    public int id;
    Announcement text;
    Text BidText;
    public int MinBid;

    private void Start()
    {
        manager = FindObjectOfType<GameManager>();
        text = transform.GetChild(3).GetComponent<Announcement>();
        BidText = transform.GetChild(4).GetComponent<Text>();
        BidText.text = ("Min bid: " + MinBid);
        BidAmount = MinBid - 1;
    }

    [Command]
    public void TurnUpdate()
    {
        RpcTurnUpdate();
    }

    void RpcTurnUpdate()
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
                if (GameObject.Find("GCHQ(Clone)").GetComponent<Entity>().Resources >= NewBid)
                {
                    BidAmount = NewBid;
                    CmdBid(NewBid);
                    transform.GetChild(0).GetComponent<InputField>().text = "";
                    text.SetText("You are the highest bidder");
                }
                else
                    text.SetText("Not enough resources to bid");
            }
            else
            {
                if (GameObject.Find("SCS(Clone)").GetComponent<Entity>().Resources >= NewBid)
                {
                    if (GameObject.Find("EventCard(Clone)") && GameObject.Find("EventCard(Clone)").GetComponent<EventCard>().card == 4)
                        text.SetText("Russia has been embargoed this month");
                    else
                    {
                        BidAmount = NewBid;
                        CmdBid(NewBid);
                        transform.GetChild(0).GetComponent<InputField>().text = "";
                        text.SetText("You are the highest bidder");
                    }
                }
                else
                    text.SetText("Not enough resources to bid");
            }
        }
        else
            text.SetText("You need to bid more than " + BidAmount);
    }

    void CmdBid(int bid)
    {
        SvrBid(bid);
    }

    [Server]
    void SvrBid(int bid)
    {
        Debug.Log("CALLED");
        active = true;
        BidAmount = bid;
        BidTurn = manager.Turns;
        if (manager.PlayerTurn)
            LastBid = "Player";
        else
            LastBid = "Enemy";
        RpcBidText();
    }

    [ClientRpc]
    void RpcBidText()
    {
        BidText.text = ("Current bid " + BidAmount);
    }
}
