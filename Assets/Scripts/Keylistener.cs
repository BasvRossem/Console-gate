using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Keylistener
{
    public delegate void keyCallback();
    private Dictionary<KeyCode, keyCallback> keyCodes;

    public Keylistener()
    {
        keyCodes = new Dictionary<KeyCode, keyCallback>();
    }

    public bool addKey(KeyCode key, keyCallback callback)
    {
        if (keyCodes.ContainsKey(key))
        {
            return false;
        }

        keyCodes[key] = callback;
        return true;
    }

    public keyCallback getCallback(KeyCode key)
    {
        if (!keyCodes.ContainsKey(key))
        {
            return null;
        }
        return keyCodes[key];
    }

    public void executeKeyCallback(KeyCode key)
    {
        if (keyCodes.ContainsKey(key))
        {
            keyCodes[key]();
            return;
        }
    }
}
