using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UserInput;
using Visuals;


public class LogicGates
{
    public static void ConnectComponents(LogicComponent input, LogicGate output, int outputPort)
    {
        input.lines.Add(new Line(output, outputPort));
    }
    
    public class Line
    {
        public bool isActive;
        public LogicGate output;
        public int outputPort;

        public Line(LogicGate output, int outputPort)
        {
            this.output = output;
            this.outputPort = outputPort;
        }

        public void SetActive(bool value)
        {
            isActive = value;
            output.inputs[outputPort] = isActive;
            output.CalculateOutput();
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
    
    public class LogicGate: LogicComponent
    {
        public bool[] inputs = {false, false};
        
        public LogicGate(){}

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
    
    public class Switch: LogicComponent
    {
        private bool _activated = false;

        public Switch(){}

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
        public AndGate(){}
        
        protected override bool IsActivated()
        {
            return inputs[0]&& inputs[1];
        }
    }
    
    public class OrGate : LogicGate
    {
        public OrGate(){}
        
        protected override bool IsActivated()
        {
            return inputs[0] || inputs[1];
        }
    }
    
    public class LoginGate : LogicGate
    {
        public LoginGate(){}
        
        protected override bool IsActivated()
        {
            return inputs[0] || inputs[1];
        }
        
        public override void CalculateOutput()
        {
            if(IsActivated()) Debug.Log("Solved!");
        }
    }
}

public class Level2 : MonoBehaviour
{
    [SerializeField] private Monitor monitor = null;
    [SerializeField] private KeyListener keyListener = null;

    private Layer _puzzleLayer;
    
    private int _textIndex = 0;

    private List<LogicGates.Switch> _switches;

    // Start is called before the first frame update
    private void Start()
    {
        if (Tools.CheckError(monitor == null, "No Monitor object has been added")) return;
        if (Tools.CheckError(keyListener == null, "No KeyListener object has been added")) return;
        
        // keyListener.AddKey(new List<KeyCode> { KeyCode.Space }, SelectSwitch);
        // keyListener.AddKey(new List<KeyCode> { KeyCode.LeftArrow }, MoveSelectLeft);
        // keyListener.AddKey(new List<KeyCode> { KeyCode.Space }, MoveSelectRight);

        _puzzleLayer = monitor.NewLayer();

        monitor.uiCursor.Show(false);
        // monitor.uiCursor.linkedLayer = _continueLayer;
        // monitor.uiCursor.Blink(true);

        _switches = new List<LogicGates.Switch>(6);

        CreateLogicLayout();
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
        
        LogicGates.ConnectComponents(_switches[0], and10, 0);
        LogicGates.ConnectComponents(_switches[1], and10, 1);
        LogicGates.ConnectComponents(_switches[2], and11, 0);
        LogicGates.ConnectComponents(_switches[3], and11, 1);
        LogicGates.ConnectComponents(_switches[4], or12, 0);
        LogicGates.ConnectComponents(_switches[5], or12, 1);
        
        LogicGates.ConnectComponents(and10, and20, 0);
        LogicGates.ConnectComponents(and11, and20, 1);
        LogicGates.ConnectComponents(and11, or21, 0);
        LogicGates.ConnectComponents(or12, or21, 1);
        
        LogicGates.ConnectComponents(and20, and30, 0);
        LogicGates.ConnectComponents(or21, and30, 1);

        LogicGates.ConnectComponents(and30, endGate, 0);

        _switches[0].Toggle();
        _switches[1].Toggle();
        _switches[2].Toggle();
        _switches[3].Toggle();
        _switches[4].Toggle();
        _switches[5].Toggle();
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
