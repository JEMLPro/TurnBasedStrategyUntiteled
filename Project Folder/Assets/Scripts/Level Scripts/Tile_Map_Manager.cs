using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

public class Tile_Map_Manager : MonoBehaviour
{
    [SerializeField]
    List<GameObject> m_Grid = new List<GameObject>(); /*!< \var This is the grid object in the game, it will hold all if the cells forming the map. */

    [SerializeField]
    Vector2 m_MinBounds;

    [SerializeField]
    Vector2 m_MaxBounds; 

    [SerializeField]
    GameObject m_Cell; /*!< /var This is the prefab object for the cell, used for cloning. */

    [SerializeField]
    bool m_bAllowSelectable = false;

    [SerializeField]
    int m_iCurrentLevel = 1;

    [SerializeField]
    GridPos m_HQPosOne;

    [SerializeField]
    GridPos m_HQPosTwo;

    public void m_SetupMaps()
    {
        Debug.Log("Attempting to load levels into game.");

        // Load the levels from a json file and store them for easy and quick switching, depending on the number of levels 
        // it might be worth loading them when needed rather than the start. Something to think about if initial loading times 
        // are too long. 

        if (gameObject.GetComponent<Level_Loader>().m_LoadMapFromJSONFile())
        {

            Debug.Log("Loading Tiles into the game");

            if (gameObject.GetComponent<Sprite_Loader>().m_LoadSpritesFromJSONFile())
            {
                // This will load the tile sprites into the game critical for loading a new map. The tiles data is stored in a Json file
                // to allow for additional tiles to be easily added into the game. 

                Debug.Log("Loading level " + m_iCurrentLevel);

                m_CreateTileMap(gameObject.GetComponent<Level_Loader>().m_GetLevelFromList(m_iCurrentLevel));

                // The map bounds will need to be stored to allow for the camera to move around the map. This will need to be done whenever a 
                // new map is loaded to ensure full vision over the map. 

                m_CalculateMapBounds(); 
            }
            else
            {
                Debug.LogError("Error 0003 - Unable to properly load and create map items. ");
            }
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
                        m_Grid[k].GetComponent<Cell_Manager>().m_SetTile(CellTile.grass, gameObject.GetComponent<Sprite_Loader>().m_GetSpriteList().sprites[1].loadedSprite);
                        break;

                    default:
                        m_Grid[k].GetComponent<Cell_Manager>().m_SetTile(CellTile.none, gameObject.GetComponent<Sprite_Loader>().m_GetSpriteList().sprites[1].loadedSprite);
                        break;
                }
            }
            else
            {
                m_Grid[k].GetComponent<Cell_Manager>().m_SetTile(CellTile.none, gameObject.GetComponent<Sprite_Loader>().m_GetSpriteList().sprites[1].loadedSprite);
            }
        }
    }

    // Used to load a level using the Level Class. 
    void m_CreateTileMap(Level levelToLoad)
    {
        // Reset the grid to it's default state.

        Debug.Log("Map being reset for map creation."); 

        m_ResetGrid(); 

        // Init local variables.

        float l_fSpacing = -1.0f;

        // Check the tile configuration for the level. 

        if (levelToLoad.tileConfig.Length > 0)
        {

            Debug.Log("Map being created with dimentions: " + levelToLoad.rows + ", " + levelToLoad.columns); 

            // Create and store the grid using level's definitions. 

            for (int i = 0; i < levelToLoad.rows; i++)
            {
                for (int j = 0; j < levelToLoad.columns; j++)
                {

                    // Create a new position for the cell.
                    Vector3 l_Pos = new Vector3(i, j, 0) * l_fSpacing;

                    // Create a new cell for the map.
                    GameObject l_NewCell = Instantiate(m_Cell, l_Pos, Quaternion.identity);

                    // Assign a grid position to the cell, for path finding and other similar functionality. 
                    l_NewCell.GetComponent<Cell_Manager>().m_SetGridPos(i, j);

                    // Update the game hierarchy, making this the parent to the new cell.
                    l_NewCell.transform.parent = gameObject.transform; 

                    // Add the new cell into a list of cells gorming a grid. 
                    m_Grid.Add(l_NewCell);
                }
            }

            Debug.Log("Map created with a number of " + m_Grid.Count + " cells in total.");

            // Loop through created the newly created map. 

            Debug.Log("Beginning Tile Assignment. Tile config size " + levelToLoad.tileConfig.Length);

            int l_iNumberOfTiles = levelToLoad.tileConfig.Length;

            for (int k = 0; k < m_Grid.Count; k++)
            {
                m_AssignCellNeighbours(k);

                // Using tile config for level assign proper tiles. 

                if (l_iNumberOfTiles >= k)
                {
                    // This will check the tile config for the code of the next cell allowing for assignment. 
                    switch (int.Parse(levelToLoad.tileConfig[k]))
                    {
                        case 1:
                            {
                                if (gameObject.GetComponent<Sprite_Loader>().m_GetSpriteList().sprites.Count >= 1)
                                {
                                    Sprite l_newSprite = gameObject.GetComponent<Sprite_Loader>().m_GetSpriteList().sprites[1].loadedSprite;

                                    m_Grid[k].GetComponent<Cell_Manager>().m_SetTile(CellTile.grass, l_newSprite);
                                }
                                break;
                            }
                        case 2:
                            {
                                if (gameObject.GetComponent<Sprite_Loader>().m_GetSpriteList().sprites.Count >= 2)
                                {
                                    m_Grid[k].GetComponent<Cell_Manager>().m_SetTile(CellTile.water, gameObject.GetComponent<Sprite_Loader>().m_GetSpriteList().sprites[2].loadedSprite);
                                }
                                break;
                            }
                        default:
                            {
                                // If the tile config is out of range the cell will be assigned a null material. 

                                m_Grid[k].GetComponent<Cell_Manager>().m_SetTile(CellTile.none, gameObject.GetComponent<Sprite_Loader>().m_GetSpriteList().sprites[0].loadedSprite);

                                Debug.LogError("Error Code 0001 : Unable to assign material. " + int.Parse(levelToLoad.tileConfig[k]));

                                break;
                            }
                    }
                }
                else
                {
                    // If tile config has not been assigned for this cell assign null material, allowing for visual repersentation in level for the error. 

                    m_Grid[k].GetComponent<Cell_Manager>().m_SetTile(CellTile.none, null);

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

    //--------------------------------------------------------------------------------------------------------------------------------
    //  Map Bounds and Camera Interaction.
    //--------------------------------------------------------------------------------------------------------------------------------

    void m_CalculateMapBounds()
    {
        m_MinBounds = m_Grid.First<GameObject>().transform.position;

        m_MaxBounds = m_Grid.Last<GameObject>().transform.position;

        Vector2 l_MapSize = m_MaxBounds - m_MinBounds;
    }

    public Vector2 m_GetMinBounds() => m_MinBounds;

    public Vector2 m_GetMaxBounds() => m_MaxBounds;

    //--------------------------------------------------------------------------------------------------------------------------------
    //  Cell Manipulation and Location. 
    //--------------------------------------------------------------------------------------------------------------------------------

    // This will return a cell depending on the cell coordiantes provided. 
    public GameObject m_GetCellUsingGridPosition(int x, int y)
    {
        // This will loop through each object in the grid until the desired cell is found. 

        foreach (var cell in m_Grid)
        {
            if((cell.GetComponent<Cell_Manager>().m_GetGridPos().x == x) && (cell.GetComponent<Cell_Manager>().m_GetGridPos().y == y))
            {
                // Debug.Log("Cell " + x + ", " + y + " has been found.");

                return cell;
            }
        }

        // Debug.Log("Cell " + x + ", " + y + " has not been found.");

        // If there is no cell of that value a null object will be outputted and will require handeling. 

        return null;
    }

    public GameObject m_GetSelectedCell()
    {
        foreach (var cell in m_Grid)
        {
            if(cell.GetComponent<Object_Selection>().m_bGetObjectSelected() == true)
            {
                cell.GetComponent<Object_Selection>().m_SetObjectSelected(false); 

                return cell;
            }
        }

        return null; 
    }

    public GameObject m_GetRandomCell()
    {
        if(m_Grid.Count <= 0)
        {
            return null; 
        }

        return m_Grid[Random.Range(0, m_Grid.Count - 1)];
    }

    public GameObject m_GetHQSpawnPoint(int index)
    {
        switch (index)
        {
            case 0:
                return m_GetCellUsingGridPosition(m_HQPosOne.x, m_HQPosOne.y);

            case 1:
                return m_GetCellUsingGridPosition(m_HQPosTwo.x, m_HQPosTwo.y);

            default:
                return null;
        }
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
                // Debug.Log("Added Cell as Neighbour"); 

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

    public void m_CheckCellRange(int dist, GameObject startCell)
    {
        if (startCell != null)
        {
            List<GameObject> l_CellsToCheck = new List<GameObject>();

            // Check each of the starting cell's neighbours, if they can be moved to add into the list to check. 

            foreach (var cell in startCell.GetComponent<Cell_Neighbours>().m_GetCellNeighbours())
            {
                if (cell.GetComponent<Cell_Manager>().m_bcheckForObsticle() == false && cell.GetComponent<Cell_Manager>().m_bCheckForOccupied() == false)
                {
                    l_CellsToCheck.Add(cell);
                }
            }

            // Begin looping through the list of cells to check. 

            for (int i = 0; i < l_CellsToCheck.Count; i++)
            {
                // If this cell is within the movement radius. 

                if (l_CellsToCheck[i].GetComponent<Cell_Manager>().m_Distance(startCell) <= dist)
                {

                    foreach (var cell in l_CellsToCheck[i].GetComponent<Cell_Neighbours>().m_GetCellNeighbours())
                    {
                        // Loop through all of this cells neighbours checking they can be moved to. 

                        if (cell.GetComponent<Cell_Manager>().m_bcheckForObsticle() == false && cell.GetComponent<Cell_Manager>().m_bCheckForOccupied() == false)
                        {
                            l_CellsToCheck.Add(cell);
                        }
                    }

                    // Change the colour of the cells to display the can be moved to. 

                    l_CellsToCheck[i].GetComponent<Renderer>().material.color = Color.blue;
                }
                else
                {
                    break; 
                }
            }
        }
    }

    public void m_ResetCellColours()
    {
        foreach (var cell in m_Grid)
        {
            cell.GetComponent<Renderer>().material.color = Color.white;
        }
    }

    public void m_ResetOccupied()
    {
        foreach (var cell in m_Grid)
        {
            cell.GetComponent<Cell_Manager>().m_bSetOccupied(false); 
        }
    }

    public void m_SetSelectable(bool newValue)
    {
        m_bAllowSelectable = newValue;
    }

    public bool m_GetAllowSelectable() => m_bAllowSelectable; 
}
