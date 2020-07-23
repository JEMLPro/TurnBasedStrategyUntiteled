using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//---------------------------------------------------------------------------------------------------------------------------\\
// File Start
//---------------------------------------------------------------------------------------------------------------------------\\

/// <summary>
/// This class will allow for the connected object to be selected by the player using the mouse. 
/// </summary>
public class Object_Selection : MonoBehaviour
{
    //---------------------------------------------------------------------------------------------------------------------------\\
    // Data Members Start
    //---------------------------------------------------------------------------------------------------------------------------\\

    /// <summary>
    /// This is used to externally 
    /// </summary>
    [SerializeField]
    bool m_bObjectSelected = false;

    //---------------------------------------------------------------------------------------------------------------------------\\
    // Member Functions Start
    //---------------------------------------------------------------------------------------------------------------------------\\

    /// <summary>
    /// This will be activated once the mouse hovers over the connected objects collider (2D or 3D). 
    /// </summary>
    private void OnMouseOver()
    {
        // If the mouse is over this object and the mouse button is pressed this object becomes selected. 

        if (GetComponentInParent<Tile_Map_Manager>().m_GetAllowSelectable() == true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                // THis will prevent the object from being selected while under the user interface. 

                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    m_bObjectSelected = true;
                }
            }
        }
    }

    /// <summary>
    /// Once the mouse exits the connected objects collider this function will be activated. 
    /// </summary>
    private void OnMouseExit()
    {
        // If the player clicks outside the object the variable will become false. 

        if ((m_bObjectSelected == true))
        {
            m_bObjectSelected = false;
        }
    }

    /// <summary>
    /// This will allow for external scripts to check if this object has been selected.
    /// </summary>
    /// <returns> bool Object Selected. </returns>
    public bool m_bGetObjectSelected() => m_bObjectSelected;

    /// <summary>
    /// This will allow for external scripts to manipulate this objects selected value, mainly used to reset the variable to false. 
    /// </summary>
    /// <param name="selectedValue"> The new selected value. </param>
    public void m_SetObjectSelected(bool selectedValue)
    {
        // This will be used to assign a new value to the selected variable. 

        m_bObjectSelected = selectedValue;
    }

    //---------------------------------------------------------------------------------------------------------------------------\\
    // File End 
    //---------------------------------------------------------------------------------------------------------------------------\\
}
