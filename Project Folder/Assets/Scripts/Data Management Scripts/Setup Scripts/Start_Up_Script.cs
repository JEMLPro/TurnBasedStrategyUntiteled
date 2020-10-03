using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//---------------------------------------------------------------------------------------------------------------------------\\
// File Start
//---------------------------------------------------------------------------------------------------------------------------\\

/// <summary>
/// This class is a main manageent script and will be attached to the game's Game Manager object. This will ensure that all 
/// items in the game are initialized and loaded in the correct order. This is to ensure that everything is where it needs to 
/// be before it is referenced. In addition it will also be the staging branch for connecting objects to their other components. 
/// </summary>
public class Start_Up_Script : Prefab_Loader
{
    #region Data Members Start

    #region Other

    bool m_bInGame = false; 

    #endregion

    #region Managers 

    /// <summary>
    /// This is the turn manager and will maintain the turn based system within the game limiting player and AI turns. 
    /// </summary>
    [SerializeField]
    GameObject m_TurnManager;

    /// <summary>
    /// This will manage the game map and the tiles within it, all levels will be loaded into here and manged within an 
    /// object it controls. 
    /// </summary>
    [SerializeField]
    GameObject m_LevelManager;

    /// <summary>
    /// This will hold references to all major UI elements within the game and through this object it will allow for the 
    /// UI elements to be connected to all of the required places upon start-up along with new game setup. 
    /// </summary>
    [SerializeField]
    GameObject m_UserInterfaceManager;

    #endregion

    #region Players 

    /// <summary>
    /// This is the player object and is created when a new game is set up. it will have: a Unit manager, a Building manager 
    /// and a Resource manager. All of these will be connected and the player will need to use these to beat the AI. 
    /// </summary>
    [SerializeField]
    GameObject m_Player;

    /// <summary>
    /// This is a fully programmed player, it will aim to beat the player following it's strict programming, it will have the 
    /// same resources the player has access to but acts independently. This is by all means a work in progress and the AI turns 
    /// will 100% need to be tested many times to ensure a working order. 
    /// 
    /// The AI will work in a set of different ways, pathfinding which will control their units and where they will move to; 
    /// combat analysis using the current climate within the game each unit may need to choose a target independenly of all others 
    /// and this section will ensure each unit will act in it's best interest and finally repositioning and construction, this is 
    /// to ensure the AI is building new buildings to keep up with demand of resources and is expanding in a proper way, as well as 
    /// spawning new units. 
    /// 
    /// Future additions could include behaviour programming such as an aggressive AI or a defencive one. Things to consider. 
    /// </summary>
    [SerializeField]
    GameObject m_AI;

    #endregion

    #endregion

    #region Member Functions Start

    /// <summary>
    /// This will be called upon start-up and will init all of the management scripts used for the game, it will also ensure the 
    /// main menu will be working as intended. 
    /// </summary>
    private void Start()
    {
        // This will be the start of the game and will initate all of the other items in the game. 

        #region Locate and start Interface Manager

        Debug.Log("Locating interface manger in game.");

        if (m_UserInterfaceManager == null)
        {
            m_UserInterfaceManager = GameObject.FindGameObjectWithTag("User_Interface");
        }

        if (m_UserInterfaceManager != null)
        {
            Debug.Log("Interface manger found and connected.");

            m_UserInterfaceManager.GetComponent<Interface_Controller>().m_StartUp(); 
        }
        else
        {
            Debug.LogError("Error code 0001-2 - Unable to load Interface");
        }

        #endregion

        #region Locate and Start Turn Manager

        // The Turn manager. 

        GameObject l_TurnManagerReference = m_ExportPrefabObject("Prefabs/Management_Prefabs/Turn Manager", "Turn Manager");

        m_TurnManager = GameObject.Instantiate(l_TurnManagerReference, gameObject.transform);

        m_TurnManager.GetComponent<Turn_Manager>().m_Startup(); 

        if (m_TurnManager == null)
        {
            Debug.LogError("Error code 0001-0 - Unable to load Turn Manager");
        }

        #endregion

        #region Load levels into the game

        // Loading the levels into the game. 

        GameObject l_LevelManagerReference = m_ExportPrefabObject("Prefabs/Management_Prefabs/Level Manager", "Level Manager");

        m_LevelManager = GameObject.Instantiate(l_LevelManagerReference, gameObject.transform);

        if (m_LevelManager != null)
        {
            m_LevelManager.GetComponent<Prefab_Loader>().m_LoadPrefabObject("Prefabs/Level Prefabs/Tile Map", "Tile Map");

            m_LevelManager.GetComponent<Prefab_Loader>().m_GetLoadedObject().GetComponent<Tile_Map_Manager>().m_SetupMaps();

            // Adding list of levels into dropdown list. 

            Debug.Log("Attempting to add levels into the dropdown list."); 

            int l_iCount = 0;

            foreach (var level in m_LevelManager.GetComponentInChildren<Level_Loader>().m_GetLevelList().levels)
            {
                if (level != null)
                {
                    m_UserInterfaceManager.GetComponent<Interface_Controller>().m_GetLevelSelectDropDown().GetComponentInChildren<Attach_Levels_To_List>().m_AddItemIntoDropdown(level.levelName, l_iCount);

                    l_iCount++;
                }
                else
                {
                    Debug.LogError("Level faild to initalize"); 
                }
            }

            m_UserInterfaceManager.GetComponent<Interface_Controller>().m_GetLevelSelectDropDown().SetActive(false); 
        }
        else
        {
            Debug.LogError("Error code 0001-1 - Unable to load Level Manager");
        }

        #endregion
    }

    /// <summary>
    /// This will be used to start a game, it will use the level selected from the level select menu to load. 
    /// </summary>
    public void m_Begin()
    {
        if (m_UserInterfaceManager.GetComponent<Interface_Controller>().m_GetLevelSelectDropDown().GetComponentInChildren<Attach_Levels_To_List>().m_GetLoadNewLevel())
        {
            m_StartSkirmishGame(m_UserInterfaceManager.GetComponent<Interface_Controller>().m_GetLevelSelectDropDown().GetComponentInChildren<Attach_Levels_To_List>().m_GetLevelToLoad());

            m_UserInterfaceManager.GetComponent<Interface_Controller>().m_GetLevelSelectDropDown().GetComponentInChildren<Attach_Levels_To_List>().m_LevelHasBeenLoaded();
        }

        Time.timeScale = 1;
    }

    /// <summary>
    /// This will be used to start a skirmish game, it will reset all of the current items before beginning. 
    /// </summary>
    /// <param name="level">The level selected and which to load.</param>
    private void m_StartSkirmishGame(int level)
    {
        m_ResetAllItems(); 

        #region Setup Selected Level

        // Load the game map using level index. 

        m_LevelManager.GetComponentInChildren<Tile_Map_Manager>().m_CreateTileMap(level); 

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

        #endregion

        // Introduce the player and AI into the game. 

        Debug.Log("Begining Player Creation.");

        #region Setup Player

        // Player Object. 

        m_Player = new GameObject();

        if (m_Player != null)
        {
            #region Assign Base Components

            // Give the player object a new name.
            m_Player.name = "Player Object";

            // Assign a parent to this new game object. 
            m_Player.transform.parent = gameObject.transform;

            m_Player.AddComponent<Prefab_Loader>();

            m_Player.AddComponent<Lose_Script>();

            m_Player.GetComponent<Lose_Script>().m_SetOwner(CurrentTurn.player);

            #endregion

            #region Create Building Manager

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

                l_BuildingManager.GetComponent<Bulding_Manager>().m_SetTurnManager(m_TurnManager);

                l_BuildingManager.GetComponent<Bulding_Manager>().m_SetTileMap(m_LevelManager.GetComponent<Prefab_Loader>().m_GetLoadedObject());

                // Spawn starting buldings. 

                l_BuildingManager.GetComponent<Bulding_Manager>().m_SpawnHQ(m_LevelManager.GetComponent<Prefab_Loader>().m_GetLoadedObject().GetComponent<Tile_Map_Manager>().m_GetSpawnPoint(CurrentTurn.player, 0));

                l_BuildingManager.GetComponent<Bulding_Manager>().m_SpawnFarm(m_LevelManager.GetComponent<Prefab_Loader>().m_GetLoadedObject().GetComponent<Tile_Map_Manager>().m_GetSpawnPoint(CurrentTurn.player, 1));

                l_BuildingManager.GetComponent<Bulding_Manager>().m_SpawnIronMine(m_LevelManager.GetComponent<Prefab_Loader>().m_GetLoadedObject().GetComponent<Tile_Map_Manager>().m_GetSpawnPoint(CurrentTurn.player, 2));

                l_BuildingManager.GetComponent<Bulding_Manager>().m_SpawnGoldMine(m_LevelManager.GetComponent<Prefab_Loader>().m_GetLoadedObject().GetComponent<Tile_Map_Manager>().m_GetSpawnPoint(CurrentTurn.player, 3));

                #region Create Resource Manager

                // Create Resource Manager. 

                GameObject l_ResourceManager = new GameObject
                {
                    name = "Resource Manager"
                };

                l_ResourceManager.AddComponent<Resource_Management>();

                l_ResourceManager.transform.parent = m_Player.transform;

                l_BuildingManager.GetComponent<Bulding_Manager>().m_SetResourceManager(l_ResourceManager);

                // Attach player's resource manager to UI

                m_UserInterfaceManager.GetComponent<Interface_Controller>().m_SetUpInfoPanel();

                l_ResourceManager.GetComponent<Resource_Management>().m_SetFoodText(m_UserInterfaceManager.GetComponent<Interface_Controller>().m_GetFoodText());
                l_ResourceManager.GetComponent<Resource_Management>().m_SetIronText(m_UserInterfaceManager.GetComponent<Interface_Controller>().m_GetIronText());
                l_ResourceManager.GetComponent<Resource_Management>().m_SetGoldText(m_UserInterfaceManager.GetComponent<Interface_Controller>().m_GetGoldText());

                // Attach unit spawn menu onto build manager 

                l_BuildingManager.GetComponent<Bulding_Manager>().m_SetUnitBuildMenu(m_UserInterfaceManager.GetComponent<Interface_Controller>().m_GetUnitBuildMenu());

                #endregion
            }
            else
            {
                Debug.LogError("Error 0100-1 - Unable to create object on player: Building Manager");
            }

            #endregion

            #region Create Unit Manager

            // Add the unit manager to the player.

            GameObject l_UnitManager = new GameObject();

            if (l_UnitManager != null)
            {
                l_UnitManager.name = "Unit Manager";

                l_UnitManager.tag = "Unit_Manager_Player";

                l_UnitManager.transform.parent = m_Player.transform;

                // Add components 

                l_UnitManager.AddComponent<Unit_Manager>();

                l_UnitManager.AddComponent<Unit_Spwaning>();

                l_UnitManager.AddComponent<Unit_Find_AtTarget1>();

                l_UnitManager.AddComponent<Activate_Radial_Menu>();

                l_UnitManager.AddComponent<Activate_Build_Menu>();

                // Assign functionality to the combat menu.

                m_Player.GetComponentInChildren<Unit_Manager>().m_AssignCombatMenu(m_UserInterfaceManager.GetComponent<Interface_Controller>().m_GetCombatMenu());

                m_UserInterfaceManager.GetComponent<Interface_Controller>().m_GetCombatMenu().GetComponent<Manage_Combat_Screen>().m_SetAttackConfirmation(l_UnitManager);

                m_UserInterfaceManager.GetComponent<Interface_Controller>().m_GetCombatMenu().GetComponent<Manage_Combat_Screen>().m_SetCancelConfirmation(l_UnitManager);

                Debug.Log("Unit Manager Created and added to player");

                #region Spawn Units

                // Load in basic unit

                l_UnitManager.GetComponent<Unit_Spwaning>().m_SetBaseUnit(m_Player.GetComponent<Prefab_Loader>().m_ExportPrefabObject("Prefabs/Unit Prefabs/Base Unit", "Basic Unit"));

                // Spawn Units

                Debug.Log("Spawning Units... ");

                int l_iNumberOfSpawnedUnits = 0;

                foreach (var Cell in l_BuildingManager.GetComponent<Bulding_Manager>().m_GetHQObject().GetComponent<Building_Positioning>().m_GetPosition().GetComponent<Cell_Neighbours>().m_GetCellNeighbours())
                {
                    // The first unit spawned should always be an engineer, allowing for the player to create new buildings from thestart of the game.
                    if (l_iNumberOfSpawnedUnits == 0)
                    {
                        l_UnitManager.GetComponent<Unit_Manager>().m_AddUnitIntoList((l_UnitManager.GetComponent<Unit_Spwaning>().m_SpawnEngineerUnit(Cell)));
                    }
                    else
                    {
                        l_UnitManager.GetComponent<Unit_Manager>().m_AddUnitIntoList((l_UnitManager.GetComponent<Unit_Spwaning>().m_SpawnMilitiaUnit(Cell)));
                    }

                    l_iNumberOfSpawnedUnits++;
                }

                Debug.Log(l_iNumberOfSpawnedUnits + " Units have been spawned");

                #endregion
            }
            else
            {
                Debug.LogError("Error 0100-2 - Unable to create object on player: Unit Manager");
            }

            #endregion

            #region Connect Interface Elements

            // Connect the user interface objects to their reference objects. 

            Debug.Log("User Interface connection");

            if (m_UserInterfaceManager == null)
            {
                m_UserInterfaceManager = GameObject.FindGameObjectWithTag("User_Interface");
            }

            if (m_UserInterfaceManager != null)
            {
                // Connect end turn button to the turn manager. 

                m_UserInterfaceManager.GetComponent<Interface_Controller>().m_SetEndTurnFunctionality(m_TurnManager);

                // Connect Radial menu to player. 

                GameObject l_RadialMenuObj = m_UserInterfaceManager.GetComponent<Interface_Controller>().m_GetActionRadialMenu();

                // Assign functions to the radial menu. 

                m_Player.GetComponentInChildren<Activate_Radial_Menu>().m_SetRadialMenuObject(l_RadialMenuObj, l_RadialMenuObj.GetComponentInChildren<RectTransform>(),
                   m_UserInterfaceManager.GetComponent<Interface_Controller>().m_GetMainCanvas(), l_UnitManager);

                // Connect unit spawnig to the spawn buttons. 

                m_UserInterfaceManager.GetComponent<Interface_Controller>().m_SetUpUnitSpanwing(l_UnitManager, l_BuildingManager, 
                    m_UserInterfaceManager.GetComponent<Interface_Controller>().m_GetUnitBuildMenu().GetComponent<Open_Unit_Spawn_Menu>().m_GetButtons());

                // Connect Building Spawn Menu.

                m_Player.GetComponentInChildren<Activate_Build_Menu>().m_SetBuildMenu(m_UserInterfaceManager.GetComponent<Interface_Controller>().m_GetBuildingBuildMenu());
                m_Player.GetComponentInChildren<Activate_Build_Menu>().m_SetButtonFunctions(l_UnitManager, m_LevelManager.GetComponent<Prefab_Loader>().m_GetLoadedObject(), l_BuildingManager);

                // Connect game over screen. 

                m_Player.GetComponent<Lose_Script>().m_SetGameOverScreen(m_UserInterfaceManager.GetComponent<Interface_Controller>().m_GetGameOverScreen());
            }

            #endregion
        }

        #endregion

        // AI Object. 

        Debug.Log("Begining AI Creation");

        #region Setup AI

        m_AI = new GameObject();

        if (m_AI != null)
        {
            #region Assign Base Components

            // Give the AI object a new name.
            m_AI.name = "AI Object";

            m_AI.tag = "AI_Manager";

            // Assign a parent to this new game object. 
            m_AI.transform.parent = gameObject.transform;

            m_AI.AddComponent<Prefab_Loader>();

            m_AI.AddComponent<Lose_Script>();

            m_AI.GetComponent<Lose_Script>().m_SetOwner(CurrentTurn.ai);

            #endregion

            #region Create Building Manager

            // Add the building manager onto the AI

            GameObject l_AIBuildingManager = new GameObject();

            if (l_AIBuildingManager != null)
            {
                l_AIBuildingManager.name = "Building Manager";

                l_AIBuildingManager.transform.parent = m_AI.transform;

                l_AIBuildingManager.AddComponent<Bulding_Manager>();

                Debug.Log("Building Manager Created and added to AI");

                l_AIBuildingManager.GetComponent<Bulding_Manager>().m_SetOwner(CurrentTurn.ai);

                l_AIBuildingManager.GetComponent<Bulding_Manager>().m_SetBasicBuilding(m_AI.GetComponent<Prefab_Loader>().m_ExportPrefabObject("Prefabs/Building Prefabs/TempBuilding", "Basic Building"));

                l_AIBuildingManager.GetComponent<Bulding_Manager>().m_SetTurnManager(m_TurnManager);

                // Spawn starting buldings. 

                l_AIBuildingManager.GetComponent<Bulding_Manager>().m_SpawnHQ(m_LevelManager.GetComponent<Prefab_Loader>().m_GetLoadedObject().GetComponent<Tile_Map_Manager>().m_GetSpawnPoint(CurrentTurn.ai, 0));

                l_AIBuildingManager.GetComponent<Bulding_Manager>().m_SpawnFarm(m_LevelManager.GetComponent<Prefab_Loader>().m_GetLoadedObject().GetComponent<Tile_Map_Manager>().m_GetSpawnPoint(CurrentTurn.ai, 1));

                l_AIBuildingManager.GetComponent<Bulding_Manager>().m_SpawnIronMine(m_LevelManager.GetComponent<Prefab_Loader>().m_GetLoadedObject().GetComponent<Tile_Map_Manager>().m_GetSpawnPoint(CurrentTurn.ai, 2));

                l_AIBuildingManager.GetComponent<Bulding_Manager>().m_SpawnGoldMine(m_LevelManager.GetComponent<Prefab_Loader>().m_GetLoadedObject().GetComponent<Tile_Map_Manager>().m_GetSpawnPoint(CurrentTurn.ai, 3));

                #region Create Resource Manager

                // Create Resource Manager. 

                GameObject l_AiResourceManager = new GameObject
                {
                    name = "Resource Manager"
                };

                l_AiResourceManager.AddComponent<Resource_Management>();

                l_AiResourceManager.transform.parent = m_AI.transform;

                l_AIBuildingManager.GetComponent<Bulding_Manager>().m_SetResourceManager(l_AiResourceManager);

                #endregion
            }
            else
            {
                Debug.LogError("Error 0200-1 - Unable to create object on AI: Building Manager");
            }

            #endregion

            #region Create Unit Manager

            // Add the unit manager to the AI.

            GameObject l_AIUnitManager = new GameObject();

            if (l_AIUnitManager != null)
            {
                l_AIUnitManager.name = "Unit Manager";

                l_AIUnitManager.tag = "Unit_Manager_AI";

                l_AIUnitManager.transform.parent = m_AI.transform;

                // Add components 

                l_AIUnitManager.AddComponent<AI_Unit_Manager>();

                l_AIUnitManager.AddComponent<Unit_Spwaning>();

                Debug.Log("Unit Manager Created and added to AI");

                // Load in basic unit

                l_AIUnitManager.GetComponent<AI_Unit_Manager>().m_SetOwner(CurrentTurn.ai);

                l_AIUnitManager.GetComponent<AI_Unit_Manager>().m_SetTurnManager(m_TurnManager);

                l_AIUnitManager.GetComponent<AI_Unit_Manager>().m_SetOtherManager(m_Player);

                l_AIUnitManager.GetComponent<Unit_Spwaning>().m_SetBaseUnit(m_AI.GetComponent<Prefab_Loader>().m_ExportPrefabObject("Prefabs/Unit Prefabs/Base Unit AI", "Basic Unit AI"));

                #region Spawn Units

                // Spawn Units

                Debug.Log("Spawning Units... ");

                int l_iNumberOfSpawnedUnits = 0;

                foreach (var Cell in l_AIBuildingManager.GetComponent<Bulding_Manager>().m_GetHQObject().GetComponent<Building_Positioning>().m_GetPosition().GetComponent<Cell_Neighbours>().m_GetCellNeighbours())
                { 
                    // The first unit spawned should always be an engineer, allowing for the player to create new buildings from thestart of the game.
                    if (l_iNumberOfSpawnedUnits == 0)
                    {
                        l_AIUnitManager.GetComponent<Unit_Manager>().m_AddUnitIntoList((l_AIUnitManager.GetComponent<Unit_Spwaning>().m_SpawnEngineerUnit(Cell)));
                    }
                    else
                    {
                        l_AIUnitManager.GetComponent<Unit_Manager>().m_AddUnitIntoList((l_AIUnitManager.GetComponent<Unit_Spwaning>().m_SpawnMilitiaUnit(Cell)));
                    }

                    l_iNumberOfSpawnedUnits++;
                }

                Debug.Log(l_iNumberOfSpawnedUnits + " Units have been spawned");

                #endregion
            }
            else
            {
                Debug.LogError("Error 0200-2 - Unable to create object on AI: Unit Manager");
            }

            #endregion

            #region Connect Interface Elements

            Debug.Log("User Interface connection - AI. ");

            if (m_UserInterfaceManager == null)
            {
                m_UserInterfaceManager = GameObject.FindGameObjectWithTag("User_Interface");
            }

            if (m_UserInterfaceManager != null)
            {
                m_AI.GetComponent<Lose_Script>().m_SetGameOverScreen(m_UserInterfaceManager.GetComponent<Interface_Controller>().m_GetGameOverScreen());
            }

            #endregion
        }

        #endregion

        m_bInGame = true; 
    }

    public void m_ResetAllItems()
    {
        #region Delete all of the preveous items

        if (m_Player != null)
        {
            Destroy(m_Player);
        }

        if (m_AI != null)
        {
            Destroy(m_AI);
        }

        m_LevelManager.GetComponentInChildren<Tile_Map_Manager>().m_ResetGrid();

        m_SetInGameBoolean(false);

        #endregion
    }

    public void m_SetInGameBoolean(bool newValue) { m_bInGame = newValue; }

    public bool m_bGetInGame() => m_bInGame; 

    #endregion

    //---------------------------------------------------------------------------------------------------------------------------\\
    // File End
    //---------------------------------------------------------------------------------------------------------------------------\\
}
