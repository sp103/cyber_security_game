using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Mirror;

public class Entity : NetworkBehaviour, IDropHandler {

    [SyncVar]
    public int Vitality;
    [SyncVar]
    public int Resources;
    public PlayerManager PlayerManager;
    readonly int[] one = new int[] { 0, 1, 1, 1, 1, 2 };
    readonly int[] two = new int[] { 0, 1, 1, 1, 2, 2 };
    readonly int[] three = new int[] { -1, 0, 1, 2, 2, 3 };
    readonly int[] four = new int[] { -1, 0, 1, 2, 3, 4 };
    readonly int[] five = new int[] { -2, -1, 2, 3, 3, 4 };
    readonly int[] six = new int[] { -2, -1, 0, 3, 5, 6 };

    // Function called when dragged to from another object
    public void OnDrop(PointerEventData eventData)
    {
        // if eventData.pointerDrag tag = entity the playermanager should be checked for attack vectors and resource routes
        if (eventData.pointerDrag.tag == "Entity")
        {
            NetworkIdentity networkIdentity = NetworkClient.connection.identity;
            PlayerManager = networkIdentity.GetComponent<PlayerManager>();
            if (PlayerManager.CheckAttackVectors(name, eventData.pointerDrag.name))
            {
                Debug.Log("attack vector found");
                Attack(eventData.pointerDrag.GetComponent<Entity>(), 1);
            }
            else if (PlayerManager.CheckResourceRoutes(name, eventData.pointerDrag.name))
            {
                Debug.Log("resource route found");
                Transfer(eventData.pointerDrag.GetComponent<Entity>(), 1);
            }
        }
        else if (eventData.pointerDrag.tag == "Revitalise")
        {
            // NumInput input = GameObject.Find("NumInput").GetComponent<NumInput>();
            // input.Create();
            Debug.Log("revitalising");
            Revitalise(1);
        }
    }

    // Function that attacks this entity from the entered entity with the entered amount of resources
    void Attack(Entity from, int amount)
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
    void Transfer(Entity from, int amount)
    {
        if(from.Resources >= amount)
        {
            from.Resources -= amount;
            Resources += amount;
            Debug.Log(amount + " transfered from " + from.name + " to " + this.name);
        }
    }

    // Function that takes the entered amount of resources and turns it into vitality
    public void Revitalise(int cost)
    {
        if (Resources >= cost)
        {
            Resources -= cost;
            switch(cost)
            {
                case 1:
                    Vitality++;
                break;
                    
                case 2:
                    Vitality += 2;
                break;
                    
                case 4:
                    Vitality += 3;
                break;
                    
                case 5:
                    Vitality += 4;
                break;

                case 6:
                    Vitality += 5;
                break;

                case 7:
                    Vitality += 6;
                break;
            }
        }
    }

}
