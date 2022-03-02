using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int Turns = 0;

    public void UpdateTurns()
    {
        if (Turns == 23) EndGame();
        Turns++;
    }

    public void EndGame()
    {

    }
}
