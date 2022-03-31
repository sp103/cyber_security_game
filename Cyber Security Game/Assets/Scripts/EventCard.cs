using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class EventCard : NetworkBehaviour
{
    public Sprite[] cardImages = new Sprite[9];
    public bool maximised;
    public int card = 8;

    // Start is called before the first frame update
    public void SetCard(int cardNo)
    {
        card = cardNo;
        GetComponent<Image>().sprite = cardImages[card];
        Maximise();
        CardEffect(card);
    }

    void CardEffect(int card)
    {
        switch (card)
        {
            case 0:
                // Nuclear Meltdown
                Entity UkEnergy = GameObject.Find("UK Energy").GetComponent <Entity>();
                UkEnergy.Vitality -= 1;
                UkEnergy.UpdateInterface();
            break;
            case 1:
                // Clumsy civil servant
                Entity UkGov = GameObject.Find("UK Government").GetComponent<Entity>();
                UkGov.Resources -= 2;
                UkGov.UpdateInterface();
                Entity Electorate = GameObject.Find("Electorate").GetComponent<Entity>();
                Electorate.Vitality -= 1;
                Electorate.UpdateInterface();
                break;
            case 2:
                // Software Update
                Entity UkPlc = GameObject.Find("UK PLC").GetComponent<Entity>();
                UkPlc.Resources -= 2;
                UkPlc.UpdateInterface();
                break;
            case 5:
                // Lax Opsec
                Entity RussainGov = GameObject.Find("Russian Government").GetComponent<Entity>();
                RussainGov.Resources =- 1;
                RussainGov.Vitality = -1;
                RussainGov.UpdateInterface();
            break;
            case 7:
                // Quantum Breakthrough
                foreach (Transform child in GameObject.Find("PlayerArea(Clone)").transform)
                {
                    Entity entity = child.GetComponent<Entity>();
                    entity.Resources += 1;
                    entity.Vitality += 1;
                    entity.UpdateInterface();
                }
                foreach (Transform child in GameObject.Find("EnemyArea(Clone)").transform)
                {
                    Entity entity = child.GetComponent<Entity>();
                    entity.Resources += 1;
                    entity.Vitality += 1;
                    entity.UpdateInterface();
                }
                break;
        }

    }

    public void ChangeSize()
    {
        if (maximised == true)
        {
            Minimise();
        }
        else
        {
            Maximise();
        }
    }

    void Maximise()
    {
        maximised = true;
        transform.localScale = new Vector2(1,1);
        transform.localPosition = new Vector3(0, 0, 0);
    }

    void Minimise()
    {
        maximised = false;
        transform.localScale = new Vector2(0.25f,0.25f);
        transform.localPosition = new Vector3(30, -275, 0);
    }
}
