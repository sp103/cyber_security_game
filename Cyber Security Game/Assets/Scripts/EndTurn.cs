using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class EndTurn : NetworkBehaviour
{
    public void Clicked()
    {
        foreach (GameObject manager in GameObject.FindGameObjectsWithTag("GameManager"))
            if (manager.GetComponent<NetworkIdentity>().hasAuthority)
                if((manager.GetComponent<GameManager>().PlayerTurn && GameObject.Find("PlayerArea(Clone)").GetComponent<NetworkIdentity>().hasAuthority) || (!manager.GetComponent<GameManager>().PlayerTurn && GameObject.Find("EnemyArea(Clone)").GetComponent<NetworkIdentity>().hasAuthority))
                    manager.GetComponent<GameManager>().CmdEndTurn();
    }
}
