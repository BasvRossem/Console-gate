using System.Collections.Generic;
using UnityEngine;
using Visuals;
using UserInput;

namespace ControllerStructures
{
    /// <summary>
    /// A menu with options the user can choose.
    /// </summary>
    public class Menu : MonoBehaviour
    {
        [SerializeField] private Monitor monitor;
        [SerializeField] private KeyListener listener;
        private Layer _layer;
        private int _optionNumber;

        private List<Option> _options = new List<Option>();

        private void Start()
        {
            listener.AddKey(new List<KeyCode> { KeyCode.Return }, SelectOption);
            listener.AddKey(new List<KeyCode> { KeyCode.UpArrow }, Previous);
            listener.AddKey(new List<KeyCode> { KeyCode.DownArrow }, Next);

            monitor.uiCursor.Show(true);
            monitor.uiCursor.Blink(false);
            
            _layer = monitor.NewLayer();
            WriteOptionsToLayer();
            
            _optionNumber = 0;
        }
        
        /// <summary>
        /// Set a new list of options.
        /// </summary>
        /// <param name="newOptions">A list of new options.</param>
        public void SetOptions(List<Option> newOptions)
        {
            _options = newOptions;
        }

        // Option interactions
        /// <summary>
        /// Select the next option in the list of options.
        /// </summary>
        /// <param name="arg">Not needed.</param>
        private void Next(List<KeyCode> arg)
        {
            if (_optionNumber + 1 < _options.Count) _optionNumber++;
            monitor.uiCursor.SelectRow(_optionNumber);
        }

        /// <summary>
        /// Select the previous option in the list of options.
        /// </summary>
        /// <param name="arg">Not needed.</param>
        private void Previous(List<KeyCode> arg)
        {
            if (_optionNumber - 1 >= 0) _optionNumber--;
            monitor.uiCursor.SelectRow(_optionNumber);
        }

        /// <summary>
        /// Run the function of the selected option.
        /// </summary>
        /// <param name="arg">Not needed</param>
        private void SelectOption(List<KeyCode> arg)
        {
            _options[_optionNumber].Run();
        }

        // Monitor interactions
        /// <summary>
        /// Writes all the options to the monitor.
        /// </summary>
        private void WriteOptionsToLayer()
        {
            foreach (Option option in _options)
            {
                _layer.WriteLine(option.text);
            }
        }
    }
}

