using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackVector
{
    public string To;
    public string From;
    public bool Enabled;

    public AttackVector(string to, string from, string enabled)
    {
        To = to;
        From = from;
        if (enabled == "True")
        {
            Enabled = true;
        }
        else
        {
            Enabled = false;
        }
    }
}
