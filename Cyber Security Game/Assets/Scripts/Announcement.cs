using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Announcement : MonoBehaviour
{
    public void SetText(string text)
    {
        this.GetComponent<Text>().enabled = true;
        this.GetComponent<Text>().text = text;
        Invoke("EndAnnouncement", 3f);
    }

    void EndAnnouncement()
    {
        this.GetComponent<Text>().enabled = false;
    }
}
