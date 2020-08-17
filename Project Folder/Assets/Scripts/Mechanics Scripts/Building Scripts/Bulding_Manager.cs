using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum BuildingToSpawn
{
    None = 0x500,
    Farm = 0x501, 
    IronMine = 0x502,
    GoldMine = 0x503, 
    Barracks = 0x504
}

/// <summary>
/// This will be used to generate and control all building within the game. 
/// </summary>
public class Bulding_Manager : MonoBehaviour
{
    // Data Members Start \\

    /// <summary>
    /// This will be the base building object, It will be given different sprites, scripts and tags to allow it 
    /// to work in different ways. 
    /// </summary>
    [SerializeField]
    GameObject m_BasicBuilding = null;

    /// <summary>
    /// This is the owner of this manager, it will allow for this to operate only on the turn player's turn. 
    /// </summary>
    [SerializeField]
    CurrentTurn m_Owner = CurrentTurn.player;

    [SerializeField]
    BuildingToSpawn m_CurrentBuildingToSPawn = BuildingToSpawn.None;  

    /// <summary>
    /// This is the list of all building objects managed by this controller, they will be operated based on their 
    /// attached tags.
    /// </summary>
    [SerializeField]
    List<GameObject> m_BuildingList = new List<GameObject>();

    /// <summary>
    /// The turn manager will be used to check which turn it is, allowing for operation only on the owener's turn. 
    /// </summary>
    [SerializeField]
    GameObject m_TurnManager = null;

    [SerializeField]
    GameObject m_ResourceManager;

    bool m_bResetOnce = false;

    [SerializeField]
    GameObject m_UnitBuildMenu;

    GameObject m_TileMap; 

    // Member Functions Start \\

    /// <summary>
    /// This will be called regularly on a fixed time basis, the current time scale for the scene will affect the number of 
    /// times this function is called. 
    /// </summary>
    private void FixedUpdate()
    {
        bool l_bCheckForHQDestruction = true;

        bool l_bSpawningBuildingSelected = false; 

        // Update all buildings. 

        foreach (var building in m_BuildingList)
        {
            // Check or HQ present.

            if (building != null)
            {
                if (building.tag == "HQ")
                {
                    l_bCheckForHQDestruction = false;

                    if (building.GetComponent<Select_Building>().m_GetSelected() == true)
                    {
                        if (m_Owner == CurrentTurn.player)
                        {
                            m_UnitBuildMenu.GetComponent<Open_Unit_Spawn_Menu>().m_HQSelected();

                            l_bSpawningBuildingSelected = true;
                        }
                    }
                }
            }
        }

        if (m_UnitBuildMenu != null)
        {
            if (l_bSpawningBuildingSelected == false)
            {
                if (m_UnitBuildMenu.GetComponent<Open_Unit_Spawn_Menu>().m_Visable())
                {
                    Debug.Log("Hiding spawn menu");

                    m_UnitBuildMenu.GetComponent<Open_Unit_Spawn_Menu>().m_HideAll();
                }
            }
        }

        // If there is no Hq in this manager it has been destroyed.

        if (l_bCheckForHQDestruction == true)
        {
            gameObject.GetComponentInParent<Lose_Script>().m_SetGameOver(); 
        }

        if(m_CheckTurn() == true)
        {
            m_bResetOnce = true;

            if(transform.parent.GetComponentInChildren<Unit_Manager>().m_GetSelectedUnit() != null)
            {
                if(m_TileMap.GetComponent<Tile_Map_Manager>().m_GetSelectedCell(false) != null)
                {
                    Debug.Log("Cell Selected");

                    if (m_TileMap.GetComponent<Tile_Map_Manager>().m_GetSelectedCell(false).GetComponent<Cell_Manager>().
                        m_Distance(transform.parent.GetComponentInChildren<Unit_Manager>().m_GetSelectedUnit().GetComponent<Unit_Movement>().m_GetCurrentPosition()) == 1)
                    {
                        Debug.Log("Cell within placing range");

                        if (m_TileMap.GetComponent<Tile_Map_Manager>().m_GetSelectedCell(false).GetComponent<Cell_Manager>().m_CanBeMovedTo())
                        {

                            Debug.Log("Cell Can be moved to");

                            if (m_CurrentBuildingToSPawn != BuildingToSpawn.None)
                            {
                                switch (m_CurrentBuildingToSPawn)
                                {
                                    case BuildingToSpawn.Farm:
                                        m_SpawnFarm(m_TileMap.GetComponent<Tile_Map_Manager>().m_GetSelectedCell(), 15, 10);
                                        break;
                                    case BuildingToSpawn.IronMine:
                                        m_SpawnIronMine(m_TileMap.GetComponent<Tile_Map_Manager>().m_GetSelectedCell(), 15, 10);
                                        break;
                                    case BuildingToSpawn.GoldMine:
                                        m_SpawnGoldMine(m_TileMap.GetComponent<Tile_Map_Manager>().m_GetSelectedCell(), 15, 10);
                                        break;
                                    case BuildingToSpawn.Barracks:

                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                        else
                        {
                            Debug.LogWarning("Unable to move to that location, so a building cannot be placed there.");
                        }
                    }

                    Debug.LogWarning("Cell out of range"); 
                }
            }
        }
        else
        {
            if(m_bResetOnce == true)
            {
                m_bResetOnce = false;

                // Debug.Log("Turn End - Resetting");

                m_EndTurn();
            }
        }
    }
    
    public void m_EndTurn()
    {
        if(m_ResourceManager != null)
        {
            float l_fAddToFood = 0, l_fAddToIron = 0, l_fAddToGold = 0;

            for (int i = 0; i < m_BuildingList.Count; i++)
            {
                if (m_BuildingList[i] != null)
                {
                    switch (m_BuildingList[i].tag)
                    {
                        case "Farm_Building":
                            l_fAddToFood += 15f;
                            break;

                        case "Iron_Mine":
                            l_fAddToIron += 10f;
                            break;

                        case "Gold_Mine":
                            l_fAddToGold += 10f;
                            break;

                        default:
                            Debug.Log("Object doesn't provide any additional resources.");
                            break;
                    }

                    if (m_BuildingList[i].tag != "Gold_Mine")
                    {
                        l_fAddToGold -= 1;
                    }
                }
                else
                {
                    m_BuildingList.RemoveAt(i);

                    i--; 
                }
            }

            m_ResourceManager.GetComponent<Resource_Management>().m_AddToFood(l_fAddToFood);
            m_ResourceManager.GetComponent<Resource_Management>().m_AddToIron(l_fAddToIron);
            m_ResourceManager.GetComponent<Resource_Management>().m_AddToGold(l_fAddToGold);

            if(m_ResourceManager.GetComponent<Resource_Management>().m_GetGold() <= 0)
            {
                m_BuildingList[m_BuildingList.Count - 1].GetComponent<Health_Management>().m_TakeHit(15); 
            }
        }
    }

    /// <summary>
    /// This will set the turn manger for the game, is needed to check for turn. 
    /// </summary>
    /// <param name="turnManager">The current turn manager in this scene. </param>
    public void m_SetTurnManager(GameObject turnManager) { m_TurnManager = turnManager; }

    public void m_SetResourceManager(GameObject resourceManager) { m_ResourceManager = resourceManager; }

    public void m_SetUnitBuildMenu(GameObject unitMenu) { m_UnitBuildMenu = unitMenu; }

    public void m_SetTileMap(GameObject tileMap) { m_TileMap = tileMap; }

    /// <summary>
    /// This will be used to assign the base building for the game, will be needed to assign all buildings 
    /// in the game. 
    /// </summary>
    /// <param name="newPrefab">A prefab object loaded form the resource file, TempBuilding. </param>
    public void m_SetBasicBuilding(GameObject newPrefab) { m_BasicBuilding = newPrefab; }

    /// <summary>
    /// This will set the owner of this building manager. 
    /// </summary>
    /// <param name="newOwner">The new owner for this building.</param>
    public void m_SetOwner(CurrentTurn newOwner) { m_Owner = newOwner; }

    /// <summary>
    /// This will be used to check the current turn.
    /// </summary>
    /// <returns>True if the current turn is equal to the owner.</returns>
    public bool m_CheckTurn()
    {
        if (m_Owner == m_TurnManager.GetComponent<Turn_Manager>().m_GetCurrentTurn())
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Allows access to the full list of buildings controlled by this manager. 
    /// </summary>
    /// <returns>List of buildings. </returns>
    public List<GameObject> m_GetBuildigList() => m_BuildingList; 

    /// <summary>
    /// This will create a new game object, and using the temp building assign it sufficient functionality.
    /// 
    /// There can only be one HQ object and this will be spawned at a specific location on the map upon start-up.
    /// </summary>
    /// <param name="cellToSpawn">A position on the map for this to spawn. </param>
    public void m_SpawnHQ(GameObject cellToSpawn)
    {
        if (m_BasicBuilding != null)
        {
            if (cellToSpawn != null)
            {
                GameObject l_TempHQ = Instantiate(m_BasicBuilding, cellToSpawn.transform.position, Quaternion.identity);

                if (l_TempHQ != null)
                {
                    // Update position. 

                    l_TempHQ.GetComponent<Building_Positioning>().m_SetPosition(cellToSpawn);

                    // Update Hierarchy 

                    l_TempHQ.transform.parent = gameObject.transform;

                    // Change Name. 

                    l_TempHQ.name = "HQ Building";

                    // Assign Tag

                    l_TempHQ.tag = "HQ"; 

                    // Add to list. 

                    m_BuildingList.Add(l_TempHQ);

                    Debug.Log("HQ Spawned");
                }
                else
                {
                    Debug.LogWarning("Unable to spawn HQ - Error instantiating.");
                }
            }
            else
            {
                Debug.LogWarning("Unable to spawn HQ - Spawn location not found."); 
            }
        }
        else
        {
            Debug.LogError("Unable to spawn HQ - Basic building not found."); 
        }
    }

    public void m_SpawnFarm(GameObject cellToSpawn)
    {
        if (m_BasicBuilding != null)
        {
            if (cellToSpawn != null)
            {
                GameObject l_TempFarm = Instantiate(m_BasicBuilding, cellToSpawn.transform.position, Quaternion.identity);

                if(l_TempFarm != null)
                {
                    // Update position. 

                    l_TempFarm.GetComponent<Building_Positioning>().m_SetPosition(cellToSpawn);

                    // Update Hierarchy 

                    l_TempFarm.transform.parent = gameObject.transform;

                    // Change Name. 

                    l_TempFarm.name = "Farm";

                    // Assign Tag

                    l_TempFarm.tag = "Farm_Building";

                    // Update Health of object.

                    l_TempFarm.GetComponent<Health_Management>().m_SetMaxHealth(100);

                    // Add to list. 

                    m_BuildingList.Add(l_TempFarm);
                }
                else
                {
                    Debug.LogWarning("Unable to spawn Farm - Error instantiating.");
                }

            }
            else
            {
                Debug.LogWarning("Unable to spawn Farm - Spawn location not found.");
            }
        }
        else
        {
            Debug.LogError("Unable to spawn Farm - Basic building not found.");
        }
    }

    public void m_SpawnIronMine(GameObject cellToSpawn)
    {
        if (m_BasicBuilding != null)
        {
            if (cellToSpawn != null)
            {
                GameObject l_TempIronMine = Instantiate(m_BasicBuilding, cellToSpawn.transform.position, Quaternion.identity);

                if (l_TempIronMine != null)
                {
                    // Update position. 

                    l_TempIronMine.GetComponent<Building_Positioning>().m_SetPosition(cellToSpawn);

                    // Update Hierarchy 

                    l_TempIronMine.transform.parent = gameObject.transform;

                    // Change Name. 

                    l_TempIronMine.name = "Iron Mine";

                    // Assign Tag

                    l_TempIronMine.tag = "Iron_Mine";

                    // Update Health of object.

                    l_TempIronMine.GetComponent<Health_Management>().m_SetMaxHealth(100);

                    // Add to list. 

                    m_BuildingList.Add(l_TempIronMine);
                }
                else
                {
                    Debug.LogWarning("Unable to spawn Iron Mine - Error instantiating.");
                }

            }
            else
            {
                Debug.LogWarning("Unable to spawn Iron Mine - Spawn location not found.");
            }
        }
        else
        {
            Debug.LogError("Unable to spawn Iron Mine - Basic building not found.");
        }
    }

    public void m_SpawnGoldMine(GameObject cellToSpawn)
    {
        if (m_BasicBuilding != null)
        {
            if (cellToSpawn != null)
            {
                GameObject l_TempGoldMine = Instantiate(m_BasicBuilding, cellToSpawn.transform.position, Quaternion.identity);

                if (l_TempGoldMine != null)
                {
                    // Update position. 

                    l_TempGoldMine.GetComponent<Building_Positioning>().m_SetPosition(cellToSpawn);

                    // Update Hierarchy 

                    l_TempGoldMine.transform.parent = gameObject.transform;

                    // Change Name. 

                    l_TempGoldMine.name = "Gold Mine";

                    // Assign Tag

                    l_TempGoldMine.tag = "Gold_Mine";

                    // Update object health. 

                    l_TempGoldMine.GetComponent<Health_Management>().m_SetMaxHealth(100);

                    // Add to list. 

                    m_BuildingList.Add(l_TempGoldMine);
                }
                else
                {
                    Debug.LogWarning("Unable to spawn Gold Mine - Error instantiating.");
                }

            }
            else
            {
                Debug.LogWarning("Unable to spawn Gold Mine - Spawn location not found.");
            }
        }
        else
        {
            Debug.LogError("Unable to spawn Gold Mine - Basic building not found.");
        }
    }

    public void m_SpawnFarm(GameObject cellToSpawn, float ironCost, float goldCost)
    {
        if (m_BasicBuilding != null)
        {
            if (cellToSpawn != null)
            {
                // Check cost requirements

                if (m_ResourceManager.GetComponent<Resource_Management>().m_CheckIronRequirement(ironCost) &&
                    m_ResourceManager.GetComponent<Resource_Management>().m_CheckGoldRequirement(goldCost))
                {

                    m_ResourceManager.GetComponent<Resource_Management>().m_RemoveFromIron(ironCost);
                    m_ResourceManager.GetComponent<Resource_Management>().m_RemoveFromGold(goldCost);

                    GameObject l_TempFarm = Instantiate(m_BasicBuilding, cellToSpawn.transform.position, Quaternion.identity);

                    if (l_TempFarm != null)
                    {
                        // Update position. 

                        l_TempFarm.GetComponent<Building_Positioning>().m_SetPosition(cellToSpawn);

                        // Update Hierarchy 

                        l_TempFarm.transform.parent = gameObject.transform;

                        // Change Name. 

                        l_TempFarm.name = "Farm";

                        // Assign Tag

                        l_TempFarm.tag = "Farm_Building";

                        // Update Health of object.

                        l_TempFarm.GetComponent<Health_Management>().m_SetMaxHealth(100);

                        // Add to list. 

                        m_BuildingList.Add(l_TempFarm);
                    }
                    else
                    {
                        Debug.LogWarning("Unable to spawn Farm - Error instantiating.");
                    }
                }
                else
                {
                    Debug.LogWarning("Unable to spawn Farm - Not enough resources");
                }
            }
            else
            {
                Debug.LogWarning("Unable to spawn Farm - Spawn location not found.");
            }
        }
        else
        {
            Debug.LogError("Unable to spawn Farm - Basic building not found.");
        }
    }

    public void m_SpawnIronMine(GameObject cellToSpawn, float ironCost, float goldCost)
    {
        if (m_BasicBuilding != null)
        {
            if (cellToSpawn != null)
            {
                if (m_ResourceManager.GetComponent<Resource_Management>().m_CheckIronRequirement(ironCost) &&
                    m_ResourceManager.GetComponent<Resource_Management>().m_CheckGoldRequirement(goldCost))
                {

                    m_ResourceManager.GetComponent<Resource_Management>().m_RemoveFromIron(ironCost);
                    m_ResourceManager.GetComponent<Resource_Management>().m_RemoveFromGold(goldCost);


                    GameObject l_TempIronMine = Instantiate(m_BasicBuilding, cellToSpawn.transform.position, Quaternion.identity);

                    if (l_TempIronMine != null)
                    {
                        // Update position. 

                        l_TempIronMine.GetComponent<Building_Positioning>().m_SetPosition(cellToSpawn);

                        // Update Hierarchy 

                        l_TempIronMine.transform.parent = gameObject.transform;

                        // Change Name. 

                        l_TempIronMine.name = "Iron Mine";

                        // Assign Tag

                        l_TempIronMine.tag = "Iron_Mine";

                        // Update Health of object.

                        l_TempIronMine.GetComponent<Health_Management>().m_SetMaxHealth(100);

                        // Add to list. 

                        m_BuildingList.Add(l_TempIronMine);
                    }
                    else
                    {
                        Debug.LogWarning("Unable to spawn Iron Mine - Error instantiating.");
                    }
                }
                else
                {
                    Debug.LogWarning("Unable to spawn Iron Mine - Not enough resources");
                }
            }
            else
            {
                Debug.LogWarning("Unable to spawn Iron Mine - Spawn location not found.");
            }
        }
        else
        {
            Debug.LogError("Unable to spawn Iron Mine - Basic building not found.");
        }
    }

    public void m_SpawnGoldMine(GameObject cellToSpawn, float ironCost, float goldCost)
    {
        if (m_BasicBuilding != null)
        {
            if (cellToSpawn != null)
            {
                if (m_ResourceManager.GetComponent<Resource_Management>().m_CheckIronRequirement(ironCost) &&
                    m_ResourceManager.GetComponent<Resource_Management>().m_CheckGoldRequirement(goldCost))
                {

                    m_ResourceManager.GetComponent<Resource_Management>().m_RemoveFromIron(ironCost);
                    m_ResourceManager.GetComponent<Resource_Management>().m_RemoveFromGold(goldCost);


                    GameObject l_TempGoldMine = Instantiate(m_BasicBuilding, cellToSpawn.transform.position, Quaternion.identity);

                    if (l_TempGoldMine != null)
                    {
                        // Update position. 

                        l_TempGoldMine.GetComponent<Building_Positioning>().m_SetPosition(cellToSpawn);

                        // Update Hierarchy 

                        l_TempGoldMine.transform.parent = gameObject.transform;

                        // Change Name. 

                        l_TempGoldMine.name = "Gold Mine";

                        // Assign Tag

                        l_TempGoldMine.tag = "Gold_Mine";

                        // Update object health. 

                        l_TempGoldMine.GetComponent<Health_Management>().m_SetMaxHealth(100);

                        // Add to list. 

                        m_BuildingList.Add(l_TempGoldMine);
                    }
                    else
                    {
                        Debug.LogWarning("Unable to spawn Gold Mine - Error instantiating.");
                    }
                }
                else
                {
                    Debug.LogWarning("Unable to spawn Gold Mine - Not enough resources");
                }
            }
            else
            {
                Debug.LogWarning("Unable to spawn Gold Mine - Spawn location not found.");
            }
        }
        else
        {
            Debug.LogError("Unable to spawn Gold Mine - Basic building not found.");
        }
    }

    public void m_SetBuildingToSpawn(BuildingToSpawn nextBuilding)
    {
        m_CurrentBuildingToSPawn = nextBuilding; 
    }

    public GameObject m_GetSpawnPointForNewSpawnedUnit()
    {
        foreach (var building in m_BuildingList)
        {
            if(building.GetComponent<Select_Building>().m_GetSelected())
            {
                Debug.Log("Found seleted building."); 

                if(building.tag == "HQ")
                {
                    foreach (var cell in building.GetComponent<Building_Positioning>().m_GetPosition().GetComponent<Cell_Neighbours>().m_GetCellNeighbours())
                    {
                        if (cell != null)
                        {
                            if (cell.GetComponent<Cell_Manager>().m_CanBeMovedTo() == true)
                            {
                                return cell;
                            }
                        }
                    }

                    Debug.LogWarning("Cell neighbours cannot be moved to thus a unit cannot be spawned there."); 
                }
                else
                {
                    Debug.LogWarning("Unble to spawn at this building");
                }
            }
        }

        return null;
    }

    /// <summary>
    /// This will allow access to the building tagged as HQ. 
    /// </summary>
    /// <returns>The HQ object. </returns>
    public GameObject m_GetHQObject()
    {
        foreach (var building in m_BuildingList)
        {
            if(building.tag == "HQ")
            {
                return building;
            }
        }

        Debug.LogWarning("Unable to find HQ item, check the list to see if it exists and contains the tag 'HQ'.");

        return null;
    }

    /// <summary>
    /// This will return the object with the lowest combat rating, with the scaler of the distance 
    /// from the unit of focus as a basis, allowng for the distance between object to have an effect. 
    /// </summary>
    /// <param name="unitOfFocus">The current unit looking for a target. </param>
    /// <returns>The closest building for the unit of focus.</returns>
    public GameObject m_GetLowestCombatRating(GameObject unitOfFocus)
    {
        if (unitOfFocus != null)
        {
            GameObject l_Targetbuilding = null;

            foreach (var building in m_BuildingList)
            {
                if (building != null)
                {
                    // Start by getting the first building which isn't a null object. 

                    l_Targetbuilding = building;

                    break;
                }
            }

            if (l_Targetbuilding != null)
            {
                // Start by calculating the combat raing for the first building. 

                l_Targetbuilding.GetComponent<Building_Combat_Rating>().m_GetCombatRating();

                float l_fPrevRating, l_fNewRating;

                float l_fCurrDist = l_Targetbuilding.GetComponent<Building_Positioning>().m_GetPosition().GetComponent<Cell_Manager>().m_Distance(unitOfFocus.GetComponent<Unit_Movement>().m_GetCurrentPosition());

                // Using the distance we can judge the viability of this target. 

                l_fPrevRating = l_Targetbuilding.GetComponent<Building_Combat_Rating>().m_GetCombatRating() + (l_fCurrDist * 10);

                l_Targetbuilding.GetComponent<Building_Combat_Rating>().m_SetCombatRating(l_fPrevRating);

                foreach (var building in m_BuildingList)
                {
                    if (building != null)
                    {
                        if (building.GetComponent<Building_Positioning>().m_GetPosition().GetComponent<Cell_Neighbours>().m_NeighboursFree())
                        {

                            l_fCurrDist = building.GetComponent<Building_Positioning>().m_GetPosition().GetComponent<Cell_Manager>().m_Distance(unitOfFocus.GetComponent<Unit_Movement>().m_GetCurrentPosition());

                            l_fNewRating = building.GetComponent<Building_Combat_Rating>().m_GetBaseCombatRating() + (l_fCurrDist * 10);

                            building.GetComponent<Building_Combat_Rating>().m_SetCombatRating(l_fNewRating);

                            // Debug.Log(unit.name + " has a combat rating of " + l_fNewRating);

                            if (l_fNewRating < l_fPrevRating)
                            {
                                l_fPrevRating = l_fNewRating;

                                l_Targetbuilding = building;

                                // Debug.Log("New Unit Selected"); 
                            }
                        }
                        else
                        {
                            Debug.Log("Unable to move to this position"); 
                        }
                    }
                }

                return l_Targetbuilding;
            }
        }

        return null;
    }

}
