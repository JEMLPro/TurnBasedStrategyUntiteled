using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    // Materials. Used for debugging.

    [SerializeField]
    Material m_ClosedSetMat; // Used for closed set. 

    [SerializeField]
    Material m_OpenSetMat; // Used for open set. 

    [SerializeField]
    Material m_PathMat; // Used for final path. 

    // Main Items. 

    [SerializeField]
    GameObject m_GameMap; // The map which cantains all cells.

    [SerializeField]
    List<GameObject> m_OpenSet; // The list of items to check. 

    [SerializeField]
    List<GameObject> m_ClosedSet; // A list of already checked items. 


    [SerializeField]
    GameObject m_StartCell; // The starting point for the path. 

    [SerializeField]
    GameObject m_CurrentCell; // The current cell being reviewed. 

    [SerializeField] 
    GameObject m_EndCell; // The goal for the path, the end point for the algorithm. 

    float m_MovementCost = 10; // The cost it takes to move from one cell to another. Can be refined with different cell costs. 

    [SerializeField]
    List<GameObject> m_Path; // The final path for the algorithm. 

    [SerializeField]
    bool m_FoundPath = false; // Used to end the findig algorithm. 

    // Start is called before the first frame update
    void Start()
    {
        m_StartCell = m_GameMap.GetComponent<Create_Map>().m_GetRandomCell();
        m_EndCell = m_GameMap.GetComponent<Create_Map>().m_GetRandomCell();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_FoundPath == false)
        {
            m_FindPath();
        }
    }

    // This will continue to look for the end point until it is found and will construct a path along the way. 
    void m_FindPath()
    {
        // If there is no start/end cell generate one. For debugging. 

        if (m_StartCell == null && m_EndCell == null)
        {
            m_StartCell = m_GameMap.GetComponent<Create_Map>().m_GetRandomCell();

            m_EndCell = m_GameMap.GetComponent<Create_Map>().m_GetRandomCell();

            m_Path.Clear(); 
        }

        // If there is no current cell it becomes equal to the start cell. 

        if(m_CurrentCell == null)
        { 
            m_CurrentCell = m_StartCell;
        }

        // Check if we are at the goal.
        if (m_CurrentCell != m_EndCell)
        {
            // Add Current Cell's neighbours to open list; 

            m_AddCellsToOpenSet(m_CurrentCell);

            // Calculate G, H and F Scores; 

            m_CalculateGScore(m_OpenSet, m_EndCell, m_MovementCost);
            m_CalculateHScore(m_OpenSet, m_CurrentCell, m_MovementCost);
            m_CalculateFScore(m_OpenSet);

            // Add Current cell to closed set.

            m_AddCellsToClosedSet(m_CurrentCell);

            // Remove Current Cell from open set. 

            m_OpenSet.Remove(m_CurrentCell);

            // Add To path.

            m_Path.Add(m_CurrentCell);

            // Move to cell with lowest F Score. 

            m_CurrentCell = m_FindCellWithLowestFScore();

            // Update Colours 

            m_SetColours(); 

        }
        else
        {
            m_Path.Add(m_EndCell); 

            Debug.Log("Reached End Cell");

            m_FoundPath = true;

            m_StartCell = null;
            m_EndCell = null;

            m_OpenSet.Clear();
            m_ClosedSet.Clear();
        }
    }

    // Used to change the look of the affected cells. For Debugging. 
    void m_SetColours()
    {
        for(int i = 0; i < m_OpenSet.Count; i++)
        {
            m_OpenSet[i].GetComponent<Renderer>().material = m_OpenSetMat; 
        }

        for (int i = 0; i < m_ClosedSet.Count; i++)
        {
            m_ClosedSet[i].GetComponent<Renderer>().material = m_ClosedSetMat;
        }

        for (int i = 0; i < m_Path.Count; i++)
        {
            m_Path[i].GetComponent<Renderer>().material = m_PathMat;
        }
    }

    // Use to add new cells to the open set. (Prevents duplicates).
    void m_AddCellsToOpenSet(GameObject currentCell)
    {
        // Add Up

        if (currentCell.GetComponent<Cell_Info>().m_GetCellNeighbour("Up") != null)
        {
            bool l_AddCell = true;

            // Check if already inside closed set. 

            if (currentCell.GetComponent<Cell_Info>().m_GetCellNeighbour("Up").GetComponent<Cell_Info>().m_GetObsticle == false)
            {
                for (int i = 0; i < m_ClosedSet.Count; i++)
                {
                    if (m_ClosedSet[i] == currentCell.GetComponent<Cell_Info>().m_GetCellNeighbour("Up"))
                    {
                        l_AddCell = false;
                    }
                }

                // Check if already in open set. 

                for (int i = 0; i < m_OpenSet.Count; i++)
                {
                    if (m_OpenSet[i] == currentCell.GetComponent<Cell_Info>().m_GetCellNeighbour("Up"))
                    {
                        l_AddCell = false;
                    }
                }

                if (l_AddCell == true)
                {
                    m_OpenSet.Add(currentCell.GetComponent<Cell_Info>().m_GetCellNeighbour("Up"));
                }
            }
            else
            {
                m_AddCellsToClosedSet(currentCell.GetComponent<Cell_Info>().m_GetCellNeighbour("Up")); 
            }
        }

        // Add Down

        if (currentCell.GetComponent<Cell_Info>().m_GetCellNeighbour("Down") != null)
        {
            bool l_AddCell = true;

            // Check if already inside closed set. 

            if (currentCell.GetComponent<Cell_Info>().m_GetCellNeighbour("Down").GetComponent<Cell_Info>().m_GetObsticle == false)
            {

                for (int i = 0; i < m_ClosedSet.Count; i++)
                {
                    if (m_ClosedSet[i] == currentCell.GetComponent<Cell_Info>().m_GetCellNeighbour("Down"))
                    {
                        l_AddCell = false;
                    }
                }

                // Check if already in open set. 

                for (int i = 0; i < m_OpenSet.Count; i++)
                {
                    if (m_OpenSet[i] == currentCell.GetComponent<Cell_Info>().m_GetCellNeighbour("Down"))
                    {
                        l_AddCell = false;
                    }
                }

                if (l_AddCell == true)
                {
                    m_OpenSet.Add(currentCell.GetComponent<Cell_Info>().m_GetCellNeighbour("Down"));
                }
            }
            else
            {
                m_AddCellsToClosedSet(currentCell.GetComponent<Cell_Info>().m_GetCellNeighbour("Down"));
            }

        }

        // Add Left

        if (currentCell.GetComponent<Cell_Info>().m_GetCellNeighbour("Left") != null)
        {
            bool l_AddCell = true;

            // Check if already inside closed set. 

            if (currentCell.GetComponent<Cell_Info>().m_GetCellNeighbour("Left").GetComponent<Cell_Info>().m_GetObsticle == false)
            {

                for (int i = 0; i < m_ClosedSet.Count; i++)
                {
                    if (m_ClosedSet[i] == currentCell.GetComponent<Cell_Info>().m_GetCellNeighbour("Left"))
                    {
                        l_AddCell = false;
                    }
                }

                // Check if already in open set. 

                for (int i = 0; i < m_OpenSet.Count; i++)
                {
                    if (m_OpenSet[i] == currentCell.GetComponent<Cell_Info>().m_GetCellNeighbour("Left"))
                    {
                        l_AddCell = false;
                    }
                }

                if (l_AddCell == true)
                {
                    m_OpenSet.Add(currentCell.GetComponent<Cell_Info>().m_GetCellNeighbour("Left"));
                }
            }
            else
            {
                m_AddCellsToClosedSet(currentCell.GetComponent<Cell_Info>().m_GetCellNeighbour("Left"));
            }
        }

        // Add Right

        if (currentCell.GetComponent<Cell_Info>().m_GetCellNeighbour("Right") != null)
        {
            bool l_AddCell = true;

            // Check if already inside closed set. 

            if (currentCell.GetComponent<Cell_Info>().m_GetCellNeighbour("Right").GetComponent<Cell_Info>().m_GetObsticle == false)
            {

                for (int i = 0; i < m_ClosedSet.Count; i++)
                {
                    if (m_ClosedSet[i] == currentCell.GetComponent<Cell_Info>().m_GetCellNeighbour("Right"))
                    {
                        l_AddCell = false;
                    }
                }

                // Check if already in open set. 

                for (int i = 0; i < m_OpenSet.Count; i++)
                {
                    if (m_OpenSet[i] == currentCell.GetComponent<Cell_Info>().m_GetCellNeighbour("Right"))
                    {
                        l_AddCell = false;
                    }
                }

                if (l_AddCell == true)
                {
                    m_OpenSet.Add(currentCell.GetComponent<Cell_Info>().m_GetCellNeighbour("Right"));
                }
            }
            else
            {
                m_AddCellsToClosedSet(currentCell.GetComponent<Cell_Info>().m_GetCellNeighbour("Right"));
            }
        }
    }

    // Use to add new cells to the closed set. (Prevents duplicates).
    void m_AddCellsToClosedSet(GameObject currentCell)
    {
        bool l_AddCell = true;

        for (int i = 0; i < m_ClosedSet.Count; i++)
        {
            if(m_ClosedSet[i] == currentCell)
            {
                l_AddCell = false;
            }
        }

        if(l_AddCell == true)
        {
            m_ClosedSet.Add(currentCell);
        }
    }

    // Finds the cell with the lowest F score. This will become the next current cell. 
    GameObject m_FindCellWithLowestFScore()
    {
        GameObject l_LowestF = m_OpenSet[0]; 

        for (int i = 0; i < m_OpenSet.Count; i++)
        {
            if(m_OpenSet[i].GetComponent<Cell_Info>().m_GetFScore < l_LowestF.GetComponent<Cell_Info>().m_GetFScore)
            {
                l_LowestF = m_OpenSet[i];
            }
        }

        return l_LowestF; 
    }

    // Calculate the G score for all of the cells in the open set. 
    void m_CalculateGScore(List<GameObject> openSet, GameObject endCell, float moveCost)
    {
        for (int i = 0; i < openSet.Count; i++)
        { 
            int l_GridPosXOne = openSet[i].GetComponent<Cell_Info>().m_GetGridPos().x;
            int l_GridPosXTwo = endCell.GetComponent<Cell_Info>().m_GetGridPos().x;

            int l_GridPosYOne = openSet[i].GetComponent<Cell_Info>().m_GetGridPos().y;
            int l_GridPosYTwo = endCell.GetComponent<Cell_Info>().m_GetGridPos().y;

            int l_XMove;

            int l_YMove;

            if (l_GridPosXOne > l_GridPosXTwo)
            {
                l_XMove = l_GridPosXOne - l_GridPosXTwo;
            }
            else
            {
                l_XMove = l_GridPosXTwo - l_GridPosXOne;
            }

            if (l_GridPosYOne > l_GridPosYTwo)
            {
                l_YMove = l_GridPosYOne - l_GridPosYTwo;
            }
            else
            {
                l_YMove = l_GridPosYTwo - l_GridPosYOne;
            }

            float l_GScore = (l_XMove * moveCost) + (l_YMove * moveCost);

            openSet[i].GetComponent<Cell_Info>().m_SetGScore(l_GScore); 
        }
    }

    // Calculate the H score for all of the cells in the open set. 
    void m_CalculateHScore(List<GameObject> openSet, GameObject currentCell, float moveCost)
    {
        for (int i = 0; i < openSet.Count; i++)
        {
            int l_GridPosXOne = openSet[i].GetComponent<Cell_Info>().m_GetGridPos().x;
            int l_GridPosXTwo = currentCell.GetComponent<Cell_Info>().m_GetGridPos().x;

            int l_GridPosYOne = openSet[i].GetComponent<Cell_Info>().m_GetGridPos().y;
            int l_GridPosYTwo = currentCell.GetComponent<Cell_Info>().m_GetGridPos().y;

            int l_XMove;

            int l_YMove;

            if (l_GridPosXOne > l_GridPosXTwo)
            {
                l_XMove = l_GridPosXOne - l_GridPosXTwo;
            }
            else
            {
                l_XMove = l_GridPosXTwo - l_GridPosXOne;
            }

            if (l_GridPosYOne > l_GridPosYTwo)
            {
                l_YMove = l_GridPosYOne - l_GridPosYTwo;
            }
            else
            {
                l_YMove = l_GridPosYTwo - l_GridPosYOne;
            }

            float l_HScore = (l_XMove * moveCost) + (l_YMove * moveCost);

            openSet[i].GetComponent<Cell_Info>().m_SetHScore(l_HScore);
        }
    }

    // Calculate the F score for all of the cells in the open set. 
    void m_CalculateFScore(List<GameObject> openSet)
    {
        for (int i = 0; i < openSet.Count; i++)
        {
            float l_FScore = openSet[i].GetComponent<Cell_Info>().m_GetGScore() + openSet[i].GetComponent<Cell_Info>().m_GetHScore;

            openSet[i].GetComponent<Cell_Info>().m_SetFScore(l_FScore); 
        }

    }

}

