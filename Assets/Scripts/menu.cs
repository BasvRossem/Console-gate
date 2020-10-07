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

public class Menu : MonoBehaviour
{
    public Monitor monitor;
    public Keylistener listener;

    private int optionNumber;

    public List<Option> options;

    void Start()
    {
        listener.addKey(new List<KeyCode> {KeyCode.Return}, selectOption);
        listener.addKey(new List<KeyCode> {KeyCode.UpArrow}, previous);
        listener.addKey(new List<KeyCode> {KeyCode.DownArrow}, next);

        monitor.ShowUICursor(true);
        monitor.SetUiCursorBlinking(false);
        monitor.cursor = new Cursor();

        optionNumber = 0;
    }

    void Update()
    {
        monitor.ResetMonitor();

        monitor.cursor.Reset();

        writeOptionsToMonitor();

        monitor.RenderMonitorText();

        monitor.SelectRow(optionNumber);
    }

    public void SetOptions(List<Option> newOptions)
    {
        options = newOptions;
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
