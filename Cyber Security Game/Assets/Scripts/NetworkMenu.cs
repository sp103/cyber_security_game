using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class NetworkMenu : MonoBehaviour
{
    NetworkManager manager;
    public InputField input;


    void Awake()
    {
        manager = GetComponent<NetworkManager>();
    }

    public void Join()
    {
        if (input.text != "")
            manager.networkAddress = input.text;
        else
            manager.networkAddress = "34.142.63.187";
        if (!NetworkClient.active)
            manager.StartClient();
    }
}
