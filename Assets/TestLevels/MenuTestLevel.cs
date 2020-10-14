using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuTestLevel : MonoBehaviour
{
    public Menu menu;

    // Start is called before the first frame update
    private void Start()
    {
        List<Option> options = new List<Option>();
        options.Add(new OptionPrint("1. Open monitor 1", "Monitor 1 is opened"));
        options.Add(new OptionPrint("2. Execute order 66", "TRAITORS"));
        options.Add(new OptionPrint("3. Choose option 2", "Option 2 has been chosen, but at what cost?"));
        options.Add(new OptionPrint("4. Have a nice dinner", "You had a nice dinner :-)"));

        menu.SetOptions(options);
    }

    // Update is called once per frame
    private void Update()
    {
    }
}