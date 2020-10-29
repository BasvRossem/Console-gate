using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using Visuals;

public class Level1 : MonoBehaviour
{
    [SerializeField] private Monitor monitor = null;
    [SerializeField] private Keylistener keylistener = null;

    private string screenCursor;
    private string terminalCursor;

    private int currentView = 0;

    private string command = "";

    // Start is called before the first frame update
    private void Start()
    {
        keylistener.addKey(new List<KeyCode> { KeyCode.DownArrow }, MoveView);
        keylistener.addKey(new List<KeyCode> { KeyCode.UpArrow }, MoveView);

        keylistener.addOption(KeyBoardOptions.Alphabetical, UpdateTerminal);
        keylistener.addKey(new List<KeyCode> { KeyCode.Space }, UpdateTerminal);
        keylistener.addKey(new List<KeyCode> { KeyCode.Period }, UpdateTerminal);
        keylistener.addKey(new List<KeyCode> { KeyCode.LeftShift, KeyCode.Alpha2 }, UpdateTerminal);
        keylistener.addKey(new List<KeyCode> { KeyCode.Backspace }, RemoveLastTerminalCharacter);
        keylistener.addKey(new List<KeyCode> { KeyCode.Return }, SendCommand);

        screenCursor = monitor.AddCursor("ScreenCursor");
        terminalCursor = monitor.AddCursor("TermminalCursor");
        monitor.SelectCursor(terminalCursor);
        monitor.selectedCursor.SetBounds(min_y: 23, max_y: 23);

        monitor.SelectCursor(screenCursor);
        monitor.selectedCursor.ResetPosition();

        LoadChatlog();
    }

    // Update is called once per frame
    private void Update()
    {
        if (currentView == 0)
        {
            LoadChatlog();
        }
        if (currentView == 1)
        {
            LoadFile();
        }

        monitor.SelectCursor(terminalCursor);
        monitor.selectedCursor.SetBounds(min_y: 23, max_y: 23);
        monitor.ClearArea(22 + monitor.verticalViewOffset, 0, 23 + monitor.verticalViewOffset, 79);
        monitor.selectedCursor.SetBounds(min_y: 23 + monitor.verticalViewOffset, max_y: 23 + monitor.verticalViewOffset);
        monitor.selectedCursor.ResetPosition();
        monitor.WriteLine(command);
    }

    private void LoadChatlog()
    {
        monitor.SelectCursor(screenCursor);
        monitor.selectedCursor.ResetPosition();
        monitor.ResetMonitor();
        monitor.SetMonitorText(@"-------------------
Chatlog 23 - 10 - 2020
------------------ -
23 - 10 - 2020 16:03, Docent:
Bij deze geef ik jullie een extra bijlage als toevoeging op de les

23 - 10 = 2020 16:04, Remco:
ligt het aan mij, of kan ik het bestand nog niet downloaden ?

23 - 10 - 2020 16:04, Julianne:
De docent heeft nog geen download - link gegeven....

23 - 10 - 2020 16:05, Docent:
Oeps, foutje, bij deze is hij meegedeeld.

23 - 10 - 2020 16:06, Julianne:
Bedankt!

23 - 10 - 2020 16:06, Docent:
Ik wil graag dat jullie deze stof voor de volgende les(26 - 10 - 2020) doornemen.

------------------ -
Chatlog 24 - 10 - 2020
------------------ -

-------------------
Chatlog 25 - 10 - 2020
------------------ -
25 - 10 - 2020 20:48, SYSTEM:
Succesfully downloaded ""Appendix A.txt""");
    }

    public void LoadFile()
    {
        monitor.SelectCursor(screenCursor);
        monitor.selectedCursor.ResetPosition();
        monitor.ResetMonitor();
        monitor.WriteLine(@"Filename: Appendix A
File extension: .txt
Path to file: / Downloads / Appendix A

Date created: 20 - 03 - 2014
Size: 12381 bytes
Author: Docent
IP - adress owner: 52.232.56.79");
        monitor.DrawLineHorizontal(8, 0, monitor.GetColumnAmount());
        monitor.selectedCursor.Move(monitor.selectedCursor.Down);
        monitor.WriteLine(@"Content:

Lorem ipsum dolor sit amet, consectetur adipiscing elit.
Nulla vehicula et ex id eleifend.
Sed cursus, eros non fringilla finibus, augue velit aliquam felis,
ac mollis augue sem in arcu.
Nunc odio sapien, varius in vestibulum in, ultrices a diam.
Nulla vestibulum ac dolor quis eleifend.
Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere curae;
Praesent suscipit feugiat felis id viverra.
Praesent a lectus sapien.
Nunc ac mollis ipsum.
Ut mauris dolor, maximus id diam in, auctor suscipit neque.
Nam velit eros, accumsan non libero at, placerat semper augue.
Maecenas scelerisque semper venenatis.
Suspendisse vel dolor velit.
Nunc imperdiet cursus velit eget porta.
In eget ex purus.
Vivamus pellentesque quam in arcu ultrices varius.

Sed euismod commodo egestas.
Aliquam posuere vulputate ullamcorper.
Nulla consequat porttitor ultricies.
Nulla augue metus, cursus ut feugiat sed, fermentum cursus dui.
Aenean non ex tortor.
Sed dolor nunc, molestie sit amet mattis sed, finibus sit amet neque.
Nullam consequat odio vel ornare facilisis.
Integer viverra, metus ac eleifend lacinia, ex risus sagittis leo,
ut maximus sem leo id neque.
Ut interdum elementum lectus sed venenatis.
Curabitur ultrices nisi eu diam interdum fringilla.
Nam eget magna ac tellus tristique pulvinar.
Suspendisse tempus nunc id metus volutpat, dictum efficitur eros feugiat.
Aliquam malesuada iaculis nulla nec lobortis.
Nam feugiat lorem quis nulla pharetra sodales.

Suspendisse vitae convallis ipsum.
Fusce tincidunt ultrices erat sagittis convallis.
Vivamus eu felis mattis, pulvinar velit sit amet, molestie mi.
Proin condimentum diam a dui sagittis mattis eu vel ex.
Nam varius pulvinar lectus et ultrices.
Sed eget nisl ut mauris interdum venenatis in sit amet tellus.
Integer facilisis pulvinar dolor a volutpat.

Vivamus lobortis congue diam, id volutpat tellus porttitor vel.
Vestibulum laoreet nulla eget elit vehicula, vitae efficitur libero tempor.
Maecenas suscipit rutrum justo at porttitor.
Proin semper rhoncus accumsan.
Mauris malesuada sed libero in scelerisque.
Nam tellus magna, ultricies eget rhoncus eget, congue volutpat nisi.
Sed non varius tortor.
Mauris consectetur blandit arcu eu pharetra.
In hac habitasse platea dictumst.
Mauris at ultrices lacus.
Interdum et malesuada fames ac ante ipsum primis in faucibus.
In vel velit quis sapien commodo vulputate eu eu mi.
Maecenas elementum libero sem, nec semper leo ultrices eu.

Ut arcu nisl, placerat vitae pharetra vel, tincidunt ornare magna.
Suspendisse potenti.
Quisque in mauris ut lacus imperdiet ornare.
Cras lacinia in diam at hendrerit.
Suspendisse congue lectus nec consectetur ultricies.
Interdum et malesuada fames ac ante ipsum primis in faucibus.
Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere curae;
Vestibulum blandit venenatis urna sit amet sagittis.
Fusce commodo malesuada nibh, et commodo metus tincidunt eleifend.
Pellentesque pellentesque est eu rutrum fringilla.
Praesent tortor orci, placerat vitae tortor ut, ultrices suscipit ex.
Nulla semper tempor metus in fringilla.
Nullam mollis ultricies posuere.
Nulla facilisi.
Pellentesque eu ante laoreet, maximus sem porta, venenatis nisl.
Integer egestas quam et diam bibendum lobortis.");
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene("Level 2");
    }

    public void MoveView(List<KeyCode> args)
    {
        if (args.Count <= 0) return;

        if (args[0] == KeyCode.DownArrow) monitor.MoveView(1);
        else if (args[0] == KeyCode.UpArrow) monitor.MoveView(-1);
    }

    public void UpdateTerminal(List<KeyCode> args)
    {
        if (args.Count <= 0) return;
        if (args[0] == KeyCode.LeftShift && args[1] == KeyCode.Alpha2)
        {
            command += "@";
        }

        foreach (KeyCode k in args)
        {
            command += (char)k;
        }
    }

    public void RemoveLastTerminalCharacter(List<KeyCode> args)
    {
        if (args.Count <= 0) return;
        if (command.Length <= 0) return;

        print(command.Length);
        StringBuilder sb = new StringBuilder(command);
        sb.Remove(command.Length - 1, 1);
        command = sb.ToString();
    }

    public void SendCommand(List<KeyCode> args)
    {
        if (args.Count <= 0) return;

        if (command == "ssh user@52.232.56.79") GotoNextScene();
        command = "";
    }

    public void GotoNextScene()
    {
        currentView = 1;
    }
}