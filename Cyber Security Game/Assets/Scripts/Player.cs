using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class Player : NetworkBehaviour
{
    [SyncVar]
    public int VictoryPoints = 0;
    int GCHQVitality = 0;
    int GCHQQuarters = 0;
    public Text VictoryPointText;
    public GameObject ReportScreen;
    // keep track of the quarter
    int quarter = 0;
    // black market defence card, true once played
    public bool PLCDefence = false;
    GameManager manager;
    public GameObject gameManager;
    public GameObject[] entities = new GameObject[5];
    public GameObject reportScreen;

    public override void OnStartAuthority()
    {
        base.OnStartAuthority();
        GameObject report = Instantiate(reportScreen);
        report.transform.GetChild(1).GetComponent<Text>().text = "You are playing as the UK\n\nYour entities are at the bottom of the screen, click on these entities to find out how to get victory points.\nThe player with the most victory points at the end of 12 turns or after an attack leaves an entity with negative vitality wins.\n\nClick on the ? button for help";
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
        transform.localPosition = new Vector3(34.5f, -30, 0);
        RpcSetPosition();
        for (int i = 0; i < entities.Length; i++)
        {
            GameObject entity = Instantiate(entities[i]);
            NetworkServer.Spawn(entity, connectionToClient);
            entity.transform.SetParent(transform);
            entity.transform.position = GameObject.Find("PlayerSpawnpoints").transform.GetChild(i).transform.position;
            RpcEntityPosition(entity, i);
        }
    }

    [ClientRpc]
    void RpcEntityPosition(GameObject entity, int i)
    {
        entity.transform.SetParent(transform);
        entity.transform.position = GameObject.Find("PlayerSpawnpoints").transform.GetChild(i).transform.position;
        entity.transform.position = new Vector3(entity.transform.position.x, entity.transform.position.y, 1);
        
    }


    [ClientRpc]
    void RpcSetPosition()
    {
        transform.SetParent(GameObject.Find("MainScreen").transform);
        transform.localPosition = new Vector3(34.5f, -30, 0);
    }

    [Command]
    public void CmdEndTurn()
    {
        if (manager.PlayerTurn)
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
        if (month == "June" && (transform.GetChild(2).GetComponent<Entity>().Vitality >= 6))
            VictoryPoints += 2;
        if (month == "December" && (transform.GetChild(2).GetComponent<Entity>().Vitality >= 9))
            VictoryPoints += 3;
        if (transform.GetChild(3).GetComponent<Entity>().Resources >= 4)
            VictoryPoints += 1;
        if (month == "April" && (transform.GetChild(0).GetComponent<Entity>().Resources >= 3))
            VictoryPoints += 2;
        if (month == "August" && (transform.GetChild(0).GetComponent<Entity>().Resources >= 6))
            VictoryPoints += 3;
        if (month == "December" && (transform.GetChild(0).GetComponent<Entity>().Resources >= 9))
            VictoryPoints += 4;
        if (month == "March" || month == "June" || month == "September" || month == "December")
            QuarterlyUpdate();
        //VictoryPointText.text = (VictoryPoints + " UK Victory Points");
    }
    public void QuarterlyUpdate()
    {
        quarter++;
        if (transform.GetChild(4).GetComponent<Entity>().Vitality > GCHQVitality)
        {
            switch (GCHQQuarters)
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
            GCHQQuarters++;
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
