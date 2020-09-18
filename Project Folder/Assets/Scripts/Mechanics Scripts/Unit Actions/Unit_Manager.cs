using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region Enums 

/// <summary>
/// This will be used to set and check which action the player would like to perform at a point in the game. 
/// </summary>
[System.Serializable]
public enum Action
{
    Nothing,
    Wait, 
    Attack, 
    Move,
    Build
}

#endregion

/// <summary>
/// This class will be used to create, control and maintain all of the units a player might have at any given point in the game. 
/// </summary>
public class Unit_Manager : MonoBehaviour
{

    #region Data Members 

    #region Other Managers

    /// <summary>
    /// The game map will control the grid and cell positioning for the game map, this allows for this manager 
    /// to properly position it's units and properly interface with the game map. 
    /// </summary>
    [SerializeField]
    protected GameObject m_GameMap; 

    /// <summary>
    /// The turn manager will be used to check the current turn within the game and allow for this managr to limit 
    /// the actions of its controlled units to ensure they cannot be controlled out of turn. 
    /// </summary>
    [SerializeField]
    protected GameObject m_TurnManager;

    #endregion

    #region Movement Management

    /// <summary>
    /// This will be used to check the movement range for the active unit. This will say to set the colour of the map tiles 
    /// the unit can move to. This will limit that assignemnt process to only once when the unit is first selected and will 
    /// be reset when this is either deselected or a new one is selected. 
    /// </summary>
    [SerializeField]
    bool m_bCheckRange = false;

    #endregion

    #region Unit Management

    /// <summary>
    /// This will hold the full list of the units controlled by this player for this game. The main functionality of this class     
    /// will be to manipulate units within this list. 
    /// </summary>
    [SerializeField]
    protected List<GameObject> m_UnitList = new List<GameObject>(); 

    /// <summary>
    /// This is the current owner of this unit manager, it will determine which turn the controlled units will be able to act. 
    /// </summary>
    [SerializeField]
    protected CurrentTurn m_Owner = CurrentTurn.player; 

    /// <summary>
    /// This will allow for the units to be reset at the end of the turn, this will only need to happen once and at the start of 
    /// the next turn it will be reset allowing for it to occur again at the end of the turn. 
    /// </summary>
    [SerializeField]
    protected bool m_bResetOnce = false;

    #endregion

    #region Action Management

    /// <summary>
    /// This will allow for this manager to determine when an action has been selected for a unit. This will then allow for that 
    /// action to be processed. 
    /// </summary>
    [SerializeField]
    bool m_bActionSelected = false; 

    /// <summary>
    /// The current action which should be processed, the list includes: Wait, Move, Attack and build. each with their own functionality.
    /// </summary>
    [SerializeField]
    Action m_Action;

    [SerializeField]
    GameObject m_CombatMenu; 

    #endregion

    #endregion

    //----------------------------------------------------------------------------------------------------------------------------
    //  Member Functions Start 
    //----------------------------------------------------------------------------------------------------------------------------

    #region Member Functions

    #region Start-up and Update

    /// <summary>
    /// When this script is first instantiated this will be called. 
    /// </summary>
    void Start()
    {
        // If the game objects are not set find the objects through a assigned tag. 
        m_GameMap = GameObject.FindGameObjectWithTag("Map");

        if(m_GameMap == null)
        {
            Debug.LogError("Error code 0100-3 - Unable to assign game object in game: " + "Game Map. ");
        }

        m_TurnManager = GameObject.FindGameObjectWithTag("Turn_Manager"); 

        if(m_TurnManager == null)
        {
            Debug.LogError("Error code 0100-0 - Unable to assign game object in game: " + "Turn Manager. ");
        }
    }

    /// <summary>
    /// This will be called regularly every frame.
    /// </summary>
    private void Update()
    {
        if (m_UnitList.Count > 0)
        {
            foreach (var unit in m_UnitList)
            {
                if (unit != null)
                {
                    // Ensure the unit's current position cannot be moved to by another unit. 

                    if (unit.GetComponent<Unit_Movement>().m_GetCurrentPosition() != null && unit.GetComponent<Unit_Movement>().m_GetCurrentPosition().GetComponent<Cell_Manager>().m_bCheckForOccupied() == false)
                    {
                        unit.GetComponent<Unit_Movement>().m_GetCurrentPosition().GetComponent<Cell_Manager>().m_bSetOccupied(true);
                    }
                }
            }

            // This will check if the over of the unit manager is the same as current turn player. 
            if (m_CheckTurn())
            {
                // Debug.Log("Player's Turn"); 

                GameObject l_SelectedUnit = m_GetSelectedUnit();

                if (gameObject.GetComponent<Activate_Radial_Menu>() != null)
                {
                    if (l_SelectedUnit != null)
                    {
                        if (m_bActionSelected != true)
                        {
                            gameObject.GetComponent<Activate_Radial_Menu>().m_ActivateMenu(l_SelectedUnit, true);

                            m_GameMap.GetComponent<Tile_Map_Manager>().m_SetSelectable(false);
                        }
                        else
                        {
                            switch (m_Action)
                            {
                                case Action.Wait:
                                    #region Attack Action
                                    m_GetSelectedUnit().GetComponent<Unit_Movement>().m_UnitWait();

                                    m_SetActionNull();
                                    #endregion
                                    break;

                                case Action.Attack:
                                    #region Attack Action
                                    if (m_GetSelectedUnit().GetComponent<Unit_Attack>().m_GetNumberOfAttacks() > 0)
                                    {
                                        gameObject.GetComponent<Unit_Find_AtTarget1>().m_AtRangeFinder();

                                        gameObject.GetComponent<Unit_Find_AtTarget1>().m_SelectAttackTarget();

                                        if (gameObject.GetComponent<Unit_Find_AtTarget1>().m_GetAtTarget() != null)
                                        {
                                            if (gameObject.GetComponent<Unit_Find_AtTarget1>().m_GetAtTarget().GetComponent<Unit_Attack>() != null)
                                            {
                                                Debug.Log("Unit Combat");

                                                if (m_CombatMenu.GetComponent<Manage_Combat_Screen>().m_CheckVisability() == false)
                                                {
                                                    m_CombatMenu.GetComponent<Manage_Combat_Screen>().m_ShowMenu(true);

                                                    m_CombatMenu.GetComponent<Manage_Combat_Screen>().m_DisplayPlayerStats(m_GetSelectedUnit());

                                                    m_CombatMenu.GetComponent<Manage_Combat_Screen>().m_DisplayAIStats(gameObject.GetComponent<Unit_Find_AtTarget1>().m_GetAtTarget());

                                                    bool l_bPlayerAdvantage = m_GetSelectedUnit().GetComponent<Unit_Attack>().m_CheckForAdvantage(gameObject.GetComponent<Unit_Find_AtTarget1>().m_GetAtTarget().GetComponent<Unit_Attack>().m_GetUnitType()), 
                                                        l_bAiAdvantage = gameObject.GetComponent<Unit_Find_AtTarget1>().m_GetAtTarget().GetComponent<Unit_Attack>().m_CheckForAdvantage(m_GetSelectedUnit().GetComponent<Unit_Attack>().m_GetUnitType());

                                                    m_CombatMenu.GetComponent<Manage_Combat_Screen>().m_DisplayCombatBreakdown(m_GetSelectedUnit(), gameObject.GetComponent<Unit_Find_AtTarget1>().m_GetAtTarget(), l_bPlayerAdvantage, l_bAiAdvantage);
                                                }

                                                // Todo Check combat confirmation. 
                                            }
                                            else
                                            {
                                                Debug.Log("Building Combat");

                                                // Todo Spawn combat screen. 

                                                // Todo Check combat confirmation. 

                                                m_AttackBuilding();

                                                m_GetSelectedUnit().GetComponent<Unit_Attack>().m_SetNumberOfAttacks(0);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        m_SetActionNull();
                                    }
                                    #endregion
                                    break;

                                case Action.Move:
                                    #region Move Action 
                                    // Debug.Log("Move");

                                    m_UpdateUnitPosition();
                                    m_GameMap.GetComponent<Tile_Map_Manager>().m_SetSelectable(true);
                                    #endregion
                                    break;

                                case Action.Build:
                                    #region Build Action 

                                    

                                    #endregion
                                    break;

                                default:
                                    m_bActionSelected = false;
                                    break;
                            }
                        }
                    }
                    else
                    {
                        m_SetActionNull();
                        gameObject.GetComponent<Activate_Radial_Menu>().m_ActivateMenu(null, false);

                        m_GameMap.GetComponent<Tile_Map_Manager>().m_ResetCellColours();
                        m_bCheckRange = false;

                    }
                }
                else
                {
                    m_SetActionNull(); 
                }

                m_bResetOnce = true;
            }
            else
            {
                // If it is not the owner's turn reset the units once. 

                if (m_bResetOnce == true)
                {
                    m_ResetUnits();

                    m_FoodConsumption(); 

                    m_bResetOnce = false;
                }
            }
        }
    }

    /// <summary>
    /// This will be used to assign the current combat menu into this class.
    /// </summary>
    /// <param name="combatMenu">The current combat UI menu. </param>
    public void m_AssignCombatMenu(GameObject combatMenu) { m_CombatMenu = combatMenu; }

    #endregion

    #region Owner/Turn Management

    /// <summary>
    /// This will be used to check if this is the owner's turn. 
    /// </summary>
    /// <returns>True if owner == turn</returns>
    public bool m_CheckTurn()
    {
        if(m_Owner == m_TurnManager.GetComponent<Turn_Manager>().m_GetCurrentTurn())
        {
            return true;
        }

        return false; 
    }

    /// <summary>
    /// This will be used to assign the turn manager into this class, this will allow for this class to perform its main actions 
    /// during the correct turns. 
    /// </summary>
    /// <param name="turnManager">The turn manager currently within the game. </param>
    public void m_SetTurnManager(GameObject turnManager) { m_TurnManager = turnManager; }

    #endregion

    #region Unit Management

    /// <summary>
    /// This will be used to update a single unit's position on the game map. This is activated using the movement action.
    /// </summary>
    void m_UpdateUnitPosition()
    {
        int l_iResetLoop = 0;

        // Update the position of the current selected unit. 
        if (m_GetSelectedUnit() != null)
        {
            if (m_GetSelectedUnit().GetComponent<Unit_Movement>().m_GetCurrentPosition() != null)
            {
                // If the unit can still has movemet points they can still move. 
                if (m_GetSelectedUnit().GetComponent<Unit_Movement>().m_GetCurrentMoveRange() > 0)
                {
                    // If the range hasn't been checked for this unit, check their movement range. 
                    if (m_bCheckRange == false)
                    {
                        m_GameMap.GetComponent<Tile_Map_Manager>().m_CheckCellRange(m_GetSelectedUnit().GetComponent<Unit_Movement>().m_GetCurrentMoveRange(),
                            m_GetSelectedUnit().GetComponent<Unit_Movement>().m_GetCurrentPosition());

                        m_bCheckRange = true;
                    }

                    // Store the selected cell locally to check it exists. 
                    GameObject l_TempPosition = m_GameMap.GetComponent<Tile_Map_Manager>().m_GetSelectedCell();

                    // If the cell exists. 
                    if (l_TempPosition != null)
                    {
                        // Check that the unit can move to the destination.

                        if (l_TempPosition.GetComponent<Cell_Manager>().m_bcheckForObsticle() == false && l_TempPosition.GetComponent<Cell_Manager>().m_bCheckForOccupied() == false)
                        { 
                            // Check the distance between the current cell and the new cell. 

                            int l_DistToTarget = m_GetSelectedUnit().GetComponent<Unit_Movement>().m_GetCurrentPosition().GetComponent<Cell_Manager>().m_Distance(l_TempPosition);

                            // If the cell is within rage the unit will move towards it. 

                            GameObject l_PreveousCell = m_GetSelectedUnit().GetComponent<Unit_Movement>().m_GetCurrentPosition(); 

                            if (m_GetSelectedUnit().GetComponent<Unit_Movement>().m_UpdateUnitPosition(l_TempPosition, l_DistToTarget) == true)
                            {
                                // If the unit has moved reset the cells back to their origional state. 

                                l_PreveousCell.GetComponent<Cell_Manager>().m_bSetOccupied(false);

                                m_GameMap.GetComponent<Tile_Map_Manager>().m_ResetCellColours();

                                m_bCheckRange = false;

                                m_SetActionNull();
                            }
                        }
                    }
                    else
                    {
                        l_iResetLoop++;

                        if (l_iResetLoop >= 100)
                        {
                            m_bCheckRange = true;
                        }
                    }
                }
            }
        }

        // Assign a new position to a unit without a current position. 
        if (m_GetSelectedUnit().GetComponent<Unit_Movement>().m_GetCurrentPosition() == null)
        {
            GameObject l_TempPos = m_GameMap.GetComponent<Tile_Map_Manager>().m_GetRandomCell();

            if (l_TempPos != null)
            {
                m_GetSelectedUnit().GetComponent<Unit_Movement>().m_SetPosition(l_TempPos);
            }
        }

    }

    /// <summary>
    /// This will be used to allow for the unit to attack another enemy object within the game. This is activated with the 
    /// attack action. This variation allow for this unit to attack another unit.
    /// </summary>
    public void m_UnitAttack()
    {
        if (m_GetSelectedUnit() != null)
        {
            m_GetSelectedUnit().GetComponent<Unit_Attack>().m_AttackTarget(gameObject.GetComponent<Unit_Find_AtTarget1>().m_GetAtTarget());
        }
    }

    /// <summary>
    /// This will be used to allow for the unit to attack another enemy object within the game. This is activated with the 
    /// attack action. This variation allow for this unit to attack a building object.
    /// </summary>
    void m_AttackBuilding()
    {
        if (m_GetSelectedUnit() != null)
        {
            m_GetSelectedUnit().GetComponent<Unit_Attack>().m_AttackBuildingTarget(gameObject.GetComponent<Unit_Find_AtTarget1>().m_GetAtTarget());
        }
    }

    /// <summary>
    /// This will happen once at the start of the opponent's turn, it will reset all of the units allowing for them to act again 
    /// on their next turn. 
    /// </summary>
    void m_ResetUnits()
    {
        // Reset Map

        // Reset the map cells allowing for proper move representation. 
        m_GameMap.GetComponent<Tile_Map_Manager>().m_ResetCellColours();
        m_GameMap.GetComponent<Tile_Map_Manager>().m_ResetOccupied(); 

        // Reset Actions

        m_SetActionNull();

        // Reset Attack Target; 
        if (gameObject.GetComponent<Unit_Find_AtTarget1>())
        {
            gameObject.GetComponent<Unit_Find_AtTarget1>().m_SetAtTarget(null);
        }

        foreach (var unit in m_UnitList)
        {
            if (unit != null)
            {
                // Reset Unit

                // Deselect Unit
                unit.GetComponent<Unit_Active>().m_SetUnitActive(false);

                // Reset Unit's Movememt points allowing for new movement.
                unit.GetComponent<Unit_Movement>().m_ResetUsedPoints();

                // Reset number of attacks.
                unit.GetComponent<Unit_Attack>().m_SetNumberOfAttacks(1);

                // Reset all waiting units. 
                unit.GetComponent<Unit_Active>().m_SetWaiting(false);
            }
        }
    }

    /// <summary>
    /// This will be used consume food at the end of the turn, it will use a set variable multiplied by the number of 
    /// units the player currently controls and will minus that from their food resource. This will act as a limit on
    /// the number of units the player can own at a single time.
    /// </summary>
    void m_FoodConsumption()
    {
        GameObject l_ResourceManager = gameObject.transform.parent.GetComponentInChildren<Resource_Management>().gameObject;

        if(l_ResourceManager != null)
        {
            const float l_fConsumption = -3f;

            foreach (var unit in m_UnitList)
            {
                if (unit != null)
                {
                    if (l_ResourceManager.GetComponent<Resource_Management>().m_GetFood() <= 2)
                    {
                        unit.GetComponent<Health_Management>().m_TakeHit(5);
                    }

                    l_ResourceManager.GetComponent<Resource_Management>().m_AddToFood(l_fConsumption);
                }
            }
        }
    }

    /// <summary>
    /// This will allow for the manipulation of the unit selected by the player, there can only be one at a single time. 
    /// </summary>
    /// <returns>The unit the player has currently selected with the mouse. </returns>
    public GameObject m_GetSelectedUnit()
    {
        foreach (var unit in m_UnitList)
        {
            if (unit != null)
            {
                if (unit.GetComponent<Unit_Active>().m_GetActiveUnit() == true)
                {
                    return unit;
                }
            }
        }

        return null; 
    }

    /// <summary>
    /// This will be used to find the selected unit and deselect them. 
    /// </summary>
    public void m_DeselectSelectedUnit()
    {
        m_GetSelectedUnit().GetComponent<Unit_Active>().m_SetUnitActive(false); 
    }

    /// <summary>
    /// This will allow access to the current list of all units the player currently controls. 
    /// </summary>
    /// <returns>The List held within this class with the list of all the GameObjects classed as Units. </returns>
    public List<GameObject> m_GetUnitList() => m_UnitList;

    /// <summary>
    /// This will return the unit with the combat rating, the combat rating is the summation of thier current stats
    /// and this will allow for the AI to decide upon a target. 
    /// </summary>
    /// <param name="unitOfFocus">This is the AI's unit which they are currently controlling, this will allow for the 
    /// AI to include distance between targets in their calculations, meaning they will most likely choose a target
    /// within movement range to attack. </param>
    /// <returns>The unit with the lowest combat rating and thus the attack target for the AI. </returns>
    public GameObject m_GetLowestCombatRating(GameObject unitOfFocus)
    {
        if (unitOfFocus != null)
        {
            GameObject l_TargetUnit = null;

            foreach (var unit in m_UnitList)
            {
                if(unit != null)
                {
                    l_TargetUnit = unit;

                    break;
                }
            }

            if (l_TargetUnit != null)
            {
                l_TargetUnit.GetComponent<Unit_Attack>().m_CalculateCombatRating();

                float l_fPrevRating, l_fNewRating;

                float l_fCurrDist = l_TargetUnit.GetComponent<Unit_Movement>().m_GetCurrentPosition().GetComponent<Cell_Manager>().m_Distance(unitOfFocus.GetComponent<Unit_Movement>().m_GetCurrentPosition());

                l_fPrevRating = l_TargetUnit.GetComponent<Unit_Attack>().m_GetCombatRating() + (l_fCurrDist * 10);

                l_TargetUnit.GetComponent<Unit_Attack>().m_SetCombatRating(l_fPrevRating);

                foreach (var unit in m_UnitList)
                {
                    if (unit != null)
                    {
                        unit.GetComponent<Unit_Attack>().m_CalculateCombatRating();

                        l_fCurrDist = unit.GetComponent<Unit_Movement>().m_GetCurrentPosition().GetComponent<Cell_Manager>().m_Distance(unitOfFocus.GetComponent<Unit_Movement>().m_GetCurrentPosition());

                        l_fNewRating = unit.GetComponent<Unit_Attack>().m_GetCombatRating() + (l_fCurrDist * 10);

                        unit.GetComponent<Unit_Attack>().m_SetCombatRating(l_fNewRating);

                        // Debug.Log(unit.name + " has a combat rating of " + l_fNewRating);

                        if (l_fNewRating < l_fPrevRating)
                        {
                            l_fPrevRating = l_fNewRating;

                            l_TargetUnit = unit;

                            // Debug.Log("New Unit Selected"); 
                        }
                    }
                }

                return l_TargetUnit;
            }
        }

        return null; 
    }

    /// <summary>
    /// This will allow for the addition of a new Unit into the unit list, this is needed for the spawning process. If the unit
    /// is not added into the controller's unit list the unit will not be able to be controlled by any player.
    /// </summary>
    /// <param name="newUnit">The new Unit being spawned. </param>
    public void m_AddUnitIntoList(GameObject newUnit)
    {
        if(newUnit != null)
        {
            newUnit.transform.parent = gameObject.transform;

            m_UnitList.Add(newUnit);

            Debug.Log("Unit '" + newUnit.name + "' has been added into this manager");
        }
    }

    #endregion

    #region Action Management

    /// <summary>
    /// This will be used to set the move action, allowing for the unit to move up to it's movement range. 
    /// </summary>
    public void m_SetActionMove()
    {
        if (m_GetSelectedUnit() != null)
        {
            if (m_GetSelectedUnit().GetComponent<Unit_Movement>().m_GetCurrentMoveRange() > 0)
            {
                m_Action = Action.Move;

                gameObject.GetComponent<Activate_Radial_Menu>().m_ActivateMenu(null, false);

                m_bActionSelected = true;

                Debug.Log("Action Selected - Move");
            }
        }
    }

    /// <summary>
    /// This will be used to set the wait action, this will set all of it's other properties to 0 ending it's turn 
    /// and preventing them from doing anything else. 
    /// </summary>
    public void m_SetActionWait()
    {
        if (m_GetSelectedUnit() != null)
        {
            m_GetSelectedUnit().GetComponent<Unit_Active>().m_SetWaiting(true);

            if (m_GetSelectedUnit().GetComponent<Unit_Movement>().m_GetCurrentMoveRange() > 0)
            {
                m_Action = Action.Wait;

                gameObject.GetComponent<Activate_Radial_Menu>().m_ActivateMenu(null, false);

                m_bActionSelected = true;

                Debug.Log("Action Selected - Wait");
            }
        }
    }

    /// <summary>
    /// This will set the attack action, following this an attack target will be needed for the action to be completed. 
    /// </summary>
    public void m_SetActionAttack()
    {
        if (m_GetSelectedUnit() != null)
        {
            m_Action = Action.Attack;

            gameObject.GetComponent<Activate_Radial_Menu>().m_ActivateMenu(null, false);

            m_bActionSelected = true;

            Debug.Log("Action Selected - Attack");
        }
    }

    /// <summary>
    /// This will be used to reset all of the actions, allowing for the transition between action. 
    /// </summary>
    public void m_SetActionNull()
    {
        m_Action = Action.Nothing;

        m_bActionSelected = false;

        // Resets the target for attact allowing for a new target to be selected. 
        gameObject.GetComponent<Unit_Find_AtTarget1>().m_SetAtTarget(null);

        // Debug.Log("Action Selected - Nothing");

        // Close Combat menu if open

        m_CombatMenu.GetComponent<Manage_Combat_Screen>().m_ShowMenu(false);

        // Close build menu if open. 

        if (gameObject.GetComponent<Activate_Build_Menu>().m_ActiveBuildMenu())
        {
            gameObject.GetComponent<Activate_Build_Menu>().m_SetBuildMenu(false);
        }
    }

    /// <summary>
    /// This will be used to set the building action, allowing for engineers and other similar units to build new buildings.
    /// </summary>
    public void m_SetActionBuild()
    {
        if (m_GetSelectedUnit() != null)
        {
            // Update Selected action. 
            m_Action = Action.Build;

            // Hide Action menu. 
            gameObject.GetComponent<Activate_Radial_Menu>().m_ActivateMenu(null, false);

            m_bActionSelected = true;

            Debug.Log("Action Selected - Build");

            // Show Build Menu

            gameObject.GetComponent<Activate_Build_Menu>().m_SetBuildMenu(true); 
        }
    }

    /// <summary>
    /// This will be used to check which action a unit is able to perform, this is to disable buttons within the game.  
    /// </summary>
    /// <param name="actionToCheck">An action with a corrisponding button in the game to check if it needs to be disabled. </param>
    /// <returns></returns>
    public bool m_CheckAction(Action actionToCheck)
    {
        if (m_GetSelectedUnit() != null)
        {
            switch (actionToCheck)
            {
                case Action.Nothing:
                    return false;

                case Action.Wait:
                    return true;

                case Action.Attack:

                    if (m_GetSelectedUnit().GetComponent<Unit_Attack>().m_GetNumberOfAttacks() > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case Action.Move:

                    if (m_GetSelectedUnit().GetComponent<Unit_Movement>().m_GetCurrentMoveRange() > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case Action.Build:

                    if (m_GetSelectedUnit().GetComponent<Unit_Attack>().m_GetUnitType() == UnitType.Engineer)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                default:
                    return false;
            }
        }

        return false; 
    }

    #endregion

    #endregion
}
