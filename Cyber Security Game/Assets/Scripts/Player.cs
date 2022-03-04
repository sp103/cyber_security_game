using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int VictoryPoints = 0;
    int GCHQVitality = 0;
    int GCHQQuarters = 0;


    public void MonthlyUpdate(string month)
    {
        if (month == "June" && (transform.GetChild(2).GetComponent<Entity>().Vitality >= 6))
            VictoryPoints += 2;
        if (month == "December" && (transform.GetChild(2).GetComponent<Entity>().Vitality >= 9))
            VictoryPoints += 3;
        if (transform.GetChild(3).GetComponent<Entity>().Resources >= 4)
            VictoryPoints += 1;
        if (month == "April" && (transform.GetChild(0).GetComponent<Entity>().Resources >= 3))
            VictoryPoints += 2;
        if (month == "August" && (transform.GetChild(0).GetComponent<Entity>().Resources >= 6))
            VictoryPoints += 3;
        if (month == "December" && (transform.GetChild(0).GetComponent<Entity>().Resources >= 9))
            VictoryPoints += 4;
        if (month == "March" || month == "June" || month == "September" || month == "December")
            QuarterlyUpdate();
        Debug.Log(VictoryPoints + " Victory Points");
    }
    public void QuarterlyUpdate()
    {
        if (transform.GetChild(4).GetComponent<Entity>().Vitality > GCHQVitality)
        {
            switch (GCHQQuarters)
            {
                case 0:
                    VictoryPoints += 1;
                break;
                case 1:
                    VictoryPoints += 3;
                break;
                case 2:
                    VictoryPoints += 5;
                break;
                case 3:
                    VictoryPoints += 7;
                break;
            }
        }
            
    }
}
