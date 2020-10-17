using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    #region Data Members

    #region Pathfinding Sets

    /// <summary>
    /// This will be used to add cell neighbours into, this is the pool of cells to check. during each 
    /// loop the cells inside must be updated to see if the current cell is cheaper to move to that cell 
    /// using this cell. If so update that cell's preveous cell. 
    /// </summary>
    [SerializeField]
    List<GameObject> m_OpenSet;

    /// <summary>
    /// This is the list of cells which have already been checked, this will allow for cells already checked
    /// to not be checked multiple times. It also needs to be cross-referenced with the cells in the open set 
    /// to prevent them from being added into the open set. 
    /// </summary>
    [SerializeField]
    List<GameObject> m_ClosedSet;

    #endregion

    #region Items Being Checked

    /// <summary>
    /// This is the cell which is currently being checked by the pathfinding algorithm. 
    /// </summary>
    [SerializeField]
    GameObject m_CurrentCell;

    /// <summary>
    /// This is the unit the pathfinding algorithm is being used for. 
    /// </summary>
    [SerializeField]
    GameObject m_UnitOfFocus = null;

    #endregion

    #region Pathfinding Start and End cells 

    /// <summary>
    /// This is the starting point for the pathfinding algorithm; at the start this cell will also be the current 
    /// cell. This will also be used during the path tracing, to ensure the final path reaches this cell. 
    /// </summary>
    [SerializeField]
    GameObject m_StartCell;

    /// <summary>
    /// This is the target cell the pathfinding algorithm needs to find, once the current cell becomes this cell 
    /// the algorithm has what it needs to trace the final path. 
    /// </summary>
    [SerializeField]
    GameObject m_EndCell;

    #endregion

    #endregion

    #region Member Functions 

    #region Updating Pathfinding

    /// <summary>
    /// This will be used to update the pathfinding algorithm for a single unit within the game.
    /// </summary>
    /// <returns>True if the pathfinding algorithm has reached the end cell otherwise returns false. </returns>
    public bool m_UpdatePathfinding()
    {
        if (m_UnitOfFocus != null)
        {
            // Check to see if the pathfinding requirements are met. 

            if ((m_StartCell != null) && (m_EndCell != null))
            {
                // Check to see if the pathfinding has reached the end

                if (m_CurrentCell == m_EndCell)
                {
                    Debug.Log("Pathfinding has found the end cell. Path tracing should now begin. ");

                    return true;
                }
                else
                {
                    // If the pathfinding has not been completed the next cell should be checked. This should be called 
                    // once per frame until the path has been found and only one set of pathfinding should occur at a 
                    // single time as the prevous cell's would overlap and cause issue. 

                    Debug.Log("End cell not found. Updating pathfinding. ");

                    if (m_OpenSet.Count > 0)
                    {
                        #region Check the neighbours of the current cell.

                        // Loop through all of the current cell's neighbours and calculate their G, H and F scores 

                        foreach (var neighbour in m_CurrentCell.GetComponent<Cell_Neighbours>().m_GetCellNeighbours())
                        {
                            // Check to see if the neighbour is already in one of the sets before deciding what to do. 

                            if (m_ClosedSet.Contains(neighbour))
                            {
                                Debug.Log("Neighbour already in closed set - skipping cell.");
                            }
                            else if (m_OpenSet.Contains(neighbour))
                            {
                                Debug.Log("Neighbour already in open set - updating values.");

                                // If the cell already exists within the open set rather than setting the values outright, 
                                // they should be updated. 

                                neighbour.GetComponent<Cell_To_Pathfinding>().m_CalculateGScore(m_CurrentCell);
                                neighbour.GetComponent<Cell_To_Pathfinding>().m_GetFScore();
                            }
                            else
                            {
                                Debug.Log("Neighbour is a new cell - values being set.");

                                // With a new cell all of the values need to be set including the distance to the end cell, 
                                // which doesn't change with each iteration.

                                neighbour.GetComponent<Cell_To_Pathfinding>().m_CalculateGScore(m_CurrentCell);
                                neighbour.GetComponent<Cell_To_Pathfinding>().m_CalculateHScore(m_EndCell);
                                neighbour.GetComponent<Cell_To_Pathfinding>().m_GetFScore();

                                // After the values have been set the neighbour needs to be added into the open set. 

                                m_AddCellIntoOpenSet(neighbour);
                            }
                        }

                        #endregion

                        #region Find next current cell.

                        // Create a placeholder object to hold the cell for comparison. 

                        GameObject l_NextCurrentCell = null;

                        // Loop through all cells in the open set for the cell with the lowest F-Score. 

                        foreach (var cell in m_OpenSet)
                        {
                            if (cell != null)
                            {
                                // If there is no cell set ensure the first free cell is set to be the next one. 

                                if (l_NextCurrentCell == null)
                                {
                                    l_NextCurrentCell = cell;
                                }
                                else
                                {
                                    // If the new cell has a lower F-Score the new cell becomes the next cell. Otherwise the currently set 
                                    // cell has the lower F-Score. 

                                    if (cell.GetComponent<Cell_To_Pathfinding>().m_GetFScore() < l_NextCurrentCell.GetComponent<Cell_To_Pathfinding>().m_GetFScore())
                                    {
                                        l_NextCurrentCell = cell;
                                    }
                                }
                            }
                        }

                        // After looping through the open set the next current cell should become the next current 
                        // cell. 

                        m_AddCellIntoClosedSet(m_CurrentCell);

                        Debug.Log("New current cell being set. ");

                        m_CurrentCell = l_NextCurrentCell;

                        #endregion
                    }

                }
            }

            Debug.LogWarning("Unable to find path, without start or end cell. ");
        }

        return false; 
    }

    #endregion

    #region Open and closed set management. 

    /// <summary>
    /// This will be used update the list of cells within the open set, but will ensure that cells 
    /// are not duplicated. 
    /// </summary>
    /// <param name="cellToAdd">This is the cell which should be added into this list. </param>
    private void m_AddCellIntoOpenSet(GameObject cellToAdd)
    {
        // Ensure that the cell that is being added to the open set is not already in play.

        if(!m_ClosedSet.Contains(cellToAdd) || !m_OpenSet.Contains(cellToAdd))
        {
            Debug.Log("Cell to add is not in the closed set or itself, adding into open set. ");

            m_OpenSet.Add(cellToAdd);
        }
        else
        {
            Debug.Log("Cell already exists in one of the sets thus should not be duplicated. "); 
        }
    }

    /// <summary>
    /// This will be used update the list of cells within the closed set, but will ensure that cells 
    /// are not duplicated. 
    /// </summary>
    /// <param name="cellToAdd">This is the cell which should be added into this list. </param>
    private void m_AddCellIntoClosedSet(GameObject cellToAdd)
    {
        // This needs to check to see if the cell is not already within the closed set, otherwise 
        // issues could arise during the path tracing if cells are checked multiple times. 

        if(!m_ClosedSet.Contains(cellToAdd))
        {
            Debug.Log("Adding cell into closed set. "); 

            // Also if the cell is inside the open set it needs to be removed to ensure that cell isn't 
            // checked multiple times. 

            if(m_OpenSet.Contains(cellToAdd))
            {
                Debug.Log("Removing cell from open set. "); 

                m_OpenSet.Remove(cellToAdd); 
            }

            m_ClosedSet.Add(cellToAdd); 

        }
        else
        {
            Debug.Log("Cell already exists in the closed set and thus should not be duplicated. ");
        }
    }

    #endregion

    #region Variable Assignment

    public void m_SetStartCell(GameObject startCell) { m_StartCell = startCell; }

    public void m_SetEndCell(GameObject endCell) { m_EndCell = endCell; }

    public void m_SetUnitOfFocus(GameObject unitOfFocus) { m_UnitOfFocus = unitOfFocus; }

    #endregion

    #endregion

}
