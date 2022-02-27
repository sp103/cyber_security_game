using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class EndTurnButton : NetworkBehaviour
{

    public PlayerManager PlayerManager;

    public void Click()
    {
        NetworkIdentity networkIdentity = NetworkClient.connection.identity;
        PlayerManager = networkIdentity.GetComponent<PlayerManager>();
        PlayerManager.EndTurn();
    }
}
