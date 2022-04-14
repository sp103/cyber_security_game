using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ReadyButton : MonoBehaviour
{
    public void Ready()
    {
        foreach(NetworkRoomPlayer player in FindObjectsOfType<NetworkRoomPlayer>())
            if (player.hasAuthority)
                player.CmdChangeReadyState(true);
    }
}
