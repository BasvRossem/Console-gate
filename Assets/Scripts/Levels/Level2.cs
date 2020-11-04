using System;
using System.Collections;
using System.Collections.Generic;
using ControllerStructures;
using Core;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
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
        public bool activated;

        public Switch()
        {
            activated = false;
        }

        public void Toggle()
        {
            activated = !activated;
            foreach (var line in lines)
            {
                line.SetActive(activated);
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
            if (IsActivated()) SceneManager.LoadScene("Level 3");
        }
    }
}

public class Level2 : MonoBehaviour
{
    [SerializeField] private Monitor monitor = null;
    [SerializeField] private KeyListener keyListener = null;

    private Layer _prefaceLayer;
    private Layer _continueLayer;
    private Layer _puzzleLayer;
    private Layer _controlLayer;
    private GridPosition _puzzleLayerOffset;

    private List<LogicGates.Switch> _switches;
    private List<GridPosition> _switchPositions;
    private int _selectedSwitch;

    // Start is called before the first frame update
    private void Start()
    {
        if (Tools.CheckError(monitor == null, "No Monitor object has been added")) return;
        if (Tools.CheckError(keyListener == null, "No KeyListener object has been added")) return;

        keyListener.AddKey(new List<KeyCode> {KeyCode.Space}, GoToPuzzle);
        keyListener.AddKey(new List<KeyCode> {KeyCode.Home}, LoadStartMenu);
        
        
        _puzzleLayer = monitor.NewLayer(false);
        _controlLayer = monitor.NewLayer(false);
        
        _prefaceLayer = monitor.NewLayer();
        _continueLayer = monitor.NewLayer();

        _continueLayer.view.SetExternalPosition(new GridPosition(23, 0));
        
        _puzzleLayer.view.SetExternalPosition(new GridPosition(3, 17));
        _puzzleLayerOffset = _puzzleLayer.view.externalPosition;
        
        _controlLayer.view.SetExternalPosition(new GridPosition(21, 0));
        _controlLayer.zIndex = 1;

        monitor.uiCursor.Show(true);
        monitor.uiCursor.Blink(true);
        monitor.uiCursor.linkedLayer = _continueLayer;


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

        LoadPreface();
    }
    
    // Load the startmenu scene
    private void LoadStartMenu(List<KeyCode> args)
    {
        SceneManager.LoadScene("Start Menu");
    }

    // Preface
    private void LoadPreface()
    {
        _prefaceLayer.WriteText(TextManager.GetLevel2Preface());
        _continueLayer.WriteText(TextManager.GetLevel2Continue(), false);
    }

    private void GoToPuzzle(List<KeyCode> args)
    {
        keyListener.ClearActions();
        
        keyListener.AddKey(new List<KeyCode> {KeyCode.Space}, SelectSwitch);
        keyListener.AddKey(new List<KeyCode> {KeyCode.LeftArrow}, MoveSelectLeft);
        keyListener.AddKey(new List<KeyCode> {KeyCode.RightArrow}, MoveSelectRight);
        
        monitor.DeleteLayer(_prefaceLayer);
        monitor.DeleteLayer(_continueLayer);
        
        monitor.AddLayer(_puzzleLayer);
        monitor.AddLayer(_controlLayer);

        monitor.uiCursor.linkedLayer = null;
        monitor.uiCursor.Blink(false);
        
        CreateLogicLayout();
        LoadLogicPuzzleToLayer();
        MoveSelectCursor();
    }
    
    // Puzzle
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
        _puzzleLayer.WriteText(TextManager.GetLevel2Puzzle());
        _controlLayer.WriteText(TextManager.GetLevel2Controls());
    }

    private void SelectSwitch(List<KeyCode> args)
    {
        _switches[_selectedSwitch].Toggle();

        if (_switches[_selectedSwitch].activated) _puzzleLayer.textGrid[_switchPositions[_selectedSwitch].row, _switchPositions[_selectedSwitch].column] = 'x';
        else _puzzleLayer.textGrid[_switchPositions[_selectedSwitch].row, _switchPositions[_selectedSwitch].column] = ' ';
        
        _puzzleLayer.Change(true);
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
}