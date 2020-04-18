using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

public class Level_Loader : MonoBehaviour
{
    [SerializeField]
    Levels m_ListOfLevels = new Levels();

    [SerializeField]
    TextAsset m_LevelManagerJson;

    [SerializeField]
    int m_iLevelLoaded = 1;

    // Start is called before the first frame update
    void Start()
    {
        // At the start of the game load the maps into the game. 

        m_LoadMapFromJSONFile(m_LevelManagerJson);
    }

    public void m_SetLevelToLoad(int levelSelecton)
    {
        m_iLevelLoaded = levelSelecton; 
    }

    public int m_GetLevelLoaded() => m_iLevelLoaded; 

    // Used to return a level class using a string used for a name. 
    public Level m_GetLevelFromName(string levelName)
    {
        // This will loop through each element in the list of levels 

        foreach (var level in m_ListOfLevels.levels)
        {
            if (level.levelName == levelName)
            {
                // When the level matches the desired level name it will return that level. 

                Debug.Log("Level " + levelName + " has been found. \n" + level.description);

                return level;
            }
        }

        Debug.Log("Unable to find level in list, returning test level. ");

        return m_ListOfLevels.levels[0];
    }

    // Used to load a list of levels from a JSON file. 
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
