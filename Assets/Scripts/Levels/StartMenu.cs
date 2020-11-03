using System.Collections;
using System.Collections.Generic;
using ControllerStructures;
using UnityEditor;
using UnityEngine;
using Menu = ControllerStructures.Menu;

public class StartMenu : MonoBehaviour
{
    [SerializeField] private Menu menu;
    private List<Option> _levelOptions;
    
    // Start is called before the first frame update
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

        menu.SetOptions(_levelOptions);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
