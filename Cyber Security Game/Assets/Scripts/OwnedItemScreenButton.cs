using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OwnedItemScreenButton : MonoBehaviour
{
    public GameObject canvas;
    GameObject previousScreen;

    // Start is called before the first frame update
    void Start()
    {
        canvas.SetActive(false);
        previousScreen = GameObject.Find("BlackMarketScreen");
    }

    public void hide()
    {
        previousScreen.SetActive(true);
        canvas.SetActive(false);
    }

    public void show()
    {
        canvas.SetActive(true);
        previousScreen.SetActive(false);
    }
}
