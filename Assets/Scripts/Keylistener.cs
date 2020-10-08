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


class KeyCodeComparer : IEqualityComparer<List<KeyCode>>
{
    public bool Equals(List<KeyCode> x, List<KeyCode> y)
    {
        return x.All(y.Contains);
    }

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


    public void Start()
    {
        customComparer = new KeyCodeComparer();
        subscribedKeyEvents = new Dictionary<List<KeyCode>, UnityEvent<List<KeyCode>>>(customComparer);
    }

    public void ping(List<KeyCode> arg)
    {
        print("Double press!");
    }

    public void ping2(List<KeyCode> arg)
    {
        print("Triple press?");
    }

    public void alphabetical(List<KeyCode> arg)
    {
        print("You pressed an alphabetical key:");
        print(arg);
    }


    public void OnEnable()
    {
        _keysDown = new List<KeyCode>();
    }
    public void OnDisable()
    {
        _keysDown = null;
    }

    public void Update()
    {
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
            executeKeyCallback(_keysUp);
        }
    }

    public bool addKey(List<KeyCode> key, UnityAction<List<KeyCode>> callback)
    {
        if (!subscribedKeyEvents.ContainsKey(key))
        {
            subscribedKeyEvents.Add(key, new UnityEvent<List<KeyCode>>());
        }

        subscribedKeyEvents[key].AddListener(callback);
        return true;
    }

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
        
        foreach(KeyCode k in keys)
        {
            addKey(new List<KeyCode> { k }, callback);
        }
        
        return true;
    }

    public UnityEvent<List<KeyCode>> getCallback(List<KeyCode> key)
    {
        if (!subscribedKeyEvents.ContainsKey(key))
        {
            return null;
        }
        return subscribedKeyEvents[key];
    }

    public void executeKeyCallback(List<KeyCode> key)
    {
        if (subscribedKeyEvents.ContainsKey(key))
        {
            subscribedKeyEvents[key].Invoke(key);
        }
    }
}
