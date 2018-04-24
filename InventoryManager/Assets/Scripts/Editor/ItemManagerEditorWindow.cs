using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ItemManagerEditorWindow : EditorWindow
{
    public const string versionNumber = "0.01";

    [MenuItem("Item Management/Item Manager " + versionNumber)]
    static void OpenWindow()
    {
        ItemManagerEditorWindow managerEditorWindow = (ItemManagerEditorWindow)GetWindow(typeof(ItemManagerEditorWindow));
        managerEditorWindow.Show();
    }

    //this is called every OnGUI frame
    private void OnGUI()
    {
        
    }

}
