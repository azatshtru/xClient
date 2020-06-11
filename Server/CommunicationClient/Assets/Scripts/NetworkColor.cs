using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NetworkColor
{
    public static Color GetColor (string s)
    {
        if(s == "Anubis")
        {
            return new Color(1f, 53f/255f, 53f/255f, 1f);
        }
        if (s == "Carton")
        {
            return new Color(130f/255f, 43f/255f, 1f, 1f);
        }

        return Color.white;
    }
}
