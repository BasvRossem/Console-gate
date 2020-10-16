using System;
using UnityEngine;

namespace ControllerStructures
{
    /// <summary>
    /// The base option class.
    /// This is the base class for the options that can be used in the menu class.
    /// </summary>
    public class Option
    {
        public string text;

        /// <summary>
        /// Initializes a new instance of the Option class.
        /// </summary>
        /// <param name="shownText">Text that will be shown on the screen.</param>
        public Option(string shownText)
        {
            text = shownText;
        }

        /// <summary>
        /// Virtual function that is ran whenever this option has been chosen.
        /// This should be implemented by every subclass.
        /// </summary>
        public virtual void Run()
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// An option class that will print text in the debug window.
    /// </summary>
    public class OptionPrint : Option
    {
        private string print;

        /// <summary>
        /// Initilizes a new instance of the OptionPrint class.
        /// </summary>
        /// <param name="shownText">The text that will be shown on the monitor.</param>
        /// <param name="printText">The text that will be printed in the debugger.</param>
        public OptionPrint(string shownText, string printText) : base(shownText)
        {
            print = printText;
        }

        /// <summary>
        /// Logs the message specified in the constructor.
        /// </summary>
        public override void Run()
        {
            Debug.Log(print);
        }
    }
}