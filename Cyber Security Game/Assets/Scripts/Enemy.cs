using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class Enemy : NetworkBehaviour
{
    public int VictoryPoints = 0;
    int RosenergoatomVitality = 0;
    int RosenergoatomMonths = 0;
    double BearVitality = 0;
    public Text text;
    GameManager manager;
    public GameObject gameManager;
    public GameObject[] entities = new GameObject[5];

    public override void OnStartAuthority()
    {
        base.OnStartAuthority();
        cmdStarter();
    }

    [Command]
    void cmdStarter()
    {
        transform.parent = GameObject.Find("MainScreen").transform;
        transform.localPosition = new Vector3(34.5f, -50, 0);
        foreach (GameObject obj in entities)
        {
            GameObject entity = Instantiate(obj);
            NetworkServer.Spawn(entity, connectionToClient);
            entity.transform.parent = transform;
            RpcEntityPosition(entity);
        }
        manager = FindObjectOfType<GameManager>();
        manager.PlayerLoaded(gameObject);
        RpcSetPosition();
    }

    [ClientRpc]
    void RpcEntityPosition(GameObject entity)
    {
        entity.transform.SetParent(transform);
    }

    [ClientRpc]
    void RpcSetPosition()
    {
        transform.parent = GameObject.Find("MainScreen").transform;
        transform.localPosition = new Vector3(34.5f, -50, 0);
    }

    [Command]
    public void CmdEndTurn()
    {
        if (!manager.PlayerTurn)
            manager.SvrEndTurn();
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
}
