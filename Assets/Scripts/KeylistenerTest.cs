using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class KeylistenerTest : MonoBehaviour
{
    private Keylistener listener;
    // Start is called before the first frame update
    void Start()
    {
        listener = new Keylistener();
        listener.addKey(KeyCode.B, this.customCallback);
        listener.addKey(KeyCode.D, this.newCallback);      
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnGUI()
    {
        Event e = Event.current;
        if (e.isKey)
        {
            listener.executeKeyCallback(e.keyCode);
        }
    }

    void customCallback()
    {
        print("B has been pressed!");
    }

    void newCallback()
    {
        print("Dit is een heeeele andere callback!");
    }
}
