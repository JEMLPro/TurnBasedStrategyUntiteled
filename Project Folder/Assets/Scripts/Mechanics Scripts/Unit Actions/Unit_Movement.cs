using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_Movement : MonoBehaviour
{
    [SerializeField]
    GameObject m_CurrentCell = null;

    GameObject m_PreveousCell = null; 

    [SerializeField]
    Vector3 m_PlcementOffset = new Vector3(0, 0, -2);

    [SerializeField]
    int m_iMovementPoints = 4;

    [SerializeField]
    int m_iUsedPoints = 0;

    [SerializeField]
    float m_fMoveSpeed = 2.5f;

    float m_fJourneyLength;

    float m_fStartTime; 

    private void Update()
    {
        if (m_CurrentCell != null)
        {
            // Update position 

            if (m_PreveousCell != null)
            {
                if (transform.position != m_CurrentCell.transform.position + m_PlcementOffset)
                {
                    float l_fDistCovered = (Time.time - m_fStartTime) * m_fMoveSpeed;

                    float l_fFractionOfJourney = l_fDistCovered / m_fJourneyLength;

                    transform.position = Vector3.Lerp(m_PreveousCell.transform.position + m_PlcementOffset, 
                        m_CurrentCell.transform.position + m_PlcementOffset, l_fFractionOfJourney);
                }
            }
            else if (transform.position != m_CurrentCell.transform.position + m_PlcementOffset)
            {
                transform.position = m_CurrentCell.transform.position + m_PlcementOffset;
            }
        }
    }

    public void m_SetPosition(GameObject newCell)
    {
        m_PreveousCell = m_CurrentCell; 

        m_CurrentCell = newCell; 
    }

    public void m_SetPosition(GameObject newCell, int moveCost)
    {
        if ((moveCost <= m_iMovementPoints) && (m_iUsedPoints + moveCost <= m_iMovementPoints))
        {
            m_PreveousCell = m_CurrentCell;

            m_CurrentCell = newCell;

            m_iUsedPoints += moveCost; 
        }
    }

    public bool m_UpdateUnitPosition(GameObject newCell, int moveCost)
    {
        if ((moveCost <= m_iMovementPoints) && (m_iUsedPoints + moveCost <= m_iMovementPoints))
        {
            m_PreveousCell = m_CurrentCell;

            m_CurrentCell = newCell;

            m_fJourneyLength = Vector3.Distance(m_CurrentCell.transform.position + m_PlcementOffset, m_PreveousCell.transform.position + m_PlcementOffset);

            m_fStartTime = Time.time; 

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
