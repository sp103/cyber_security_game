using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Entity : MonoBehaviour, IDropHandler, IDragHandler {

    public GameObject input;
    public int Vitality;
    public int Resources;
    public GameManager GameManager;
    readonly int[] one = new int[] { 0, 1, 1, 1, 1, 2 };
    readonly int[] two = new int[] { 0, 1, 1, 1, 2, 2 };
    readonly int[] three = new int[] { -1, 0, 1, 2, 2, 3 };
    readonly int[] four = new int[] { -1, 0, 1, 2, 3, 4 };
    readonly int[] five = new int[] { -2, -1, 2, 3, 3, 4 };
    readonly int[] six = new int[] { -2, -1, 0, 3, 5, 6 };

    void Start()
    {
        GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Debug.Log(eventData.position);
    }

    // Function called when dragged to from another object
    public void OnDrop(PointerEventData eventData)
    {
        if (GameManager.PlayerTurn && this.transform.parent.name == "PlayerArea" || !GameManager.PlayerTurn && this.transform.parent.name == "EnemyArea")
        {
            // if eventData.pointerDrag tag = entity the playermanager should be checked for attack vectors and resource routes
            if (eventData.pointerDrag.tag == "Entity")
            {
                if (GameManager.CheckResourceRoutes(name, eventData.pointerDrag.name))
                {
                    // check if event card is block uk resource transfer
                    if (!(transform.parent.name == "PlayerArea" && GameObject.Find("EventCard(Clone)").GetComponent<EventCard>().card == 3))
                    {
                        input.GetComponent<NumInput>().SetEntity(this, "transfer", eventData.pointerDrag.GetComponent<Entity>());
                        Debug.Log("resource route found");
                    }
                }
            }
            else if (eventData.pointerDrag.tag == "Revitalise")
            {
                input.GetComponent<NumInput>().SetEntity(this, "revitalise");
                Debug.Log("revitalising");
            }
        }
        else if (!GameManager.PlayerTurn && this.transform.parent.name == "PlayerArea" || GameManager.PlayerTurn && this.transform.parent.name == "EnemyArea")
        {
            if (eventData.pointerDrag.tag == "Entity")
            {
                if (GameManager.CheckAttackVectors(name, eventData.pointerDrag.name))
                {
                    input.GetComponent<NumInput>().SetEntity(this, "attack", eventData.pointerDrag.GetComponent<Entity>());
                    Debug.Log("attack vector found");
                }
            }
        }
    }

    // Function that attacks this entity from the entered entity with the entered amount of resources
    public void Attack(Entity from, int amount)
    {
        if (from.Resources >= amount && amount <= 6)
        {
            from.Resources -= amount;
            int dice = Random.Range(0, 6);
            int attack = 0;
            Debug.Log(dice);
            switch (amount)
            {
                case 1:
                    attack = one[dice];
                break;
                case 2:
                    attack = two[dice];
                break;
                case 3:
                    attack = three[dice];
                break;
                case 4:
                    attack = four[dice];
                break;
                case 5:
                    attack = five[dice];
                break;
                case 6:
                    attack = six[dice];
                break;

            }
            Debug.Log(attack);
            if (attack >= 0)
            {
                this.Vitality -= attack;
            }
            else
            {
                from.Vitality -= (attack * -1);
            }
        }
    }

    // Function used to transfer the entered amount from the entered entity to this entity
    public void Transfer(Entity from, int amount)
    {
        if(from.Resources >= amount)
        {
            from.Resources -= amount;
            Resources += amount;
            Debug.Log(amount + " transfered from " + from.name + " to " + this.name);
        }
        from.UpdateInterface();
        UpdateInterface();
    }

    // Function that takes the entered amount of resources and turns it into vitality
    public void Revitalise(int cost)
    {
        if (Resources >= cost)
        {
            switch(cost)
            {
                case 1:
                    Vitality++;
                    Resources -= cost;
                break;
                    
                case 2:
                    Vitality += 2;
                    Resources -= cost;
                break;
                    
                case 4:
                    Vitality += 3;
                    Resources -= cost;
                break;
                    
                case 5:
                    Vitality += 4;
                    Resources -= cost;
                break;

                case 6:
                    Vitality += 5;
                    Resources -= cost;
                break;

                case 7:
                    Vitality += 6;
                    Resources -= cost;
                break;
            }
            UpdateInterface();
        }
    }

    public void UpdateInterface()
    {
        transform.GetChild(0).GetComponent<Text>().text = Vitality.ToString();
        transform.GetChild(1).GetComponent<Text>().text = Resources.ToString();
    }
}
