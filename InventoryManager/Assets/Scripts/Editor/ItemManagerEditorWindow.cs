using System.Collections;
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

    //This group is used to pass info onto the created category script    
    string cName = "";
    List<string> strings = new List<string>();
    List<string> floats = new List<string>();
    List<string> ints = new List<string>();
    List<string> bools = new List<string>();
    List<string> vector3s = new List<string>();

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
    }

    /// <summary>
    /// This Draws the right hand panel when the user is creating an item.
    /// </summary>
    void DrawRightHandCreateItemsPanel()
    {

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
