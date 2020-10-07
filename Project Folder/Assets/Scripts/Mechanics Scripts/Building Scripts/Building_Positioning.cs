using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building_Positioning : MonoBehaviour
{

    [SerializeField]
    GameObject m_CurrentCell = null;

    [SerializeField]
    Vector3 m_PlcementOffset = new Vector3(0, 0, -0.001f);

    private void Update()
    {
        // This will be used to ensure that the building is in the correct position. 

        if (m_CurrentCell != null)
        {
            if (transform.position != m_CurrentCell.transform.position + m_PlcementOffset)
            {
                transform.position = m_CurrentCell.transform.position + m_PlcementOffset;
            }
        }
    }

    public void m_SetPosition(GameObject newCellPos)
    {
        m_CurrentCell = newCellPos;

        m_CurrentCell.GetComponent<Cell_Manager>().m_bSetObsticle(true);
    }

    public GameObject m_GetPosition() => m_CurrentCell; 

}
