﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

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

        //Button to create a category
        if (GUILayout.Button("Create Category"))
        {
            if (!creatingItem)
            {
                //start category creation
                rightPanelState = RightPanelState.createCategory;
            }
            else
            {
                //show 'Currently Editing Object' dialogue
                ShowCurrentlyEditingItemDialogue();
            }
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

        EditorGUILayout.EndVertical();
    }

    /// <summary>
    /// This Draws the Right Hand Panel that shows the "Create Category" options
    /// </summary>
    void DrawRightHandCreateCategoryPanel()
    {

    }

    /// <summary>
    /// This Draws the right hand panel when a category has been selected
    /// </summary>
    void DrawRightHandShowItemsPanel()
    {

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
    /// This is called when the user starts an action that may make them lose data.
    /// Yes will change the screen and lose data. No will just close the dialogue.
    /// </summary>
    void ShowCurrentlyEditingItemDialogue()
    {
        EditorUtility.DisplayDialog("Unsaved Data", "You currently have unsaved data. If you change menus you will lose any unsaved changes. Press 'Cancel' to return, press 'OK' to go to new menu.", "OK");
    }
}