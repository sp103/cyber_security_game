using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumInput : MonoBehaviour
{
    public GameObject canvas;
    public InputField input;
    public int num = 0;

    public void Hide()
    {
        Destroy(canvas);
    }

    public void Create()
    {
        Instantiate(canvas, new Vector3(0, 0, 0), Quaternion.identity);
    }

    public void EnterNum()
    {
        Hide();
        num = int.Parse(input.text);
    }
}
