using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Select_Building : MonoBehaviour
{
    [SerializeField]
    bool m_bSelected = false;

    bool m_bWithinRange = false;

    private void Update()
    {
        if(m_bWithinRange == false)
        {
            if(gameObject.GetComponentInParent<Bulding_Manager>().m_CheckTurn() == false )
            {
                m_bSelected = false; 
            }
        }

        if(Input.GetMouseButtonDown(1))
        {
            m_bSelected = false; 
        }
    }

    private void OnMouseDown()
    {
        if (m_bWithinRange == true)
        {
            m_bSelected = true;
        }

        if(gameObject.GetComponentInParent<Bulding_Manager>().m_CheckTurn())
        {
            m_bSelected = true; 
        }
    }

    private void OnMouseExit()
    {
        if(m_bWithinRange == true)
        {
            m_bSelected = false;
        }
    }

    public bool m_GetSelected() => m_bSelected; 

    public void m_SetWithinRange(bool newValue) { m_bWithinRange = newValue; }

    public bool m_GetWithinRange() => m_bWithinRange; 
}
