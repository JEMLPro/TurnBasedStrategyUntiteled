using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_Movement : MonoBehaviour
{
    [SerializeField]
    GameObject m_CurrentCell = null;

    [SerializeField]
    GameObject m_GameManager;

    [SerializeField]
    Vector3 m_MousePos;

    [SerializeField]
    Material m_MoveRangeMat; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (m_CurrentCell != null)
        {
            if (gameObject.transform.position != m_CurrentCell.GetComponent<Cell_Info>().m_GetCellPosition())
            {
                gameObject.transform.position = m_CurrentCell.GetComponent<Cell_Info>().m_GetCellPosition();
            }

            m_CellRange(); 
        }


    }


    void m_CellRange()
    {
        GameObject l_CurrentCellInRange = m_CurrentCell;

        m_CurrentCell.GetComponent<Cell_Info>().m_SetWithinRange(true);

        // Cell Range Up 

        for (int i = 0; i < gameObject.GetComponent<UnitStat>().m_GetMoveRadius(); i++)
        {
            l_CurrentCellInRange = l_CurrentCellInRange.GetComponent<Cell_Info>().m_GetCellNeighbour("Up");

            if (l_CurrentCellInRange != null)
            {
                l_CurrentCellInRange.GetComponent<Cell_Info>().m_SetWithinRange(true);

                l_CurrentCellInRange.GetComponent<Renderer>().material = m_MoveRangeMat; 
            }
        }

        // Cell Range Down 

        l_CurrentCellInRange = m_CurrentCell;

        for (int i = 0; i < gameObject.GetComponent<UnitStat>().m_GetMoveRadius(); i++)
        {
            l_CurrentCellInRange = l_CurrentCellInRange.GetComponent<Cell_Info>().m_GetCellNeighbour("Down");

            if (l_CurrentCellInRange != null)
            {
                l_CurrentCellInRange.GetComponent<Cell_Info>().m_SetWithinRange(true);

                l_CurrentCellInRange.GetComponent<Renderer>().material = m_MoveRangeMat;
            }
        }

        // Cell Range Left

        l_CurrentCellInRange = m_CurrentCell;

        for (int i = 0; i < gameObject.GetComponent<UnitStat>().m_GetMoveRadius(); i++)
        {
            l_CurrentCellInRange = l_CurrentCellInRange.GetComponent<Cell_Info>().m_GetCellNeighbour("Left");

            if (l_CurrentCellInRange != null)
            {
                l_CurrentCellInRange.GetComponent<Cell_Info>().m_SetWithinRange(true);

                l_CurrentCellInRange.GetComponent<Renderer>().material = m_MoveRangeMat;
            }
        }

        // Cell Range Right 

        l_CurrentCellInRange = m_CurrentCell;

        for (int i = 0; i < gameObject.GetComponent<UnitStat>().m_GetMoveRadius(); i++)
        {
            l_CurrentCellInRange = l_CurrentCellInRange.GetComponent<Cell_Info>().m_GetCellNeighbour("Right");

            if (l_CurrentCellInRange != null)
            {
                l_CurrentCellInRange.GetComponent<Cell_Info>().m_SetWithinRange(true);

                l_CurrentCellInRange.GetComponent<Renderer>().material = m_MoveRangeMat;
            }
        }

        // Cell Range Diagonal

        // First Step. 

        l_CurrentCellInRange = m_CurrentCell.GetComponent<Cell_Info>().m_GetCellNeighbour("Left");

        string l_Direction = "Up"; 

        for (int j = 0; j <= 1; j++)
        {
            for (int i = 0; i < gameObject.GetComponent<UnitStat>().m_GetMoveRadius() - 1; i++)
            {
                l_CurrentCellInRange = l_CurrentCellInRange.GetComponent<Cell_Info>().m_GetCellNeighbour(l_Direction);

                if (l_CurrentCellInRange != null)
                {
                    l_CurrentCellInRange.GetComponent<Cell_Info>().m_SetWithinRange(true);

                    l_CurrentCellInRange.GetComponent<Renderer>().material = m_MoveRangeMat;
                }
            }

            l_CurrentCellInRange = m_CurrentCell.GetComponent<Cell_Info>().m_GetCellNeighbour("Left");

            l_Direction = "Down"; 
        }

    }

}
