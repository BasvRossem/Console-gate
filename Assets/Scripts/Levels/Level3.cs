using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core;
using UnityEngine;
using UnityEngine.UI;
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

    private int _progressStep;
    private Dictionary<int, string> _binaryAnswers;

    // Start is called before the first frame update
    void Start()
    {
        // Add the required key listeners
        keylistener.AddKey(new List<KeyCode> {KeyCode.Escape}, StartHelp);
        keylistener.AddKey(new List<KeyCode> { KeyCode.DownArrow }, MoveView);
        keylistener.AddKey(new List<KeyCode> { KeyCode.UpArrow }, MoveView);

        // Add the base text layer
        _textLayer = monitor.NewLayer();
        _textLayer.view.SetSize(new GridSize(22, Monitor.Size.columns));
        _textLayer.view.StayInBounds(false);
        _textLayer.view.MakeStatic(false);
        
        // Add the user input layer
        _terminal = new Terminal(monitor, keylistener, TerminalCallback);
        
        _myMonitorWriter = Intro;
        _myMonitorWriter();

        _progressStep = 0;
        InitBinaryAnswers();
    }

    private void MoveView(List<KeyCode> args)
    {
        if (args.Count <= 0) return;

        if (args[0] == KeyCode.DownArrow) _textLayer.view.MoveInternalPosition(down: 1);
        else if (args[0] == KeyCode.UpArrow) _textLayer.view.MoveInternalPosition(up: 1);
    }

    private void InitBinaryAnswers()
    {
        _binaryAnswers = new Dictionary<int, string>();
        _binaryAnswers[2] = "01101010";
        _binaryAnswers[3] = "10111111";
        _binaryAnswers[4] = "00101001";
        _binaryAnswers[5] = "00011100";
        _binaryAnswers[6] = "00001101";
        _binaryAnswers[7] = "00011010";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void TerminalCallback(String command)
    {
        var splitCommand = command.Split(' ');
        if (splitCommand.Length < 1) return;
        switch (splitCommand[0])
        {
            case "cat":
                CatCall(command);
                break;

            case "ls":
            case "dir":
                DirCall(command);
                break;
            
            case "decryptor.exe":
            case "decryptor":
                DecryptorCall(command);
                break;
            default:
                break;
        }
    }

    private void ChangeContext(MonitorWriter newContext, bool changePreviousContext = true)
    {
        if(changePreviousContext) _previousMonitorWriter = _myMonitorWriter;
        _myMonitorWriter = newContext;
        _myMonitorWriter();
    }
    
    private void Intro()
    {
        _textLayer.WriteText(TextManager.GetLevel3Intro());
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
            _textLayer.WriteText(TextManager.GetLevel3Help1());
        }else if (_progressStep >= 1)
        {
            _textLayer.WriteText(TextManager.GetLevel3Help2());
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

    private void PasswdWriter()
    {
        _textLayer.WriteText(TextManager.GetLevel3Passwd());
    }
    
    private void CatCall(string command)
    {
        if (command == null) return;
        if (command == "cat passwd" || command == "cat /etc/passwd")
        {
            ChangeContext(PasswdWriter, true);
            // Set the progress to 1, unless it's already higher
            _progressStep = _progressStep == 0 ? 1 : _progressStep;
        }
        else
        {
            ChangeContext(CatWriter, true);
        }
    }

    private void CatWriter()
    {
        _textLayer.WriteText(TextManager.GetLevel3CatWrong());
        _textLayer.view.MakeStatic(true);
    }
    
    private void DirCall(string command)
    {
        if (command == null) return;
        if (command == "dir" || command == "ls")
        {
            ChangeContext(DirWriter, true);
        }
    }

    private void DirWriter()
    {
        _textLayer.WriteText(TextManager.GetLevel3Dir());
        _textLayer.view.MakeStatic(true);
        
    }

    private void DecryptorCall(String command)
    {
        if (command == null || command.Length < 1) return; 
        if (_progressStep < 2) _progressStep = 2;
        var parameters = command.Split(' ');
        if (parameters.Length < 1) return;
        if (parameters.Length < 2)
        {
            ChangeContext(DecryptorWriter);
        }else if (parameters.Length == 2)
        {
            // Decryptor 01100110
            CheckDecryptor(parameters[1]);
        }
        else if (parameters.Length == 3)
        {
            // Decryptor 0110 0110
            CheckDecryptor(String.Concat(parameters[1], parameters[2]));
        }
    }

    private void CheckDecryptor(String entry)
    {
        if (!_binaryAnswers.ContainsKey(_progressStep)) return;
        if (_binaryAnswers[_progressStep] == entry)
        {
            _progressStep += 1;
        }

        ChangeContext(DecryptorWriter);
    }

    private void DecryptorWriter()
    {
        _textLayer.WriteText(TextManager.GetLevel3Decryptor(_progressStep));
    }
}
