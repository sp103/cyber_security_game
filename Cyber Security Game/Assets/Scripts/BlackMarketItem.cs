using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class BlackMarketItem : NetworkBehaviour
{

    public int PlayerBid = 0;
    public int EnemyBid = 0;

    public void EndAuction()
    {
        if (PlayerBid > EnemyBid)
        {
            //player gets the item
        }
        else
        {
            //enemy gets the item
        }
    }
}
