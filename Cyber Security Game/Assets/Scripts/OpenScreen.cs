using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenScreen : MonoBehaviour
{

    public GameObject canvas;

    // Start is called before the first frame update
    void Start()
    {
        canvas.SetActive(false);
    }

    public void hide()
    {
        canvas.SetActive(false);
    }

    public void show()
    {
        canvas.SetActive(true);
    }
}
