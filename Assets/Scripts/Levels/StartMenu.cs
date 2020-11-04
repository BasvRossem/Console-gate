using System.Collections;
using System.Collections.Generic;
using ControllerStructures;
using Core;
using UnityEditor;
using UnityEngine;
using Visuals;
using Menu = ControllerStructures.Menu;

public class StartMenu : MonoBehaviour
{
    [SerializeField] private Monitor monitor;
    [SerializeField] private Menu menu;
    private List<Option> _levelOptions;

    private Layer _artLayer;

    void Start()
    {
        if(Tools.CheckError(menu == null, "No menu available.")) return;
        _levelOptions = new List<Option>
        {
            new OptionLoadLevel("Introduction", "Level 0"),
            new OptionLoadLevel("Chapter 1", "Level 1"),
            new OptionLoadLevel("Chapter 2", "Level 2"),
            new OptionLoadLevel("Chapter 3", "Level 3"),
            new OptionLoadLevel("Chapter 4", "Level 4")
        };

        // monitor.CalibrateTextMesh();
        _artLayer = monitor.NewLayer();
        _artLayer.WriteText(TextManager.GetStartScreenArt());
        menu.layer.view.SetExternalPosition(new GridPosition(11, 0));
        menu.SetOptions(_levelOptions);
        menu.layer.zIndex = 1;
    }

}
