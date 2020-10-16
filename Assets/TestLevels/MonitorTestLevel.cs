using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditorInternal;
using UnityEngine;
using Visuals;
using MonitorCursor = Visuals.MonitorCursor;

public class MonitorTestLevel : MonoBehaviour
{
    public Monitor monitor;

    public string testString = "The cursor should now follow this line";
    private int characterIndex;

    private float time;
    private float delayAmount = 0.15f;

    private void Start()
    {
        monitor.uiCursor.Show(true);
        monitor.uiCursor.Blink(false);
        monitor.uiCursor.linkedCursor = monitor.selectedCursor;

        monitor.ResetMonitor();

        monitor.DrawRectangle(0, 0, 23, 79);
        monitor.DrawRectangle(1, 2, 6, 25);

        monitor.SelectCursor(MonitorCursor.DefaultName);
        monitor.selectedCursor.SetBounds(4, 2, monitor.GetColumnAmount() - 1, monitor.GetRowAmount() - 1);

        monitor.selectedCursor.ResetPosition();

        monitor.AddMonitorTextLine("Hello World");
        monitor.AddMonitorTextLine("How are you doing?");
        monitor.AddMonitorTextLine("Im fine!");
        monitor.AddMonitorTextLine(System.DateTime.Now.ToString());

        Debug.Log(monitor.selectedCursor.GetName());

        monitor.AddCursor("Eraser");
        monitor.SelectCursor("Eraser");
        monitor.ClearArea(4, 4, 10, 10);

        monitor.SelectCursor(MonitorCursor.DefaultName);
        monitor.AddMonitorTextLine("Hello World");

        monitor.SelectRow(2);
    }

    // Update is called once per frame
    private void Update()
    {
        time += Time.deltaTime;
        if ((time >= delayAmount) && (characterIndex < testString.Count()))
        {
            time = 0f;
            monitor.WriteCharacter(testString[characterIndex]);
            characterIndex++;
        }
        if (characterIndex >= testString.Count()) monitor.uiCursor.Blink(true);
    }
}