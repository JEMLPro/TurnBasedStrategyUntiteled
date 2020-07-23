using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//---------------------------------------------------------------------------------------------------------------------------\\
// File Start
//---------------------------------------------------------------------------------------------------------------------------\\

/// <summary>
/// This enum will be used to assign a tile to each cell, this will also have the functionality of setting a cell to be a natural 
/// obsticle of the game, this is a working list and more tile types may need to be added as more are created and implemented. 
/// </summary>
public enum  CellTile 
{
        none = 0x00, 
        grass = 0x01, 
        water = 0x02
}

/// <summary>
/// This is the curent position for this cell within the grid, without this a cell would be much harder to find within the grid. 
/// </summary>
[System.Serializable]
public struct GridPos 
{
    /// <summary>
    /// A pair of data which repersents coord within a grid, The X and Y position in the grid. this will allow for any single cell 
    /// to be found within the grid. 
    /// </summary>
    [SerializeField]
    public int x, y; 
}

/// <summary>
/// This class will contain all of the data needed to control, update and maintain a cell within the game world.
/// </summary>
public class Cell_Manager : MonoBehaviour
{
    //---------------------------------------------------------------------------------------------------------------------------\\
    // Data Members Start
    //---------------------------------------------------------------------------------------------------------------------------\\

    // Tile management. 

    /// <summary>
    /// This is the assigned sprite for this cell. 
    /// </summary>
    [SerializeField]
    Sprite m_CellMaterial; 

    /// <summary>
    /// This is the tile currentlu assigned to this cell. 
    /// </summary>
    [SerializeField]
    CellTile m_TileType; 

    // Cell Positioning and Movement Interaction. 

    /// <summary>
    /// A data pair used to locate this individual cell within the game world. 
    /// </summary>
    [SerializeField]
    GridPos m_GridPos; 

    /// <summary>
    /// This boolean will be used to check if this cell shares a position with another object within the game world. 
    /// 
    /// This is used t prevent multiple different objects from sharing a single cell. 
    /// </summary>
    [SerializeField]
    bool m_bOccupied = false;

    /// <summary>
    /// This will be used to check if this cell is an obsticle, many different reasons for this being true might occur, 
    /// much like if there is another object shairng this cell, but something which cannot naturally move like a building; 
    /// or simply if thsi cell is a tile which does't make sense to move through like water or a mountain tile. 
    /// </summary>
    [SerializeField]
    bool m_bObsticle = false;

    //---------------------------------------------------------------------------------------------------------------------------\\
    // Member Functions Start
    //---------------------------------------------------------------------------------------------------------------------------\\

    /// <summary>
    /// This will allow for this cell to be assigned a new tile.
    /// </summary>
    /// <param name="newTile">The new tile enum allows for external checking for this tile. </param>
    /// <param name="newMaterial">This is the new sprite for the tile, this will change the look of the cell. </param>
    public void m_SetTile(CellTile newTile, Sprite newMaterial)
    {
        // Debug.Log("This cell recived sprite " + newMaterial.name); 

        m_TileType = newTile; 

        if(m_TileType == CellTile.water)
        {
            // If the tile is a select few types then they will be an obsticle. 

            m_bObsticle = true; 
        }

        m_CellMaterial = newMaterial;

        gameObject.GetComponent<SpriteRenderer>().sprite = newMaterial; 
    }

    /// <summary>
    /// This will be used to set the grid position for this cell, this will be used when the cell is first created 
    /// and and this time given a position in the map. 
    /// </summary>
    /// <param name="x">The X axis position in the grid. </param>
    /// <param name="y">The Y axis position in the grid. </param>
    public void m_SetGridPos(int x, int y)
    {
        m_GridPos.x = x;
        m_GridPos.y = y; 
    }

    /// <summary>
    /// This will allow for the external checking if this cell is an obsticle. 
    /// </summary>
    /// <returns></returns>
    public bool m_bcheckForObsticle() => m_bObsticle;

    /// <summary>
    /// This is used to set the new value for the obsticle, used is a building is created on this cell, 
    /// or at initilization if the tile is an obsticle by default. 
    /// </summary>
    /// <param name="value">| True if a building is created | False if that building is destroyed |</param>
    public void m_bSetObsticle(bool value) { m_bObsticle = value; }

    /// <summary>
    /// This is used to check if this cell is occupied, this will only be true if a unit is over top of this 
    /// cell.
    /// </summary>
    /// <returns></returns>
    public bool m_bCheckForOccupied() => m_bOccupied;

    /// <summary>
    /// This will be used to set the new value for occupied. 
    /// </summary>
    /// <param name="value">| True if a unit has moved into this cell position | False if the unit has left |</param>
    public void m_bSetOccupied(bool value) { m_bOccupied = value; }

    /// <summary>
    /// This will be used to check if this cell is able to be moved to, it will check both obticle and occupied 
    /// variables and output a single boolean.
    /// </summary>
    /// <returns>| True if both variable are false. | False if either variables are true. |</returns>
    public bool m_CanBeMovedTo()
    {
        if(m_bObsticle == true || m_bOccupied == true)
        {
            return false;
        }

        return true; 
    }

    /// <summary>
    /// This will be used to check the distance between two different points on the grid. 
    /// </summary>
    /// <param name="x">The X position on the grid.</param>
    /// <param name="y">The Y position on the grid. </param>
    /// <returns>A whole number; the number of cells between this cell and another point on the grid. </returns>
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

    /// <summary>
    /// This will be used to check the distance between two different points on the grid. 
    /// </summary>
    /// <param name="otherCell">Another cell on the grid. </param>
    /// <returns>A whole number; the number of cells between this cell and another point on the grid. </returns>
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

    /// <summary>
    /// This allows access to the cell's grid position.
    /// </summary>
    /// <returns></returns>
    public GridPos m_GetGridPos() => m_GridPos;

    //---------------------------------------------------------------------------------------------------------------------------\\
    // File End
    //---------------------------------------------------------------------------------------------------------------------------\\
}
