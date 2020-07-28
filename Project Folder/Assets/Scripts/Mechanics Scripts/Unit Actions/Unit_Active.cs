using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_Active : MonoBehaviour
{
    [SerializeField]
    bool m_bUnitActive;

    [SerializeField]
    bool m_bUnitWaiting; 

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            m_bUnitActive = false;
        }

        // If the unit has been set to wait, this will deselect the this unit.
        if(m_bUnitWaiting == true)
        {
            if(m_bUnitActive == true)
            {
                m_bUnitActive = false; 
            }
        }
    }

    private void OnMouseOver()
    {
        if (GetComponentInParent<Unit_Manager>() != null)
        {
            // Player Turn Manager 
            if (GetComponentInParent<Unit_Manager>().m_CheckTurn() == true)
            {
                if (m_bUnitWaiting == false) //< If the unit is waiting then they should not be selectable.
                {
                    if (Input.GetMouseButton(0))
                    {
                        m_bUnitActive = true;
                    }
                }
            }
        }
        else
        {
            // Ai Turn Manager
            if (GetComponentInParent<AI_Unit_Manager>().m_CheckTurn() == true)
            {
                if (Input.GetMouseButton(0))
                {
                    m_bUnitActive = true;
                }
            }
        }
    }

    public bool m_GetActiveUnit() => m_bUnitActive;

    public void m_SetUnitActive(bool newValue)
    {
        m_bUnitActive = newValue; 
    }

    public void m_SetWaiting(bool newValue)
    {
        m_bUnitWaiting = newValue; 
    }
}
