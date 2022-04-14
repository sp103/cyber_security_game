using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using UnityEngine.SceneManagement;

public class GameManager : NetworkBehaviour
{
    // reference to uk player
    public Player player;
    // reference to russian player
    public Enemy enemy;
    [SyncVar]
    public int Turns = 0;
    [SyncVar(hook = nameof(PlayerTurnUpdate))]
    public bool PlayerTurn = true;

    // Arrays for storing attack vectors and resource routes
    public SyncList<AttackVector> AttackVectors = new SyncList<AttackVector>();
    public SyncList<ResourceRoute> ResourceRoutes = new SyncList<ResourceRoute>();

    // Object that will read the xml file
    XmlReader reader = new XmlReader();

    // event card prefab
    public GameObject EventCard;
    string[] months = new string[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
    [SyncVar(hook = nameof(MonthChange))]
    public string month = "January";
    [SyncVar]
    public int quarter;

    // text objects for displaying turn info to players
    public Text TurnData;
    public Text PlayerData;
    Text PlayerVictoryPoints;
    Text EnemyVictoryPoints;

    public GameObject blackMarket;
    public GameObject reportScreen;

    public Sprite ukFlag;
    public Sprite RussianFlag;

    // funtion called on server once player object has loaded
    [Server]
    public void PlayerLoaded()
    {
        if (FindObjectOfType<Player>() && FindObjectOfType<Enemy>())
        {
            blackMarket = FindObjectOfType<BlackMarket>(true).gameObject;
            blackMarket.GetComponent<BlackMarket>().Load();
            // load data on server instance
            LoadData();
            // load data on all client instances
            RpcLoadDataOnClients();
            // if an event card has not already been drawen draw one
            //if (!GameObject.Find("EventCard(Clone)"))
            //    DrawEventCard();
        }
    }

    // function that calls LoadData on all clients
    [ClientRpc]
    public void RpcLoadDataOnClients()
    {
        blackMarket = FindObjectOfType<BlackMarket>(true).gameObject;
        LoadData();
        DisplayInfo();
    }

    public void LoadData()
    {
        //List<GameObject> items = new List<GameObject>();
        //foreach (BlackMarketItem item in FindObjectsOfType<BlackMarketItem>(true))
        //    blackMarket.GetComponent<BlackMarket>().BlackMarketItems.Add(item.gameObject);
        //    items.Add(item.gameObject);
        //items.Reverse();
        //blackMarket.GetComponent<BlackMarket>().BlackMarketItems = items;
        blackMarket.GetComponent<BlackMarket>().LoadItems();
        // read data from xml file
        reader.LoadData();
        AttackVectors = reader.LoadVectors();
        ResourceRoutes = reader.LoadRoutes();

        // find player objects
        player = FindObjectOfType<Player>();
        if (player.hasAuthority)
            player.CmdStarter();
        enemy = FindObjectOfType<Enemy>();
        if (enemy.hasAuthority)
            enemy.CmdStarter();

        // set text fields
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
    [Command(requiresAuthority = false)]
    public void CmdEnableAttackVector(string To, string From)
    {
        EnableAttackVector(To, From);
        RpcEnableAttackVector(To, From);
    }

    [ClientRpc]
    public void RpcEnableAttackVector(string To, string From)
    {
        EnableAttackVector(To, From);
        GameObject.Find(From + To).GetComponent<Image>().enabled = true;
        GameObject.Find(From + To).transform.GetChild(0).gameObject.SetActive(true);
    }

    public void EnableAttackVector(string To, string From)
    {
        Debug.Log(CheckAttackVectors(To,From));
        foreach (AttackVector vector in AttackVectors)
        {
            if (vector.To == To && vector.From == From)
            {
                AttackVectors.Remove(vector);
                Debug.Log("FOUND");
                vector.Enabled = true;
                AttackVectors.Add(vector);
            }
        }
        Debug.Log(CheckAttackVectors(To, From));
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

    void PlayerTurnUpdate(bool oldTurn, bool newTurn)
    {
        if (newTurn)
        {
            if (player.GetComponent<NetworkIdentity>().hasAuthority)
                GameObject.Find("UK Government(Clone)").GetComponent<Entity>().SetResources(3);
        }
        else
        {
            if (!GameObject.Find("EventCard(Clone)") || (GameObject.Find("EventCard(Clone)") && !(GameObject.Find("EventCard(Clone)").GetComponent<EventCard>().card == 6)))
            {
                if (enemy.GetComponent<NetworkIdentity>().hasAuthority)
                    GameObject.Find("Russian Government(Clone)").GetComponent<Entity>().SetResources(3);
            }
        }
        DisplayInfo();
    }

    void MonthChange(string oldMonth, string newMonth)
    {
        DisplayInfo();
    }


    [Server]
    public void SvrEndTurn()
    {
        player.TurnUpdate();
        enemy.TurnUpdate();
        if (Turns % 2 == 1)
        {
            player.MonthlyUpdate(month);
            enemy.MonthlyUpdate(month);
        }
        blackMarket.GetComponent<BlackMarket>().MonthlyUpdate();
        // end game after 24 turns
        if (Turns == 23)
        {
            SvrEndGame();
            return;
        }
            PlayerTurn = !PlayerTurn;
        // update month and quarter based off turns
        month = months[(Turns + 1) / 2];
        quarter = (Turns + 1) / 6;
        Turns++;
        if (Turns % 2 == 0)
            DrawEventCard();
        RpcDisplayInfo();
    }

    //public void EndTurn()
    //{
    //    player.TurnUpdate();
    //    enemy.TurnUpdate();
    //    if (Turns % 2 == 1)
    //    {
    //        player.MonthlyUpdate(month);
    //        enemy.MonthlyUpdate(month);
    //    }
    //    // BlackMarket.GetComponent<BlackMarket>().MonthlyUpdate();
    //    // end game after 24 turns
    //    if (Turns == 23)
    //        EndGame();

    //    Turns++;
    //    // update month and quarter based off turns
    //    month = months[Turns / 2];
    //    quarter = Turns / 6;
    //}

    // Display info about this turn
    [ClientRpc]
    void RpcDisplayInfo()
    {
        DisplayInfo();
    }

    void DisplayInfo()
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

    [Server]
    public void SvrEndGame()
    {
        RpcEndInfo();
        StartCoroutine(DelayEnd());
    }

    [ClientRpc]
    void RpcEndInfo()
    {
        GameObject report = Instantiate(reportScreen);
        report.transform.GetChild(2).GetComponent<Image>().preserveAspect = true;
        if (player.VictoryPoints > enemy.VictoryPoints)
        {
            report.transform.GetChild(1).GetComponent<Text>().text = "UK has won";
            report.transform.GetChild(2).gameObject.SetActive(true);
            report.transform.GetChild(2).GetComponent<Image>().sprite = ukFlag;
        }
        else if (enemy.VictoryPoints > player.VictoryPoints)
        {
            report.transform.GetChild(1).GetComponent<Text>().text = "Russia has won";
            report.transform.GetChild(2).gameObject.SetActive(true);
            report.transform.GetChild(2).GetComponent<Image>().sprite = RussianFlag;
        }
        else
            report.transform.GetChild(1).GetComponent<Text>().text = "Stalemate nobody won";

    }

    IEnumerator DelayEnd()
    {
        yield return new WaitForSeconds(10.0f);

        NetworkRoomManager.singleton.StopClient();
        NetworkRoomManager.singleton.StopHost();
        NetworkServer.DisconnectAll();
    }
}
