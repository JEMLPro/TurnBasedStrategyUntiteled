using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Unit_Movement : Unit_Movement
{

    public bool m_bPathReached = false; 

    public Pathfinding m_GetPathfinding() => gameObject.GetComponent<Pathfinding>();

    public void m_SetStartandEndPoints(GameObject endPoint)
    {
        Debug.Log("Setting Requirements"); 

        m_GetPathfinding().m_SetStartCell(gameObject.GetComponent<AI_Unit_Movement>().m_GetCurrentPosition());
        m_GetPathfinding().m_SetEndCell(endPoint);
        m_GetPathfinding().m_SetUnitOfFocus(gameObject.GetComponent<AI_Unit_Manager>().m_GetSelectedUnit()); 
    }

    public void FixedUpdate()
    {
        
        if(m_bPathReached != true)
        {
            m_bPathReached = m_GetPathfinding().m_UpdatePathfinding(); 
        }

        base.Update();

    } 

}
