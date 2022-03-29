using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
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
    public Text TurnInfo;
    public Text PlayerInfo;
    public Text PlayerVictoryPoints;
    public Text EnemyVictoryPoints;
    public GameObject BlackMarket;

    private void Start()
    {
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
        DisplayInfo();
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
        }
        else
        {
            if (!(GameObject.Find("EventCard(Clone)").GetComponent<EventCard>().card == 6))
                GameObject.Find("Russian Government").GetComponent<Entity>().Resources += 3;
            GameObject.Find("Russian Government").GetComponent<Entity>().UpdateInterface();
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
        DisplayInfo();
    }

    public void EndGame()
    {
        Debug.Log("Game Ended");
    }

    void DisplayInfo()
    {
        TurnInfo.text = month + " Q" + (quarter + 1);
        if (PlayerTurn)
        {
            PlayerInfo.text = "UK's Turn";
            PlayerVictoryPoints.text = "You have " + player.VictoryPoints.ToString() + " victory points";
            EnemyVictoryPoints.text = "Russia has " + enemy.VictoryPoints.ToString() + " victory points";
        }
        else
        {
            PlayerInfo.text = "Russia's Turn";
            PlayerVictoryPoints.text = "You have " + enemy.VictoryPoints.ToString() + " victory points";
            EnemyVictoryPoints.text = "UK has " + player.VictoryPoints.ToString() + " victory points";
        }
    }
}
