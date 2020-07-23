using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//---------------------------------------------------------------------------------------------------------------------------\\
// File Start
//---------------------------------------------------------------------------------------------------------------------------\\

/// <summary>
/// This class will allow for a cell to keep track of the other cells around it, this is a critical component to creating 
/// a pathfinding algorithm. 
/// </summary>
public class Cell_Neighbours : MonoBehaviour
{
    //---------------------------------------------------------------------------------------------------------------------------\\
    // Data Members Start
    //---------------------------------------------------------------------------------------------------------------------------\\

    /// <summary>
    /// A list of all of the cells around this one. It shoul hold at most the cells above, bellow and to both sides of this 
    /// cell. diagonals are not included at the moment, but could be added if needed. 
    /// </summary>
    [SerializeField]
    List<GameObject> m_CellNeighbours = new List<GameObject>(); /*! < /var This will hold all of the cells adjacent to this one. */

    //---------------------------------------------------------------------------------------------------------------------------\\
    // Member Functions Start
    //---------------------------------------------------------------------------------------------------------------------------\\

    /// <summary>
    /// This will be used to add a new neighbour into the list. 
    /// </summary>
    /// <param name="newNeighbour">A cell which lies beside this one. </param>
    public void m_SetCellNeighbour(GameObject newNeighbour)
    {
        m_CellNeighbours.Add(newNeighbour); 
    }

    /// <summary>
    /// This will return the full list of neighbours around this cell. 
    /// </summary>
    /// <returns>The list of neighbours. </returns>
    public List<GameObject> m_GetCellNeighbours()
    {
        return m_CellNeighbours; 
    }

    /// <summary>
    /// This will loop through all of the neighbours around this cell and given another point in the map, this 
    /// will find which one of those neighbours are closest to the given point.
    /// </summary>
    /// <param name="otherCell">The cell you are trying to find the neighbour closest to. </param>
    /// <returns></returns>
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

    /// <summary>
    /// This will be used to check if there are any free spaces arund this cell, this will be used to ensure
    /// the unit shairing this cell can be targeted by an AI opponent.
    /// </summary>
    /// <returns></returns>
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

    //---------------------------------------------------------------------------------------------------------------------------\\
    // File End
    //---------------------------------------------------------------------------------------------------------------------------\\
}
