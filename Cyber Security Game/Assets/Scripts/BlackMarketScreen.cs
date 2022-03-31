using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class BlackMarketScreen : NetworkBehaviour
{
    public GameObject screen;

    public void OpenScreen()
    {
        foreach (GameObject manager in GameObject.FindGameObjectsWithTag("GameManager"))
            if (manager.GetComponent<NetworkIdentity>().hasAuthority)
                if ((manager.GetComponent<GameManager>().PlayerTurn && GameObject.Find("PlayerArea(Clone)").GetComponent<NetworkIdentity>().hasAuthority) || (!manager.GetComponent<GameManager>().PlayerTurn && GameObject.Find("EnemyArea(Clone)").GetComponent<NetworkIdentity>().hasAuthority))
                    screen.GetComponent<OpenScreen>().show();
    }
}
