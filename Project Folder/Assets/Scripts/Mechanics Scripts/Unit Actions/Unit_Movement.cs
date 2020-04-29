using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_Movement : MonoBehaviour
{
    [SerializeField]
    GameObject m_CurrentCell = null;

    [SerializeField]
    Vector3 m_PlcementOffset = new Vector3(0, 0, -1);

    [SerializeField]
    int m_iMovementPoints = 4;

    [SerializeField]
    int m_iUsedPoints = 0;

    private void Update()
    {
        if (m_CurrentCell != null)
        {
            if (transform.position != m_CurrentCell.transform.position + m_PlcementOffset)
            {
                transform.position = m_CurrentCell.transform.position + m_PlcementOffset;
            }
        }
    }

    public void m_SetPosition(GameObject newCell)
    {
        m_CurrentCell = newCell; 
    }

    public void m_SetPosition(GameObject newCell, int moveCost)
    {
        if ((moveCost <= m_iMovementPoints) && (m_iUsedPoints + moveCost <= m_iMovementPoints))
        {
            m_CurrentCell = newCell;

            m_iUsedPoints += moveCost; 
        }
    }

    public bool m_UpdateUnitPosition(GameObject newCell, int moveCost)
    {
        if ((moveCost <= m_iMovementPoints) && (m_iUsedPoints + moveCost <= m_iMovementPoints))
        {
            m_CurrentCell = newCell;

            m_iUsedPoints += moveCost;

            Debug.Log("New Position Acepted");

            return true;
        }

        Debug.Log("New Position Rejected");

        return false; 
    }

    public void m_UnitWait()
    {
        m_iUsedPoints = m_iMovementPoints;
    }

    public int m_GetCurrentMoveRange() => m_iMovementPoints - m_iUsedPoints; 

    public GameObject m_GetCurrentPosition() => m_CurrentCell;

    public void m_ResetUsedPoints()
    {
        m_iUsedPoints = 0; 
    }
}
