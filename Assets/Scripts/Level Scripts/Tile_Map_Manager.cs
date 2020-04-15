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
    List<GameObject> m_Grid;

    [SerializeField]
    GameObject m_Cell;

    [SerializeField]
    List<Material> m_Tiles;

    Levels m_ListOfLevels = new Levels();

    [SerializeField]
    TextAsset m_LevelManagerJson; 

    private void Start()
    {
        m_LoadMapFromJSONFile(m_LevelManagerJson);

        m_CreateTileMap(m_GetLevelFromName("Level One")); 
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
        float l_fSpacing = -1.0f;

        List<string> l_AdjustedTileConfig = new List<string>();

        if (levelToLoad.tileConfig.Length > 0)
        {
            foreach (var item in levelToLoad.tileConfig)
            {
                l_AdjustedTileConfig.AddRange(item.Split(','));
            }

            for (int i = 0; i < levelToLoad.rows; i++)
            {
                for (int j = 0; j < levelToLoad.columns; j++)
                {
                    Vector3 l_Pos = new Vector3(i, j, 0) * l_fSpacing;

                    m_Grid.Add(Instantiate(m_Cell, l_Pos, Quaternion.identity));
                }
            }

            for (int k = 0; k < m_Grid.Count; k++)
            {
                m_Grid[k].transform.parent = gameObject.transform;

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

                            m_Grid[k].GetComponent<Cell_Manager>().m_SetTile(CellTile.none, m_Tiles[0]);

                            Debug.Log("Using Default with Assigned Tile : " + int.Parse(l_AdjustedTileConfig[k]));

                            break;
                    }
                }
                else
                {
                    m_Grid[k].GetComponent<Cell_Manager>().m_SetTile(CellTile.none, m_Tiles[0]);
                }
            }
        }
        else
        {
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
        // Override the new object to have the data from the json file. 

        m_ListOfLevels = JsonUtility.FromJson<Levels>(m_LevelJson.text);

        // m_Levels.Add(l_Level);

        Debug.Log(m_ListOfLevels.levels[0].levelName); 
    }

}
