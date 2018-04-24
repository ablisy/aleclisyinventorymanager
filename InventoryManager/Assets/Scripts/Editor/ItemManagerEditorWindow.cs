using System.Collections;
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
        createItem
    }
    //Current state of the right panel.
    RightPanelState rightPanelState = RightPanelState.showItems;

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
        //Draw the left hand panel.
        DrawLeftHandCategoryPanel();

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
            default:
                DrawDefaultMenu();
                break;
        }
    }

    /// <summary>
    /// This Draws the Left Hand Panel that contains the list of current categories and a button to create a new category
    /// </summary>
    void DrawLeftHandCategoryPanel()
    {

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

    }
}
