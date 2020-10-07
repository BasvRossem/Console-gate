using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class menu : MonoBehaviour
{
    public Monitor monitor;

    void Start()
    {
        monitor.ShowUICursor(true);
        monitor.SetUiCursorBlinking(false);
        monitor.cursor = new Cursor();
    }

    void Update()
    {
        monitor.ResetMonitor();

        
        monitor.cursor.Reset();
        monitor.AddMonitorTextLine("1. Open monitor 1");
        monitor.AddMonitorTextLine("2. Execute order 66");
        monitor.AddMonitorTextLine("3. Choose option 2");
        monitor.AddMonitorTextLine("4. Have a nice dinner");

        monitor.RenderMonitorText();


        monitor.SelectRow(2);
    }
}
