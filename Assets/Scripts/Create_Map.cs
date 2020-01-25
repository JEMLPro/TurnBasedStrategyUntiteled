using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Create_Map : MonoBehaviour
{
    [SerializeField]
    GameObject m_Cell; // The prefab for the cell object. Used for spawning clones. 

    [SerializeField]
    List<GameObject> m_GridMap; // This will hold references to all of the cells created within this function. allowing for access later. 

    [SerializeField]
    int m_Rows = 2, m_Columns = 2; // The Default dimentions for the grid, can be modified within engine. 

    [SerializeField]
    Transform m_StartPos; // The empty game object which will act as the parent of all the cell objects. The transform will be used to place the grid. 

    // Start is called before the first frame update
    void Start()
    {
        // Set start point for map.

        m_StartPos = gameObject.transform; // The empty map object. 

        // Create basic grid.

        for(int i = 0; i < m_Rows; i++)
        {
            for (int j = 0; j < m_Columns; j++)
            {
                // Spawn new cell object using prefab. 
                GameObject l_TempCell =  Instantiate(m_Cell, new Vector3(m_StartPos.position.x + i, m_StartPos.position.y + j, 0), Quaternion.identity);

                l_TempCell.GetComponent<Cell_Info>().m_SetGridPos(i, j);

                l_TempCell.transform.parent = gameObject.transform;

                // Add to the grid for easy access. 
                m_GridMap.Add(l_TempCell); 
            }
        }

        Debug.Log("Map Created with " + m_GridMap.Count + " Cells."); 

        // Assign Cell Neighbours. Used for pathfinding. 

        for(int i = 0; i < m_GridMap.Count; i++)
        {
            // Check Up 

            if (m_GridMap[i].GetComponent<Cell_Info>().m_GetGridPos().y < m_Columns - 1)
            {
                m_GridMap[i].GetComponent<Cell_Info>().m_SetCellNeighbour("Up", m_FindGridPos(m_GridMap[i].GetComponent<Cell_Info>().m_GetGridPos().x, m_GridMap[i].GetComponent<Cell_Info>().m_GetGridPos().y + 1));
            }

            // Check Down

            if (m_GridMap[i].GetComponent<Cell_Info>().m_GetGridPos().y > 0)
            {
                m_GridMap[i].GetComponent<Cell_Info>().m_SetCellNeighbour("Down", m_FindGridPos(m_GridMap[i].GetComponent<Cell_Info>().m_GetGridPos().x, m_GridMap[i].GetComponent<Cell_Info>().m_GetGridPos().y - 1)); 
            }

            // Check Left


            if (m_GridMap[i].GetComponent<Cell_Info>().m_GetGridPos().x > 0)
            {
                m_GridMap[i].GetComponent<Cell_Info>().m_SetCellNeighbour("Left", m_FindGridPos(m_GridMap[i].GetComponent<Cell_Info>().m_GetGridPos().x - 1, m_GridMap[i].GetComponent<Cell_Info>().m_GetGridPos().y));
            }

            // Check Right 

            if (m_GridMap[i].GetComponent<Cell_Info>().m_GetGridPos().x < m_Rows - 1)
            {
                m_GridMap[i].GetComponent<Cell_Info>().m_SetCellNeighbour("Right", m_FindGridPos(m_GridMap[i].GetComponent<Cell_Info>().m_GetGridPos().x + 1, m_GridMap[i].GetComponent<Cell_Info>().m_GetGridPos().y));
            }
        }

        Debug.Log("Cell Neighbours Assigned"); 

    }

    // Will be used to return a game object at a position (X, Y). 
    GameObject m_FindGridPos(int x, int y)
    {
        for(int i = 0; i < m_GridMap.Count; i++)
        {
            if(m_GridMap[i].GetComponent<Cell_Info>().m_GetGridPos().x == x && m_GridMap[i].GetComponent<Cell_Info>().m_GetGridPos().y == y)
            {
                return m_GridMap[i]; 
            }
        }

        return null;
    }

    // Will return a random cell. Used for debugging. 
    public GameObject m_GetRandomCell()
    {
        if(m_GridMap.Count > 0)
        {
            return m_GridMap[Random.Range(0, m_GridMap.Count)];
        }

        return null; 
    }
}
