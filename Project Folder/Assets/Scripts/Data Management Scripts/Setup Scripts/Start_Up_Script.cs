using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public struct objectManager
{
    [SerializeField]
    string objectName;

    [SerializeField]
    GameObject objectType; 
}

public class Start_Up_Script : Prefab_Loader
{
    [SerializeField]
    GameObject m_TurnManager;

    [SerializeField]
    GameObject m_LevelManager;

    [SerializeField]
    GameObject m_Player;

    [SerializeField]
    GameObject m_AI;

    [SerializeField]
    List<objectManager> m_BuildingManagers;

    [SerializeField]
    List<objectManager> m_UnitManagers;

    private void Start()
    {
        // This will be the start of the game and will initate all of the other items in the game. 

        // Start with the Turn manager. 

        m_TurnManager = m_ExportPrefabObject("Prefabs/Management_Prefabs/Turn Manager", "Turn Manager");

        if (m_TurnManager == null)
        {
            Debug.LogError("Error code 0001 - Unable to load Turn Manager");
        }

        // Loding the levels into the game. 

        m_LevelManager = m_ExportPrefabObject("Prefabs/Management_Prefabs/Level Manager", "Level Manager");

        if (m_LevelManager != null)
        {
            m_LevelManager.GetComponent<Prefab_Loader>().m_LoadPrefabObject("Prefabs/Level Prefabs/Tile Map", "Tile Map");

            m_LevelManager.GetComponent<Prefab_Loader>().m_GetLoadedObject().GetComponent<Tile_Map_Manager>().m_SetupMaps();

            // Set the bounds for the camera

            Debug.Log("Updating main camera bounds");

            if (Camera.main.GetComponent<Move_Object>() != null)
            {
                // Set the movement bounds of the camera 

                Camera.main.GetComponent<Move_Object>().m_SetMinBounds(m_LevelManager.GetComponent<Prefab_Loader>().m_GetLoadedObject().GetComponent<Tile_Map_Manager>().m_GetMinBounds());
                Camera.main.GetComponent<Move_Object>().m_SetMaxBounds(m_LevelManager.GetComponent<Prefab_Loader>().m_GetLoadedObject().GetComponent<Tile_Map_Manager>().m_GetMaxBounds());
            }
            else
            {
                // If the camera doesn't have the movement script add it and continue. 

                Camera.main.gameObject.AddComponent<Move_Object>();
                Camera.main.GetComponent<Move_Object>().m_SetMinBounds(m_LevelManager.GetComponent<Prefab_Loader>().m_GetLoadedObject().GetComponent<Tile_Map_Manager>().m_GetMinBounds());
                Camera.main.GetComponent<Move_Object>().m_SetMaxBounds(m_LevelManager.GetComponent<Prefab_Loader>().m_GetLoadedObject().GetComponent<Tile_Map_Manager>().m_GetMaxBounds());
            }
        }
        else
        {
            Debug.LogError("Error code 0001-1 - Unable to load Level Manager");
        }

    }

}
