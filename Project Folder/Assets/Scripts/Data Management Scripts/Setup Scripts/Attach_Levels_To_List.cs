using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//---------------------------------------------------------------------------------------------------------------------------\\
// File Start
//---------------------------------------------------------------------------------------------------------------------------\\

/// <summary>
/// This will be used to add a list of levels and their data onto a dropdown list for a level select menu. 
/// </summary>
public class Attach_Levels_To_List : MonoBehaviour
{
    //---------------------------------------------------------------------------------------------------------------------------\\
    // Data Members Start
    //---------------------------------------------------------------------------------------------------------------------------\\

    /// <summary>
    /// This is the dropdown list which will be used for the level select menu. 
    /// </summary>
    [SerializeField]
    Dropdown m_LevelDropDown;

    /// <summary>
    /// This is the level which has been selected by the player on the level select menu. 
    /// </summary>
    int m_iLevelToLoad = -1;

    /// <summary>
    /// This will allow for the game to heck if a level has been selected and only then will it allow for a level to be loaded. 
    /// </summary>
    bool m_bLoadnewLevel = false;

    //---------------------------------------------------------------------------------------------------------------------------\\
    // Member Functions Start
    //---------------------------------------------------------------------------------------------------------------------------\\

    /// <summary>
    /// This will be used to setup the dropdown menu. It's main functionality will be to clear the dropdown list of all of it's 
    /// current options allowing for new ones to be assigned. 
    /// </summary>
    public void m_StartUp()
    {
        // If the dropdown variable has not been assigned, find the dropdown component on the same object as this 
        // attached script. 

        if (m_LevelDropDown == null)
        {
            m_LevelDropDown = gameObject.GetComponent<Dropdown>();
        }

        // Attach a listener for when the value of the dropdown has been changed.

        m_LevelDropDown.onValueChanged.AddListener(delegate { m_ValueChanged(); });

        // Remove all options on the current dropdown list. 

        m_LevelDropDown.ClearOptions();
    }

    /// <summary>
    /// This allows the addition of new options into the exsisting dropdown list, all new options will be added to the bottom 
    /// of the list. 
    /// </summary>
    /// <param name="nameToAdd">The name of option being added. </param>
    /// <param name="index">The position in the list for the new option, if the index is larger than the current number of options
    /// it will be added to the end of the list. </param>
    public void m_AddItemIntoDropdown(string nameToAdd, int index)
    {
        if (index < m_LevelDropDown.options.Count)
        {
            // Create a new dropdown data type for the option. 

            Dropdown.OptionData l_NewItem = new Dropdown.OptionData(nameToAdd);

            // Remove the existing option.

            m_LevelDropDown.options.RemoveAt(index);

            // Replace with the new option.

            m_LevelDropDown.options.Insert(index, l_NewItem);

            // DEBUGGING \\
            /*
            Debug.Log(nameToAdd + " has been added to the dropdown list at position " + index + ".");
            */
        }
        else
        { 
            // Create new dropdown data type.

            Dropdown.OptionData l_NewItem = new Dropdown.OptionData(nameToAdd);

            // Add new option to the end of the list.

            m_LevelDropDown.options.Add(l_NewItem);

            // DEBUGGING \\
            /*
            Debug.Log(nameToAdd + " has been onto the end of the dropdown list.");
            */
        }
    }

    /// <summary>
    /// This will be called whenever a new option is selected from the dropdown menu. It will be used to change variables used to load 
    /// the selected level. 
    /// </summary>
    private void m_ValueChanged()
    {
        m_iLevelToLoad = m_LevelDropDown.value;

        m_bLoadnewLevel = true; 
    }

    /// <summary>
    /// This will be used to check if a level has been selected. 
    /// </summary>
    /// <returns>This will remain false until a new value has been selected in the dropdown list. </returns>
    public bool m_GetLoadNewLevel() => m_bLoadnewLevel;

    /// <summary>
    /// This returns the index number of the level which has been selected. 
    /// </summary>
    /// <returns>The number in the drop down list which has been selected. </returns>
    public int m_GetLevelToLoad() => m_iLevelToLoad;

    /// <summary>
    /// This will reset the boolean after a level has been loaded. This will prevent the player from imidately playing the same 
    /// level again.
    /// </summary>
    public void m_LevelHasBeenLoaded()
    {
        m_bLoadnewLevel = false;
    }

    //---------------------------------------------------------------------------------------------------------------------------\\
    // File End. 
    //---------------------------------------------------------------------------------------------------------------------------\\

}
