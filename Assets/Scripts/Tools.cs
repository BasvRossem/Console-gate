using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tools
{
    public Tools()
    {
    }

    /// <summary>
    /// Check the condition and throw an error with a custom message if the condition is true.
    /// </summary>
    /// <param name="condition">Condition to check.</param>
    /// <param name="errorMessage">Message to throw on error.</param>
    /// <returns></returns>
    public static bool CheckError(bool condition, string errorMessage)
    {
        if (condition)
        {
            Debug.LogError(errorMessage);
        }
        return condition;
    }
}