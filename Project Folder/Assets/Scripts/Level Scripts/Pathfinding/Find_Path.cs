using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*! \class This will be used to generate a path between two cells on a grid based map. */
[System.Serializable]
public class Find_Path : MonoBehaviour
{
    [SerializeField]
    GameObject m_StartCell = null; /*!< \var This is the starting point for the algorithm. */

    [SerializeField]
    GameObject m_EndCell = null; /*! \var This is the target location for the pathfinding. */

    [SerializeField]
    List<GameObject> m_OpenSet = new List<GameObject>(); /*!< \var This is the list of objects which still need to be checked. */ 

    [SerializeField]
    List<GameObject> m_ClosedSet = new List<GameObject>(); /*!< \var This is the list of objects which have already been checked. */

    [SerializeField]
    GameObject m_CurrentCell = null; 

    [SerializeField]
    bool m_bReachdEnd = false;

    [SerializeField]
    List<GameObject> m_FinalPath = new List<GameObject>(); 

    public void m_FindPath()
    {
        // At the start of the path finding algorithm check if you have reached the end. 
        if(m_CurrentCell == m_EndCell)
        {
            m_AddIntoClosedSet(m_CurrentCell);

            m_bReachdEnd = true;

            m_FinalPath = m_ClosedSet; 

            Debug.Log("End Reached");

            m_StartCell = null;
            m_EndCell = null;

        }
        else
        {
            Debug.Log("Searching for Path"); 

            // Check all the requirements are met. 
            if(m_CurrentCell != null && m_StartCell != null && m_EndCell != null)
            {
                // Add al of the current cell's neighbours into the open set. 
                m_AddIntoOpenSet(m_CurrentCell.GetComponent<Cell_Neighbours>().m_GetCellNeighbours());

                // Calculate the G, H and F scores for all cells in the 
                foreach (var cell in m_OpenSet)
                {
                    cell.GetComponent<Pathfinding_Info>().m_CalculateGScore(m_StartCell);
                    cell.GetComponent<Pathfinding_Info>().m_CalculateHScore(m_EndCell);
                    cell.GetComponent<Pathfinding_Info>().m_CalculateFScore(); 
                }

                m_AddIntoClosedSet(m_CurrentCell);

                m_CurrentCell = m_GetCellWithLowestFScore(); 

            }
            else
            {
                Debug.Log("Required Conditions are not met Check for start and end cell assignment. "); 
            }
        }
    }

    /*! \fn Used to add cells into the open set but excludes cells lready inside the open set. */
    public void m_AddIntoOpenSet(List<GameObject> cellsToAdd)
    {
        foreach (var neighbour in cellsToAdd)
        {

            bool l_bAddNeighbour = true;

            foreach (var cell in m_ClosedSet)
            {
                if (neighbour == cell)
                {
                    l_bAddNeighbour = false;
                }
            }

            if (neighbour.GetComponent<Cell_Manager>().m_bcheckForObsticle() == true)
            {
                l_bAddNeighbour = false;
            }

            if (l_bAddNeighbour == true)
            {
                m_OpenSet.Add(neighbour);
                
            }
        }

    }

    /*! \fn This will add a cell into the closed set, it will also remove it from the open set at the same time. */
    void m_AddIntoClosedSet(GameObject cellToAdd)
    {
        m_OpenSet.Remove(cellToAdd);

        m_ClosedSet.Add(cellToAdd); 
    }

    /*! \fn This will loop through the open set and find the cell which currently has the lowest F (Final) Score. */
    GameObject m_GetCellWithLowestFScore()
    {
        GameObject l_NextCell = m_OpenSet[0];

        foreach (var cell in m_OpenSet)
        {
            if(cell.GetComponent<Pathfinding_Info>().m_GetFScore() < l_NextCell.GetComponent<Pathfinding_Info>().m_GetFScore())
            {
                if (cell.GetComponent<Cell_Manager>().m_bcheckForObsticle() == false) 
                {
                    l_NextCell = cell;
                }
            }
        }

        return l_NextCell;
    }

    /*! \fn Used to set the starting point for the pathfinding, also assigns the current cell to be the same as the start cell. */ 
    public void m_SetStartCell(GameObject startCell)
    {
        m_StartCell = startCell;

        m_CurrentCell = startCell;

        if (m_OpenSet.Count > 0)
        {
            m_OpenSet.Clear();
        }

        if (m_ClosedSet.Count > 0)
        {
            m_ClosedSet.Clear();
        }
    }

    /*! \fn Used to assign the target cell for the pathfinding. */
    public void m_SetEndCell(GameObject endCell)
    {
        m_EndCell = endCell;

        m_bReachdEnd = false; 
    }

    public void m_ResetPathfinding()
    {
        m_SetStartCell(null);
        m_SetEndCell(null);
    }

    public bool m_CheckRequirements()
    {
        if(m_StartCell != null && m_EndCell != null)
        {
            return true;
        }

        return false; 
    }

    /*! \fn Used to get access to the final path outputted by the function. */
    public List<GameObject> m_GetFinalPath() => m_FinalPath;

    public bool m_CheckStateOfPath() => m_bReachdEnd;
}
