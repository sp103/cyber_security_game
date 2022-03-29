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
    public Slider AttackSlider;
    public Sprite[] images = new Sprite[6];
    public GameObject reportScreen;

    //public void Hide()
    //{
    //    transform.GetChild(0).gameObject.SetActive(true);
    //    transform.GetChild(5).gameObject.SetActive(false);
    //    transform.GetChild(6).gameObject.SetActive(false);
    //    canvas.SetActive(false);
    //    input.text = "";
    //}

    //public void Show()
    //{
    //    canvas.SetActive(true);
    //}

    void Start()
    {
        //Hide();
        AttackSlider.onValueChanged.AddListener(delegate { SliderValue(); });
    }

    public void SetEntity(Entity NewEntity, string func)
    {
        //Show();
        entity = NewEntity;
        function = func;
        transform.GetChild(3).GetComponent<Text>().text = ("Revitalising " + NewEntity.name);
        transform.GetChild(4).GetComponent<Text>().text = (entity.Resources + " resources available");
    }

    public void SetEntity(Entity NewEntity, string func, Entity from)
    {
        //Show();
        entity = NewEntity;
        function = func;
        fromEntity = from;
        if (func == "attack")
        {
            transform.GetChild(3).GetComponent<Text>().text = ("Attacking " + NewEntity.name);
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(5).gameObject.SetActive(true);
            transform.GetChild(6).gameObject.SetActive(true);
            transform.GetChild(6).GetComponent<Image>().sprite = images[(int)AttackSlider.value];
            if (from.Resources > 0 && from.Resources < 7)
                AttackSlider.maxValue = from.Resources - 1;
            else if (from.Resources == 0)
                AttackSlider.maxValue = 0;
        }
        if (func == "transfer")
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
        {
            Vector3 result = entity.Attack(fromEntity, ((int)AttackSlider.value + 1));
            AttackSlider.value = 0;
            reportScreen.transform.GetChild(1).GetComponent<Text>().text = ("Attack Report \n\n\n Attack launched from " + fromEntity.name + " to " + entity.name + "\n\n You rolled a " + result.x + "\n\n You suffered " + result.y + " damage \n\n" + entity.name + " suffered " + result.z + " damage");
            reportScreen.GetComponent<OpenScreen>().show();
        }
        //Hide();
        DestroyScreen();
    }

    void SliderValue()
    {
        transform.GetChild(6).GetComponent<Image>().sprite = images[(int)AttackSlider.value];
    }

    public void DestroyScreen()
    {
        Destroy(this.gameObject);
    }
}
