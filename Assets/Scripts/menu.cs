using System;
using System.Collections.Generic;
using UnityEngine;

public class Option
{
    public string text;

    public virtual void Run()
    {
        throw new NotImplementedException();
    }
}

public class OptionPrint : Option
{
    private string print;
    public OptionPrint(string shownText, string printText)
    {
        text = shownText;
        print = printText;
    }

    public override void Run()
    {
        Debug.Log(print);
    }
}

public class menu : MonoBehaviour
{
    public Monitor monitor;
    public Keylistener listener;

    private int optionNumber;

    private List<Option> options;

    void Start()
    {
        listener.addKey(new List<KeyCode> {KeyCode.Return}, selectOption);
        listener.addKey(new List<KeyCode> {KeyCode.UpArrow}, previous);
        listener.addKey(new List<KeyCode> {KeyCode.DownArrow}, next);

        monitor.ShowUICursor(true);
        monitor.SetUiCursorBlinking(false);
        monitor.cursor = new Cursor();

        optionNumber = 0;

        options = new List<Option>();
        options.Add(new OptionPrint("1. Open monitor 1", "Monitor 1 is opened"));
        options.Add(new OptionPrint("2. Execute order 66", "TRAITORS"));
        options.Add(new OptionPrint("3. Choose option 2", "Option 2 has been chosen, but at what cost?"));
        options.Add(new OptionPrint("4. Have a nice dinner", "You had a nice dinner :-)"));
    }



    void Update()
    {
        monitor.ResetMonitor();

        monitor.cursor.Reset();

        writeOptionsToMonitor();

        monitor.RenderMonitorText();

        monitor.SelectRow(optionNumber);
    }

    private void writeOptionsToMonitor()
    {
        foreach(Option option in options)
        {
            monitor.AddMonitorTextLine(option.text);
        }
    }

    public void next(List<KeyCode> arg)
    {
        if (optionNumber + 1 < options.Count) optionNumber++;
    }

    public void previous(List<KeyCode> arg)
    {
        if (optionNumber - 1 >= 0) optionNumber--;
    }

    public void selectOption(List<KeyCode> arg)
    {
        options[optionNumber].Run();
    }
}
