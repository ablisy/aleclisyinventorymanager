using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class ParseCategoryScripts
{
    /// <summary>
    /// This gets fed the raw text of a .cs script made by the CreateCategory function
    /// It returns a dictionary that can then be parsed into actual variables
    /// </summary>
    /// <param name="rawText"></param>
    /// <returns></returns>
    public static Dictionary<string, string> GetParsedCategoryDictionary(string rawText)
    {
        Dictionary<string, string> newDictionary = new Dictionary<string, string>();

        //split the text results into separate lines
        string[] result = rawText.Split(new[] { '\r', '\n' });

        foreach (string line in result)
        {
            //we don't want to do anything with the line that identifies this as a public script
            if (line.Contains("public") && !line.Contains("MonoBehaviour"))
            {
                //trim excess whitespace from the line
                string trimmedLine = line.Trim();

                //take the semicolon off the end of the line
                trimmedLine = trimmedLine.TrimEnd(';');

                //separate the line based on whitespace
                char[] delimiters = new char[] {' '};
                string[] words = trimmedLine.Split(delimiters);

                //we need this in a list so we can remove a part of it
                List<string> wordsList = words.ToList<string>();

                //remove the public identifier
                wordsList.Remove("public");

                if (wordsList.Count == 2)
                {
                    //we add the strings in the opposite way so the dictionary reads 'VariableName', 'Variable' so that each key is unique
                    newDictionary.Add(wordsList[1], wordsList[0]);
                }
                else
                {
                    Debug.Log("Something terrible has happened. Please keep category variables in standard format.");
                }
            }
        }

        return newDictionary;
    }
	
}
