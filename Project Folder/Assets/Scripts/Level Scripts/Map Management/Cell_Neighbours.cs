using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell_Neighbours : MonoBehaviour
{
    [SerializeField]
    List<GameObject> m_CellNeighbours = new List<GameObject>(); /*! < /var This will hold all of the cells adjacent to this one. */

    // This will be used to add a new game obejct to it's list of neighbours. 
    public void m_SetCellNeighbour(GameObject newNeighbour)
    {
        m_CellNeighbours.Add(newNeighbour); 
    }

    // This will be used to get the current adjacent cells. 
    public List<GameObject> m_GetCellNeighbours()
    {
        return m_CellNeighbours; 
    }

    public GameObject m_GetClosestNeighbour(GameObject otherCell)
    {
        GameObject l_ReturnNeighbour = null;

        int l_iPrevDist = int.MaxValue; 

        foreach (var cell in m_CellNeighbours)
        {
            if (cell.GetComponent<Cell_Manager>().m_CanBeMovedTo() == true)
            {
                int l_iNewDist = cell.GetComponent<Cell_Manager>().m_Distance(otherCell);

                if (l_iNewDist < l_iPrevDist)
                {
                    l_ReturnNeighbour = cell;

                    l_iPrevDist = l_iNewDist;
                }
            }
        }

        return l_ReturnNeighbour;
    }

    public bool m_NeighboursFree()
    {
        bool l_bCheckMove = false;

        foreach (var neighbour in m_CellNeighbours)
        {
            if(neighbour.GetComponent<Cell_Manager>().m_bCheckForOccupied() == false)
            {
                l_bCheckMove = true;

                break;
            }
        }

        return l_bCheckMove; 
    }

}
