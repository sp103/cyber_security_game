using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class EndTurn : MonoBehaviour
{
    public void Clicked()
    {
        GameManager manager = FindObjectOfType<GameManager>();
        if (GameObject.Find("PlayerArea(Clone)").GetComponent<NetworkIdentity>().hasAuthority)
            GameObject.Find("PlayerArea(Clone)").GetComponent<Player>().CmdEndTurn();
        else if (GameObject.Find("EnemyArea(Clone)").GetComponent<NetworkIdentity>().hasAuthority)
            GameObject.Find("EnemyArea(Clone)").GetComponent<Enemy>().CmdEndTurn();
    }
}
