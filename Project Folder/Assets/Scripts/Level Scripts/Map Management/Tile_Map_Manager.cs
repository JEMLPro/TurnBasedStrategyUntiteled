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
    Level m_CurrentLevelLoaded; 

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

                Debug.Log("Map Sucsessfully Setup"); 
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

    // Used to load a level using the Level Class. 
    public void m_CreateTileMap(Level levelToLoad)
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

                                Debug.LogError("Error Code 0004 : Unable to assign material. " + int.Parse(levelToLoad.tileConfig[k]));

                                break;
                            }
                    }
                }
                else
                {
                    // If tile config has not been assigned for this cell assign null material, allowing for visual repersentation in level for the error. 

                    m_Grid[k].GetComponent<Cell_Manager>().m_SetTile(CellTile.none, null);

                    Debug.LogError("Error Code 0004 : Unable to assign material. ");
                }
            }

            if(levelToLoad.playerPoints.Length > 0)
            {
                

                m_HQPosOne.x = levelToLoad.playerPoints[0];
                m_HQPosOne.y = levelToLoad.playerPoints[1];

                Debug.Log("First Hq spawn point found");
            }
            else
            {
                Debug.LogError("Unable to get spawn positions");
            }

            if (levelToLoad.aiPoints.Length > 0)
            {
                foreach (var point in levelToLoad.aiPoints)
                {
                    Debug.Log(point); 
                }

                m_HQPosTwo.x = levelToLoad.aiPoints[0];
                m_HQPosTwo.y = levelToLoad.aiPoints[1];

                Debug.Log("Second Hq spawn point found");
            }
            else
            {
                Debug.LogError("Unable to get spawn positions");
            }

            // Debugging - Output when level finished loading. 

            Debug.Log("Level: " + levelToLoad.levelName + " has been loaded. ");
        }
        else
        {
            // This will output when the level cannot be loaded. 

            Debug.LogError("Error Code 0003 : Unable to load level from file. "); 
        }

        m_CalculateMapBounds(); 
    }

    public void m_CreateTileMap(int levelindex)
    {
        m_CurrentLevelLoaded = gameObject.GetComponent<Level_Loader>().m_GetLevelFromList(levelindex);

        // Reset the grid to it's default state.

        Debug.Log("Map being reset for map creation.");

        m_ResetGrid();

        // Get level to load  

        Level l_LevelToLoad = gameObject.GetComponent<Level_Loader>().m_GetLevelFromList(levelindex);

        if (l_LevelToLoad != null)
        {
            // Init local variables.

            float l_fSpacing = -1.0f;

            // Check the tile configuration for the level. 

            if (l_LevelToLoad.tileConfig.Length > 0)
            {

                Debug.Log("Map being created with dimentions: " + l_LevelToLoad.rows + ", " + l_LevelToLoad.columns);

                // Create and store the grid using level's definitions. 

                for (int i = 0; i < l_LevelToLoad.rows; i++)
                {
                    for (int j = 0; j < l_LevelToLoad.columns; j++)
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

                Debug.Log("Beginning Tile Assignment. Tile config size " + l_LevelToLoad.tileConfig.Length);

                int l_iNumberOfTiles = l_LevelToLoad.tileConfig.Length;

                for (int k = 0; k < m_Grid.Count; k++)
                {
                    m_AssignCellNeighbours(k);

                    // Using tile config for level assign proper tiles. 

                    if (l_iNumberOfTiles >= k)
                    {
                        // This will check the tile config for the code of the next cell allowing for assignment. 
                        switch (int.Parse(l_LevelToLoad.tileConfig[k]))
                        {
                            case 1:
                                {
                                    if (gameObject.GetComponent<Sprite_Loader>().m_GetSpriteList().sprites.Count >= 2)
                                    {
                                        Sprite l_newSprite = gameObject.GetComponent<Sprite_Loader>().m_GetSpriteList().sprites[1].loadedSprite;

                                        m_Grid[k].GetComponent<Cell_Manager>().m_SetTile(CellTile.grass, l_newSprite);
                                    }
                                    break;
                                }
                            case 2:
                                {
                                    if (gameObject.GetComponent<Sprite_Loader>().m_GetSpriteList().sprites.Count >= 3)
                                    {
                                        m_Grid[k].GetComponent<Cell_Manager>().m_SetTile(CellTile.water, gameObject.GetComponent<Sprite_Loader>().m_GetSpriteList().sprites[2].loadedSprite);
                                    }
                                    break;
                                }
                            default:
                                {
                                    // If the tile config is out of range the cell will be assigned a null material. 

                                    m_Grid[k].GetComponent<Cell_Manager>().m_SetTile(CellTile.none, gameObject.GetComponent<Sprite_Loader>().m_GetSpriteList().sprites[0].loadedSprite);

                                    Debug.LogError("Error Code 0004 : Unable to assign material. " + int.Parse(l_LevelToLoad.tileConfig[k]));

                                    break;
                                }
                        }
                    }
                    else
                    {
                        // If tile config has not been assigned for this cell assign null material, allowing for visual repersentation in level for the error. 

                        m_Grid[k].GetComponent<Cell_Manager>().m_SetTile(CellTile.none, null);

                        Debug.LogError("Error Code 0004 : Unable to assign material. ");
                    }
                }

                if (l_LevelToLoad.playerPoints.Length > 0)
                {

                    m_HQPosOne.x = l_LevelToLoad.playerPoints[0];
                    m_HQPosOne.y = l_LevelToLoad.playerPoints[1];

                    Debug.Log("First Hq spawn point found");
                }
                else
                {
                    Debug.LogError("Unable to get spawn positions");
                }

                if (l_LevelToLoad.aiPoints.Length > 0)
                {
                    foreach (var point in l_LevelToLoad.aiPoints)
                    {
                        Debug.Log(point);
                    }

                    m_HQPosTwo.x = l_LevelToLoad.aiPoints[0];
                    m_HQPosTwo.y = l_LevelToLoad.aiPoints[1];

                    Debug.Log("Second Hq spawn point found");
                }
                else
                {
                    Debug.LogError("Unable to get spawn positions");
                }

                // Debugging - Output when level finished loading. 

                Debug.Log("Level: " + l_LevelToLoad.levelName + " has been loaded. ");
            }
            else
            {
                // This will output when the level cannot be loaded. 

                Debug.LogError("Error Code 0003 : Unable to load level from file. ");
            }

            m_CalculateMapBounds();
        }
        else
        {
            Debug.LogError("Error Code 0003 : Unable to load level from file. ");
        }
    }

    //--------------------------------------------------------------------------------------------------------------------------------
    //  Map Bounds and Camera Interaction.
    //--------------------------------------------------------------------------------------------------------------------------------

    void m_CalculateMapBounds()
    {
        m_MinBounds = m_Grid.First<GameObject>().transform.position;

        m_MaxBounds = m_Grid.Last<GameObject>().transform.position;
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

    public GameObject m_GetSelectedCell(bool resetCell)
    {
        foreach (var cell in m_Grid)
        {
            if (cell.GetComponent<Object_Selection>().m_bGetObjectSelected() == true)
            {
                if (resetCell)
                {
                    cell.GetComponent<Object_Selection>().m_SetObjectSelected(false);
                }

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
                Debug.Log("HQ spawn point one found");

                return m_GetCellUsingGridPosition(m_HQPosOne.x, m_HQPosOne.y);

            case 1:
                if (m_GetCellUsingGridPosition(m_HQPosTwo.x, m_HQPosTwo.y) != null)
                {

                    Debug.Log("HQ spawn point two found");

                    return m_GetCellUsingGridPosition(m_HQPosTwo.x, m_HQPosTwo.y);
                }
                else
                {
                    Debug.Log("HQ spawn point two not found with coords " + m_HQPosTwo.x + ", " + m_HQPosTwo.y);

                    return null;
                }

            default:
                break;
        };

        Debug.Log("HQ spawn point not found");

        return null;
    }

    public GameObject m_GetSpawnPoint(CurrentTurn owner, int spawnIndex)
    {
        GameObject l_ReturnValue = null;

        if(owner == CurrentTurn.player)
        {
            // Use the player spawn points. 

            switch (spawnIndex)
            {
                case 0:
                    // Spawn point for HQ.

                    if(m_CurrentLevelLoaded.playerPoints.Length > spawnIndex)
                    {
                        l_ReturnValue = m_GetCellUsingGridPosition(m_CurrentLevelLoaded.playerPoints[0], m_CurrentLevelLoaded.playerPoints[1]);
                    }

                    break;

                case 1:
                    // Spawn point for Farm.

                    if (m_CurrentLevelLoaded.playerPoints.Length > spawnIndex)
                    {
                        l_ReturnValue = m_GetCellUsingGridPosition(m_CurrentLevelLoaded.playerPoints[2], m_CurrentLevelLoaded.playerPoints[3]);
                    }

                    break;

                case 2:
                    // Spawn point for Iron Mine.

                    if (m_CurrentLevelLoaded.playerPoints.Length > spawnIndex)
                    {
                        l_ReturnValue = m_GetCellUsingGridPosition(m_CurrentLevelLoaded.playerPoints[4], m_CurrentLevelLoaded.playerPoints[5]);
                    }

                    break;

                case 3:
                    // Spawn Point for Gold Mine.

                    if (m_CurrentLevelLoaded.playerPoints.Length > spawnIndex)
                    {
                        l_ReturnValue = m_GetCellUsingGridPosition(m_CurrentLevelLoaded.playerPoints[6], m_CurrentLevelLoaded.playerPoints[7]);
                    }

                    break;

                default:
                    // Base will output error if something is wrong. 
                    Debug.LogError("Index out of range index value is " + spawnIndex + " should be " + m_CurrentLevelLoaded.playerPoints.Length / 2); 

                    break;
            }
        }
        else
        {
            // Use the ai spawn points. 

            switch (spawnIndex)
            {
                case 0:
                    // Spawn point for HQ.

                    if (m_CurrentLevelLoaded.aiPoints.Length > spawnIndex)
                    {
                        l_ReturnValue = m_GetCellUsingGridPosition(m_CurrentLevelLoaded.aiPoints[0], m_CurrentLevelLoaded.aiPoints[1]);
                    }

                    break;

                case 1:
                    // Spawn point for Farm.

                    if (m_CurrentLevelLoaded.aiPoints.Length > spawnIndex)
                    {
                        l_ReturnValue = m_GetCellUsingGridPosition(m_CurrentLevelLoaded.aiPoints[2], m_CurrentLevelLoaded.aiPoints[3]);
                    }

                    break;

                case 2:
                    // Spawn point for Iron Mine.

                    if (m_CurrentLevelLoaded.aiPoints.Length > spawnIndex)
                    {
                        l_ReturnValue = m_GetCellUsingGridPosition(m_CurrentLevelLoaded.aiPoints[4], m_CurrentLevelLoaded.aiPoints[5]);
                    }

                    break;

                case 3:
                    // Spawn Point for Gold Mine.

                    if (m_CurrentLevelLoaded.aiPoints.Length > spawnIndex)
                    {
                        l_ReturnValue = m_GetCellUsingGridPosition(m_CurrentLevelLoaded.aiPoints[6], m_CurrentLevelLoaded.aiPoints[7]);
                    }

                    break;

                default:
                    // Base will output error if something is wrong. 
                    Debug.LogError("Index out of range index value is " + spawnIndex + " should be " + m_CurrentLevelLoaded.aiPoints.Length / 2);

                    break;
            }
        }

        return l_ReturnValue; 
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
