using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MonitorTestLevel : MonoBehaviour
{
    public Monitor monitor;

    public string testString = "The cursor should now follow this line";
    private int characterIndex;

    private float time;
    private float delayAmount = 0.25f;

    private void Start()
    {
        monitor.uiCursor.Show(true);
        monitor.uiCursor.Blink(false);
        monitor.uiCursor.linkedCursor = monitor.selectedCursor;

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
        monitor.ClearArea(4, 4, 10, 10);
        monitor.SelectCursor(Cursor.DefaultName);

        monitor.AddMonitorTextLine("Hello World");

        monitor.SelectRow(2);

        //monitor.textGrid.Fill('*');
    }

    // Update is called once per frame
    private void Update()
    {
        time += Time.deltaTime;
        if (time >= delayAmount)
        {
            time = 0f;
            monitor.WriteCharacter(testString[characterIndex]);
            characterIndex++;
        }
        if (characterIndex >= testString.Count()) monitor.uiCursor.Blink(true);
    }
}