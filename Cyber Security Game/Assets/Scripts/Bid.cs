using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class Bid : MonoBehaviour
{
    public void PlaceBid()
    {
        Debug.Log("Sending bid to player");
        int bid = int.Parse(transform.parent.GetChild(0).GetComponent<InputField>().text);
        int id = transform.parent.GetComponent<BlackMarketItem>().id;
        transform.parent.GetChild(0).GetComponent<InputField>().text = "";
        if (GameObject.Find("PlayerArea(Clone)").GetComponent<NetworkIdentity>().hasAuthority)
            FindObjectOfType<Player>().CmdPlaceBid(bid, id);
        if (GameObject.Find("EnemyArea(Clone)").GetComponent<NetworkIdentity>().hasAuthority)
            FindObjectOfType<Enemy>().CmdPlaceBid(bid, id);
    }
}
