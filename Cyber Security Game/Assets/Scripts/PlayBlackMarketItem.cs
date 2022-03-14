using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayBlackMarketItem : MonoBehaviour
{
    public int card;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void Play()
    {
        switch (card)
        {
            case 0:
            case 1:
                // drop down
                // open resource route
            break;
            case 2:
                // electorate takes 1/2 damage for 3 turns

            break;
            case 3:
                // uk plc gets 1 vitality every time it takes damage
                Debug.Log("Play card 4");
            break;
            case 4:
            case 5:
                // uk energy or rosenergoatom are immune for 2 turns

            break;
            case 6:
                // russian government takes 1/2 damage for 3 turns

            break;
            case 7:
            case 11:
                // drop down
                // makes an entity immune from splash damage but can't send or recieve more than 2 resources
            break;
            case 8:
            case 9:
            case 10:
                // attack from GCHQ or SCS deals double damage to UK energy or Rosenergoatom

            break;
            case 12:
            case 13:
                // attack against uk plc or electorate paralyses for 2 turns

            break;
            case 14:
            case 15:
                // drop down
                // the cost of vitality for an entity costs one less resource
            break;
        }
    }
}
