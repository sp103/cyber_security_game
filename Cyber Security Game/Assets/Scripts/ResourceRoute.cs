using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceRoute
{
    public string To;
    public string From;

    public ResourceRoute()
    {
        To = "None";
        From = "None";
    }

    public ResourceRoute(string to, string from)
    {
        To = to;
        From = from;
    }
}
