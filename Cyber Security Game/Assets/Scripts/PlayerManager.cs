using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerManager : NetworkBehaviour
{
    // On screen game objects
    public GameObject EventCard;
    public GameObject MainCanvas;
    public GameObject Entity;
    // Arrays for storing attack vectors and resource routes
    public AttackVector[] AttackVectors;
    public ResourceRoute[] ResourceRoutes;
    // Object that will read the xml file
    XmlReader reader = new XmlReader();
    [SyncVar]
    int players = 0;

    //Override function when client is started
    public override void OnStartClient()
    {
        // run base function
        base.OnStartClient();
        MainCanvas = GameObject.Find("MainScreen");
        players++;
        RpcLogToClients(players.ToString());
        if (players == 1)
        {
            GameObject.Find("GCHQ").GetComponent<NetworkIdentity>().AssignClientAuthority(connectionToClient);
            GameObject.Find("UK Energy").GetComponent<NetworkIdentity>().AssignClientAuthority(connectionToClient);
            GameObject.Find("UK Government").GetComponent<NetworkIdentity>().AssignClientAuthority(connectionToClient);
            GameObject.Find("UK PLC").GetComponent<NetworkIdentity>().AssignClientAuthority(connectionToClient);
            GameObject.Find("Electorate").GetComponent<NetworkIdentity>().AssignClientAuthority(connectionToClient);

        }
        else if (players == 2)
        {
            GameObject.Find("SCS").GetComponent<NetworkIdentity>().AssignClientAuthority(connectionToClient);
            GameObject.Find("Rosenergoatom").GetComponent<NetworkIdentity>().AssignClientAuthority(connectionToClient);
            GameObject.Find("Russian Government").GetComponent<NetworkIdentity>().AssignClientAuthority(connectionToClient);
            GameObject.Find("Energetic Bear").GetComponent<NetworkIdentity>().AssignClientAuthority(connectionToClient);
            GameObject.Find("Online Trolls").GetComponent<NetworkIdentity>().AssignClientAuthority(connectionToClient);
        }
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
