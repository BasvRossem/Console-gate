using System.Collections.Generic;
using UnityEngine;
using Visuals;

namespace ControllerStructures
{
    /// <summary>
    /// A menu with options the user can choose.
    /// </summary>
    public class Menu : MonoBehaviour
    {
        [SerializeField] private Monitor monitor;
        [SerializeField] private Keylistener listener;
        private Layer layer;
        private int optionNumber;

        public List<Option> options = new List<Option>();

        private void Start()
        {
            listener.addKey(new List<KeyCode> { KeyCode.Return }, selectOption);
            listener.addKey(new List<KeyCode> { KeyCode.UpArrow }, previous);
            listener.addKey(new List<KeyCode> { KeyCode.DownArrow }, next);

            monitor.uiCursor.Show(true);
            monitor.uiCursor.Blink(false);

            layer = monitor.NewLayer();
            writeOptionsToLayer();
            
            optionNumber = 0;
        }
        
        /// <summary>
        /// Set a new list of options.
        /// </summary>
        /// <param name="newOptions">A list of new options.</param>
        public void SetOptions(List<Option> newOptions)
        {
            options = newOptions;
        }

        // Option interactions
        /// <summary>
        /// Select the next option in the list of options.
        /// </summary>
        /// <param name="arg">Not needed.</param>
        private void next(List<KeyCode> arg)
        {
            if (optionNumber + 1 < options.Count) optionNumber++;
            monitor.uiCursor.SelectRow(optionNumber);
        }

        /// <summary>
        /// Select the previous option in the list of options.
        /// </summary>
        /// <param name="arg">Not needed.</param>
        private void previous(List<KeyCode> arg)
        {
            if (optionNumber - 1 >= 0) optionNumber--;
            monitor.uiCursor.SelectRow(optionNumber);
        }

        /// <summary>
        /// Run the function of the selected option.
        /// </summary>
        /// <param name="arg">Not needed</param>
        private void selectOption(List<KeyCode> arg)
        {
            options[optionNumber].Run();
        }

        // Monitor interactions
        /// <summary>
        /// Writes all the options to the monitor.
        /// </summary>
        private void writeOptionsToLayer()
        {
            foreach (Option option in options)
            {
                layer.WriteLine(option.text);
            }
        }
    }
}