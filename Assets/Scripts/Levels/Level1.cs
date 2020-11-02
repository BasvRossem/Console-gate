﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using Visuals;
using Cursor = Visuals.Cursor;

public class Level1 : MonoBehaviour
{
    [SerializeField] private Monitor monitor = null;
    [SerializeField] private Keylistener keylistener = null;

    private Layer _textLayer;
    private Layer _userInputLayer;

    private string _command = "";
    
    private delegate void MonitorWriter();
    private MonitorWriter _myMonitorWriter;

    // Start is called before the first frame update
    private void Start()
    {
        keylistener.addKey(new List<KeyCode> { KeyCode.DownArrow }, MoveView);
        keylistener.addKey(new List<KeyCode> { KeyCode.UpArrow }, MoveView);

        keylistener.addOption(KeyBoardOptions.Alphabetical, UpdateTerminal);
        keylistener.addOption(KeyBoardOptions.Numerical, UpdateTerminal);
        keylistener.addKey(new List<KeyCode> { KeyCode.Space }, UpdateTerminal);
        keylistener.addKey(new List<KeyCode> { KeyCode.Period }, UpdateTerminal);
        keylistener.addKey(new List<KeyCode> { KeyCode.Backspace }, RemoveLastTerminalCharacter);
        keylistener.addKey(new List<KeyCode> { KeyCode.Return }, SendCommand);
        keylistener.addKeyCombination(new Tuple<List<KeyCode>, KeyCode>(new List<KeyCode> { KeyCode.LeftShift }, KeyCode.Alpha2), UpdateTerminal);

        _textLayer = monitor.NewLayer();
        _textLayer.view.SetSize(new GridSize(22, Monitor.Size.columns));
        _textLayer.view.StayInBounds(true);

        _userInputLayer = monitor.NewLayer();
        _userInputLayer.view.SetSize(new GridSize(1, Monitor.Size.columns));
        _userInputLayer.view.SetExternalPosition(new GridPosition(23, 0));
        
        monitor.uiCursor.linkedLayer = _userInputLayer;
        monitor.uiCursor.Blink(true);

        _myMonitorWriter = LoadChatlog;
        _myMonitorWriter();
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void LoadChatlog()
    {
       _textLayer.WriteText(@"-------------------
Chatlog 23 - 10 - 2020
------------------ -
23 - 10 - 2020 16:03, Docent:
Bij deze geef ik jullie een extra bijlage als toevoeging op de les

23 - 10 = 2020 16:04, Remco:
ligt het aan mij, of kan ik het bestand nog niet downloaden ?

23 - 10 - 2020 16:04, Julianne:
De docent heeft nog geen download - link gegeven....

23 - 10 - 2020 16:05, Docent:
Oeps, foutje, bij deze is hij meegedeeld.

23 - 10 - 2020 16:06, Julianne:
Bedankt!

23 - 10 - 2020 16:06, Docent:
Ik wil graag dat jullie deze stof voor de volgende les(26 - 10 - 2020) doornemen.

------------------ -
Chatlog 24 - 10 - 2020
------------------ -

-------------------
Chatlog 25 - 10 - 2020
------------------ -
25 - 10 - 2020 20:48, SYSTEM:
Succesfully downloaded ""appendix.txt""");
       _textLayer.view.SetBounds(_textLayer.textGrid.GetSize());
    }

    public void LoadFile()
    {
        _textLayer.WriteText(@"Filename: appendix
File extension: .txt
Path to file: / Downloads / apendix

Date created: 20 - 03 - 2014
Size: 12381 bytes
Author: Docent
IP - adress owner: 52.232.56.79");
        _textLayer.DrawLineHorizontal(8, 0, Monitor.Size.columns);
        _textLayer.cursor.Move(Cursor.Down);
        _textLayer.WriteLine(@"Content:

Lorem ipsum dolor sit amet, consectetur adipiscing elit.
Nulla vehicula et ex id eleifend.
Sed cursus, eros non fringilla finibus, augue velit aliquam felis,
ac mollis augue sem in arcu.
Nunc odio sapien, varius in vestibulum in, ultrices a diam.
Nulla vestibulum ac dolor quis eleifend.
Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere curae;
Praesent suscipit feugiat felis id viverra.
Praesent a lectus sapien.
Nunc ac mollis ipsum.
Ut mauris dolor, maximus id diam in, auctor suscipit neque.
Nam velit eros, accumsan non libero at, placerat semper augue.
Maecenas scelerisque semper venenatis.
Suspendisse vel dolor velit.
Nunc imperdiet cursus velit eget porta.
In eget ex purus.
Vivamus pellentesque quam in arcu ultrices varius.");
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

    public void UpdateTerminal(List<KeyCode> args)
    {
        if (args.Count <= 0) return;
        if (args[0] == KeyCode.LeftShift && args[1] == KeyCode.Alpha2)
        {
            _command += "@";
        }

        foreach (KeyCode k in args)
        {
            _command += (char)k;
        }

        UpdateTerminalLayer();
    }

    public void UpdateTerminal(Tuple<List<KeyCode>, KeyCode> args)
    {
        if (args == null) return;
        if (args.Item1[0] == KeyCode.LeftShift && args.Item2 == KeyCode.Alpha2)
        {
            _command += "@";
        }

        UpdateTerminalLayer();
    }

    public void RemoveLastTerminalCharacter(List<KeyCode> args)
    {
        if (args.Count <= 0) return;
        if (_command.Length <= 0) return;

        StringBuilder sb = new StringBuilder(_command);
        sb.Remove(_command.Length - 1, 1);
        _command = sb.ToString();
        UpdateTerminalLayer();
    }

    private void UpdateTerminalLayer()
    {
        _userInputLayer.WriteText(_command, false);
    }

    public void SendCommand(List<KeyCode> args)
    {
        if (args.Count <= 0) return;
        var splitCommand = _command.Split(' ');

        switch (splitCommand[0])
        {
            case "ssh":
                sshCall(_command);
                break;

            case "cat":
                catCall(_command);
                break;

            case "dir":
                dirCall(_command);
                break;

            default:
                break;
        }
        _command = "";
        _myMonitorWriter();
        UpdateTerminalLayer();
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
        _textLayer.WriteText("That IP-address can't be connected to");
    }

    private void catCall(string command)
    {
        if (command == null) return;
        if (command == "cat appendix.txt")
        {
            _myMonitorWriter = LoadFile;
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
        _textLayer.WriteText("Can't find that file, try searching recently downloaded files.\n(cat chatlog.txt might contain useful info)");
    }

    private void dirCall(string command)
    {
        Debug.Log("dirCall is called");
        if (command == null) return;
        if (command == "dir")
        {
            Debug.Log("dirWriter is being set");
            _myMonitorWriter = dirWriter;
        }
    }
    private void dirWriter()
    {
        _textLayer.WriteText("/:\n\tchatlog.txt\n\tappendix.txt");
    }
}