using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using Visuals;
using UserInput;

namespace UserInput
{
    public class Terminal
    {
        private string _command;
        private Monitor _monitor;
        private KeyListener _keyListener;
        private Layer _userInputLayer;

        public delegate void Callback(String command);
        private Callback _myCallback;

        public Terminal(Monitor tmpMonitor, KeyListener tmpKeyListener, Callback terminalCallback)
        {
            // Assign base values
            _command = "";
            _monitor = tmpMonitor;
            _keyListener = tmpKeyListener;
            _myCallback = terminalCallback;

            // Instantiate keyListeners
            InitializeKeyListeners();

            InitializeMonitorLayer();

        }

        /// <summary>
        /// Initializes the alphabetical and numerical keylisteners, in addition to space, period, backspace and return.
        /// Also initializes shift+2 = @ listener.
        /// </summary>
        private void InitializeKeyListeners()
        {
            _keyListener.AddOption(KeyBoardOptions.Alphabetical, UpdateTerminal);
            _keyListener.AddOption(KeyBoardOptions.Numerical, UpdateTerminal);
            _keyListener.AddKey(new List<KeyCode> { KeyCode.Space }, UpdateTerminal);
            _keyListener.AddKey(new List<KeyCode> { KeyCode.Period }, UpdateTerminal);
            _keyListener.AddKey(new List<KeyCode> { KeyCode.Backspace }, RemoveLastTerminalCharacter);
            _keyListener.AddKey(new List<KeyCode> { KeyCode.Return }, ProcessReturn);
            _keyListener.AddKeyCombination(new Tuple<List<KeyCode>, KeyCode>(new List<KeyCode> { KeyCode.LeftShift }, KeyCode.Alpha2), UpdateTerminal);
            _keyListener.AddKeyCombination(new Tuple<List<KeyCode>, KeyCode>(new List<KeyCode> { KeyCode.RightShift }, KeyCode.Alpha2), UpdateTerminal);
        }

        /// <summary>
        /// Initializes the monitor layer for the terminal, in addition to the uiCursor
        /// </summary>
        private void InitializeMonitorLayer()
        {
            _userInputLayer = _monitor.NewLayer();
            _userInputLayer.view.SetSize(new GridSize(1, Monitor.Size.columns));
            _userInputLayer.view.SetExternalPosition(new GridPosition(23, 0));
            _monitor.uiCursor.linkedLayer = _userInputLayer;
            _monitor.uiCursor.Blink(true);
        }

        /// <summary>
        /// Process the enter button call in terminal. This calls the given callback from the constructor, passing the
        /// current command as parameter.
        /// </summary>
        /// <param name="args"></param>
        private void ProcessReturn(List<KeyCode> args)
        {
            if (args.Count <= 0) return;
            _myCallback(_command);
            _command = "";
            UpdateTerminalLayer();
        }
        
        /// <summary>
        /// Updates the terminal with custom statement for @-symbol.
        /// </summary>
        /// <param name="args"></param>
        private void UpdateTerminal(List<KeyCode> args)
        {
            if (args.Count <= 0) return;

            foreach (KeyCode k in args)
            {
                _command += (char)k;
            }

            UpdateTerminalLayer();
        }

        /// <summary>
        /// Adds the specific usecase of @ to the terminal.
        /// </summary>
        /// <param name="args"></param>
        private void UpdateTerminal(Tuple<List<KeyCode>, KeyCode> args)
        {
            if (args == null || args.Item1 == null) return;
            if (args.Item1[0] == KeyCode.LeftShift && args.Item2 == KeyCode.Alpha2)
            {
                _command += "@";
            }else if (args.Item1[0] == KeyCode.RightShift && args.Item2 == KeyCode.Alpha2)
            {
                _command += "@";
            }

            UpdateTerminalLayer();
        }

        /// <summary>
        /// Removes the last character from the current command.
        /// </summary>
        /// <param name="args"></param>
        private void RemoveLastTerminalCharacter(List<KeyCode> args)
        {
            if (args.Count <= 0) return;
            if (_command.Length <= 0) return;

            StringBuilder sb = new StringBuilder(_command);
            sb.Remove(_command.Length - 1, 1);
            _command = sb.ToString();
            UpdateTerminalLayer();
        }

        /// <summary>
        /// Writes the command to the screen.
        /// </summary>
        private void UpdateTerminalLayer()
        {
            _userInputLayer.WriteText(_command, false);
        }
    }
}
