using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonitorTest : MonoBehaviour
{
    public Monitor monitor;

    // Start is called before the first frame update
    private void Start()
    {
        monitor.ShowUICursor(true);
    }

    // Update is called once per frame
    private void Update()
    {
        monitor.ResetMonitor();

        monitor.DrawRectangle(0, 0, 23, 79);
        monitor.DrawRectangle(1, 2, 6, 25);

        monitor.cursor = new Cursor();
        monitor.cursor.SetBounds(4, monitor.GetColumnAmount() - 1, 2, monitor.GetRowAmount() - 1);

        monitor.cursor.Reset();

        monitor.AddMonitorTextLine("Hello World");
        monitor.AddMonitorTextLine("How are you doing?");
        monitor.AddMonitorTextLine("Im fine!");
        monitor.AddMonitorTextLine(System.DateTime.Now.ToString());

        monitor.RenderMonitorText();

        monitor.SelectRow(2);
    }
}