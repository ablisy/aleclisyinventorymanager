﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This holds data for categories that we are currently editing/using
/// </summary>
public class CategoryDataHolder
{
    public string cName;

    public Dictionary<string, string> categoryStrings = new Dictionary<string, string>();

    public Dictionary<string, float> categoryFloats = new Dictionary<string, float>();

    public Dictionary<string, int> categoryInts = new Dictionary<string, int>();

    public Dictionary<string, bool> categoryBools = new Dictionary<string, bool>();

    public Dictionary<string, Vector3> categoryVector3s = new Dictionary<string, Vector3>();
    //!!!ADD ADDITIONAL VARIABLES HERE!!!

    /// <summary>
    /// Populate our script with all of the settings of a category.
    /// I don't like that this just has various lists of strings. Can be improved upon in the future.
    /// </summary>
    /// <param name="newName"></param>
    /// <param name="stringNames"></param>
    /// <param name="floatNames"></param>
    /// <param name="intNames"></param>
    /// <param name="boolNames"></param>
    /// <param name="vector3Names"></param>
    public void PopulateDataStructure(string newName, List<string> stringNames, List<string> floatNames, List<string> intNames, List<string> boolNames, List<string> vector3Names)
    {
        cName = newName;

        foreach (string stringName in stringNames)
        {
            categoryStrings.Add(stringName, "");
        }

        foreach (string floatName in floatNames)
        {
            categoryFloats.Add(floatName, 0.0f);
        }

        foreach (string intName in intNames)
        {
            categoryInts.Add(intName, 0);
        }

        foreach (string boolName in boolNames)
        {
            categoryBools.Add(boolName, false);
        }

        foreach (string vector3Name in vector3Names)
        {
            categoryVector3s.Add(vector3Name, Vector3.zero);
        }
        //!!!ADD ADDITIONAL VARIABLES HERE!!!
    }
}
