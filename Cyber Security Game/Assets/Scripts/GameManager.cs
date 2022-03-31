using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class GameManager : NetworkBehaviour
{
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
    string[] months = new string[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
    string month = "January";
    int quarter;

    // text objects for displaying turn info to players
    public Text TurnData;
    public Text PlayerData;
    Text PlayerVictoryPoints;
    Text EnemyVictoryPoints;

    public GameObject BlackMarket;

    // funtion called on server once player object has loaded
    [Server]
    public void PlayerLoaded(GameObject calledFrom)
    {
        if (calledFrom.name == "PlayerArea(Clone)")
            player = calledFrom.GetComponent<Player>();
        if (calledFrom.name == "EnemyArea(Clone)")
            enemy = calledFrom.GetComponent<Enemy>();
        // load data on server instance
        LoadData();
        // load data on all client instances
        RpcLoadDataOnClients();
        // if an event card has not already been drawen draw one
        if (!GameObject.Find("EventCard(Clone)"))
            DrawEventCard();
        // display turn info on clients
        RpcDisplayInfo();
    }

    // function that calls LoadData on all clients
    [ClientRpc]
    public void RpcLoadDataOnClients()
    {
        LoadData();
    }

    public void LoadData()
    {
        reader.LoadData();
        AttackVectors = reader.LoadVectors();
        ResourceRoutes = reader.LoadRoutes();

        if (GameObject.Find("PlayerArea(Clone)"))
            player = GameObject.Find("PlayerArea(Clone)").GetComponent<Player>();
        if (GameObject.Find("EnemyArea(Clone)"))
            enemy = GameObject.Find("EnemyArea(Clone)").GetComponent<Enemy>();

        TurnData = GameObject.Find("TurnData").GetComponent<Text>();
        PlayerData = GameObject.Find("PlayerInfo").GetComponent<Text>();

        PlayerVictoryPoints = GameObject.Find("PlayerVictoryPoints").GetComponent<Text>();
        EnemyVictoryPoints = GameObject.Find("EnemyVictoryPoints").GetComponent<Text>();
    }

    [Server]
    public void DrawEventCard()
    {
        if (GameObject.Find("EventCard(Clone)"))
            Destroy(GameObject.Find("EventCard(Clone)"));
        // Draw an event card
        GameObject card = Instantiate(EventCard);
        NetworkServer.Spawn(card);
        int num = Random.Range(0, 9);
        card.GetComponent<EventCard>().SetCard(num);
        DrawCardOnClient(card, num);
    }

    [ClientRpc]
    void DrawCardOnClient(GameObject card, int num)
    {
        card.transform.SetParent(GameObject.Find("MainScreen").transform);
        card.GetComponent<EventCard>().SetCard(num);
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


    [Command]
    public void CmdEndTurn()
    {
        RpcEndTurn();
        RpcDisplayInfo();
        foreach (GameObject manager in GameObject.FindGameObjectsWithTag("GameManager"))
            manager.GetComponent<GameManager>().Turns++;
        if (Turns % 2 == 0)
            DrawEventCard();
    }

    [ClientRpc]
    public void RpcEndTurn()
    {
        foreach (GameObject manager in GameObject.FindGameObjectsWithTag("GameManager"))
            manager.GetComponent<GameManager>().EndTurn();
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
        // BlackMarket.GetComponent<BlackMarket>().MonthlyUpdate();
        // end game after 24 turns
        if (Turns == 23) EndGame();

        PlayerTurn = !PlayerTurn;
        // give government entities resources at the start of next turn
        if (PlayerTurn)
        {
            GameObject.Find("UK Government").GetComponent<Entity>().Resources += 3;
            GameObject.Find("UK Government").GetComponent<Entity>().UpdateInterface();
            //VictoryPoints.text = (player.VictoryPoints + " Victory Points");
        }
        else
        {
            if (!(GameObject.Find("EventCard(Clone)").GetComponent<EventCard>().card == 6))
                GameObject.Find("Russian Government").GetComponent<Entity>().Resources += 3;
            GameObject.Find("Russian Government").GetComponent<Entity>().UpdateInterface();
            //VictoryPoints.text = (enemy.VictoryPoints + " Victory Points");
        }
        // update number of turns
        Turns++;
        // update month and quarter based off turns
        month = months[Turns / 2];
        quarter = Turns / 6;
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
        if (player.GetComponent<NetworkIdentity>().hasAuthority)
        {
            PlayerVictoryPoints.text = "You have " + player.VictoryPoints.ToString() + " victory points";
            EnemyVictoryPoints.text = "Russia has " + enemy.VictoryPoints.ToString() + " victory points";
        }
        else
        {
            PlayerVictoryPoints.text = "You have " + enemy.VictoryPoints.ToString() + " victory points";
            EnemyVictoryPoints.text = "UK has " + player.VictoryPoints.ToString() + " victory points";
        }
    }

    public void EndGame()
    {
        Debug.Log("Game Ended");
    }
}
