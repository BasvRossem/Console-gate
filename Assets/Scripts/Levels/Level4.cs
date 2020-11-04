using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UserInput;
using Visuals;

public class Level4 : MonoBehaviour
{
[SerializeField] private Monitor monitor = null;
    [SerializeField] private KeyListener keylistener = null;

    private Layer _textLayer;
    private Layer _continueLayer;
    private Terminal _terminal;
    
    private delegate void MonitorWriter();
    private MonitorWriter _myMonitorWriter;
    private MonitorWriter _previousMonitorWriter;

    private int _progressStep;
    private Dictionary<int, string> _binaryAnswers;

    // Start is called before the first frame update
    void Start()
    {
        // Add the required key listeners
        keylistener.AddKey(new List<KeyCode> {KeyCode.Space}, WritePreFace);
        keylistener.AddKey(new List<KeyCode> { KeyCode.DownArrow }, MoveView);
        keylistener.AddKey(new List<KeyCode> { KeyCode.UpArrow }, MoveView);
        keylistener.AddKey(new List<KeyCode> {KeyCode.Home}, LoadStartMenu);

        // Add the base text layer
        _textLayer = monitor.NewLayer();
        _textLayer.view.SetSize(new GridSize(22, Monitor.Size.columns));
        _textLayer.view.StayInBounds(false);
        _textLayer.view.MakeStatic(false);

        _continueLayer = monitor.NewLayer();
        _continueLayer.view.SetExternalPosition(new GridPosition(23, 0));
        
        // Add the user input layer
        monitor.uiCursor.Show(false);

        ConcludeChapterThree();
    }
    
    // Load the startmenu scene
    private void LoadStartMenu(List<KeyCode> args)
    {
        SceneManager.LoadScene("Start Menu");
    }


    private void MoveView(List<KeyCode> args)
    {
        if (args.Count <= 0) return;

        if (args[0] == KeyCode.DownArrow) _textLayer.view.MoveInternalPosition(down: 1);
        else if (args[0] == KeyCode.UpArrow) _textLayer.view.MoveInternalPosition(up: 1);
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    private void ConcludeChapterThree()
    {
        _textLayer.WriteText(@"""the goat has flown away""");

        _continueLayer.WriteText("Press [Space] to continue.");
    }
    
    private void WritePreFace(List<KeyCode> args)
    {
        keylistener.ClearActions(new List<KeyCode> {KeyCode.Space}, WritePreFace);
        keylistener.AddKey(new List<KeyCode>{KeyCode.Space}, LoadLevel);
        
        _textLayer.WriteText(@"Chapter 4

This all starts to feel a bit too relevant.");
    }
    
    

    private void LoadLevel(List<KeyCode> args)
    {
        keylistener.ClearActions(new List<KeyCode> {KeyCode.Space}, LoadLevel);
        _terminal = new Terminal(monitor, keylistener, TerminalCallback);
        monitor.uiCursor.Show(true);
        string text = @"I see that you have come far.
As far as my statistics show you are the only one that has managed to decrypt
the password.
Now please fill in the password you just created. 
";
        _textLayer.WriteText(text);
    }
    
    private void TerminalCallback(String command)
    {
        
        if (command != "the goat has flown away") return;

        LoadEnd();
    }

    private void LoadEnd()
    {
        _textLayer.WriteText(@"Well done!

You are indeed the first to have succeeded the test.
Could you please tell your classmates that the test will be coming thursday?
You of course don't have to make it.
You have already passed with flying colors!

See you next semester, and enjoy your holiday!
");
    }
}
