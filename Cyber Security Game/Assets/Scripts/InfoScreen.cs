using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoScreen : MonoBehaviour
{

    public GameObject canvas;
    public Sprite[] infoImg = new Sprite[10];
    GameManager manager;

    public void Hide()
    {
        Destroy(canvas);
    }

    public void Show(int i)
    {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (i < 5 && i >= 0 && manager.PlayerTurn)
        {
            GetComponent<Image>().sprite = infoImg[i];
            Instantiate(canvas, new Vector3(0, 0, 0), Quaternion.identity);
            
        }
        else if (i > 4 && i < 10 && !manager.PlayerTurn)
        {
            GetComponent<Image>().sprite = infoImg[i];
            Instantiate(canvas, new Vector3(0, 0, 0), Quaternion.identity);
        }
    }
}
