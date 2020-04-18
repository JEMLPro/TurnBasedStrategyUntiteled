using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_Movement : MonoBehaviour
{
    [SerializeField]
    GameObject m_CurrentCell = null;

    [SerializeField]
    Vector3 m_PlcementOffset = new Vector3(0, 0, -1);

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
}
