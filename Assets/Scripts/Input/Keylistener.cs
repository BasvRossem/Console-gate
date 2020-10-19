using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public enum KeyBoardOptions
{
    Alphabetical,
    Numerical,
    Function,
    Interpunction,
    Any
}

/// <summary>
/// Custom comparer for lists of KeyCodes
/// </summary>
internal class KeyCodeComparer : IEqualityComparer<List<KeyCode>>
{
    /// <summary>
    /// Checks that all occurances are equal in both lists, rather than checking reference locations
    /// </summary>
    /// <param name="x">LHS list of keycodes</param>
    /// <param name="y">RHS list of keycodes</param>
    /// <returns>Bool to return</returns>
    public bool Equals(List<KeyCode> x, List<KeyCode> y)
    {
        return x.All(y.Contains);
    }

    /// <summary>
    /// Makes a hashcode generated from the contents
    /// </summary>
    /// <param name="obj">list of keycodes</param>
    /// <returns>int hashcode</returns>
    public int GetHashCode(List<KeyCode> obj)
    {
        int hCode = obj.Sum(x => (int)x);
        return hCode.GetHashCode();
    }
}

public class Keylistener : MonoBehaviour
{
    // All non-mouse/joystick keydowns
    private static readonly KeyCode[] keyCodes = Enum.GetValues(typeof(KeyCode))
        .Cast<KeyCode>()
        .Where(k => ((int)k < (int)KeyCode.Mouse0))
        .ToArray();

    private KeyCodeComparer customComparer;
    private Dictionary<List<KeyCode>, UnityEvent<List<KeyCode>>> subscribedKeyEvents;
    private List<KeyCode> _keysDown;

    public void Awake()
    {
        customComparer = new KeyCodeComparer();
        subscribedKeyEvents = new Dictionary<List<KeyCode>, UnityEvent<List<KeyCode>>>(customComparer);
    }

    /// <summary>
    /// Resets the list of keys down
    /// </summary>
    public void OnEnable()
    {
        _keysDown = new List<KeyCode>();
    }

    /// <summary>
    /// Nulls the keydown list
    /// </summary>
    public void OnDisable()
    {
        _keysDown = null;
    }

    /// <summary>
    /// Catches all pressed keys and adds them to the stored keydown list.
    /// Catches all keypress releases and invokes the callback for all released keypresses
    /// </summary>
    public void Update()
    {
        // Catch all keydowns
        if (Input.anyKeyDown)
        {
            for (int i = 0; i < keyCodes.Length; i++)
            {
                KeyCode kc = keyCodes[i];
                if (Input.GetKeyDown(kc))
                {
                    _keysDown.Add(kc);
                }
            }
        }

        // Catch all keyups
        if (_keysDown.Count > 0)
        {
            List<KeyCode> _keysUp = new List<KeyCode>();
            for (int i = 0; i < _keysDown.Count; i++)
            {
                KeyCode kc = _keysDown[i];
                if (Input.GetKeyUp(kc))
                {
                    _keysDown.RemoveAt(i);
                    i--;
                    _keysUp.Add(kc);
                }
            }
            // Invoke callbacks with all keyups
            executeKeyCallback(_keysUp);
        }
    }

    /// <summary>
    /// Adds a UnityAction mapped to a list of common keypresses
    /// </summary>
    /// <param name="key">The combination of keys pressed in order to fire the callback</param>
    /// <param name="callback">The UnityAction to invoke after the required keys are pressed</param>
    /// <returns>bool wether the adding is succesfull. A false indicates that the keys are already in use.</returns>
    public bool addKey(List<KeyCode> key, UnityAction<List<KeyCode>> callback)
    {
        if (!subscribedKeyEvents.ContainsKey(key))
        {
            subscribedKeyEvents.Add(key, new UnityEvent<List<KeyCode>>());
        }

        subscribedKeyEvents[key].AddListener(callback);
        return true;
    }

    /// <summary>
    /// Adds a number key combinations that are grouped together.
    /// </summary>
    /// <param name="option">The KeyBoardOption enum</param>
    /// <param name="callback">The UnityAction to invoke after the required keys are pressed</param>
    /// <returns></returns>
    public bool addOption(KeyBoardOptions option, UnityAction<List<KeyCode>> callback)
    {
        List<KeyCode> keys = new List<KeyCode>();
        switch (option)
        {
            case KeyBoardOptions.Alphabetical:
                keys.AddRange(Enum.GetValues(typeof(KeyCode)).Cast<KeyCode>().Where(k => (int)k >= (int)KeyCode.A && (int)k <= (int)KeyCode.Z).ToArray());
                break;

            case KeyBoardOptions.Numerical:
                keys.AddRange(Enum.GetValues(typeof(KeyCode)).Cast<KeyCode>().Where(k => (int)k >= (int)KeyCode.Alpha0 && (int)k <= (int)KeyCode.Alpha9).ToArray());
                keys.AddRange(Enum.GetValues(typeof(KeyCode)).Cast<KeyCode>().Where(k => (int)k >= (int)KeyCode.Keypad0 && (int)k <= (int)KeyCode.Keypad9).ToArray());
                break;

            case KeyBoardOptions.Function:
                keys.AddRange(Enum.GetValues(typeof(KeyCode)).Cast<KeyCode>().Where(k => (int)k >= (int)KeyCode.F1 && (int)k <= (int)KeyCode.F15).ToArray());
                break;

            case KeyBoardOptions.Interpunction:
                keys.Add(KeyCode.Period);
                keys.Add(KeyCode.KeypadPeriod);
                keys.Add(KeyCode.Comma);
                keys.Add(KeyCode.Slash);
                keys.Add(KeyCode.Backslash);
                keys.Add(KeyCode.Colon);
                keys.Add(KeyCode.Semicolon);
                break;

            case KeyBoardOptions.Any:
                keys.AddRange(Enum.GetValues(typeof(KeyCode)).Cast<KeyCode>());
                break;

            default:
                throw new NotImplementedException();
        }

        // Check if all keys are added succesfully
        bool returnvalue = true;
        foreach (KeyCode k in keys)
        {
            if (!addKey(new List<KeyCode> { k }, callback))
            {
                returnvalue = false;
            }
        }

        return returnvalue;
    }

    /// <summary>
    /// Clears all actions from each key. DOT NOT USE LIGHTLY.
    /// </summary>
    public void clearActions()
    {
        foreach (KeyValuePair<List<KeyCode>, UnityEvent<List<KeyCode>>> entry in subscribedKeyEvents)
        {
            entry.Value.RemoveAllListeners();
        }
    }

    /// <summary>
    /// Removes a listener from each keycode list.
    /// </summary>
    /// <param name="listener">Listener to remove</param>
    public void clearActions(UnityAction<List<KeyCode>> listener)
    {
        if (listener != null)
        {
            foreach (KeyValuePair<List<KeyCode>, UnityEvent<List<KeyCode>>> entry in subscribedKeyEvents)
            {
                entry.Value.RemoveListener(listener);
            }
        }
    }

    /// <summary>
    /// Removes delegated listeners.
    /// </summary>
    /// <param name="args">List of keycodes to match</param>
    public void clearActions(List<KeyCode> args)
    {
        if (args != null)
        {
            subscribedKeyEvents[args].RemoveAllListeners();
        }
    }

    /// <summary>
    /// Removes a specific listener from a specific keycode list
    /// </summary>
    /// <param name="args">List of keycodes mapped to the listener</param>
    /// <param name="listener">Listener method to remove</param>
    public void clearActions(List<KeyCode> args, UnityAction<List<KeyCode>> listener)
    {
        if (args != null && listener != null)
        {
            subscribedKeyEvents[args].RemoveListener(listener);
        }
    }

    /// <summary>
    /// Returns an event matched by the keypair
    /// </summary>
    /// <param name="key">List of keycodes</param>
    /// <returns></returns>
    public UnityEvent<List<KeyCode>> getEvent(List<KeyCode> key)
    {
        if (!subscribedKeyEvents.ContainsKey(key))
        {
            return null;
        }
        return subscribedKeyEvents[key];
    }

    /// <summary>
    /// Invokes the events and all delegates for the given list of keycodes
    /// </summary>
    /// <param name="key">List of keycodes</param>
    public void executeKeyCallback(List<KeyCode> key)
    {
        if (subscribedKeyEvents.ContainsKey(key))
        {
            subscribedKeyEvents[key].Invoke(key);
        }
    }
}