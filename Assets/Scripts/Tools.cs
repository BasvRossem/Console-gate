using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tools
{
    public Tools()
    {
    }

    public static bool CheckError(bool condition, string errorMessage)
    {
        if (condition)
        {
            Debug.LogError(errorMessage);
        }
        return condition;
    }
}