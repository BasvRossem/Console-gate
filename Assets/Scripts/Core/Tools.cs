using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

public class Tools
{
    public delegate void WebCallback(string response);
    
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

    public static void ReadFile(MonoBehaviour owner, string path, WebCallback callback)
    {
        owner.StartCoroutine(GetText(path, callback));
        // StreamReader reader = new StreamReader(path);
        // string text = reader.ReadToEnd();
        // reader.Close();
        // return text;
    }
    
    public static IEnumerator GetText(string path, WebCallback callback)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(path))
        {
            yield return request.SendWebRequest();

            if (request.isNetworkError) // Error
            {
                Debug.Log(request.error);
            }
            else // Success
            {
                callback(request.downloadHandler.text);
                Debug.Log(request.downloadHandler.text);
            }
        }
    }
}