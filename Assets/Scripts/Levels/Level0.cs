using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Visuals;

public class Level0 : MonoBehaviour
{
    [SerializeField] private Monitor monitor = null;
    [SerializeField] private Keylistener keyListener = null;

    private Layer _textLayer;
    private Layer _continueLayer;
    
    private List<string> _text = new List<string>();
    private int _textIndex = 0;

    // Start is called before the first frame update
    private void Start()
    {
        _text.Add("Welcome to Console Gate!\nThis game is created by Jens Bouman and Bas van Rossem.");
        _text.Add("Because of some pandemic, a lot of your classes are online.\nHowever, the professor has not arrived in the chat.\nNo one knows where he is.\nYou decide to take initiative in finding him.");
        _text.Add("The game is on.");

        if (Tools.CheckError(monitor == null, "No Monitor object has been added")) return;
        if (Tools.CheckError(keyListener == null, "No KeyListener object has been added")) return;

        keyListener.addKey(new List<KeyCode> { KeyCode.Space }, LoadNext);

        _textLayer = monitor.NewLayer();
        _textLayer.view.SetSize(new GridSize(22, Monitor.Size.columns));

        _continueLayer = monitor.NewLayer();
        _continueLayer.view.SetSize(new GridSize(1, Monitor.Size.columns));
        _continueLayer.view.SetExternalPosition(new GridPosition(23, 0));

        monitor.uiCursor.linkedLayer = _continueLayer;
        monitor.uiCursor.Blink(true);

        WriteText(_text[0]);
    }

    private void WriteText(string monitorText)
    {
        _textLayer.WriteText(monitorText);
        _continueLayer.WriteText("Press [space] to continue...", false);
    }

    public void LoadNext(List<KeyCode> args)
    {
        Debug.Log("Next screen");
        _textIndex += 1;
        if (_textIndex < _text.Count)
        {
            WriteText(_text[_textIndex]);
        }
        else
        {
            SceneManager.LoadScene("Level 1");
        }
    }
}