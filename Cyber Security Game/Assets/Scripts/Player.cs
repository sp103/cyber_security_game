using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class Player : NetworkBehaviour
{
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

    public override void OnStartAuthority()
    {
        base.OnStartAuthority();
        transform.parent = GameObject.Find("MainScreen").transform;
        transform.localPosition = new Vector3(34.5f, -30, 0);
        cmdStarter();
    }

    [Command]
    void cmdStarter()
    {
        GameObject man = Instantiate(gameManager);
        manager = man.GetComponent<GameManager>();
        NetworkServer.Spawn(man, connectionToClient);
        manager.PlayerLoaded(gameObject);
        RpcSetPosition();
    }


    [ClientRpc]
    void RpcSetPosition()
    {
        transform.parent = GameObject.Find("MainScreen").transform;
        transform.localPosition = new Vector3(34.5f, -30, 0);
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
        //string info = ("UK Government Q" + quarter + " Report \n\n");
        //ReportScreen.transform.GetChild(1).GetComponent<Text>().text = info;
        //ReportScreen.GetComponent<OpenScreen>().show();
    }
}
