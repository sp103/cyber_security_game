using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayBlackMarketItem : MonoBehaviour
{
    public int card;
    public string owner;
    Dropdown dropdown;
    GameManager manager;

    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        dropdown = transform.GetChild(3).GetComponent<Dropdown>();
        if (owner == "Player" && (card == 1 || card == 0))
        {
            transform.GetChild(1).gameObject.SetActive(false);
            dropdown.gameObject.SetActive(true);
            dropdown.onValueChanged.AddListener(delegate { DropDownSelection(); });
            dropdown.options.Add(new Dropdown.OptionData("GCHQ -> Rosenergoatom"));
            dropdown.options.Add(new Dropdown.OptionData("UK Govenment -> Russian Government"));
        }
        if ((card == 7)||(card == 11)||(card == 14)||(card == 15))
        {
            transform.GetChild(1).gameObject.SetActive(false);
            dropdown.gameObject.SetActive(true);
            dropdown.onValueChanged.AddListener(delegate { DropDownSelection(); });
            if (owner == "Player")
                foreach (Transform child in GameObject.Find("PlayerArea").transform)
                    dropdown.options.Add(new Dropdown.OptionData(child.name));
            else if (owner == "Enemy")
                foreach (Transform child in GameObject.Find("EnemyArea").transform)
                    dropdown.options.Add(new Dropdown.OptionData(child.name));
        }
    }

    void DropDownSelection()
    {
        switch (card)
        {
            case 0:
            case 1:
                if (dropdown.value == 0)
                    manager.EnableAttackVector("Rosenergoatom", "GCHQ");
                else if (dropdown.value == 1)
                    manager.EnableAttackVector("Russian Government", "UK Government");
                break;
            case 7:
            case 11:
                // drop down
                // makes an entity immune from splash damage but can't send or recieve more than 2 resources

            break;
            case 14:
            case 15:
                // drop down
                // the cost of vitality for an entity costs one less resource
                if (owner == "Player")
                    GameObject.Find("PlayerArea").transform.GetChild(dropdown.value).GetComponent<Entity>().discount = true;
                else if (owner == "Enemy")
                    GameObject.Find("EnemyArea").transform.GetChild(dropdown.value).GetComponent<Entity>().discount = true;
                break;
        }
        Destroy(gameObject);
    }

    public void Play()
    {
        switch (card)
        {
            case 0:
            case 1:
                // if russia plays this card attack vector from scs to uk energy is enabled
                if (owner == "Enemy")
                    manager.EnableAttackVector("UK Energy","SCS");
            break;
            case 2:
                // electorate takes 1/2 damage for 3 turns
                GameObject.Find("Electorate").GetComponent<Entity>().DamageMultiplier = 0.5;
                GameObject.Find("Electorate").GetComponent<Entity>().MultiplierTurns = 3;
                break;
            case 3:
                // uk plc gets 1 vitality every time it takes damage
                GameObject.Find("PlayerArea").GetComponent<Player>().PLCDefence = true;
            break;
            case 4:
            case 5:
                // uk energy or rosenergoatom are immune for 2 turns
                if (owner == "Player")
                {
                    GameObject.Find("UK Energy").GetComponent<Entity>().DamageMultiplier = 0;
                    GameObject.Find("UK Energy").GetComponent<Entity>().MultiplierTurns = 2;
                }
                else if (owner == "Enemy")
                {
                    GameObject.Find("Rosenergoatom").GetComponent<Entity>().DamageMultiplier = 0;
                    GameObject.Find("Rosenergoatom").GetComponent<Entity>().MultiplierTurns = 2;
                }
            break;
            case 6:
                // russian government takes 1/2 damage for 3 turns
                GameObject.Find("Russian Government").GetComponent<Entity>().DamageMultiplier = 0.5;
                GameObject.Find("Russian Government").GetComponent<Entity>().MultiplierTurns = 3;
                break;
            case 8:
            case 9:
            case 10:
                // attack from GCHQ or SCS deals double damage to UK energy or Rosenergoatom
                if (owner == "Player")
                {
                    GameObject.Find("Rosenergoatom").GetComponent<Entity>().DamageMultiplier = 2;
                    GameObject.Find("Rosenergoatom").GetComponent<Entity>().MultiplierTurns = 0;
                }
                else if (owner == "Enemy")
                {
                    GameObject.Find("UK Energy").GetComponent<Entity>().DamageMultiplier = 2;
                    GameObject.Find("UK Energy").GetComponent<Entity>().MultiplierTurns = 0;
                }
                break;
            case 12:
            case 13:
                // attack against uk plc or electorate paralyses for 2 turns

            break;
        }
        Destroy(gameObject);
    }
}
