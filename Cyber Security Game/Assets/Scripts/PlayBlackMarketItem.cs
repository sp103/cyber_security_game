using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayBlackMarketItem : MonoBehaviour
{
    public int card;
    Dropdown dropdown;

    // Start is called before the first frame update
    void Start()
    {
        dropdown = transform.GetChild(3).GetComponent<Dropdown>();
        if (card == 1)
        {
            dropdown.gameObject.SetActive(true);
            dropdown.options.Add(new Dropdown.OptionData("hi"));
        }
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
        Destroy(gameObject);
    }
}
