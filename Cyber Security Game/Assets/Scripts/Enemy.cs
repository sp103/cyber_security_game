using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class Enemy : NetworkBehaviour
{
    [SyncVar]
    public int VictoryPoints = 0;
    int RosenergoatomVitality = 0;
    int RosenergoatomMonths = 0;
    double BearVitality = 0;
    public Text text;
    GameManager manager;
    public GameObject gameManager;
    public GameObject[] entities = new GameObject[5];
    public GameObject reportScreen;

    public override void OnStartAuthority()
    {
        base.OnStartAuthority();
        GameObject report = Instantiate(reportScreen);
        report.transform.GetChild(1).GetComponent<Text>().text = "You are playing as Russia\n\nYour entities are at the top of the screen, click on these entities to find out how to get victory points.\nThe player with the most victory points at the end of 12 turns or after an attack leaves an entity with negative vitality wins.\n\nClick on the ? button for help";
        CmdFindManager();
    }

    [Command]
    void CmdFindManager()
    {
        manager = FindObjectOfType<GameManager>();
        manager.PlayerLoaded();
    }

    [Command]
    public void CmdStarter()
    {
        transform.SetParent(GameObject.Find("MainScreen").transform);
        transform.localPosition = new Vector3(34.5f, -50, 0);
        RpcSetPosition();
        for (int i = 0; i < entities.Length; i++)
        {
            GameObject entity = Instantiate(entities[i]);
            NetworkServer.Spawn(entity, connectionToClient);
            entity.transform.SetParent(transform);
            entity.transform.position = GameObject.Find("EnemySpawnpoints").transform.GetChild(i).transform.position;
            RpcEntityPosition(entity, i);
        }
    }

    [ClientRpc]
    void RpcEntityPosition(GameObject entity, int i)
    {
        entity.transform.SetParent(transform);
        entity.transform.position = GameObject.Find("EnemySpawnpoints").transform.GetChild(i).transform.position;
        entity.transform.position = new Vector3(entity.transform.position.x, entity.transform.position.y, 1);
    }

    [ClientRpc]
    void RpcSetPosition()
    {
        transform.SetParent(GameObject.Find("MainScreen").transform);
        transform.localPosition = new Vector3(34.5f, -50, 0);
    }

    [Command]
    public void CmdEndTurn()
    {
        if (!manager.PlayerTurn)
            manager.SvrEndTurn();
    }

    [Command]
    public void CmdPlaceBid(int bidAmount, int id)
    {
        foreach (BlackMarketItem item in FindObjectsOfType<BlackMarketItem>(true))
        {
            if (item.id == id)
                item.Bid(bidAmount);
        }
    }
    
    public void TurnUpdate()
    {
        foreach (Transform child in transform)
            child.GetComponent<Entity>().TurnUpdate();
    }

    public void MonthlyUpdate(string month)
    {
        if (transform.GetChild(1).GetComponent<Entity>().Resources > 3)
            VictoryPoints += 1;
        if (month == "April" && (transform.GetChild(4).GetComponent<Entity>().Vitality > BearVitality))
        {
            BearVitality = transform.GetChild(4).GetComponent<Entity>().Vitality;
            VictoryPoints += 1;
        }
        if (month == "August" && (transform.GetChild(4).GetComponent<Entity>().Vitality > BearVitality))
        {
            BearVitality = transform.GetChild(4).GetComponent<Entity>().Vitality;
            VictoryPoints += 3;
        }
        if (month == "December" && (transform.GetChild(4).GetComponent<Entity>().Vitality > BearVitality))
            VictoryPoints += 5;

        if (month == "March" || month == "June" || month == "September" || month == "December")
            QuarterlyUpdate();
        //text.text = (VictoryPoints + " Russian Victory Points");
    }

    public void QuarterlyUpdate()
    {
        if (transform.GetChild(4).GetComponent<Entity>().Vitality > RosenergoatomVitality)
        {
            switch (RosenergoatomMonths)
            {
                case 0:
                    VictoryPoints += 1;
                break;
                case 1:
                    VictoryPoints += 3;
                break;
                case 2:
                    VictoryPoints += 5;
                break;
                case 3:
                    VictoryPoints += 7;
                break;
            }
            RosenergoatomMonths++;
        }
    }

    public void IncrementVictoryPoints(int points)
    {
        CmdIncrementVictoryPoints(points);
    }

    [Command]
    void CmdIncrementVictoryPoints(int points)
    {
        VictoryPoints += points;
    }
}
