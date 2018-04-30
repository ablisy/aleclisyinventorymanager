﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;

public class ItemManagerEditorWindow : EditorWindow
{
    public const string versionNumber = "0.01";

    #region PrivateVariables

    //This is used to cycle through what the right panel should be showing at the top level.
    enum RightPanelState
    {
        createCategory,
        showItems,
        createItem,
        defaultMenu
    }
    //Current state of the right panel.
    RightPanelState rightPanelState = RightPanelState.defaultMenu;

    //Width for the left Panel
    float leftPanelWidth = 0.0f;

    //Width for the right Panel
    float rightPanelWidth = 0.0f;

    //set to true when creating something the user should have to save
    bool creatingItem = false;

    //this is to hold the current category that we're showing
    MonoScript currentCategory;

    //Current Items dictionary. This is used to pass info over from the category to the 'create item' function.
    //it's needed to determine what fields we should be drawing in our create item panel
    Dictionary<string, string> currentCategoryDataSet = new Dictionary<string, string>();

    //This group is used to pass info onto the created category script    
    string cName = "";
    List<string> strings = new List<string>();
    List<string> floats = new List<string>();
    List<string> ints = new List<string>();
    List<string> bools = new List<string>();
    List<string> vector3s = new List<string>();

    //This group is used to maintain references to the data types while we type them into the editor window
    Dictionary<string, string> stringsDictionaryList = new Dictionary<string, string>();
    Dictionary<string, float> floatsDictionaryList = new Dictionary<string, float>();
    Dictionary<string, int> intsDictionaryList = new Dictionary<string, int>();
    Dictionary<string, bool> boolsDictionaryList = new Dictionary<string, bool>();
    Dictionary<string, Vector3> vector3sDictionaryList = new Dictionary<string, Vector3>();



    //current category data
    CategoryDataHolder currentCategoryDataHolder;

    #endregion

    #region PublicVariables

    #endregion

    /// <summary>
    /// Open the Editor window
    /// </summary>
    [MenuItem("Item Management/Item Manager " + versionNumber)]
    static void OpenWindow()
    {
        ItemManagerEditorWindow managerEditorWindow = (ItemManagerEditorWindow)GetWindow(typeof(ItemManagerEditorWindow));
        managerEditorWindow.Show();
    }

    //this is called every OnGUI frame
    void OnGUI()
    {
        EditorGUILayout.BeginHorizontal("box");

        //Draw the left hand panel.
        DrawLeftHandCategoryPanel();

        //Draw the Right hand panel.
        //Logic handling what the right panel shows is inside this method
        DrawRightHandPanelToplevel();

        EditorGUILayout.EndHorizontal();
    }

    /// <summary>
    /// This Draws the Left Hand Panel that contains the list of current categories and a button to create a new category
    /// </summary>
    void DrawLeftHandCategoryPanel()
    {
        EditorGUILayout.BeginVertical("box", GUILayout.Width(leftPanelWidth));

        EditorGUILayout.LabelField("Categories");

        //====DRAW THE LIST OF CATEGORIES THAT WE CURRENTLY HAVE====
        //Search through the Resources folder to get a list of categories
        //Get the individual monoscripts inside of the folder, this way there's no cleanup
        List<MonoScript> categoryNames = new List<MonoScript>();
        categoryNames = Resources.LoadAll("Categories", typeof(MonoScript)).Cast<MonoScript>().ToList();
        foreach (MonoScript script in categoryNames)
        {
            if (GUILayout.Button(script.name))
            {
                //maintain a reference to the dictionary that we'll be working with
                currentCategory = script;

                //switch the right panel over because we're not creating a new item yet
                SwitchRightPanel(RightPanelState.showItems, false);
            }
        }
        
        //Button to create a category
        if (GUILayout.Button("Create Category"))
        {
            SwitchRightPanel(RightPanelState.createCategory, true);
        }

        EditorGUILayout.EndVertical();
    }

    /// <summary>
    /// This is where we decide what to draw in our right hand panel. This is where general buttons like 'cancel' go as well
    /// </summary>
    void DrawRightHandPanelToplevel()
    {
        //Set the right panel general structure
        EditorGUILayout.BeginVertical("box", GUILayout.Width(rightPanelWidth));

        //Cycle through what the Right Hand Panel should be showing at the current time
        switch (rightPanelState)
        {
            //Show Create Category Menu
            case (RightPanelState.createCategory):
                DrawRightHandCreateCategoryPanel();
                break;
            //Show Items for Current Selected Category
            case (RightPanelState.showItems):
                DrawRightHandShowItemsPanel();
                break;
            //Show Create Item Menu
            case (RightPanelState.createItem):
                DrawRightHandCreateItemsPanel();
                break;
            //Show "Please Select or Create a Category" Default Menu
            case (RightPanelState.defaultMenu):
                DrawDefaultMenu();
                break;
        }

        //Set the menu back to default. !!!!Temp State while I'm getting category selection in!!!!
        if (GUILayout.Button("Cancel"))
        {
            SwitchRightPanel(RightPanelState.defaultMenu, false);
        }

        EditorGUILayout.EndVertical();
    }

    /// <summary>
    /// This Draws the Right Hand Panel that shows the "Create Category" options
    /// </summary>
    void DrawRightHandCreateCategoryPanel()
    {
        //have the name be the only permanent option
        cName = EditorGUILayout.TextField("Name: ", cName);

        //==STRINGS==
        //draw all of the strings that we've made
        DrawAllListItems("String Name: ", strings);
        //have a button to create new strings
        DrawAddFieldButton("Add A String", strings);
        
        //==FLOATS==
        //draw all of the floats
        DrawAllListItems("Float Name: ", floats);
        //have a button to add a float
        DrawAddFieldButton("Add A Float", floats);

        //==INTS==
        //draw all of the ints
        DrawAllListItems("Int Name: ", ints);
        //have a button to add an int
        DrawAddFieldButton("Add An Int", ints);

        //==BOOLS==
        //draw all of the bools
        DrawAllListItems("Bool Name: ", bools);
        //have a button to add a bool
        DrawAddFieldButton("Add A bool", bools);

        //==VECTOR 3S==
        //draw all of the Vector3s
        DrawAllListItems("Vector3 Name: ", vector3s);
        //have a button to add a vector3
        DrawAddFieldButton("Add A Vector3", vector3s);

        //Save all current entered fields into a new Category
        if (GUILayout.Button("Create Category"))
        {
            currentCategoryDataHolder = new CategoryDataHolder();

            currentCategoryDataHolder.PopulateDataStructure(cName, strings, floats, ints, bools, vector3s);

            //Create the Category using the variables we've made
            CreateCategory.CreateCategoryObject(currentCategoryDataHolder);

            //Clear our global variables
            ResetData();
        }
    }

    /// <summary>
    /// Resets all of the global variables we keep around to populate the category lists.
    /// </summary>
    void ResetData()
    {
        cName = "";
        strings.Clear();
        floats.Clear();
        ints.Clear();
        bools.Clear();
        vector3s.Clear();
    }

    /// <summary>
    /// This creates an add button based on what label it receives
    /// </summary>
    void DrawAddFieldButton(string title, List<string> listToAdd)
    {
        if (GUILayout.Button(title))
        {
            listToAdd.Add("");
        }
    }

    /// <summary>
    /// Draws all items of the provided list, with the provided string as the title for the text field
    /// </summary>
    /// <param name="title"></param>
    /// <param name="listToDraw"></param>
    void DrawAllListItems(string title, List<string> listToDraw)
    {
        for (int i = 0; i < listToDraw.Count; i++)
        {
            listToDraw[i] = EditorGUILayout.TextField(title, listToDraw[i]);
        }
    }

    /// <summary>
    /// This Draws the right hand panel when a category has been selected
    /// </summary>
    void DrawRightHandShowItemsPanel()
    {
        //get our script and use its name for our panel title
        EditorGUILayout.LabelField(currentCategory.name);

        //Draw all of the item scripts that are in our category folder
        List<MonoScript> itemNames = new List<MonoScript>();
        itemNames = Resources.LoadAll("Categories/" + currentCategory.name + "Items", typeof(MonoScript)).Cast<MonoScript>().ToList();

        foreach (MonoScript script in itemNames)
        {
            if (GUILayout.Button(script.name))
            {
                Debug.Log(script.text);
            }
        }

        //for creating items, we need to switch over to the new right panel, while determining what we should be showing in the panel
        if (GUILayout.Button("Create Item"))
        {
            string rawCategoryText = currentCategory.text;

            currentCategoryDataSet = ParseCategoryScripts.GetParsedCategoryDictionary(rawCategoryText);

            currentCategoryDataHolder = new CategoryDataHolder();

            PopulateCategoryDictionaries();

            //go to the create item right panel window
            SwitchRightPanel(RightPanelState.createItem, true);
        }
    }

    /// <summary>
    /// This is called to populate our currentCategoryDataHolder when creating a new item. This is used to create what fields our new item is going to need.
    /// </summary>
    void PopulateCategoryDictionaries()
    {
        foreach (KeyValuePair<string, string> pair in currentCategoryDataSet)
        {
            switch (pair.Value)
            {
                case ("string"):
                    //I can use this to draw things, but to pass it I should be creating a CategoryDataHolder
                    currentCategoryDataHolder.categoryStrings.Add(pair.Key, "");
                    break;
                case ("float"):
                    currentCategoryDataHolder.categoryFloats.Add(pair.Key, 0.0f);
                    break;
                case ("int"):
                    currentCategoryDataHolder.categoryInts.Add(pair.Key, 0);
                    break;
                case ("bool"):
                    currentCategoryDataHolder.categoryBools.Add(pair.Key, false);
                    break;
                case ("Vector3"):
                    currentCategoryDataHolder.categoryVector3s.Add(pair.Key, Vector3.zero);
                    break;
            }
        }
    }

    void ClearGlobalCategoryDictionaries()
    {

    }

    /// <summary>
    /// This Draws the right hand panel when the user is creating an item.
    /// The currentCategoryDataSet is populated before this is called. I don't think it's ideal to make this rely on a global variable but it's the most flexible way.
    /// </summary>
    void DrawRightHandCreateItemsPanel()
    {

        //create another dictionary of string, string then pass that to created bool
        //because we need to pass the actual name and value, we need to pass name, type, and value to the next script. How do we do that?

        //create a new script type, or whatevery it's called to pass it. What I wrote for front row objects

        cName = EditorGUILayout.TextField("Item Name: ", cName);

        //have a loop to draw every variable we might have in the categories

        //==STRINGS==
        DrawCategoryDictionary(currentCategoryDataHolder.categoryStrings);

        //==FLOATS==
        DrawCategoryDictionary(currentCategoryDataHolder.categoryFloats);

        //==INTS==
        DrawCategoryDictionary(currentCategoryDataHolder.categoryInts);

        //==BOOLS==
        DrawCategoryDictionary(currentCategoryDataHolder.categoryBools);

        //==VECTOR3S==
        DrawCategoryDictionary(currentCategoryDataHolder.categoryVector3s);

        if (GUILayout.Button("CreateTheItem"))
        {
            CreateItem.CreateItemScript(cName, currentCategoryDataHolder, currentCategory.name);

            SwitchRightPanel(RightPanelState.showItems, false);
        }
    }

    void DrawCategoryDictionary(Dictionary<string, string> passedDictionary)
    {
        var dictionary = new Dictionary<string, string>();
        //dictionary = currentCategoryDataHolder.categoryStrings;
        dictionary = passedDictionary;
        var keys = new List<string>(dictionary.Keys);
        foreach (string key in keys)
        {
            dictionary[key] = EditorGUILayout.TextField(key, dictionary[key]);
        }
    }

    void DrawCategoryDictionary(Dictionary<string, float> passedDictionary)
    {
        var dictionary = new Dictionary<string, float>();
        //dictionary = currentCategoryDataHolder.categoryStrings;
        dictionary = passedDictionary;
        var keys = new List<string>(dictionary.Keys);
        foreach (string key in keys)
        {
            dictionary[key] = EditorGUILayout.FloatField(key, dictionary[key]);
        }
    }

    void DrawCategoryDictionary(Dictionary<string, int> passedDictionary)
    {
        var dictionary = new Dictionary<string, int>();
        //dictionary = currentCategoryDataHolder.categoryStrings;
        dictionary = passedDictionary;
        var keys = new List<string>(dictionary.Keys);
        foreach (string key in keys)
        {
            dictionary[key] = EditorGUILayout.IntField(key, dictionary[key]);
        }
    }

    void DrawCategoryDictionary(Dictionary<string, bool> passedDictionary)
    {
        var dictionary = new Dictionary<string, bool>();
        //dictionary = currentCategoryDataHolder.categoryStrings;
        dictionary = passedDictionary;
        var keys = new List<string>(dictionary.Keys);
        foreach (string key in keys)
        {
            dictionary[key] = EditorGUILayout.Toggle(key, dictionary[key]);
        }
    }

    void DrawCategoryDictionary(Dictionary<string, Vector3> passedDictionary)
    {
        var dictionary = new Dictionary<string, Vector3>();
        //dictionary = currentCategoryDataHolder.categoryStrings;
        dictionary = passedDictionary;
        var keys = new List<string>(dictionary.Keys);
        foreach (string key in keys)
        {
            dictionary[key] = EditorGUILayout.Vector3Field(key, dictionary[key]);
        }
    }

    /// <summary>
    /// This is drawn when nothing is selected
    /// </summary>
    void DrawDefaultMenu()
    {
        EditorGUILayout.LabelField("Select a category or create a new one.");
    }

    /// <summary>
    /// This is the only place panels should be changed. This will call a dialog if user has unsaved data.
    /// </summary>
    /// <param name="newPanelState"></param>
    /// <param name="Set true if new panel is a creation menu."></param>
    void SwitchRightPanel(RightPanelState newPanelState, bool creatingNewItem)
    {
        //We're creating an Item. We need to let the user know that they'll lose data if they do switch the panel
        if (creatingItem)
        {
            //open dialog to tell user they'll lose data if they continue
            if (EditorUtility.DisplayDialog("Unsaved Data",
            "You currently have unsaved data. If you change menus you will lose any unsaved changes. Press 'Cancel' to return, press 'OK' to go to new menu.",
            "OK",
            "Cancel"))
            {
                //User knows they're losing data. Switch the menu.
                rightPanelState = newPanelState;
                creatingItem = creatingNewItem;
                ResetData();
            }
        }
        else
        {
            //We're not Creating an item. We can switch right away.
            rightPanelState = newPanelState;
            creatingItem = creatingNewItem;
        }
    }
}
