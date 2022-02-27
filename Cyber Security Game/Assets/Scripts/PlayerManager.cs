using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerManager : NetworkBehaviour
{
    // On screen game objects
    public GameObject EventCard;
    public GameObject PlayerArea;
    public GameObject EnemyArea;
    public GameObject MainCanvas;
    // Arrays for storing attack vectors and resource routes
    public AttackVector[] AttackVectors;
    public ResourceRoute[] ResourceRoutes;
    // Object that will read the xml file
    XmlReader reader = new XmlReader();

    //Override function when client is started
    public override void OnStartClient()
    {
        // run base function
        base.OnStartClient();
        // find the player and enemy areas
        PlayerArea = GameObject.Find("PlayerArea");
        EnemyArea = GameObject.Find("EnemyArea");
    }

    // Override funtion when  a server is started
    [Server]
    public override void OnStartServer()
    {
        // run the base function
        base.OnStartServer();
        // Draw an event card at the start of the game
        GameObject card = Instantiate(EventCard);
        card.transform.SetParent(GameObject.Find("MainScreen").transform);
        // Load Attack vectors and Resource routes form Xml file
        reader.LoadData();
        AttackVectors = reader.LoadVectors();
        ResourceRoutes = reader.LoadRoutes();
        // debug check ** to be removed **
        Debug.Log(CheckResourceRoutes("GCHQ","UK Energy"));
        Entity Government = GameObject.Find("UK Government").GetComponent<Entity>();
        Government.Resources += 3;
    }

    [Server]
    // Function used for finding specific attack vectors
    public bool CheckAttackVectors(string To, string From)
    {
        foreach (AttackVector vector in AttackVectors)
        {
            if (vector.To == To && vector.From == From && vector.Enabled) return true;
        }
        return false;
    }

    [Server]
    // Function used for finding specific resource routes
    public bool CheckResourceRoutes(string To, string From)
    {
        foreach (ResourceRoute route in ResourceRoutes)
        {
            if (route.To == To && route.From == From) return true;
        }
        return false;
    }

    // Public function used to call the endturn command
    public void EndTurn()
    {
        CmdEndTurn();
    }

    // Command used for callig the server function
    [Command]
    void CmdEndTurn()
    {
        if (isServer) updateTurns();
    }

    // Server function that handles the end of a player's turn
    [Server]
    void updateTurns()
    {
        GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        gm.UpdateTurns();
        // Debugging call to send a message to clients
        RpcLogToClients("Turns Played" + gm.Turns);
        // Once both players have taken their turn an event card is drawn
        if (gm.Turns % 2 == 0)
        {
            Destroy(GameObject.Find("EventCard(Clone)"));
            GameObject card = Instantiate(EventCard);
            card.transform.SetParent(GameObject.Find("MainScreen").transform);
        }
    }

    // Debugging function to send a message to both clients
    [ClientRpc]
    void RpcLogToClients(string message)
    {
        Debug.Log(message);
    }
}
