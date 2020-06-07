using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Select_Building : MonoBehaviour
{
    [SerializeField]
    bool m_bSelected = false;

    bool m_bWithinRange = false; 

    private void OnMouseDown()
    {
        if (m_bWithinRange == true)
        {
            m_bSelected = true;
        }
    }

    private void OnMouseExit()
    {
        m_bSelected = false;
    }

    public bool m_GetSelected() => m_bSelected; 

    public void m_SetWithinRange(bool newValue) { m_bWithinRange = newValue; }

    public bool m_GetWithinRange() => m_bWithinRange; 
}
