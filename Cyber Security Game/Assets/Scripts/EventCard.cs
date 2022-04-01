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
                Entity UkEnergy = GameObject.Find("UK Energy(Clone)").GetComponent <Entity>();
                if (UkEnergy.hasAuthority)
                    UkEnergy.SetVitality(-1);
            break;
            case 1:
                // Clumsy civil servant
                Entity UkGov = GameObject.Find("UK Government(Clone)").GetComponent<Entity>();
                if (UkGov.hasAuthority)
                    UkGov.SetResources(-2);
                Entity Electorate = GameObject.Find("Electorate(Clone)").GetComponent<Entity>();
                if (Electorate.hasAuthority)
                    Electorate.SetVitality(-1);
                break;
            case 2:
                // Software Update
                Entity UkPlc = GameObject.Find("UK PLC(Clone)").GetComponent<Entity>();
                if (UkPlc.hasAuthority)
                    UkPlc.SetVitality(-2);
                break;
            case 5:
                // Lax Opsec
                Entity RussainGov = GameObject.Find("Russian Government(Clone)").GetComponent<Entity>();
                if (RussainGov.hasAuthority)
                {
                    RussainGov.SetResources(-1);
                    RussainGov.SetVitality(-1);
                }
                break;
            case 7:
                // Quantum Breakthrough
                foreach (Transform child in GameObject.Find("PlayerArea(Clone)").transform)
                {
                    Entity entity = child.GetComponent<Entity>();
                    if (entity.hasAuthority)
                    {
                        entity.SetResources(1);
                        entity.SetVitality(1);
                    }
                }
                foreach (Transform child in GameObject.Find("EnemyArea(Clone)").transform)
                {
                    Entity entity = child.GetComponent<Entity>();
                    if (entity.hasAuthority)
                    {
                        entity.SetResources(1);
                        entity.SetVitality(1);
                    }
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
