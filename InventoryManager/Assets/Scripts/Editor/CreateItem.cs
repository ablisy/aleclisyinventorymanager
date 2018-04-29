using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This creates a .cs script that inherits from the category that has been selected in the ItemManagerEditorWindow.
/// This is called by ItemManagerEditorWindow.
/// </summary>
public static class CreateItem
{
    /// <summary>
    /// This accepts a dictionary structured as "Type", "name". 
    /// </summary>
    /// <param name="acceptedVariables"></param>
    /// <returns></returns>
    public static void CreateItemScript(Dictionary<string, string> acceptedVariables, string categoryName, string iName)
    {
        //we receive what the category has as variables and apply them to our script
        //now should I make this so that we just go ahead and parse it? Or should I make another class that parses them

        //find where we're supposed to be placing our item
        string folderPath = "Assets/Resources/Categories/" + categoryName + "Items";




    }
}
