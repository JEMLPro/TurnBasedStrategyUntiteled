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
        if (Input.GetMouseButton(0))
        {
            m_bUnitActive = true;
        }
    }

    public bool m_GetActiveUnit() => m_bUnitActive;

    public void m_SetUnitActive(bool newValue)
    {
        m_bUnitActive = newValue; 
    }
}
