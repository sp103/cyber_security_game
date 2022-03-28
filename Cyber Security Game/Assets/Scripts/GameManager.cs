using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class GameManager : NetworkBehaviour
{
    public Player player;
    public Enemy enemy;
    public int Turns = 0;
    public bool PlayerTurn = true;
    // Arrays for storing attack vectors and resource routes
    public AttackVector[] AttackVectors;
    public ResourceRoute[] ResourceRoutes;
    // Object that will read the xml file
    XmlReader reader = new XmlReader();
    // On screen game objects
    public GameObject EventCard;
    string[] months = new string[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"};
    string month = "January";
    int quarter;
    public Announcement announcement;
    public Announcement PlayerAnnouncement;
    public Text VictoryPoints;
    public GameObject BlackMarket;

    [Server]
    public override void OnStartServer()
    {
        base.OnStartServer();
        // Load Attack vectors and Resource routes form Xml file
        reader.LoadData();
        AttackVectors = reader.LoadVectors();
        ResourceRoutes = reader.LoadRoutes();
        // Give government resources for the first turn
        GameObject.Find("UK Government").GetComponent<Entity>().Resources += 3;
        GameObject.Find("UK Government").GetComponent<Entity>().UpdateInterface();
        // Draw an event card
        GameObject card = Instantiate(EventCard);
        card.transform.SetParent(GameObject.Find("MainScreen").transform);
        announcement.SetText(month + " Q" + (quarter + 1));
        announcement.transform.SetAsLastSibling();
        PlayerAnnouncement.SetText("UK's Turn");
        PlayerAnnouncement.transform.SetAsLastSibling();
        VictoryPoints.text = "0 Victory Points";
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
            PlayerAnnouncement.SetText("UK's Turn");
            PlayerAnnouncement.transform.SetAsLastSibling();
            VictoryPoints.text = (player.VictoryPoints + " Victory Points");
        }
        else
        {
            if (!(GameObject.Find("EventCard(Clone)").GetComponent<EventCard>().card == 6))
                GameObject.Find("Russian Government").GetComponent<Entity>().Resources += 3;
            GameObject.Find("Russian Government").GetComponent<Entity>().UpdateInterface();
            PlayerAnnouncement.SetText("Russia's Turn");
            PlayerAnnouncement.transform.SetAsLastSibling();
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
        announcement.SetText(month + " Q" + (quarter + 1));
        announcement.transform.SetAsLastSibling();
    }

    public void EndGame()
    {
        Debug.Log("Game Ended");
    }
}
