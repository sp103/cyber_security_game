using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Entity : MonoBehaviour, IDropHandler {

    // num input prefab
    public GameObject numInput;
    public double Vitality;
    public int Resources;
    public GameManager GameManager;
    readonly int[] one = new int[] { 0, 1, 1, 1, 1, 2 };
    readonly int[] two = new int[] { 0, 1, 1, 1, 2, 2 };
    readonly int[] three = new int[] { -1, 0, 1, 2, 2, 3 };
    readonly int[] four = new int[] { -1, 0, 1, 2, 3, 4 };
    readonly int[] five = new int[] { -2, -1, 2, 3, 3, 4 };
    readonly int[] six = new int[] { -2, -1, 0, 3, 5, 6 };
    // damage multiplier for black market cards
    public double DamageMultiplier = 1;
    // keeps track of how many turns the multiplier effect lasts
    public int MultiplierTurns = 0;
    // used to give revitalise discount from black market card
    public bool discount = false;
    // true if this entity has been paralysed from an attack
    public bool paralysed = false;
    // keeps track of how long the paralysed effect lasts
    public int paralysedTurns = 0;
    bool revitalised = false;

    void Start()
    {
        GameManager = FindObjectOfType<GameManager>();
    }

    // Function called when dragged to from another object
    public void OnDrop(PointerEventData eventData)
    {
        if ((GameManager.PlayerTurn && this.transform.parent.name == "PlayerArea(Clone)" || !GameManager.PlayerTurn && this.transform.parent.name == "EnemyArea(Clone)")&& !paralysed)
        {
            // if eventData.pointerDrag tag = entity the playermanager should be checked for attack vectors and resource routes
            if (eventData.pointerDrag.tag == "Entity")
            {
                if (GameManager.CheckResourceRoutes(name, eventData.pointerDrag.name))
                {
                    // check if event card is blocking uk resource transfer
                    if (!(transform.parent.name == "PlayerArea(Clone)" && GameObject.Find("EventCard(Clone)").GetComponent<EventCard>().card == 3))
                    {
                        Instantiate(numInput).GetComponent<NumInput>().SetEntity(this, "transfer", eventData.pointerDrag.GetComponent<Entity>());
                    }
                }
            }
            else if ((eventData.pointerDrag.tag == "Revitalise") && !revitalised)
            {
                Instantiate(numInput).GetComponent<NumInput>().SetEntity(this, "revitalise");
            }
        }
        else if (!GameManager.PlayerTurn && this.transform.parent.name == "PlayerArea(Clone)" || GameManager.PlayerTurn && this.transform.parent.name == "EnemyArea(Clone)")
        {
            if (eventData.pointerDrag.tag == "Entity" && DamageMultiplier != 0)
            {
                if (GameManager.CheckAttackVectors(name, eventData.pointerDrag.name))
                {
                    Instantiate(numInput).GetComponent<NumInput>().SetEntity(this, "attack", eventData.pointerDrag.GetComponent<Entity>());
                }
            }
        }
    }

    // Function that attacks this entity from the entered entity with the entered amount of resources
    public Vector3 Attack(Entity from, int amount)
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
            int attackerDamage = 0;
            if (attack >= 0)
            {
                this.Vitality -= attack * DamageMultiplier;
            }
            else
            {
                from.Vitality -= (attack * - 1);
                attackerDamage = attack * - 1;
                attack = 0;
            }
            if (from.name == "Online Trolls" && (amount == 3 || amount == 4))
                GameObject.Find("EnemyArea").GetComponent<Enemy>().VictoryPoints--;
            if (from.name == "Online Trolls" && (amount == 5 || amount == 6))
                GameObject.Find("EnemyArea").GetComponent<Enemy>().VictoryPoints -= 2;
            // check if uk plc defence card is active and give uk plc vitality if it is
            if (name == "UK PLC" && attack > 0)
                if (transform.parent.GetComponent<Player>().PLCDefence)
                    Vitality++;
            // update both entity's interfaces
            UpdateInterface();
            from.UpdateInterface();
            return new Vector3(dice, attackerDamage, attack);
        }
        return Vector3.zero;
    }

    // Function used to transfer the entered amount from the entered entity to this entity
    public void Transfer(Entity from, int amount)
    {
        if(from.Resources >= amount)
        {
            from.Resources -= amount;
            Resources += amount;
        }
        if (from.name == "Electorate")
            GameObject.Find("PlayerArea").GetComponent<Player>().VictoryPoints--;
        from.UpdateInterface();
        UpdateInterface();
    }

    //Function that takes the entered amount of resources and turns it into vitality
    public void Revitalise(int cost)
    {
        if (Resources >= cost)
        {
            switch (cost)
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
            revitalised = true;
            UpdateInterface();
        }
    }

    public void UpdateInterface()
    {
        transform.GetChild(0).GetComponent<Text>().text = Vitality.ToString();
        transform.GetChild(1).GetComponent<Text>().text = Resources.ToString();
    }

    public void TurnUpdate()
    {
        if (MultiplierTurns != 0)
            MultiplierTurns--;
        else if (DamageMultiplier != 1)
            DamageMultiplier = 1;
        if (paralysedTurns != 0)
            paralysedTurns--;
        else if (paralysed)
            paralysed = false;
        revitalised = false;
    }
}
