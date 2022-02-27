using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GameManager : NetworkBehaviour
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
