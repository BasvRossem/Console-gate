﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class Tools
{
    /// <summary>
    /// Check the condition and throw an error with a custom message if the condition is true.
    /// </summary>
    /// <param name="condition">Condition to check.</param>
    /// <param name="errorMessage">Message to throw on condition.</param>
    /// <returns>Condition outcome</returns>
    public static bool CheckError(bool condition, string errorMessage)
    {
        if (condition)
        {
            Debug.LogError(errorMessage);
        }
        return condition;
    }

    /// <summary>
    /// Check the condition and throw a warning with a custom message if the condition is true.
    /// </summary>
    /// <param name="condition">Condition to check.</param>
    /// <param name="errorMessage">Message to throw on condition.</param>
    /// <returns>Condition outcome</returns>
    public static bool CheckWarning(bool condition, string errorMessage)
    {
        if (condition)
        {
            Debug.LogWarning(errorMessage);
        }
        return condition;
    }

    public static string ReadFile(string path)
    {
        StreamReader reader = new StreamReader(path);
        string text = reader.ReadToEnd();
        reader.Close();
        return text;
    }
}