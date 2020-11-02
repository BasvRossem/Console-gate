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
        public Monitor monitor;
        public Keylistener listener;

        private int optionNumber;

        public List<Option> options = new List<Option>();

        private void Start()
        {
            listener.addKey(new List<KeyCode> { KeyCode.Return }, selectOption);
            listener.addKey(new List<KeyCode> { KeyCode.UpArrow }, previous);
            listener.addKey(new List<KeyCode> { KeyCode.DownArrow }, next);

            monitor.uiCursor.Show(true);
            monitor.uiCursor.Blink(false);

            optionNumber = 0;
        }

        private void Update()
        {
            monitor.ResetMonitor();
            monitor.selectedCursor.ResetPosition();

            writeOptionsToMonitor();

            monitor.SelectRow(optionNumber);
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
        }

        /// <summary>
        /// Select the previous option in the list of options.
        /// </summary>
        /// <param name="arg">Not needed.</param>
        private void previous(List<KeyCode> arg)
        {
            if (optionNumber - 1 >= 0) optionNumber--;
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
        private void writeOptionsToMonitor()
        {
            foreach (Option option in options)
            {
                monitor.WriteLine(option.text);
            }
        }
    }
}