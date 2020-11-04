using System.Collections.Generic;
using Core;
using UnityEngine;
using UnityEngine.SceneManagement;
using Visuals;
using UserInput;

public class Level0 : MonoBehaviour
{
    [SerializeField] private Monitor monitor = null;
    [SerializeField] private KeyListener keyListener = null;

    private Layer _textLayer;
    private Layer _continueLayer;
    
    private List<string> _text = new List<string>();
    private int _textIndex = 0;

    // Start is called before the first frame update
    private void Start()
    {
        _text.Add(TextManager.GetLevel0Intro1());
        _text.Add(TextManager.GetLevel0Intro2());
        _text.Add(TextManager.GetLevel0Intro3());

        if (Tools.CheckError(monitor == null, "No Monitor object has been added")) return;
        if (Tools.CheckError(keyListener == null, "No KeyListener object has been added")) return;
        
        keyListener.AddKey(new List<KeyCode> { KeyCode.Space }, LoadNext);

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