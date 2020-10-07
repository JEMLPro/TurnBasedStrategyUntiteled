using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//---------------------------------------------------------------------------------------------------------------------------\\
// File Start
//---------------------------------------------------------------------------------------------------------------------------\\

/// <summary>
/// This class will be used to store sprite data from a Json file. 
/// </summary>
[System.Serializable]
public class SpriteData
{
    /// <summary>
    /// The name of the sprite. 
    /// </summary>
    public string spriteName;

    /// <summary>
    /// The path to find the sprite in the file explorer. 
    /// </summary>
    public string filePath;

    /// <summary>
    /// The sprite data used to display a sprite within the game. 
    /// </summary>
    public Sprite loadedSprite; 
}

/// <summary>
/// This will contain a list of sprites within the game, and allow for easy utilization. 
/// </summary>
[System.Serializable]
public class Sprites
{
    /// <summary>
    /// A list containing all of the sprites loaded from the file. 
    /// </summary>
    public List<SpriteData> sprites; 
}

/// <summary>
/// This class will handle the loading and management of number of sprites loaded from a Json file. 
/// </summary>
public class Sprite_Loader : MonoBehaviour
{
    //---------------------------------------------------------------------------------------------------------------------------\\
    // Data Members Start
    //---------------------------------------------------------------------------------------------------------------------------\\

    /// <summary>
    /// This is the Json file which contains the data to be loaded for the sprites.
    /// </summary>
    [SerializeField]
    TextAsset m_SpriteManager;

    /// <summary>
    /// This list will contain all of the sprites to be used by this manager. 
    /// </summary>
    [SerializeField]
    Sprites m_ListOfSprites = new Sprites();

    //---------------------------------------------------------------------------------------------------------------------------\\
    // Member Functions Start
    //---------------------------------------------------------------------------------------------------------------------------\\

    /// <summary>
    /// Using the text asset assigned to this class the sprites will be loaded into the game. 
    /// </summary>
    /// <returns>A boolean used to check if there are any errors with the loading process. </returns>
    public bool m_LoadSpritesFromJSONFile()
    {
        Debug.Log("Loading " + m_SpriteManager.name + " from file");

        // Override the new object to have the data from the json file. 

        m_ListOfSprites = JsonUtility.FromJson<Sprites>(m_SpriteManager.text);

        // Debugging - Output the number of levels loaded from file \\

        Debug.Log("Number of Spries loaded : " + m_ListOfSprites.sprites.Count);

        if (m_ListOfSprites.sprites.Count <= 0)
        {
            Debug.LogError("Error Code 0002-1 : Unable to load Json file into game. Tile Sprites have not been loaded.  ");

            return false;
        }
        else
        {
            foreach (var tile in m_ListOfSprites.sprites)
            {
                // This will load the sprites onto the loaded sprite slot uisng the provided file path.                 

                tile.loadedSprite = Resources.Load<Sprite>(tile.filePath);

                if (tile.loadedSprite != null)
                {
                    // Debugging - This will be used to display the names of all of the sprites loaded into the game. \\ 

                    Debug.Log(tile.spriteName + " has been found.\n");
                }
                else
                {
                    Debug.LogWarning(tile.spriteName + " has not been found.\n");
                }
            }
        }

        return true;

    }

    /// <summary>
    /// This will allow access to the list of sprites loaded into this class. 
    /// </summary>
    /// <returns></returns>
    public Sprites m_GetSpriteList() => m_ListOfSprites;

    /// <summary>
    /// This will be used to assign the text asset into this class, the text asset should contain the 
    /// details needed to load sprites from file.
    /// </summary>
    /// <param name="newSpriteManager">The new text asset. </param>
    public void m_AssignTextAsset(TextAsset newSpriteManager) { m_SpriteManager = newSpriteManager; }

    /// <summary>
    /// This will allow access to a sprite stored by this class using a string reference. 
    /// </summary>
    /// <param name="nameIndex">The string name given to the sprite given when loaded. </param>
    /// <returns>The sprite if it is stored within this file. </returns>
    public Sprite m_GetSpriteFromList(string nameIndex)
    {
        foreach (var sprite in m_ListOfSprites.sprites)
        {
            if(sprite.spriteName == nameIndex)
            {
                return sprite.loadedSprite;
            }
        }

        Debug.LogWarning("Unable to find sprite " + nameIndex + " in sprite list."); 

        return null;
    }

    //---------------------------------------------------------------------------------------------------------------------------\\
    // File End
    //---------------------------------------------------------------------------------------------------------------------------\\

}
