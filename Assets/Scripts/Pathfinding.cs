using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    [SerializeField]
    Material m_ClosedSetMat;

    [SerializeField]
    Material m_OpenSetMat;

    [SerializeField]
    Material m_PathMat;

    [SerializeField]
    GameObject m_GameMap; 

    [SerializeField]
    List<GameObject> m_OpenSet;

    [SerializeField]
    List<GameObject> m_ClosedSet;

    [SerializeField]
    GameObject m_StartCell; 

    [SerializeField]
    GameObject m_CurrentCell;

    [SerializeField]
    GameObject m_EndCell;

    float m_MovementCost = 10;

    [SerializeField]
    List<GameObject> m_Path;

    bool m_FoundPath = false; 

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

    void m_FindPath()
    {
        if (m_CurrentCell == null && m_EndCell == null)
        {
            m_StartCell = m_GameMap.GetComponent<Create_Map>().m_GetRandomCell();

            m_CurrentCell = m_StartCell; 

            m_EndCell = m_GameMap.GetComponent<Create_Map>().m_GetRandomCell();
        }

        if (m_CurrentCell != m_EndCell)
        {
            // Add Current Cell's neighbours to open list; 

            m_AddCellsToOpenSet(m_CurrentCell);

            // Calculate G, H and F Scores; 

            m_CalculateGScore(m_OpenSet, m_EndCell, m_MovementCost);
            m_CalculateHScore(m_OpenSet, m_CurrentCell, m_MovementCost);
            m_CalculateFScore(m_OpenSet);

            // Add Current cell to closed set

            m_AddCellsToClosedSet(m_CurrentCell);

            // Remove Current Cell from open set. 

            m_OpenSet.Remove(m_CurrentCell);

            // Add To path

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
        }
    }

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

    void m_AddCellsToOpenSet(GameObject currentCell)
    {
        // Add Up

        if (currentCell.GetComponent<Cell_Info>().m_GetCellNeighbour("Up") != null)
        {
            bool l_AddCell = true;

            // Check if already inside closed set. 

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

        // Add Down

        if (currentCell.GetComponent<Cell_Info>().m_GetCellNeighbour("Down") != null)
        {
            bool l_AddCell = true;

            // Check if already inside closed set. 

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

        // Add Left

        if (currentCell.GetComponent<Cell_Info>().m_GetCellNeighbour("Left") != null)
        {
            bool l_AddCell = true;

            // Check if already inside closed set. 

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

        // Add Right

        if (currentCell.GetComponent<Cell_Info>().m_GetCellNeighbour("Right") != null)
        {
            bool l_AddCell = true;

            // Check if already inside closed set. 

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
    }

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

    void m_CalculateGScore(List<GameObject> openSet, GameObject endCell, float moveCost)
    {
        for (int i = 0; i < openSet.Count; i++)
        {
            float l_GScore = 0;

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

            l_GScore = (l_XMove * moveCost) + (l_YMove * moveCost);

            Debug.Log(l_GScore);

            openSet[i].GetComponent<Cell_Info>().m_SetGScore(l_GScore); 
        }
    }

    void m_CalculateHScore(List<GameObject> openSet, GameObject currentCell, float moveCost)
    {
        for (int i = 0; i < openSet.Count; i++)
        {
            float l_HScore = 0;

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

            l_HScore = (l_XMove * moveCost) + (l_YMove * moveCost);

            Debug.Log(l_HScore);

            openSet[i].GetComponent<Cell_Info>().m_SetHScore(l_HScore);
        }
    }

    void m_CalculateFScore(List<GameObject> openSet)
    {
        float l_FScore = 0; 

        for (int i = 0; i < openSet.Count; i++)
        {
            l_FScore = openSet[i].GetComponent<Cell_Info>().m_GetGScore() + openSet[i].GetComponent<Cell_Info>().m_GetHScore;

            openSet[i].GetComponent<Cell_Info>().m_SetFScore(l_FScore); 
        }

    }

}

