using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum  CellTile /*!< \enum This will be the tile assigned to this cell, will be used for obsticle detection. */
{
        none = 0x00, 
        grass = 0x01, 
        water = 0x02
}

[System.Serializable]
public struct GridPos /*!< \struct This will hold a pair of ints which represent the cells coords in a grid. */
{
    [SerializeField]
    public int x, y; /*!< \ var A pair of coords. */
}

public class Cell_Manager : MonoBehaviour
{
    
    [SerializeField]
    Sprite m_CellMaterial; /*! < \var This will hold the cells current material. */

    [SerializeField]
    CellTile m_TileType; /*! < \var This is the type of tile the cell is, will allow for pathfinding/obsticle detection. */

    [SerializeField]
    GridPos m_GridPos; /*! < \var A pair of (X, Y) coordiantes for this cell's position on the grid. */

    [SerializeField]
    bool m_bOccupied = false;

    [SerializeField]
    bool m_bObsticle = false; 

    // This will be used to set a new tile to this cell, allowing for a tile map to be cretaed. 
    public void m_SetTile(CellTile newTile, Sprite newMaterial)
    {
        m_TileType = newTile; 

        if(m_TileType == CellTile.water)
        {
            // If the tile is a select few types then they will be an obsticle. 

            m_bObsticle = true; 
        }

        m_CellMaterial = newMaterial;

        gameObject.GetComponent<SpriteRenderer>().sprite = newMaterial; 
    }

    // This will be used to update the cell's coordinates on the grid.
    public void m_SetGridPos(int x, int y)
    {
        m_GridPos.x = x;
        m_GridPos.y = y; 
    }

    public bool m_bcheckForObsticle() => m_bObsticle;

    public void m_bSetObsticle(bool value) { m_bObsticle = value; }

    public bool m_bCheckForOccupied() => m_bOccupied;

    public void m_bSetOccupied(bool value) { m_bOccupied = value; }

    public bool m_CanBeMovedTo()
    {
        if(m_bObsticle == true || m_bOccupied == true)
        {
            return false;
        }

        return true; 
    }

    // This will return the distance between two coordinates as a single integer. 
    public int m_Distance(int x, int y)
    {
        int l_iNewX, l_iNewY; 

        // This checks which value is bigger to ensure the end product is always positive. 

        if(m_GridPos.x > x)
        {
            l_iNewX = m_GridPos.x - x;
        }
        else
        {
            // Minus the smaller from the larger to get the value between them, for example: 9 - 3 = 6, the distance from 3 to get to 9. 

            l_iNewX = x - m_GridPos.x; 
        }

        if (m_GridPos.y > y)
        {
            l_iNewY = m_GridPos.y - y;
        }
        else
        {
            l_iNewY = y - m_GridPos.y;
        }

        // Add the two values together and the final total distance is calculated. For example: (0, 3) and (1, 2) has a distance of 
        // (1, 1) between them or a total of 2 steps. 

        return l_iNewX + l_iNewY; 
    }

    /*! \fn This will be used to find the distance between two cell objects, the object passed into this function needs to be a cell or things won't work. */
    public int m_Distance(GameObject otherCell)
    {
        if(otherCell.GetComponent<Cell_Manager>() == null)
        {
            // Check the object passed into this function is a cell. 

            Debug.LogError("Unable to find cell: " + otherCell.name);

            return -1; 
        }

        int l_iNewX, l_iNewY;

        // This checks which value is bigger to ensure the end product is always positive. 

        if (m_GridPos.x > otherCell.GetComponent<Cell_Manager>().m_GridPos.x)
        {
            l_iNewX = m_GridPos.x - otherCell.GetComponent<Cell_Manager>().m_GridPos.x;
        }
        else
        {
            // Minus the smaller from the larger to get the value between them, for example: 9 - 3 = 6, the distance from 3 to get to 9. 

            l_iNewX = otherCell.GetComponent<Cell_Manager>().m_GridPos.x - m_GridPos.x;
        }

        if (m_GridPos.y > otherCell.GetComponent<Cell_Manager>().m_GridPos.y)
        {
            l_iNewY = m_GridPos.y - otherCell.GetComponent<Cell_Manager>().m_GridPos.y;
        }
        else
        {
            l_iNewY = otherCell.GetComponent<Cell_Manager>().m_GridPos.y - m_GridPos.y;
        }

        // Add the two values together and the final total distance is calculated. For example: (0, 3) and (1, 2) has a distance of 
        // (1, 1) between them or a total of 2 steps. 

        // Debug.Log("Distance = " + l_iNewX + l_iNewY); 

        return l_iNewX + l_iNewY;
    }

    // This will allow for the access of the cells current coordinates. 
    public GridPos m_GetGridPos() => m_GridPos; 

}
