using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Visuals;

public class Level0 : MonoBehaviour
{
    [SerializeField] private Monitor monitor;
    [SerializeField] private Keylistener keylistener;

    private List<string> text = new List<string>();
    private int textIndex = 0;

    private string nextCursor;

    // Start is called before the first frame update
    private void Start()
    {
        text.Add("Welcome to console gate!\nThis game is created by Jens Bouman and Bas van Rossem.");
        text.Add("Because of some pandemic, a lot of your classes are online.\nHowever, the professor has not arrived in the chat.\nNo one knows where he is.\nYou decide to take initiative in findinig him.");
        text.Add("The game is on.");

        Debug.Log(keylistener.addKey(new List<KeyCode> { KeyCode.Space }, LoadNext));
        keylistener.addKey(new List<KeyCode> { KeyCode.A }, LoadNext);

        nextCursor = monitor.AddCursor("NextCursor");
        monitor.SelectCursor(nextCursor);
        monitor.selectedCursor.SetBounds(0, 23);

        writeText(text[0]);
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void writeText(string monitorText)
    {
        monitor.SelectCursor(MonitorCursor.DefaultName);
        monitor.SetMonitorText(monitorText);

        monitor.SelectCursor(nextCursor);
        monitor.AddMonitorTextLine("Press [space] to continue...");
    }

    public void LoadNext(List<KeyCode> args)
    {
        Debug.Log("Next screen");
        textIndex += 1;
        if (textIndex < text.Count)
        {
            writeText(text[textIndex]);
        }
        else
        {
            SceneManager.LoadScene("Level 1");
        }
    }
}