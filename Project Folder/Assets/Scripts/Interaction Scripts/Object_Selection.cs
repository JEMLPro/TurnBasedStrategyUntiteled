using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_Selection : MonoBehaviour
{
    [SerializeField]
    bool m_bObjectSelected = false; /*!< /var This will be used to check if the player has selected this object. */


    private void OnMouseOver()
    {
        // If the mouse is over this object and the mouse button is pressed this object becomes selected. 

        if(Input.GetMouseButtonDown(0))
        {
            m_bObjectSelected = true;
        }

    }

    private void OnMouseExit()
    {
        // If the player clicks outside the object the variable will become false. 

        if ((m_bObjectSelected == true))
        {
            m_bObjectSelected = false;
        }
    }

    // This will return the current value of our selected variable. 
    public bool m_bGetObjectSelected() => m_bObjectSelected;

    public void m_SetObjectSelected(bool selectedValue)
    {
        // This will be used to assign a new value to the selected variable. 

        m_bObjectSelected = selectedValue;
    }

}
