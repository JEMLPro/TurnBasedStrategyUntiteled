﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Create_Map : MonoBehaviour
{
    [SerializeField]
    GameObject m_Cell;

    [SerializeField]
    List<GameObject> m_GridMap;

    [SerializeField]
    int m_Rows = 2, m_Columns = 2;

    [SerializeField]
    Transform m_StartPos;

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

        // Assign Cell Neighbours. 

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

    // Update is called once per frame
    void Update()
    {
        
    }

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
}
