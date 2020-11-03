using System;
using System.Collections;
using System.Collections.Generic;
using ControllerStructures;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UserInput;
using Visuals;


public class LogicGates
{
    public static void ConnectComponents(LogicComponent input, LogicGate output, int outputPort, Layer puzzleLayer, List<GridPosition> positionsHorizontal, List<GridPosition> positionsVertical)
    {
        var line = new Line(output, outputPort);
        line.AddGridPositions(puzzleLayer, positionsHorizontal, positionsVertical);
        input.lines.Add(line);
    }

    public class Line
    {
        public bool isActive;
        public LogicGate output;
        public int outputPort;

        private Layer _layer;
        private List<GridPosition> _characterPositionsHorizontal;
        private List<GridPosition> _characterPositionsVertical;

        public Line(LogicGate output, int outputPort)
        {
            this.output = output;
            this.outputPort = outputPort;
        }

        public void AddGridPositions(Layer puzzleLayer, List<GridPosition> positionsHorizontal, List<GridPosition> positionsVertical)
        {
            _layer = puzzleLayer;
            _characterPositionsHorizontal = positionsHorizontal;
            _characterPositionsVertical = positionsVertical;
        }

        public void SetActive(bool value)
        {
            isActive = value;
            output.inputs[outputPort] = isActive;
            output.CalculateOutput();

            UpdateCharacters();
        }

        private void UpdateCharacters()
        {
            UpdateCharactersHorizontal();
            UpdateCharactersVertical();
            _layer.Change(true);
        }

        private void UpdateCharactersVertical()
        {
            var character = '.';
            if (_characterPositionsVertical.Count == 0) return;
            if (isActive) character = '|';
            
            foreach (GridPosition position in _characterPositionsVertical)
            {
                _layer.textGrid[position.row, position.column] = character;
            }
        }

        private void UpdateCharactersHorizontal()
        {
            var character = '.';
            if (_characterPositionsHorizontal.Count == 0) return;
            if (isActive) character = '_';
            
            foreach (GridPosition position in _characterPositionsHorizontal)
            {
                _layer.textGrid[position.row, position.column] = character;
            }
        }
    }

    public class LogicComponent
    {
        public List<Line> lines;

        protected LogicComponent()
        {
            lines = new List<Line>();
        }
    }

    public class LogicGate : LogicComponent
    {
        public bool[] inputs = {false, false};

        public LogicGate()
        {
        }

        protected virtual bool IsActivated()
        {
            return false;
        }

        public virtual void CalculateOutput()
        {
            foreach (var line in lines)
            {
                line.SetActive(IsActivated());
            }
        }
    }

    public class Switch : LogicComponent
    {
        private bool _activated;

        public Switch()
        {
            _activated = false;
        }

        public void Toggle()
        {
            _activated = !_activated;
            foreach (var line in lines)
            {
                line.SetActive(_activated);
            }
        }
    }

    public class AndGate : LogicGate
    {
        public AndGate()
        {
        }

        protected override bool IsActivated()
        {
            return inputs[0] && inputs[1];
        }
    }

    public class OrGate : LogicGate
    {
        public OrGate()
        {
        }

        protected override bool IsActivated()
        {
            return inputs[0] || inputs[1];
        }
    }

    public class LoginGate : LogicGate
    {
        public LoginGate()
        {
        }

        protected override bool IsActivated()
        {
            return inputs[0] || inputs[1];
        }

        public override void CalculateOutput()
        {
            if (IsActivated()) Debug.Log("Solved!");
        }
    }
}

public class Level2 : MonoBehaviour
{
    [SerializeField] private Monitor monitor = null;
    [SerializeField] private KeyListener keyListener = null;

    private Layer _puzzleLayer;
    private GridPosition _puzzleLayerOffset;

    private int _textIndex = 0;

    private List<LogicGates.Switch> _switches;
    private List<GridPosition> _switchPositions;
    private int _selectedSwitch;

    // Start is called before the first frame update
    private void Start()
    {
        if (Tools.CheckError(monitor == null, "No Monitor object has been added")) return;
        if (Tools.CheckError(keyListener == null, "No KeyListener object has been added")) return;

        keyListener.AddKey(new List<KeyCode> {KeyCode.Space}, SelectSwitch);
        keyListener.AddKey(new List<KeyCode> {KeyCode.LeftArrow}, MoveSelectLeft);
        keyListener.AddKey(new List<KeyCode> {KeyCode.RightArrow}, MoveSelectRight);

        _puzzleLayer = monitor.NewLayer();
        _puzzleLayer.view.SetExternalPosition(new GridPosition(5, 17));
        _puzzleLayerOffset = _puzzleLayer.view.externalPosition;

        monitor.uiCursor.Show(true);
        // monitor.uiCursor.linkedLayer = _continueLayer;
        // monitor.uiCursor.Blink(true);

        _switches = new List<LogicGates.Switch>(6);
        _switchPositions = new List<GridPosition>(6) {
            new GridPosition(12, 1),
            new GridPosition(12, 9),
            new GridPosition(12, 17),
            new GridPosition(12, 25),
            new GridPosition(12, 33),
            new GridPosition(12, 41)
        };
        _selectedSwitch = 0;

        CreateLogicLayout();
        LoadLogicPuzzleToLayer();
        MoveSelectCursor();
    }

    private void CreateLogicLayout()
    {
        var and10 = new LogicGates.AndGate();
        var and11 = new LogicGates.AndGate();
        var or12 = new LogicGates.OrGate();
        var and20 = new LogicGates.AndGate();
        var or21 = new LogicGates.OrGate();
        var and30 = new LogicGates.AndGate();
        var endGate = new LogicGates.LoginGate();

        _switches.Add(new LogicGates.Switch());
        _switches.Add(new LogicGates.Switch());
        _switches.Add(new LogicGates.Switch());
        _switches.Add(new LogicGates.Switch());
        _switches.Add(new LogicGates.Switch());
        _switches.Add(new LogicGates.Switch());

        LogicGates.ConnectComponents(_switches[0], and10, 0, _puzzleLayer, new List<GridPosition>(){}, new List<GridPosition>(){new GridPosition(10, 1), new GridPosition(11,1)});
        LogicGates.ConnectComponents(_switches[1], and10, 1, _puzzleLayer, new List<GridPosition>(){}, new List<GridPosition>(){new GridPosition(10, 9), new GridPosition(11,9)});
        LogicGates.ConnectComponents(_switches[2], and11, 0, _puzzleLayer, new List<GridPosition>(){}, new List<GridPosition>(){new GridPosition(10, 17), new GridPosition(11,17)});
        LogicGates.ConnectComponents(_switches[3], and11, 1, _puzzleLayer, new List<GridPosition>(){}, new List<GridPosition>(){new GridPosition(10, 25), new GridPosition(11,25)});
        LogicGates.ConnectComponents(_switches[4], or12, 0, _puzzleLayer, new List<GridPosition>(){}, new List<GridPosition>(){new GridPosition(10, 33), new GridPosition(11,33)});
        LogicGates.ConnectComponents(_switches[5], or12, 1, _puzzleLayer, new List<GridPosition>(){}, new List<GridPosition>(){new GridPosition(10, 41), new GridPosition(11,41)});

        LogicGates.ConnectComponents(and10, and20, 0, _puzzleLayer, new List<GridPosition>(){new GridPosition(7, 6), new GridPosition(7, 7), new GridPosition(7, 8)}, new List<GridPosition>(){new GridPosition(8, 5), new GridPosition(7, 9)});
        LogicGates.ConnectComponents(and11, and20, 1, _puzzleLayer, new List<GridPosition>(){new GridPosition(7, 18), new GridPosition(7, 19), new GridPosition(7, 20)}, new List<GridPosition>(){new GridPosition(8, 21), new GridPosition(7, 17)});
        LogicGates.ConnectComponents(and11, or21, 0, _puzzleLayer, new List<GridPosition>(){new GridPosition(7, 22), new GridPosition(7, 23), new GridPosition(7, 24)}, new List<GridPosition>(){new GridPosition(8, 21), new GridPosition(7, 25)});
        LogicGates.ConnectComponents(or12, or21, 1, _puzzleLayer, new List<GridPosition>(){new GridPosition(7, 34), new GridPosition(7, 35), new GridPosition(7, 36)}, new List<GridPosition>(){new GridPosition(8, 37), new GridPosition(7, 33)});

        LogicGates.ConnectComponents(and20, and30, 0, _puzzleLayer, new List<GridPosition>(){new GridPosition(4, 14), new GridPosition(4, 15), new GridPosition(4, 16)}, new List<GridPosition>(){new GridPosition(5, 13), new GridPosition(4, 17)});
        LogicGates.ConnectComponents(or21, and30, 1, _puzzleLayer, new List<GridPosition>(){new GridPosition(4, 26), new GridPosition(4, 27), new GridPosition(4, 28)}, new List<GridPosition>(){new GridPosition(4, 25), new GridPosition(5, 29)});

        LogicGates.ConnectComponents(and30, endGate, 0, _puzzleLayer, new List<GridPosition>(){}, new List<GridPosition>(){new GridPosition(2, 21), new GridPosition(1, 21)});
    }

    private void LoadLogicPuzzleToLayer()
    {
        _puzzleLayer.WriteText(
@"                 [ Login ]
                     .
                     .
                 [  And  ]
             .....       .....
             .               .
         [  And  ]       [  OR   ]
     .....       .........       .....
     .               .               .
 [  And  ]       [  And  ]       [  OR   ]
 .       .       .       .       .       .
 .       .       .       .       .       .
[ ]     [ ]     [ ]     [ ]     [ ]     [ ]");
    }

    private void SelectSwitch(List<KeyCode> args)
    {
        _switches[_selectedSwitch].Toggle();
        char currentCharacter = _puzzleLayer.textGrid[_switchPositions[_selectedSwitch].row,_switchPositions[_selectedSwitch].column];
        if (currentCharacter == ' ') _puzzleLayer.textGrid[_switchPositions[_selectedSwitch].row, _switchPositions[_selectedSwitch].column] = 'x';
        if (currentCharacter == 'x') _puzzleLayer.textGrid[_switchPositions[_selectedSwitch].row, _switchPositions[_selectedSwitch].column] = ' ';
    }

    private void MoveSelectLeft(List<KeyCode> args)
    {
        _selectedSwitch -= 1;
        if (_selectedSwitch < 0) _selectedSwitch = 0;
        MoveSelectCursor();
    }

    private void MoveSelectRight(List<KeyCode> args)
    {
        _selectedSwitch += 1;
        if (_selectedSwitch >= _switches.Count) _selectedSwitch = _switches.Count - 1;
        MoveSelectCursor();
    }
    
    private void MoveSelectCursor()
    {
        monitor.uiCursor.SetGridPosition(_switchPositions[_selectedSwitch] + _puzzleLayerOffset);
    }

    // private void WriteText(string monitorText)
    // {
    //     _puzzleLayer.WriteText(monitorText);
    //     _continueLayer.WriteText("Press [space] to continue...", false);
    // }
    //
    // public void LoadNext(List<KeyCode> args)
    // {
    //     Debug.Log("Next screen");
    //     _textIndex += 1;
    //     if (_textIndex < _text.Count)
    //     {
    //         WriteText(_text[_textIndex]);
    //     }
    //     else
    //     {
    //         SceneManager.LoadScene("Level 1");
    //     }
    // }
}