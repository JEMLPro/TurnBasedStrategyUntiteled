using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpriteData
{
    public string spriteName;

    public string filePath;

    public Sprite loadedSprite; 
}

[System.Serializable]
public class Sprites
{
    public List<SpriteData> sprites; 
}

public class Sprite_Loader : MonoBehaviour
{

    [SerializeField]
    TextAsset m_TileSpriteManager;

    [SerializeField]
    Sprites m_ListOfTiles = new Sprites(); 

    public bool m_LoadSpritesFromJSONFile()
    {
        Debug.Log("Loading " + m_TileSpriteManager.name + " from file");

        // Override the new object to have the data from the json file. 

        m_ListOfTiles = JsonUtility.FromJson<Sprites>(m_TileSpriteManager.text);

        // Debugging - Output the number of levels loaded from file 

        Debug.Log("Number of Spries loaded : " + m_ListOfTiles.sprites.Count);

        if (m_ListOfTiles.sprites.Count <= 0)
        {
            Debug.LogError("Error Code 0002-1 : Unable to load Json file into game. Sprites have not been loaded.  ");

            return false;
        }
        else
        {
            foreach (var tile in m_ListOfTiles.sprites)
            {
                // This will be used to display the names of all of the sprites loaded into the game. 

                tile.loadedSprite = Resources.Load<Sprite>(tile.filePath);

                if (tile.loadedSprite != null)
                {

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

    public Sprites m_GetSpriteList() => m_ListOfTiles; 

}
