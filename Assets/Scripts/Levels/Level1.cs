using System;
using System.Collections.Generic;
using System.Text;
using Core;
using UnityEngine;
using UnityEngine.SceneManagement;
using Visuals;
using UserInput;

public class Level1 : MonoBehaviour
{
    [SerializeField] private Monitor monitor = null;
    [SerializeField] private KeyListener keylistener = null;

    private Layer _textLayer;
    private Terminal _userTerminal;

    
    private delegate void MonitorWriter();
    private MonitorWriter _myMonitorWriter;
    private MonitorWriter _previousMonitorWriter;
    private int _progressStep;

    // Start is called before the first frame update
    private void Start()
    {
        _userTerminal = new Terminal(monitor, keylistener, SendCommand);
        keylistener.AddKey(new List<KeyCode> { KeyCode.DownArrow }, MoveView);
        keylistener.AddKey(new List<KeyCode> { KeyCode.UpArrow }, MoveView);
        keylistener.AddKey(new List<KeyCode> {KeyCode.Escape}, StartHelp);
        keylistener.AddKey(new List<KeyCode> {KeyCode.M}, LoadStartMenu);

        _textLayer = monitor.NewLayer();
        _textLayer.view.SetSize(new GridSize(22, Monitor.Size.columns));
        _textLayer.view.StayInBounds(true);
        
        _myMonitorWriter = LoadChatlog;
        _myMonitorWriter();
        _progressStep = 0;
    }
    
    // Load the startmenu scene
    private void LoadStartMenu(List<KeyCode> args)
    {
        SceneManager.LoadScene("Start Menu");
    }

    private void ChangeContext(MonitorWriter newContext, bool changePreviousContext = true)
    {
        if(changePreviousContext) _previousMonitorWriter = _myMonitorWriter;
        _myMonitorWriter = newContext;
        _myMonitorWriter();
    }

    private void StartHelp(List<KeyCode> args)
    {
        // Write tips for the current layer.
        keylistener.ClearActions(new List<KeyCode>{KeyCode.Escape}, StartHelp);
        keylistener.AddKey(new List<KeyCode> {KeyCode.Escape}, ExitHelp);
        ChangeContext(WriteHelp, true);
    }

    private void WriteHelp()
    {
        if (_progressStep <= 0)
        {
            _textLayer.WriteText(TextManager.GetLevel1Help1());
        }else if (_progressStep >= 1)
        {
            _textLayer.WriteText(TextManager.GetLevel1Help2());
        }
    }

    private void ExitHelp(List<KeyCode> args)
    {
        // don't use the context changer because the menu should be exempt of previous changes.
        if (_previousMonitorWriter == null) ChangeContext(LoadChatlog, false);
        else ChangeContext(_previousMonitorWriter, false);
        // Remove previous listener
        keylistener.ClearActions(new List<KeyCode> {KeyCode.Escape}, ExitHelp);
        // Add new listener, back to menu
        keylistener.AddKey(new List<KeyCode>{KeyCode.Escape}, StartHelp);
    }
    private void LoadChatlog()
    { 
        _textLayer.view.MakeStatic(false); 
        _textLayer.WriteText(TextManager.GetLevel1Chatlog());
       _textLayer.view.SetBounds(_textLayer.textGrid.GetSize());
    }

    public void LoadFile()
    {
        _textLayer.WriteText(TextManager.GetLevel1AppendixMetadata());
        _textLayer.DrawLineHorizontal(8, 0, Monitor.Size.columns);
        _textLayer.cursor.Move(Visuals.Cursor.Down);
        _textLayer.WriteLine(TextManager.GetLevel1AppendixContent());
        _textLayer.view.SetBounds(_textLayer.textGrid.GetSize());
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene("Level 2");
    }

    public void MoveView(List<KeyCode> args)
    {
        if (args.Count <= 0) return;

        if (args[0] == KeyCode.DownArrow) _textLayer.view.MoveInternalPosition(down: 1);
        else if (args[0] == KeyCode.UpArrow) _textLayer.view.MoveInternalPosition(up: 1);
    }


    public void SendCommand(String command)
    {
        var splitCommand = command.Split(' ');

        switch (splitCommand[0])
        {
            case "ssh":
                sshCall(command);
                break;

            case "cat":
                catCall(command);
                break;

            case "dir":
                dirCall(command);
                break;

            default:
                break;
        }
        _myMonitorWriter();
    }

    private void sshCall(string command)
    {
        if (command == "ssh user@52.232.56.79")
        {
            _myMonitorWriter = LoadNextLevel;
        }
        else
        {
            _myMonitorWriter = sshWriter;
        }
    }

    private void sshWriter()
    {
        _textLayer.WriteText(TextManager.GetLevel1SshWrongConnect());
        _textLayer.view.MakeStatic(true); 
    }

    private void catCall(string command)
    {
        if (command == null) return;
        if (command == "cat appendix.txt")
        {
            _myMonitorWriter = LoadFile;
            _progressStep = _progressStep != 0 ? _progressStep : 1;
        }
        else if(command == "cat chatlog.txt")
        {
            _myMonitorWriter = LoadChatlog;
        }
        else
        {
            _myMonitorWriter = catWriter;
        }
    }

    private void catWriter()
    {
        _textLayer.WriteText(TextManager.GetLevel1CatWrongFile());
        _textLayer.view.MakeStatic(false);
    }

    private void dirCall(string command)
    {
        if (command == null) return;
        if (command == "dir")
        {
            _myMonitorWriter = dirWriter;
        }
    }
    private void dirWriter()
    {
        _textLayer.WriteText(TextManager.GetLevel1DirContent());
        _textLayer.view.MakeStatic(true);
    }
}