using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;

public class Monitor : MonoBehaviour
{
    private TextMeshProUGUI monitor;
    private ArrayList textArray;
    private string text;

    private void Start()
    {
        monitor = GetComponent<TextMeshProUGUI>();
        textArray = new ArrayList();

        RemoveMonitorTextLineAtPosition(-1);
        RemoveMonitorTextLineAtPosition(0);
        RemoveMonitorTextLineAtPosition(800);
    }

    private void Update()
    {
        // System.DateTime.Now.ToString()
        SetMonitorText("Hello World");
        AddMonitorTextLine("How are you doing?");
        RenderMonitorText();
    }

    private void RenderMonitorText()
    {
        text = "";

        foreach (string line in textArray)
        {
            text += line;
            text += '\n';
        }

        monitor.SetText(text);
    }

    public void SetMonitorText(string newText)
    {
        textArray.Clear();
        AddMonitorTextLine(newText);
    }

    public void AddMonitorTextLine(string newTextLine)
    {
        textArray.Add(newTextLine);
    }

    public void RemoveMonitorTextLineLast()
    {
        if (textArray.Count > 0)
        {
            int lastIndex = textArray.Count - 1;
            textArray.RemoveAt(lastIndex);
        }
    }

    public void RemoveMonitorTextLineAtPosition(int index)
    {
        if (checkError(index < 0, string.Format("Index {0} cannot be negative.", index))) return;
        if (checkError(index > textArray.Count - 1, string.Format("Index {0} is higher than lines on the monitor.", index))) return;

        textArray.RemoveAt(index);
    }

    private bool checkError(bool condition, string errorMessage)
    {
        if (condition)
        {
            Debug.LogError(errorMessage);
        }
        return condition;
    }
}