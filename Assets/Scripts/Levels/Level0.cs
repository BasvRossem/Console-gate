using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Visuals;

public class Level0 : MonoBehaviour
{
    [SerializeField] private Monitor _monitor = null;
    [SerializeField] private Keylistener _keyListener = null;

    private readonly List<string> _text = new List<string>();
    private int _textIndex = 0;

    private string _nextCursor;
    private Layer _textLayer;
    private Layer _continueLayer;

    // Start is called before the first frame update
    private void Start()
    {
        _text.Add("Welcome to Console Gate!\nThis game is created by Jens Bouman and Bas van Rossem.");
        _text.Add("Because of some pandemic, a lot of your classes are online.\nHowever, the professor has not arrived in the chat.\nNo one knows where he is.\nYou decide to take initiative in finding him.");
        _text.Add("The game is on.");

        Debug.Log(_keyListener.addKey(new List<KeyCode> { KeyCode.Space }, LoadNext));

        _textLayer = _monitor.NewLayer();
        _textLayer.view.SetSize(22, Monitor.ColumnAmount);

        _continueLayer = _monitor.NewLayer();
        _continueLayer.view.SetSize(1, Monitor.ColumnAmount);
        _continueLayer.view.SetPosition(23, 0);

        _monitor.uiCursor.linkedLayer = _continueLayer;
        _monitor.uiCursor.Blink(true);

        writeText(_text[0]);
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void writeText(string monitorText)
    {
        _textLayer.WriteText(monitorText);
        _continueLayer.WriteText("Press [space] to continue...");
    }

    public void LoadNext(List<KeyCode> args)
    {
        Debug.Log("Next screen");
        _textIndex += 1;
        if (_textIndex < _text.Count)
        {
            writeText(_text[_textIndex]);
        }
        else
        {
            SceneManager.LoadScene("Level 1");
        }
    }
}