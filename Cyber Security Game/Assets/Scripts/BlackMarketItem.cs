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
    [SyncVar]
    public int id;
    Text BidText;
    [SyncVar]
    public int MinBid;

    public void Load()
    {
        manager = FindObjectOfType<GameManager>();
        BidText = transform.GetChild(4).GetComponent<Text>();
        BidText.text = ("Min bid: " + MinBid);
    }

    private void OnEnable()
    {
        if (active)
            BidText.text = ("Current bid " + BidAmount);
    }

    [Server]
    public void TurnUpdate()
    {
        manager = FindObjectOfType<GameManager>();
        if (active & (manager.Turns != BidTurn))
        {
            Debug.Log("Auction ended");
            Destroy(gameObject);
            FindObjectOfType<BlackMarket>(true).BuyItem(gameObject);
            RpcEndAuction();
        }
    }

    [ClientRpc]
    void RpcEndAuction()
    {
        Destroy(gameObject);
        transform.parent.GetComponent<BlackMarket>().BuyItem(gameObject);
    }

    [Server]
    public void Bid(int NewBid)
    {
        manager = FindObjectOfType<GameManager>();
        //int NewBid = int.Parse(transform.GetChild(0).GetComponent<InputField>().text);
        if (NewBid > BidAmount)
        {
            if (manager.PlayerTurn)
            {
                if (GameObject.Find("GCHQ(Clone)").GetComponent<Entity>().Resources >= NewBid)
                {
                    active = true;
                    BidAmount = NewBid;
                    BidTurn = manager.Turns;
                    LastBid = "Player";
                    RpcAlertText(1, NewBid);
                    RpcBidText();
                }
                else
                    RpcAlertText(0, NewBid);
            }
            else
            {
                if (GameObject.Find("SCS(Clone)").GetComponent<Entity>().Resources >= NewBid)
                {
                    if (GameObject.Find("EventCard(Clone)") && GameObject.Find("EventCard(Clone)").GetComponent<EventCard>().card == 4)
                        RpcAlertText(2, NewBid);
                    else
                    {
                        active = true;
                        BidAmount = NewBid;
                        BidTurn = manager.Turns;
                        LastBid = "Enemy";
                        RpcAlertText(1, NewBid);
                        RpcBidText();
                    }
                }
                else
                    RpcAlertText(0, NewBid);
            }
        }
        else
            RpcAlertText(3, NewBid);
    }

    [ClientRpc]
    void RpcAlertText(int i, int NewBid)
    {
        switch (i)
        {
            case 0:
                transform.GetChild(3).GetComponent<Announcement>().SetText("Not enough resources to bid");
                break;
            case 1:
                transform.GetChild(3).GetComponent<Announcement>().SetText("You are the highest bidder");
                if (manager.PlayerTurn && manager.player.hasAuthority)
                    GameObject.Find("GCHQ(Clone)").GetComponent<Entity>().SetResources(-NewBid);
                else if (!manager.PlayerTurn && manager.enemy.hasAuthority)
                    GameObject.Find("SCS(Clone)").GetComponent<Entity>().SetResources(-NewBid);
                break;
            case 2:
                transform.GetChild(3).GetComponent<Announcement>().SetText("Russia has been embargoed this month");
                break;
            case 3:
                transform.GetChild(3).GetComponent<Announcement>().SetText("You need to bid more than " + BidAmount);
                break;
        }
    }

    [ClientRpc]
    void RpcBidText()
    {
        StartCoroutine(ChangeText());
        ChangeText();
    }

    IEnumerator ChangeText()
    {
        yield return new WaitForSeconds(0.1f);
        BidText.text = ("Current bid " + BidAmount);
    }
}
