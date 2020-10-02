using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

//public enum KeyBoardOptions: List<KeyCode>
//{
//    Alphabetical = new List<KeyCode>(
//        Enum.GetValues(typeof(KeyCode))
//        .Where(k => (int)k <= KeyCode.Z && (int)k >= (int)KeyCode.A)
//    )
//}


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
    private Dictionary<List<KeyCode>, UnityEvent<List<KeyCode>>> subscribedEvents;
    private List<KeyCode> _keysDown;


    public void Start()
    {
        customComparer = new KeyCodeComparer();
        subscribedEvents = new Dictionary<List<KeyCode>, UnityEvent<List<KeyCode>>>(customComparer);
        addKey(new List<KeyCode> { KeyCode.A , KeyCode.B}, ping);
        addKey(new List<KeyCode> { KeyCode.A , KeyCode.B, KeyCode.C}, ping2);
    }

    public void ping(List<KeyCode> arg)
    {
        print("Double press!");
    }

    public void ping2(List<KeyCode> arg)
    {
        print("Triple press?");
    }


    public void OnEnable()
    {
        print(Enum.GetValues(typeof(KeyCode)));
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
            for (int i = 0; i < _keysDown.Count; i++)
            {
                KeyCode kc = _keysDown[i];
                if (Input.GetKeyUp(kc))
                {
                    executeKeyCallback(_keysDown);
                    _keysDown.RemoveAt(i);
                    i--;
                    //executeKeyCallback(_keysDown);
                }
            }
        }
    }

    public bool addKey(List<KeyCode> key, UnityAction<List<KeyCode>> callback)
    {
        if (!subscribedEvents.ContainsKey(key))
        {
            subscribedEvents.Add(key, new UnityEvent<List<KeyCode>>());
        }

        subscribedEvents[key].AddListener(callback);
        return true;
    }

    public UnityEvent<List<KeyCode>> getCallback(List<KeyCode> key)
    {
        if (!subscribedEvents.ContainsKey(key))
        {
            return null;
        }
        return subscribedEvents[key];
    }

    public void executeKeyCallback(List<KeyCode> key)
    {
        if (subscribedEvents.ContainsKey(key))
        {
            subscribedEvents[key].Invoke(key);
        }
    }
}
