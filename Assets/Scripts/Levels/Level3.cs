using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Visuals;
using UserInput;

public class Level3 : MonoBehaviour
{
    [SerializeField] private Monitor monitor = null;
    [SerializeField] private KeyListener keylistener = null;

    private Layer _textLayer;
    private Terminal _terminal;
    
    private delegate void MonitorWriter();
    private MonitorWriter _myMonitorWriter;
    private MonitorWriter _previousMonitorWriter;

    private int _encryptionLayer;
    private void OnEnable()
    {
        Debug.Log("Enable");
    }

    private void OnDisable()
    {
        Debug.Log("Disable");
    }

    // Start is called before the first frame update
    void Start()
    {
        // Add the required key listeners
        keylistener.AddKey(new List<KeyCode> {KeyCode.Escape}, StartHelp);

        // Add the base text layer
        _textLayer = monitor.NewLayer();
        _textLayer.view.SetSize(new GridSize(22, Monitor.Size.columns));
        _textLayer.view.StayInBounds(true);
        
        // Add the user input layer
        _terminal = new Terminal(monitor, keylistener, TerminalCallback);
        
        
        
        _myMonitorWriter = Intro;
        _myMonitorWriter();

        _encryptionLayer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void TerminalCallback(String command)
    {
        Debug.Log(command);
    }

    private void ChangeContext(MonitorWriter newContext, bool changePreviousContext = true)
    {
        if(changePreviousContext) _previousMonitorWriter = _myMonitorWriter;
        _myMonitorWriter = newContext;
        _myMonitorWriter();
    }
    
    private void Intro()
    {
        _textLayer.WriteText(@"                                                                               
                      Chapter 3: Breaking the encryption.                      
                                                                               
                                                                               
Na het verbinden met de computer van je docent, 
en het hacken van zijn logische poorten sta je nu voor een volgende uitdaging.

Om bij de agenda van de docent te komen moet je dit keer een wachtwoord zien te achterhalen. 

Gelukkig staan alle wachtwoorden opgeslagen op een centrale locatie.");
        Debug.Log("Intro message, press [Space] to continue, and press [esc] when you are in need of assistance.");
    }

    private void StartHelp(List<KeyCode> args)
    {
        
        // Write tips for the current layer.
        Debug.Log("Esc pressed, press [esc] again to close it.");
        
        
        keylistener.ClearActions(new List<KeyCode>{KeyCode.Escape}, StartHelp);
        keylistener.AddKey(new List<KeyCode> {KeyCode.Escape}, ExitHelp);
        ChangeContext(writeHelp, true);
    }

    private void writeHelp()
    {
        // Layer 0 of encryption help
        _textLayer.WriteText(@"                                                                               
                      Helpmenu:                      
                                                                               
                                                                               
Alle wachtwoorden zijn opgeslagen op de centrale plaats van het systeem.
(Probeer ""passwd"" te openen).
");
        // Layer 1 of encryption help
        if (_encryptionLayer > 0)
        {
            _textLayer.WriteText("Alright, je bent nog verder gekomen!");
        }
    }

    private void ExitHelp(List<KeyCode> args)
    {
        // don't use the context changer because the menu should be exempt of previous changes.
        if (_previousMonitorWriter == null) ChangeContext(Intro, false);
        else ChangeContext(_previousMonitorWriter, false);
        // Remove previous listener
        keylistener.ClearActions(new List<KeyCode> {KeyCode.Escape}, ExitHelp);
        // Add new listener, back to menu
        keylistener.AddKey(new List<KeyCode>{KeyCode.Escape}, StartHelp);
    }
}
