using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Start_Up_Script : Prefab_Loader
{
    [SerializeField]
    GameObject m_TurnManager;

    [SerializeField]
    GameObject m_LevelManager;

    [SerializeField]
    GameObject m_UserInterfaceManager;

    [SerializeField]
    GameObject m_Player;

    [SerializeField]
    GameObject m_AI;

    private void Start()
    {
        // This will be the start of the game and will initate all of the other items in the game. 

        // Start with the Turn manager. 

        GameObject l_TurnManagerReference = m_ExportPrefabObject("Prefabs/Management_Prefabs/Turn Manager", "Turn Manager");

        m_TurnManager = GameObject.Instantiate(l_TurnManagerReference, gameObject.transform);

        if (m_TurnManager == null)
        {
            Debug.LogError("Error code 0001 - Unable to load Turn Manager");
        }

        // Loding the levels into the game. 

        GameObject l_LevelManagerReference = m_ExportPrefabObject("Prefabs/Management_Prefabs/Level Manager", "Level Manager");

        m_LevelManager = GameObject.Instantiate(l_LevelManagerReference, gameObject.transform);

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

        // Introduce the player and AI into the game. 

        Debug.Log("Begining Player Creation.");

        m_Player = new GameObject(); 

        if(m_Player != null)
        {
            // Give the player object a new name.
            m_Player.name = "Player Object";

            // Assign a parent to this new game object. 
            m_Player.transform.parent = gameObject.transform;

            m_Player.AddComponent<Prefab_Loader>();

            // Add the building manager onto the player

            GameObject l_BuildingManager = new GameObject();

            if (l_BuildingManager != null)
            {
                l_BuildingManager.name = "Building Manager";

                l_BuildingManager.transform.parent = m_Player.transform;

                l_BuildingManager.AddComponent<Bulding_Manager>();

                Debug.Log("Building Manager Created and added to player"); 

                l_BuildingManager.GetComponent<Bulding_Manager>().m_SetOwner(CurrentTurn.player);

                l_BuildingManager.GetComponent<Bulding_Manager>().m_SetBasicBuilding(m_Player.GetComponent<Prefab_Loader>().m_ExportPrefabObject("Prefabs/Building Prefabs/TempBuilding", "Basic Building"));

                l_BuildingManager.GetComponent<Bulding_Manager>().m_SpawnHQ(m_LevelManager.GetComponent<Prefab_Loader>().m_GetLoadedObject().GetComponent<Tile_Map_Manager>().m_GetHQSpawnPoint(0));
            }
            else
            {
                Debug.LogError("Error 0100-1 - Unable to create object on player: Building Manager");
            }

            // Add the unit manager to the player.

            GameObject l_UnitManager = new GameObject();

            if (l_UnitManager != null)
            {
                l_UnitManager.name = "Unit Manager";

                l_UnitManager.transform.parent = m_Player.transform;

                // Add components 

                l_UnitManager.AddComponent<Unit_Manager>();

                l_UnitManager.AddComponent<Unit_Spwaning>();

                l_UnitManager.AddComponent<Activate_Radial_Menu>(); 

                Debug.Log("Unit Manager Created and added to player");

                // Load in basic unit

                l_UnitManager.GetComponent<Unit_Spwaning>().m_SetBaseUnit(m_Player.GetComponent<Prefab_Loader>().m_ExportPrefabObject("Prefabs/Unit Prefabs/Base Unit", "Basic Unit"));

                // Spawn Units

                Debug.Log("Spawning Units... ");

                int l_iNumberOfSpawnedUnits = 0;

                foreach (var Cell in l_BuildingManager.GetComponent<Bulding_Manager>().m_GetHQObject().GetComponent<Building_Positioning>().m_GetPosition().GetComponent<Cell_Neighbours>().m_GetCellNeighbours())
                {
                    l_UnitManager.GetComponent<Unit_Manager>().m_AddUnitIntoList((l_UnitManager.GetComponent<Unit_Spwaning>().m_SpawnMilitiaUnit(Cell)));

                    l_iNumberOfSpawnedUnits++;
                }

                Debug.Log(l_iNumberOfSpawnedUnits + " Units have been spawned"); 
            }
            else
            {
                Debug.LogError("Error 0100-2 - Unable to create object on player: Unit Manager");
            }

            // Connect the user interface objects to their reference objects. 

            Debug.Log("User Interface connection");

            if(m_UserInterfaceManager == null)
            {
                m_UserInterfaceManager = GameObject.FindGameObjectWithTag("User_Interface");
            }

            if(m_UserInterfaceManager != null)
            {
                // Connect end turn button to theturn manager. 

                m_UserInterfaceManager.GetComponent<Interface_Controller>().m_SetEndTurnFunctionality(m_TurnManager);

                // Connect Radial menu to player. 

                GameObject l_RadialMenuObj = m_UserInterfaceManager.GetComponent<Interface_Controller>().m_GetRadialMenu();

                m_Player.GetComponentInChildren<Activate_Radial_Menu>().m_SetRadialMenuObject(l_RadialMenuObj, l_RadialMenuObj.GetComponentInChildren<RectTransform>(),
                   m_UserInterfaceManager.GetComponent<Interface_Controller>().m_GetMainCanvas()); 

                // Assign functions to the buttons on the radial menu. 
            }

        }

    }

}
