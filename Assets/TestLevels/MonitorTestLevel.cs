using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonitorTestLevel : MonoBehaviour
{
    public Monitor monitor;

    // Start is called before the first frame update
    private void Start()
    {
        monitor.uiCursor.Show(true);

        monitor.ResetMonitor();

        monitor.DrawRectangle(0, 0, 23, 79);
        monitor.DrawRectangle(1, 2, 6, 25);

        monitor.selectedCursor.SetBounds(4, monitor.GetColumnAmount() - 1, 2, monitor.GetRowAmount() - 1);

        monitor.selectedCursor.ResetPosition();

        monitor.AddMonitorTextLine("Hello World");
        monitor.AddMonitorTextLine("How are you doing?");
        monitor.AddMonitorTextLine("Im fine!");
        monitor.AddMonitorTextLine(System.DateTime.Now.ToString());

        monitor.AddCursor("Eraser");
        monitor.SelectCursor("Eraser");
        monitor.AddMonitorTextLine("Hello World");

        monitor.SelectRow(2);
    }

    // Update is called once per frame
    private void Update()
    {
    }
}