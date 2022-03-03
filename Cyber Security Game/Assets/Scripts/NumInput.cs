using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumInput : MonoBehaviour
{
    public GameObject canvas;
    public string function;
    public InputField input;
    public Entity entity;
    public int num = 0;

    public void Hide()
    {
        canvas.SetActive(false);
    }

    public void Show()
    {
        canvas.SetActive(true);
    }

    void Start()
    {
        Hide();
    }

    public void SetEntity(Entity NewEntity, string func)
    {
        Show();
        entity = NewEntity;
        function = func;
    }

    public void EnterValue()
    {
        if (function == "revitalise")
            entity.Revitalise(int.Parse(input.text));
        Hide();
    }
}
