using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class Level
{
    /*! /var This will hold the name of the level allowing for it to be located. */
    public string levelName;

    /*! /var The number of rows the level's grid has. */
    public int rows;

    /*! /var the number of columns the level's grid has. */
    public int columns;

    /*! /var A breif description of the level. */
    public string description;

    /*! /var The configuation of tiles on the map. */
    public string[] tileConfig; 
}

[System.Serializable]
public class Levels
{
    /*! /var A list of levels loaded from a json file. */
    public List<Level> levels = new List<Level>(); 
}

public class Tile_Map_Manager : MonoBehaviour
{
    [SerializeField]
    List<GameObject> m_Grid = new List<GameObject>();

    [SerializeField]
    GameObject m_Cell;

    [SerializeField]
    List<Material> m_Tiles;

    Levels m_ListOfLevels = new Levels();

    [SerializeField]
    TextAsset m_LevelManagerJson;

    [SerializeField]
    int m_iLevelLoaded = 0; 

    private void Start()
    {
        m_LoadMapFromJSONFile(m_LevelManagerJson);

    }

    private void Update()
    {
        switch (m_iLevelLoaded)
        {
            case 0:
                m_CreateTileMap(m_GetLevelFromName("Level Test"));

                m_iLevelLoaded = -1;
                break;

            case 1:
                m_CreateTileMap(m_GetLevelFromName("Level One"));

                m_iLevelLoaded = -1;
                break;

            case 2:
                m_CreateTileMap(m_GetLevelFromName("Level Two"));

                m_iLevelLoaded = -1;
                break;

            default:
                break;
        }
    }

    public void m_ResetGrid()
    {
        for (int i = 0; i < m_Grid.Count; i++)
        {
            Destroy(m_Grid[i]); 
        }

        m_Grid.Clear(); 
    }

    public void m_CreateTileMap(int rows, int columns)
    {
        float l_fSpacing = -1.0f; 

        for(int i = 0; i < columns; i++)
        {
            for(int j = 0; j < rows; j++)
            {
                Vector3 l_Pos = new Vector3(i, j, 0) * l_fSpacing;
                m_Grid.Add(Instantiate(m_Cell, l_Pos, Quaternion.identity));
            }
        }

        for(int k = 0; k < m_Grid.Count; k++)
        {
            m_Grid[k].transform.parent = gameObject.transform;
        }
    }

    public void m_CreateTileMap(int rows, int columns, string[] tileConfig)
    {
        float l_fSpacing = -1.0f;

        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                Vector3 l_Pos = new Vector3(i, j, 0) * l_fSpacing;

                m_Grid.Add(Instantiate(m_Cell, l_Pos, Quaternion.identity));
            }
        }

        for (int k = 0; k < m_Grid.Count; k++)
        {
            m_Grid[k].transform.parent = gameObject.transform;

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

    public void m_CreateTileMap(Level levelToLoad)
    {
        // Reset the grid to it's default state.

        m_ResetGrid(); 

        // Init local variables.

        float l_fSpacing = -1.0f;

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
                }
            }

            // Loop through created the newly created map. 

            for (int k = 0; k < m_Grid.Count; k++)
            {
                // Assign a parent to the cells in the level heirarchy for organisation purposes. 

                m_Grid[k].transform.parent = gameObject.transform;

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
    
    public Level m_GetLevelFromName(string levelName)
    {
        foreach (var level in m_ListOfLevels.levels)
        {
            if(level.levelName == levelName)
            {
                Debug.Log("Level " + levelName + " has been found. \n" + level.description); 

                return level; 
            }
        }

        Debug.Log("Unable to find level in list, returning test level. ");

        return m_ListOfLevels.levels[0];
    }

    public void m_LoadMapFromJSONFile(TextAsset m_LevelJson)
    {
        Debug.Log("Loading " + m_LevelJson.name + " from file");

        // Override the new object to have the data from the json file. 

        m_ListOfLevels = JsonUtility.FromJson<Levels>(m_LevelJson.text);

        // Debugging - Output the number of levels loaded from file 

        Debug.Log("Number of levels loaded : " + m_ListOfLevels.levels.Count);

        if (m_ListOfLevels.levels.Count <= 0)
        {
            Debug.LogError("Error Code 0002 : Unable to load file into game. ");
        }

    }

}
