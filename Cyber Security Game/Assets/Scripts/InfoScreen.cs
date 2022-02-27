using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoScreen : MonoBehaviour
{

    public GameObject canvas;
    public Sprite[] infoImg = new Sprite[5];

    public void Hide()
    {
        Destroy(canvas);
    }

    public void Show(int i)
    {
        if (i < 5)
        {
            GetComponent<Image>().sprite = infoImg[i];
            Instantiate(canvas, new Vector3(0, 0, 0), Quaternion.identity);
            
        }
    }
}
