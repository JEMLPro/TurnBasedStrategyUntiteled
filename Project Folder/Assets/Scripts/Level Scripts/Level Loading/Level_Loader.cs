using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//---------------------------------------------------------------------------------------------------------------------------\\
// File Start
//---------------------------------------------------------------------------------------------------------------------------\\

/// <summary>
/// This is the data required to crate a level for the game. All of this data will be contaied within a Json file.
/// </summary>
[System.Serializable]
public class Level
{
    /// <summary>
    /// This is the name for the level. 
    /// </summary>
    public string levelName;

    /// <summary>
    /// The number of rows the level has, used to create a grid for the level.
    /// </summary>
    public int rows;

    /// <summary>
    /// The number of columns the level has, used to create a grid forthe level. 
    /// </summary>
    public int columns;

    /// <summary>
    /// A simple description for the level, this will be displayed on the level select screen to let the player 
    /// know what kind of map it is. 
    /// </summary>
    public string description;

    /// <summary>
    /// The configuration of tiles for the level, this is mainly used for giving the cell tiles a texture for the game.
    /// </summary>
    public string[] tileConfig;

    /// <summary>
    /// This will hold all of the player's building spawn points for the game. 
    /// </summary>
    public int[] playerPoints;

    /// <summary>
    /// This will hold all of the AI player's building spawn points for the game. 
    /// </summary>
    public int[] aiPoints;
}

/// <summary>
/// This is a list of multiple levels for the game, if there is more than one level within the level Json 
/// this class will load all of them into this, providing all of the data is within the level class above. 
/// </summary>
[System.Serializable]
public class Levels
{
    /// <summary>
    /// A list of levels, the data recived from a Json file containing multiple levels. 
    /// </summary>
    public List<Level> levels = new List<Level>();
}

/// <summary>
/// This class will handle the loading, storing and management of all of the data required to create a level 
/// for the game. 
/// </summary>
public class Level_Loader : MonoBehaviour
{
    //---------------------------------------------------------------------------------------------------------------------------\\
    // Data Members Start
    //---------------------------------------------------------------------------------------------------------------------------\\

    /// <summary>
    /// The current list of loaded levels, this is what will be manipulated to load a level. 
    /// </summary>
    [SerializeField]
    Levels m_ListOfLevels = new Levels();

    /// <summary>
    /// This is the text asset for the Json file holding all of the level data. 
    /// </summary>
    [SerializeField]
    TextAsset m_LevelManagerJson;

    /// <summary>
    /// This is a reference to which level in the list which is currently loaded into the game. 
    /// </summary>
    [SerializeField]
    int m_iLevelLoaded = 2;

    //---------------------------------------------------------------------------------------------------------------------------\\
    // Member Functions Start
    //---------------------------------------------------------------------------------------------------------------------------\\

    /// <summary>
    /// This will allow external access to the list of levels loaded. 
    /// </summary>
    /// <returns> The level list </returns>
    public Levels m_GetLevelList() => m_ListOfLevels; 

    /// <summary>
    /// This will update which level is currently loaded. 
    /// </summary>
    /// <param name="levelSelecton">The level being loaded. </param>
    public void m_SetLevelToLoad(int levelSelecton)
    {
        m_iLevelLoaded = levelSelecton; 
    }

    /// <summary>
    /// This will output which level is loaded into the game. 
    /// </summary>
    /// <returns></returns>
    public int m_GetLevelLoaded() => m_iLevelLoaded; 

    
    /// <summary>
    /// Using a static name assigned to each level, this function will search the list of levels and output a level (if any)
    /// match the desred name. 
    /// </summary>
    /// <param name="levelName">The name of the level being looked for. </param>
    /// <returns>A level matching the value inputted into this function. </returns>
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

        return null;
    }

    /// <summary>
    /// Using the order the levels were loaded into the game, this will attempt to search for a level using the index 
    /// for the list. 
    /// </summary>
    /// <param name="index">The position in the list for loaded levels. </param>
    /// <returns>A level in the position of the inputted index. </returns>
    public Level m_GetLevelFromList(int index)
    {
        if (index < m_ListOfLevels.levels.Count)
        {
            m_iLevelLoaded = index; 

            return m_ListOfLevels.levels[index];
        }

        Debug.Log("Unable to find level in list, returning test level. ");

        return null;
    }

    /// <summary>
    /// This will use a text asset in the form of a Json file and translate it's data into usable level information and store them 
    /// in the ListOfLevels variable for later use. 
    /// </summary>
    /// <param name="m_LevelJson">This is the Json file containing information to create a level or levels. </param>
    /// <returns></returns>
    public bool m_LoadMapFromJSONFile(TextAsset m_LevelJson)
    {
        Debug.Log("Loading " + m_LevelJson.name + " from file");

        // Override the new object to have the data from the json file. 

        m_ListOfLevels = JsonUtility.FromJson<Levels>(m_LevelJson.text);

        // Debugging - Output the number of levels loaded from file 

        Debug.Log("Number of levels loaded : " + m_ListOfLevels.levels.Count);

        if (m_ListOfLevels.levels.Count <= 0)
        {
            Debug.LogError("Error Code 0002-0 : Unable to load level Json into game. ");

            return false;
        }

        return true; 

    }

    /// <summary>
    /// Using the assigned variable for the text asset this will load the level data into the ListOfLevels variable. 
    /// </summary>
    /// <returns> True if the levels loaded successfully. | false if there are any issues loading. </returns>
    public bool m_LoadMapFromJSONFile()
    {
        Debug.Log("Loading " + m_LevelManagerJson.name + " from file");

        // Override the new object to have the data from the json file. 

        m_ListOfLevels = JsonUtility.FromJson<Levels>(m_LevelManagerJson.text);

        // Debugging - Output the number of levels loaded from file 

        Debug.Log("Number of levels loaded : " + m_ListOfLevels.levels.Count);

        if (m_ListOfLevels.levels.Count <= 0)
        {
            Debug.LogError("Error Code 0002-0 : Unable to load level Json into game. ");

            return false;
        }

        return true; 

    }

    //---------------------------------------------------------------------------------------------------------------------------\\
    // File End
    //---------------------------------------------------------------------------------------------------------------------------\\
}