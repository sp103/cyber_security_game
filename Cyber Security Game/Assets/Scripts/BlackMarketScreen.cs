using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class BlackMarketScreen : NetworkBehaviour
{
    public GameObject screen;

    public void OpenScreen()
    {
        GameManager manager = FindObjectOfType<GameManager>();
        if ((manager.PlayerTurn && GameObject.Find("PlayerArea(Clone)").GetComponent<NetworkIdentity>().hasAuthority) || (!manager.PlayerTurn && GameObject.Find("EnemyArea(Clone)").GetComponent<NetworkIdentity>().hasAuthority))
            screen.GetComponent<OpenScreen>().show();
    }
}
