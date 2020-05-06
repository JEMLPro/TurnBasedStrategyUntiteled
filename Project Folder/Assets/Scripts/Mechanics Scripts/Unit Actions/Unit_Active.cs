using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_Active : MonoBehaviour
{
    [SerializeField]
    bool m_bUnitActive;

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            m_bUnitActive = false;
        }
    }

    private void OnMouseOver()
    {
        if (GetComponentInParent<Unit_Manager>() != null)
        {
            // Player Turn Manager 
            if (GetComponentInParent<Unit_Manager>().m_CheckTurn() == true)
            {
                if (Input.GetMouseButton(0))
                {
                    m_bUnitActive = true;
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
}
