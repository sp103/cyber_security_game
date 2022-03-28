using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CustomNetworkLobby : NetworkRoomManager
{
    public GameObject PlayerPrefab2;
    bool hasPlayerSpawned = false;

    public override GameObject OnRoomServerCreateGamePlayer(NetworkConnectionToClient conn, GameObject roomPlayer)
    {
        if (!hasPlayerSpawned)
        {
            hasPlayerSpawned = true;
            return Instantiate(PlayerPrefab2, Vector3.zero, Quaternion.identity);
        }
        else
        {
            return Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        }
    }
}
