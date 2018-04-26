using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// This script runs at startup and ensures that all of our 
/// </summary>
[InitializeOnLoad]
public static class CreateFolders
{
    static CreateFolders()
    {
        if (!AssetDatabase.IsValidFolder("Assets/Resources"))
        {
            AssetDatabase.CreateFolder("Assets", "Resources");
        }

        if (!AssetDatabase.IsValidFolder("Assets/Resources/Categories"))
        {
            AssetDatabase.CreateFolder("Assets/Resources", "Categories");
        }

        if (!AssetDatabase.IsValidFolder("Assets/Resources/Items"))
        {
            AssetDatabase.CreateFolder("Assets/Resources", "Items");
        }
    }
}
