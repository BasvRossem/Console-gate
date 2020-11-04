using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        _textLayer.WriteText(@"                                                                               
                      Chapter 3: Breaking the encryption.                      
                                                                               
                                                                               
Na het verbinden met de computer van je docent, 
en het hacken van zijn logische poorten sta je nu voor een volgende uitdaging.

Om bij de agenda van de docent te komen moet je dit keer een wachtwoord zien te achterhalen. 

Gelukkig staan alle wachtwoorden opgeslagen op een centrale locatie.

(Als je er even niet uit komt, [escape] kan je helpen)");
    }

    private void StartHelp(List<KeyCode> args)
    {
        
        // Write tips for the current layer.
        Debug.Log("Esc pressed, press [esc] again to close it.");
        
        
        keylistener.ClearActions(new List<KeyCode>{KeyCode.Escape}, StartHelp);
        keylistener.AddKey(new List<KeyCode> {KeyCode.Escape}, ExitHelp);
        ChangeContext(WriteHelp, true);
    }

    private void WriteHelp()
    {
        StringBuilder helpText = new StringBuilder(@"+------------------------------------------------------------------------------+");
        helpText.Append(@"
|                                  Helpmenu:                                   |
|                       (Druk [escape] om terug te gaan)                       |
|                                                                              |
| Alle wachtwoorden zijn opgeslagen op de centrale plaats van het systeem.     |
| (Probeer ""passwd"" te openen, of ""dir"" / ""ls"" te gebruiken).                  |
|                                                                              |");
        if (_progressStep >= 1)
        {
            helpText.Append(@"|                                                                              |
| Het lijkt erop dat zijn binaire wachtwoord versleuteld is.                   |
| Tijd om zijn stappen op te volgen.                                           |
|                                                                              |
| Voorbeeld binair optellen: 1001 1111 + 1010 0101                             |
| 1001 1111                                                                    |
| 1010 0101                                                                    |
| --------- +                                                                  |
| 0100 0100                                                                    |
|                                                                              |
| (Altijd afronden naar 8 bits)                                                |");
        }
        
        
        helpText.Append(@"
+------------------------------------------------------------------------------+");
        
        
        // Layer of encryption help
        _textLayer.WriteText(helpText.ToString());
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
        _textLayer.WriteText(@"                                                                                
                                     passwd                                     
                                                                                
root:x:0:1:System Operator:/:/bin/ksh
daemon:x:1:1::/tmp:
uucp:x:4:4::/var/spool/uucppublic:/usr/lib/uucp/uucico
user:01101010:181:100:Rik de Jong:/u/user:/bin/ksh

# Je bent aardig ver gekomen, als jij in mijn passwd file zit te rommelen.
# Tel de basis bij de som van de gebruiker op
# Haal hier de som van de lagen af.
# Vermenigvuldig dit met 2, dit leidt je naar je toekomst.");
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
        _textLayer.WriteText("Can't find that file.\n(Opening the menu might provide additional information)");
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
        _textLayer.WriteText(":/\n-passwd\n-decryptor.exe");
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
        StringBuilder decryptorText = new StringBuilder(
            @"                                                                                
Voer de juiste antwoorden in om het wachtwoord te ontgrendelen als parameter:
(Bijvoorbeeld ""decryptor 1111 1111"" of ""decryptor 11111111"")   
Correcte antwoorden komen hieronder te staan

                                                                                
Stap 1: Basiswachtwoord:                                                    ");
        
        if (_progressStep >= 3)
        {
            decryptorText.Append(@"
0110 1010                                 

Stap 2: Som der gebruiker ");
        }

        if (_progressStep >= 4)
        {
            decryptorText.Append(@"
1011 1111
                                                                                
Stap 3: Basiswachtwoord + som der gebruiker");
        }

        if (_progressStep >= 5)
        {
            decryptorText.Append(@"
0010 1001                                                                                
                                                                                
Stap 4: De som van de lagen");
        }

        if (_progressStep >= 6)
        {
            decryptorText.Append(@"
0001 1100
                                                                                
Stap 5: Stap 3 - Stap 4");
        }

        if (_progressStep >= 7)
        {
            decryptorText.Append(@"
0000 1101
                                                                                
Stap 6: Stap 5 x 2");
        }

        if (_progressStep >= 8)
        {
            SceneManager.LoadScene("Level 4");
        }
        _textLayer.WriteText(decryptorText.ToString());

    }
}
