using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using UnityEngine;

public class UserInput : MonoBehaviour
{
    public Keylistener listener;
    public Monitor monitor;
    private Cursor userInputCursor;

    public int bottomLine;
    public string textBuffer;


    // Start is called before the first frame update
    void Start()
    {
        //TODO: After Monitor merge, fix cursor
        userInputCursor = new Cursor();
        userInputCursor.SetBounds(4, monitor.GetColumnAmount() - 1, 2, monitor.GetRowAmount() - 1);
        monitor.cursor = userInputCursor;
        monitor.ShowUICursor(true);

        listener.addOption(KeyBoardOptions.Alphabetical, addCharacter);
        listener.addKey(new List<KeyCode> { KeyCode.Space }, addSpace);
        listener.addKey(new List<KeyCode> { KeyCode.Backspace }, removeCharacter);
        bottomLine = monitor.GetRowAmount() - 2;
        textBuffer = "";
        processTextBuffer();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Removes the last character from the internal textbuffer. This is a method used in conjunction with the keylistener.
    /// </summary>
    /// <param name="args">A list of keycodes</param>
    private void removeCharacter(List<KeyCode> args)
    {
        if(args.Count > 0 && textBuffer.Length > 0)
        {
            print(textBuffer.Length); 
            StringBuilder sb = new StringBuilder(textBuffer);
            sb.Remove(textBuffer.Length - 1, 1);
            textBuffer = sb.ToString();
        }
        processTextBuffer();
    }

    /// <summary>
    /// Adds a space to the textbuffer
    /// </summary>
    /// <param name="args">A list of keycodes</param>
    private void addSpace(List<KeyCode> args)
    {
        if(args.Count > 0)
        {
            textBuffer += " ";
        }
        processTextBuffer();
    }

    /// <summary>
    /// Casts each KeyCode given to a char and adds it to the textbuffer.
    /// </summary>
    /// <param name="args">List of keycodes.</param>
    private void addCharacter(List<KeyCode> args)
    {
        if(args.Count > 0)
        {
            foreach(KeyCode k in args)
            {
                print(k);
                textBuffer += (char)k;
            }
        }
        processTextBuffer();
    }


    /// <summary>
    /// Resets the monitor for display. 
    /// </summary>
    private void processTextBuffer()
    {
        // TODO: fix refreshing and writing after Monitor update
        //Cursor previousCursor = monitor.cursor;
        monitor.ResetMonitor();

        monitor.cursor.Reset();
        //monitor.cursor.SetPosition(4, 4);

        //monitor.cursor = userInputCursor;
        monitor.AddMonitorTextLine(textBuffer);
        monitor.RenderMonitorText();



        //monitor.cursor = previousCursor;

    }
}
