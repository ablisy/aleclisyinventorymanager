using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;
using UnityEditor;

/// <summary>
/// This creates a .cs script based on inputs from ItemManagerEditorWindow
/// The portion of this script that creates and populates the .cs file was taken from this source: 
/// https://answers.unity.com/questions/12599/editor-script-need-to-create-class-script-automati.html
/// </summary>
public static class CreateCategory
{
    /// <summary>
    /// Create Category with chosen variables as roots.
    /// </summary>
    /// <param name="cName"></param>
    /// <param name="strings"></param>
    /// <param name="floats"></param>
    /// <param name="ints"></param>
    /// <param name="bools"></param>
    /// <param name="vector3s"></param>
    public static void CreateCategoryObject(CategoryDataHolder categoryDataHolder)
    {
        //Get our lists of keys from the dictionaries. These become our variable names
        string cName = categoryDataHolder.cName;
        List<string> strings = new List<string>(categoryDataHolder.categoryStrings.Keys);
        List<string> floats = new List<string>(categoryDataHolder.categoryFloats.Keys);
        List<string> ints = new List<string>(categoryDataHolder.categoryInts.Keys);
        List<string> bools = new List<string>(categoryDataHolder.categoryBools.Keys);
        List<string> vector3s = new List<string>(categoryDataHolder.categoryVector3s.Keys);

        //Remove whitespace and minus
        cName = cName.Replace(" ", "_");
        cName = cName.Replace("-", "_");

        //create the folder for the category
        AssetDatabase.CreateFolder("Assets/Resources", cName + "Items");

        //Create the path that our category script will live
        string copyPath = "Assets/Resources/Categories/" + cName + ".cs";
        Debug.Log("Creating Category File: " + cName);
        //do not overwrite
        if (File.Exists(copyPath) == false)
        {
            using (StreamWriter outfile = new StreamWriter(copyPath))
            {
                //Write our basic functions
                outfile.WriteLine("using UnityEngine;");
                outfile.WriteLine("using System.Collections;");
                outfile.WriteLine("");
                outfile.WriteLine("public class " + cName + " : MonoBehaviour");
                outfile.WriteLine("{");
                outfile.WriteLine("");
                
                //write out our public strings
                foreach (string uString in strings)
                {
                    outfile.WriteLine("    public string " + uString + ";");
                }

                outfile.WriteLine("");


                //write out our public floats
                foreach (string uFloat in floats)
                {
                    outfile.WriteLine("    public float " + uFloat + ";");
                }

                outfile.WriteLine("");

                //write out our public ints
                foreach (string uInt in ints)
                {
                    outfile.WriteLine("    public int " + uInt + ";");
                }

                outfile.WriteLine("");

                //write out our public bools
                foreach (string uBool in bools)
                {
                    outfile.WriteLine("    public bool " + uBool + ";");
                }

                outfile.WriteLine("");

                //write out our public vector3s
                foreach (string uVector3 in vector3s)
                {
                    outfile.WriteLine("    public Vector3 " + uVector3 + ";");
                }

                outfile.WriteLine("");

                outfile.WriteLine("}");
            }//File Written
        }
        AssetDatabase.Refresh();
    }
}
