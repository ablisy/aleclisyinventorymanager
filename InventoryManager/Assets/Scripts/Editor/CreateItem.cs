using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

/// <summary>
/// This creates a .cs script that inherits from the category that has been selected in the ItemManagerEditorWindow.
/// This is called by ItemManagerEditorWindow.
/// </summary>
public static class CreateItem
{
    /// <summary>
    /// Create Item that inherits from the given category
    /// </summary>
    /// <param name="iName"></param>
    /// <param name="categoryDataHolder"></param>
    public static void CreateItemScript(string iName, CategoryDataHolder categoryDataHolder, string categoryName)
    {
        //create the path that our item script will live
        string copyPath = "Assets/Resources/Categories/" + categoryName + "Items/" + iName + ".cs";
        //do not overwrite
        if (File.Exists(copyPath) == false)
        {
            using (StreamWriter outfile = new StreamWriter(copyPath))
            {
                //write our basic functions
                outfile.WriteLine("using UnityEngine;");
                outfile.WriteLine("using System.Collections;");
                outfile.WriteLine("");
                outfile.WriteLine("public class " + iName + " : " + categoryName);
                outfile.WriteLine("{");
                outfile.WriteLine("");

                //write out our inheritences and our assigned values
                outfile.WriteLine("    public " + iName + "()");
                outfile.WriteLine("    {");

                //===STRINGS===
                foreach (KeyValuePair<string, string> kvp in categoryDataHolder.categoryStrings)
                {
                    outfile.WriteLine("        " + kvp.Key + " = " + "\"" + kvp.Value + "\";");
                }

                outfile.WriteLine("");

                //===floats===
                foreach (KeyValuePair<string, float> kvp in categoryDataHolder.categoryFloats)
                {
                    outfile.WriteLine("        " + kvp.Key + " = "  + kvp.Value.ToString() + "f;");
                }

                outfile.WriteLine("");

                //===ints===
                foreach (KeyValuePair<string, int> kvp in categoryDataHolder.categoryInts)
                {
                    outfile.WriteLine("        " + kvp.Key + " = " + kvp.Value.ToString() + ";");
                }

                outfile.WriteLine("");

                //===bools===
                foreach (KeyValuePair<string, bool> kvp in categoryDataHolder.categoryBools)
                {
                    string writtenBool = "";

                    if (kvp.Value == true)
                    {
                        writtenBool = "true";
                    }
                    else
                    {
                        writtenBool = "false";
                    }

                    outfile.WriteLine("        " + kvp.Key + " = " + writtenBool + ";");
                }

                outfile.WriteLine("");

                //===vector3s===
                foreach (KeyValuePair<string, Vector3> kvp in categoryDataHolder.categoryVector3s)
                {
                    outfile.WriteLine("        " + kvp.Key + " = " + "new Vector3(" + kvp.Value.x + "f, " + kvp.Value.y + "f, " + kvp.Value.z + "f);");
                }

                outfile.WriteLine("");
                outfile.WriteLine("    }");
                outfile.WriteLine("}");
                //done writing
            }
        }

        AssetDatabase.Refresh();

    }
}
