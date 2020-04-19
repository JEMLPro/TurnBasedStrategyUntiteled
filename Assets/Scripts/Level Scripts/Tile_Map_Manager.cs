using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;



public class Tile_Map_Manager : MonoBehaviour
{
    [SerializeField]
    List<GameObject> m_Grid = new List<GameObject>();

    [SerializeField]
    GameObject m_Cell;

    [SerializeField]
    List<Sprite> m_Tiles;

    private void Update()
    {
        // This will be used to check which level should be loaded, will reset the variable when a new level is loaded. 

        switch (gameObject.GetComponent<Level_Loader>().m_GetLevelLoaded())
        {
            case 0:
                m_CreateTileMap(gameObject.GetComponent<Level_Loader>().m_GetLevelFromName("Level Test"));

                gameObject.GetComponent<Level_Loader>().m_SetLevelToLoad(-1);
                break;

            case 1:
                m_CreateTileMap(gameObject.GetComponent<Level_Loader>().m_GetLevelFromName("Level One"));

                gameObject.GetComponent<Level_Loader>().m_SetLevelToLoad(-1);
                break;

            case 2:
                m_CreateTileMap(gameObject.GetComponent<Level_Loader>().m_GetLevelFromName("Level Two"));

                gameObject.GetComponent<Level_Loader>().m_SetLevelToLoad(-1);
                break;

            default:
                break;
        }
    }

    // Used to reset the grid allowing for a new one to be built. 
    void m_ResetGrid()
    {
        // Loops through the current cells and removes them from the game.

        for (int i = 0; i < m_Grid.Count; i++)
        {
            Destroy(m_Grid[i]); 
        }

        // Clear the list of all elements allowing for new ones to be assigned in an empty list. 

        m_Grid.Clear(); 
    }

    // Used to create a grid with a set number of rows and columns. 
    void m_CreateTileMap(int rows, int columns)
    {
        // Reset grid for new level to be loaded. 

        m_ResetGrid();

        // Init variables. 

        float l_fSpacing = -1.0f; 

        // Create grid with defined dimentions. 

        for(int i = 0; i < columns; i++)
        {
            for(int j = 0; j < rows; j++)
            {
                Vector3 l_Pos = new Vector3(i, j, 0) * l_fSpacing;
                m_Grid.Add(Instantiate(m_Cell, l_Pos, Quaternion.identity));

                m_Grid[m_Grid.Count - 1].GetComponent<Cell_Manager>().m_SetGridPos(i, j);
            }
        }

        for(int k = 0; k < m_Grid.Count; k++)
        {
            // Assign all cells a parent for oroganisation purposes. 

            m_Grid[k].transform.parent = gameObject.transform;

            m_AssignCellNeighbours(k);
        }
    }

    // Used to form a grid with a set number of rows and columns, with the addes benefit of a configuration for the tiles, 
    void m_CreateTileMap(int rows, int columns, string[] tileConfig)
    {
        // Reset grid for new level to be loaded. 

        m_ResetGrid();

        // Init variables. 

        float l_fSpacing = -1.0f;

        // Create grid with defined dimentions. 

        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                Vector3 l_Pos = new Vector3(i, j, 0) * l_fSpacing;

                m_Grid.Add(Instantiate(m_Cell, l_Pos, Quaternion.identity));

                m_Grid[m_Grid.Count - 1].GetComponent<Cell_Manager>().m_SetGridPos(i, j);
            }
        }

        for (int k = 0; k < m_Grid.Count; k++)
        {
            // Assign all cells a parent for oroganisation purposes. 

            m_Grid[k].transform.parent = gameObject.transform;

            m_AssignCellNeighbours(k);

            if (tileConfig.Length <= k)
            {
                // Look through the tile config and assign the tiles depending upon its number. 
                switch (tileConfig[k])
                {
                    case "1":
                        m_Grid[k].GetComponent<Cell_Manager>().m_SetTile(CellTile.grass, m_Tiles[1]);
                        break;

                    default:
                        m_Grid[k].GetComponent<Cell_Manager>().m_SetTile(CellTile.none, m_Tiles[0]);
                        break;
                }
            }
            else
            {
                m_Grid[k].GetComponent<Cell_Manager>().m_SetTile(CellTile.none, m_Tiles[0]);
            }
        }
    }

    // Used to load a level using the Level Class. 
    void m_CreateTileMap(Level levelToLoad)
    {
        // Reset the grid to it's default state.

        m_ResetGrid(); 

        // Init local variables.

        float l_fSpacing = -0.33f;

        List<string> l_AdjustedTileConfig = new List<string>();

        // Debugging - Output name of level being loaded. 

        Debug.Log(levelToLoad.levelName + ": " + levelToLoad.description);

        // Check the tile configuration for the level. 

        if (levelToLoad.tileConfig.Length > 0)
        {
            // Split the tile config into usable data. 

            foreach (var item in levelToLoad.tileConfig)
            {
                l_AdjustedTileConfig.AddRange(item.Split(','));
            }

            // Create and store the grid using level's definitions. 

            for (int i = 0; i < levelToLoad.rows; i++)
            {
                for (int j = 0; j < levelToLoad.columns; j++)
                {
                    Vector3 l_Pos = new Vector3(i, j, 0) * l_fSpacing;

                    m_Grid.Add(Instantiate(m_Cell, l_Pos, Quaternion.identity));

                    m_Grid[m_Grid.Count - 1].GetComponent<Cell_Manager>().m_SetGridPos(i, j);
                }
            }

            // Loop through created the newly created map. 

            for (int k = 0; k < m_Grid.Count; k++)
            {
                // Assign a parent to the cells in the level heirarchy for organisation purposes. 

                m_Grid[k].transform.parent = gameObject.transform;

                m_AssignCellNeighbours(k); 

                // Using tile config for level assign proper tiles. 

                if (l_AdjustedTileConfig.Count >= k)
                {
                    // Debug.Log("Cell " + k + " Has been assigned tile : " + int.Parse(l_AdjustedTileConfig[k]));

                    switch (int.Parse(l_AdjustedTileConfig[k]))
                    {
                        case 1:
                            m_Grid[k].GetComponent<Cell_Manager>().m_SetTile(CellTile.grass, m_Tiles[1]);
                            break;

                        case 2:
                            m_Grid[k].GetComponent<Cell_Manager>().m_SetTile(CellTile.water, m_Tiles[2]);
                            break;

                        default:

                            // If the tile config is out of range the cell will be assigned a null material. 

                            m_Grid[k].GetComponent<Cell_Manager>().m_SetTile(CellTile.none, m_Tiles[0]);

                            Debug.LogError("Error Code 0001 : Unable to assign material. " + int.Parse(l_AdjustedTileConfig[k]));

                            break;
                    }
                }
                else
                {
                    // If tile config has not been assigned for this cell assign null material, allowing for visual repersentation in level for the error. 

                    m_Grid[k].GetComponent<Cell_Manager>().m_SetTile(CellTile.none, m_Tiles[0]);

                    Debug.LogError("Error Code 0001 : Unable to assign material. ");
                }
            }

            // Debugging - Output when level finished loading. 

            Debug.Log("Level: " + levelToLoad.levelName + " has been loaded. ");
        }
        else
        {
            // This will output when the level cannot be loaded. 

            Debug.LogError("Error Code 0000 : Unable to load level from file. "); 
        }
    }
   
    // This will return a cell depending on the cell coordiantes provided. 
    public GameObject m_GetCellUsingGridPosition(int x, int y)
    {
        // This will loop through each object in the grid until the desired cell is found. 

        foreach (var cell in m_Grid)
        {
            if((cell.GetComponent<Cell_Manager>().m_GetGridPos().x == x) && (cell.GetComponent<Cell_Manager>().m_GetGridPos().y == y))
            {
                return cell;
            }
        }

        // If there is no cell of that value a null object will be outputted and will require handeling. 

        return null;
    }

    public GameObject m_GetSelectedCell()
    {
        foreach (var cell in m_Grid)
        {
            if(cell.GetComponent<Object_Selection>().m_bGetObjectSelected() == true)
            {
                return cell;
            }
        }

        return null; 
    }

    // This will be used to assign the neighbours to a cell in the grid, a position in the grid will need to be provide. 
    void m_AssignCellNeighbours(int numberInGrid)
    {
        // This will check that the number provided isn't out of the grid's range. 

        if (numberInGrid < m_Grid.Count)
        {
            // Init variables. 

            GameObject l_TempCell;

            // Assign Cell in up position

            if (m_Grid[numberInGrid].GetComponent<Cell_Manager>().m_GetGridPos().y - 1 >= 0)
            {
                Debug.Log("Added Cell as Neighbour"); 

                l_TempCell = m_GetCellUsingGridPosition(m_Grid[numberInGrid].GetComponent<Cell_Manager>().m_GetGridPos().x, m_Grid[numberInGrid].GetComponent<Cell_Manager>().m_GetGridPos().y - 1);

                if (l_TempCell != null)
                {
                    m_Grid[numberInGrid].GetComponent<Cell_Neighbours>().m_SetCellNeighbour(l_TempCell);
                }
            }

            // Assign Cell in down position

            if (m_Grid[numberInGrid].GetComponent<Cell_Manager>().m_GetGridPos().y + 1 >= 0)
            {
                l_TempCell = m_GetCellUsingGridPosition(m_Grid[numberInGrid].GetComponent<Cell_Manager>().m_GetGridPos().x, m_Grid[numberInGrid].GetComponent<Cell_Manager>().m_GetGridPos().y + 1);

                if (l_TempCell != null)
                {
                    m_Grid[numberInGrid].GetComponent<Cell_Neighbours>().m_SetCellNeighbour(l_TempCell);
                }
            }

            // Assign Cell in left position

            if (m_Grid[numberInGrid].GetComponent<Cell_Manager>().m_GetGridPos().x - 1 >= 0)
            {
                l_TempCell = m_GetCellUsingGridPosition(m_Grid[numberInGrid].GetComponent<Cell_Manager>().m_GetGridPos().x - 1, m_Grid[numberInGrid].GetComponent<Cell_Manager>().m_GetGridPos().y);

                if (l_TempCell != null)
                {
                    m_Grid[numberInGrid].GetComponent<Cell_Neighbours>().m_SetCellNeighbour(l_TempCell);
                }
            }

            // Assign Cell in right position

            if (m_Grid[numberInGrid].GetComponent<Cell_Manager>().m_GetGridPos().x + 1 >= 0)
            {
                l_TempCell = m_GetCellUsingGridPosition(m_Grid[numberInGrid].GetComponent<Cell_Manager>().m_GetGridPos().x + 1, m_Grid[numberInGrid].GetComponent<Cell_Manager>().m_GetGridPos().y);

                if (l_TempCell != null)
                {
                    m_Grid[numberInGrid].GetComponent<Cell_Neighbours>().m_SetCellNeighbour(l_TempCell);
                }
            }
        }
    }

    

}
