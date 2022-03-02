using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventCard : MonoBehaviour
{

    public Sprite[] cardImages = new Sprite[9];
    public bool maximised;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Image>().sprite = cardImages[Random.Range(0, cardImages.Length)];
        Maximise();
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
        transform.localPosition = new Vector3(147, -272, 0);
    }
}
