using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class InfoScreen : MonoBehaviour
{

    public GameObject canvas;
    public Sprite[] infoImg = new Sprite[10];
    string[] entities = {"UK Government","UK Energy","UK PLC","GCHQ","Electorate","Russian Government","Online Trolls","SCS","Energetic Bear","Rosenergoatom" };

    public void Hide()
    {
        Destroy(canvas);
    }

    public void Show(string entity)
    {
        int entityID = Array.FindIndex(entities, row => row.Contains(entity));
        GameManager manager = GameObject.Find("GameManager(Clone)").GetComponent<GameManager>();
        GetComponent<Image>().sprite = infoImg[entityID];
        Instantiate(canvas, new Vector3(0, 0, 0), Quaternion.identity);
    }
}
