using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*! \enum This is an action in the game, one whcih will be controlled by a radial menu. */
[System.Serializable]
public enum Action
{
    Nothing,
    Wait, 
    Attack, 
    Move,
    Build
}

/*! \class This will be used to manage a list of units. */ 
public class Unit_Manager : MonoBehaviour
{
    //----------------------------------------------------------------------------------------------------------------------------
    //  Data Members Start 
    //----------------------------------------------------------------------------------------------------------------------------

    // Other Managers

    [SerializeField]
    protected GameObject m_GameMap; /*!< \var This will be the game object holding the game's map. */

    [SerializeField]
    protected GameObject m_TurnManager; /*!< \var The turn manager for the game, used to check the current turn. */

    // Movement Management.

    [SerializeField]
    bool m_bCheckRange = false; /*!< \var This will be used to check the movement range of the selected unit. */

    // Unit Management.

    [SerializeField]
    protected List<GameObject> m_UnitList = new List<GameObject>(); /*!< \var This will hold all of the controlled units. */

    [SerializeField]
    protected CurrentTurn m_Owner = CurrentTurn.player; /*!< \var The owner ofthis set of units. */

    [SerializeField]
    protected bool m_bResetOnce = false; /*!< \var Used to reset the list of units at the end of the turn. */

    // Action Management. 

    [SerializeField]
    bool m_bActionSelected = false; /*!< \var Used to stop another action from being selected before the action is finished. */

    [SerializeField]
    Action m_Action; /*!< \var The current action which has been selected. */

    //----------------------------------------------------------------------------------------------------------------------------
    //  Member Functions Start 
    //----------------------------------------------------------------------------------------------------------------------------

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

    private void Update()
    {
        if (m_UnitList.Count > 0)
        {
            // Init temp locations for units.
            foreach (var unit in m_UnitList)
            {
                if (unit != null)
                {
                    if (unit.GetComponent<Unit_Movement>().m_GetCurrentPosition() == null)
                    {
                        GameObject l_TempPos = m_GameMap.GetComponent<Tile_Map_Manager>().m_GetRandomCell();

                        if (l_TempPos != null)
                        {
                            if (l_TempPos.GetComponent<Cell_Manager>().m_bcheckForObsticle() == false && l_TempPos.GetComponent<Cell_Manager>().m_bCheckForOccupied() == false)
                            {
                                unit.GetComponent<Unit_Movement>().m_SetPosition(l_TempPos);
                            }
                        }
                    }
                    else if (unit.GetComponent<Unit_Movement>().m_GetCurrentPosition().GetComponent<Cell_Manager>().m_bCheckForOccupied() == false)
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

                                    m_GetSelectedUnit().GetComponent<Unit_Movement>().m_UnitWait();

                                    m_SetActionNull();

                                    break;

                                case Action.Attack:

                                    if (m_GetSelectedUnit().GetComponent<Unit_Attack>().m_GetNumberOfAttacks() > 0)
                                    {
                                        gameObject.GetComponent<Unit_Find_AtTarget1>().m_AtRangeFinder();

                                        gameObject.GetComponent<Unit_Find_AtTarget1>().m_SelectAttackTarget();

                                        if (gameObject.GetComponent<Unit_Find_AtTarget1>().m_GetAtTarget() != null)
                                        {
                                            if (gameObject.GetComponent<Unit_Find_AtTarget1>().m_GetAtTarget().GetComponent<Unit_Attack>() != null)
                                            {
                                                Debug.Log("Unit Combat");

                                                m_UnitAttack();

                                                m_GetSelectedUnit().GetComponent<Unit_Attack>().m_SetNumberOfAttacks(0);
                                            }
                                            else
                                            {
                                                Debug.Log("Building Combat");

                                                m_AttackBuilding();

                                                m_GetSelectedUnit().GetComponent<Unit_Attack>().m_SetNumberOfAttacks(0);
                                            }

                                            m_SetActionNull();
                                        }
                                    }
                                    else
                                    {
                                        m_SetActionNull();
                                    }

                                    break;

                                // Update Unit Position. 
                                case Action.Move:
                                    // Debug.Log("Move");

                                    m_UpdateUnitPosition();
                                    m_GameMap.GetComponent<Tile_Map_Manager>().m_SetSelectable(true);

                                    break;

                                case Action.Build:
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

                    m_bResetOnce = false;
                }
            }
        }
    }

    // This will be ued to check which trun it is. If the turn is the owners.
    public bool m_CheckTurn()
    {
        if(m_Owner == m_TurnManager.GetComponent<Turn_Manager>().m_GetCurrentTurn())
        {
            return true;
        }

        return false; 
    }

    // Unit Management. 

    // This will be used to update the position of the selected unit. 
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

    // This will be used to allow for the selected unit to attack a selected target. 
    void m_UnitAttack()
    {
        if (m_GetSelectedUnit() != null)
        {
            m_GetSelectedUnit().GetComponent<Unit_Attack>().m_AttackTarget(gameObject.GetComponent<Unit_Find_AtTarget1>().m_GetAtTarget());
        }
    }

    void m_AttackBuilding()
    {
        if (m_GetSelectedUnit() != null)
        {
            m_GetSelectedUnit().GetComponent<Unit_Attack>().m_AttackBuildingTarget(gameObject.GetComponent<Unit_Find_AtTarget1>().m_GetAtTarget());
        }
    }

    // This will be used to reset the selected units at the end of the turn. 
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

                // Reset number of attacks
                unit.GetComponent<Unit_Attack>().m_SetNumberOfAttacks(1);

            }
        }
    }

    // This will be used to get the current unit which is selected. 
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

    // This will be used to get the list of units. 
    public List<GameObject> m_GetUnitList() => m_UnitList;

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

    public void m_AddUnitIntoList(GameObject newUnit)
    {
        if(newUnit != null)
        {
            newUnit.transform.parent = gameObject.transform;

            m_UnitList.Add(newUnit);

            Debug.Log("Unit '" + newUnit.name + "' has been added into this manager");
        }
    }

    public void m_SetTurnManager(GameObject turnManager) { m_TurnManager = turnManager; }

    // Action Management. 

    // This will be ued to set the new action.
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

    // This will be ued to set the new action.
    public void m_SetActionWait()
    {
        if (m_GetSelectedUnit() != null)
        {
            if (m_GetSelectedUnit().GetComponent<Unit_Movement>().m_GetCurrentMoveRange() > 0)
            {
                m_Action = Action.Wait;

                gameObject.GetComponent<Activate_Radial_Menu>().m_ActivateMenu(null, false);

                m_bActionSelected = true;

                Debug.Log("Action Selected - Wait");
            }
        }
    }

    // This will be ued to set the new action.
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

    // This will be ued to set the new action.
    public void m_SetActionNull()
    {
        m_Action = Action.Nothing;

        m_bActionSelected = false;

        // Resets the target for attact allowing for a new target to be selected. 
        gameObject.GetComponent<Unit_Find_AtTarget1>().m_SetAtTarget(null); 

        // Debug.Log("Action Selected - Nothing");
    }
}
