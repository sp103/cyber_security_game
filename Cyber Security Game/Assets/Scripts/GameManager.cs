using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class GameManager : NetworkBehaviour
{
    public static GameManager instance;

    // reference to uk player
    public Player player;
    // reference to russian player
    public Enemy enemy;

    public int Turns = 0;
    public bool PlayerTurn = true;

    // Arrays for storing attack vectors and resource routes
    public SyncList<AttackVector> AttackVectors = new SyncList<AttackVector>();
    public SyncList<ResourceRoute> ResourceRoutes = new SyncList<ResourceRoute>();

    // Object that will read the xml file
    XmlReader reader = new XmlReader();

    // event card prefab
    public GameObject EventCard;
    string[] months = new string[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"};
    string month = "January";
    int quarter;

    // text objects for displaying turn info to players
    public Text TurnData;
    public Text PlayerData;
    public Text VictoryPoints;

    public GameObject BlackMarket;

    // the number of players connected
    [SyncVar]
    int connections = 0;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    [ClientRpc]
    public void PlayerLoaded()
    {
        connections++;
        if (connections == 1)
        {
            // Load Attack vectors and Resource routes form Xml file
            reader.LoadData();
            AttackVectors = reader.LoadVectors();
            ResourceRoutes = reader.LoadRoutes();

            TurnData = GameObject.Find("TurnData").GetComponent<Text>();
            PlayerData = GameObject.Find("PlayerInfo").GetComponent<Text>();

            RpcDrawEventCard();
            // announcements ** TODO online **
            RpcDisplayInfo();
        }
    }

    [ClientRpc]
    void RpcDrawEventCard()
    {
        // Draw an event card
        GameObject card = Instantiate(EventCard);
        card.transform.SetParent(GameObject.Find("MainScreen").transform);
        NetworkServer.Spawn(card);
    }

    // Function used for finding specific attack vectors
    public bool CheckAttackVectors(string To, string From)
    {
        foreach (AttackVector vector in AttackVectors)
        {
            if (vector.To == To && vector.From == From && vector.Enabled) return true;
        }
        return false;
    }

    // Function used for Enabling specific attack vectors
    public void EnableAttackVector(string To, string From)
    {
        foreach (AttackVector vector in AttackVectors)
        {
            if (vector.To == To && vector.From == From)
                vector.Enabled = true;
        }
    }

    // Function used for finding specific resource routes
    public bool CheckResourceRoutes(string To, string From)
    {
        foreach (ResourceRoute route in ResourceRoutes)
        {
            if (route.To == To && route.From == From) return true;
        }
        return false;
    }

    public void EndTurn()
    {
        player.TurnUpdate();
        enemy.TurnUpdate();
        if (Turns % 2 == 1)
        {
            player.MonthlyUpdate(month);
            enemy.MonthlyUpdate(month);
        }
        BlackMarket.GetComponent<BlackMarket>().MonthlyUpdate();
        if (Turns == 23) EndGame();
        PlayerTurn = !PlayerTurn;
        if (PlayerTurn)
        {
            GameObject.Find("UK Government").GetComponent<Entity>().Resources += 3;
            GameObject.Find("UK Government").GetComponent<Entity>().UpdateInterface();
            VictoryPoints.text = (player.VictoryPoints + " Victory Points");
        }
        else
        {
            if (!(GameObject.Find("EventCard(Clone)").GetComponent<EventCard>().card == 6))
                GameObject.Find("Russian Government").GetComponent<Entity>().Resources += 3;
            GameObject.Find("Russian Government").GetComponent<Entity>().UpdateInterface();
            VictoryPoints.text = (enemy.VictoryPoints + " Victory Points");
        }
        Turns++;
        month = months[Turns / 2];
        quarter = Turns / 6;
        Debug.Log(month + " Q" + quarter);
        if (Turns % 2 == 0)
        {
            Destroy(GameObject.Find("EventCard(Clone)"));
            GameObject card = Instantiate(EventCard);
            card.transform.SetParent(GameObject.Find("MainScreen").transform);
        }
        RpcDisplayInfo();
    }

    // Display info about this turn
    [ClientRpc]
    void RpcDisplayInfo()
    {
        if (PlayerTurn)
            PlayerData.text = "UK's Turn";
        else
            PlayerData.text = "Russia's Turn";
        TurnData.text = month + " Q" + (quarter + 1);
    }

    public void EndGame()
    {
        Debug.Log("Game Ended");
    }
}
