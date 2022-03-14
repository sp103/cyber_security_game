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
    public Entity fromEntity;
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

    public void SetEntity(Entity NewEntity, string func, Entity from)
    {
        Show();
        entity = NewEntity;
        function = func;
        fromEntity = from;
        transform.GetChild(3).GetComponent<Text>().text = ("Transfering from " + from.name + " to " + NewEntity.name);
        transform.GetChild(4).GetComponent<Text>().text = (from.Resources + " resources available");
    }

    public void EnterValue()
    {
        if (function == "revitalise")
            entity.Revitalise(int.Parse(input.text));
        else if (function == "transfer")
            entity.Transfer(fromEntity, int.Parse(input.text));
        else if (function == "attack")
            entity.Attack(fromEntity, int.Parse(input.text));
        input.text = "";
        Hide();
    }
}
