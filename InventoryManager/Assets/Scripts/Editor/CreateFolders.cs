using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// This script runs at startup and ensures that all of our scripts have a folder they need.
/// This runs on initialization of the editor
/// </summary>
[InitializeOnLoad]
public static class CreateFolders
{
    static CreateFolders()
    {
        //This will be a top level Resources folder that will hold most of our created variables
        if (!AssetDatabase.IsValidFolder("Assets/Resources"))
        {
            AssetDatabase.CreateFolder("Assets", "Resources");
        }

        //Where we store our category scripts
        if (!AssetDatabase.IsValidFolder("Assets/Resources/Categories"))
        {
            AssetDatabase.CreateFolder("Assets/Resources", "Categories");
        }

        //where we store our ItemPrefabs
        if (!AssetDatabase.IsValidFolder("Assets/Resources/Items"))
        {
            AssetDatabase.CreateFolder("Assets/Resources", "Items");
        }
    }
}
