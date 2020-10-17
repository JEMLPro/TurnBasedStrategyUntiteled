using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Unit_Manager : Unit_Manager
{
    /// <summary>
    /// The player's object manager, allows for access to their manages items, like buildings and units. 
    /// </summary>
    [SerializeField] GameObject m_OtherManger; 


    /// <summary>
    /// This will be the unit of focus for the AI, using this object they will slowly loop through their list of units until 
    /// all units have acted on this turn. 
    /// </summary>
    [SerializeField] GameObject m_ActiveUnit = null;

    //----------------------------------------------------------------------------------------------------------------------------
    //  Member Functions Start 
    //----------------------------------------------------------------------------------------------------------------------------

    private void Update()
    {
        foreach (var unit in m_UnitList)
        {
            if (unit != null)
            {
                // Ensure that all of the current positions for the managed units cannot be moved to.

                if (unit.GetComponent<Unit_Movement>().m_GetCurrentPosition().GetComponent<Cell_Manager>().m_bCheckForOccupied() == false)
                {
                    unit.GetComponent<Unit_Movement>().m_GetCurrentPosition().GetComponent<Cell_Manager>().m_bSetOccupied(true);
                }
            }
        }

        if (m_CheckTurn())
        {
            // Start of AI Turn

            // Debug.Log("AI Turn");

            if (m_ActiveUnit != null)
            {
                // Find enemy target.

                GameObject l_Target = m_FindTarget();

                //-------------------------------------------------------
                // Unit Move.

                if (l_Target != null)
                {
                    // This will check which set of movement code will need to be used. 

                    if (l_Target.GetComponent<Unit_Movement>())
                    {
                        // Debug.Log("Moving to unit"); 

                        m_MoveToUnit(l_Target);
                    }
                    else
                    {
                        m_MoveToBuilding(l_Target);
                    }

                    //------------------------------------------------------
                    // Unit Attack. 

                    if(l_Target.GetComponent<Unit_Movement>())
                    {
                        m_AttackUnit(l_Target);
                    }
                    else
                    {
                        m_AttackBuiding(l_Target); 
                    }

                    //-------------------------------------------------------
                    // Unit Build.
                    //-------------------------------------------------------


                    // Spawn new items/Update objects. 

                    m_ActiveUnit = null;

                    // End Turn. 

                    m_bResetOnce = true;
                }
                else
                {
                    Debug.LogWarning("Unable to find target unit. ");
                }
            }
            else
            {
                m_ActiveUnit = m_ActivateUnit();

                if (m_ActiveUnit == null)
                {
                    m_TurnManager.GetComponent<Turn_Manager>().m_SwitchTurn();
                }
            }
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

    /// <summary>
    /// This will check both the unit and building manager on the player object. 
    /// </summary>
    /// <returns>The object with the lowest total combat rating + distance. </returns>
     GameObject m_FindTarget()
    {
        // Check Units first. 

        GameObject l_TargetUnit = m_OtherManger.GetComponentInChildren<Unit_Manager>().m_GetLowestCombatRating(m_ActiveUnit);

        // Debug.Log("Target unit found, their rating is : " + l_TargetUnit.GetComponent<Unit_Attack>().m_GetCombatRating());

        // Then Check buildigs 

        GameObject l_TargetBuilding = m_OtherManger.GetComponentInChildren<Bulding_Manager>().m_GetLowestCombatRating(m_ActiveUnit);

        // Debug.Log("Target building found, their rating is : " + l_TargetBuilding.GetComponent<Building_Combat_Rating>().m_GetCombatRating());

        if (l_TargetBuilding != null && l_TargetUnit != null)
        {
            if (l_TargetUnit.GetComponent<Unit_Attack>().m_GetCombatRating() < l_TargetBuilding.GetComponent<Building_Combat_Rating>().m_GetCombatRating())
            {
                Debug.Log("Next target is a unit.");

                return l_TargetUnit;
            }
            else
            {
                Debug.Log("Next target is a building.");

                return l_TargetBuilding;
            }
        }
        else if(l_TargetBuilding != null)
        {
            return l_TargetBuilding;
        }
        else if(l_TargetUnit != null)
        {
            return l_TargetUnit;
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// This will be used to move the active unit towards another unit. 
    /// </summary>
    /// <param name="targetUnit">Requires a unit object to act as a target. </param>
    private void m_MoveToUnit(GameObject targetUnit)
    {
        if (m_ActiveUnit.GetComponent<Unit_Movement>().m_GetCurrentPosition().GetComponent<Cell_Manager>().m_Distance(targetUnit.GetComponent<Unit_Movement>().m_GetCurrentPosition()) > 1)
        {
            if (m_ActiveUnit.GetComponent<Unit_Movement>().m_GetCurrentMoveRange() > 0)
            {
                // Set path finding requirements. 

                if (m_ActiveUnit.GetComponent<AI_Unit_Movement>().m_bPathReached == false)
                {
                    if (m_ActiveUnit.GetComponent<AI_Unit_Movement>().m_bPathReached == false)
                    {
                        Debug.Log("Setting variables");

                        // Check the target unit's position for a free space next to it. 

                        GameObject l_TargetPosition = targetUnit.GetComponent<Unit_Movement>().m_GetCurrentPosition().GetComponent<Cell_Neighbours>().m_GetClosestNeighbour(m_ActiveUnit.GetComponent<AI_Unit_Movement>().m_GetCurrentPosition());

                        if (l_TargetPosition != null)
                        {
                            m_ActiveUnit.GetComponent<AI_Unit_Movement>().m_SetStartandEndPoints(l_TargetPosition);
                        }
                        else
                        {
                            // If a target position cannot be found (if the target is surrounded for example), wait for this turn. 

                            m_ActiveUnit.GetComponent<AI_Unit_Movement>().m_UnitWait();
                        }
                    }
                }
            }
        }
        else
        {
            Debug.Log("No need to move");

            m_ActiveUnit.GetComponent<AI_Unit_Movement>().m_UnitWait();
        }
    }

    /// <summary>
    /// This will be used to move the active unit towards a building object. 
    /// </summary>
    /// <param name="targetBuilding">Requires a building object to act as a target. </param>
    private void m_MoveToBuilding(GameObject targetBuilding)
    {
        if (m_ActiveUnit.GetComponent<Unit_Movement>().m_GetCurrentPosition().GetComponent<Cell_Manager>().m_Distance(targetBuilding.GetComponent<Building_Positioning>().m_GetPosition()) > 1)
        {
            if (m_ActiveUnit.GetComponent<Unit_Movement>().m_GetCurrentMoveRange() > 0)
            {
                // Set path finding requirements. 

                if (m_ActiveUnit.GetComponent<AI_Unit_Movement>().m_bPathReached == false)
                {
                    if (m_ActiveUnit.GetComponent<AI_Unit_Movement>().m_bPathReached == false)
                    {
                        Debug.Log("Setting variables");

                        // Check the target unit's position for a free space next to it. 

                        GameObject l_TargetPosition = targetBuilding.GetComponent<Building_Positioning>().m_GetPosition().GetComponent<Cell_Neighbours>().m_GetClosestNeighbour(m_ActiveUnit.GetComponent<AI_Unit_Movement>().m_GetCurrentPosition());

                        if (l_TargetPosition != null)
                        {
                            m_ActiveUnit.GetComponent<AI_Unit_Movement>().m_SetStartandEndPoints(l_TargetPosition);
                        }
                        else
                        {
                            // If a target position cannot be found (if the target is surrounded for example), wait for this turn. 

                            m_ActiveUnit.GetComponent<AI_Unit_Movement>().m_UnitWait();
                        }
                    }
                }
            }
        }
        else
        {
            Debug.Log("No need to move");

            m_ActiveUnit.GetComponent<AI_Unit_Movement>().m_UnitWait();
        }
    }

    /*! \fn This will be used to attack a target unit on the map */
    private void m_AttackUnit(GameObject targetUnit)
    {
        // Start off by checking if the unit can attack in the first place. 

        if (m_ActiveUnit.GetComponent<Unit_Attack>().m_GetNumberOfAttacks() > 0)
        {
            // If the target unit is within range attack them. 
            if (targetUnit.GetComponent<Unit_Movement>().m_GetCurrentPosition().GetComponent<Cell_Manager>().m_Distance(m_ActiveUnit.GetComponent<Unit_Movement>().m_GetCurrentPosition())
                == m_ActiveUnit.GetComponent<Unit_Attack>().m_GetAttackRange())
            {
                m_ActiveUnit.GetComponent<Unit_Attack>().m_AttackTarget(targetUnit);
            }

            // End of combat set the number of attacks to 0. 
            m_ActiveUnit.GetComponent<Unit_Attack>().m_SetNumberOfAttacks(0);
        }
    }

    /*! \fn This will be used to attack a target building on the map */
    private void m_AttackBuiding(GameObject targetBuilding)
    {
        // Start off by checking if the unit can attack in the first place. 

        if (m_ActiveUnit.GetComponent<Unit_Attack>().m_GetNumberOfAttacks() > 0)
        {
            // If the target building is within range attack them. 
            if (targetBuilding.GetComponent<Building_Positioning>().m_GetPosition().GetComponent<Cell_Manager>().m_Distance(m_ActiveUnit.GetComponent<Unit_Movement>().m_GetCurrentPosition())
                == m_ActiveUnit.GetComponent<Unit_Attack>().m_GetAttackRange())
            {
                m_ActiveUnit.GetComponent<Unit_Attack>().m_AttackBuildingTarget(targetBuilding);
            }

            // End of combat set the number of attacks to 0. 
            m_ActiveUnit.GetComponent<Unit_Attack>().m_SetNumberOfAttacks(0);
        }
    }

    public void m_SetOtherManager(GameObject playerUnitManager) { m_OtherManger = playerUnitManager; }

    void m_ResetUnits()
    {
        // Reset Map

        // Reset the map cells allowing for proper move representation. 
        m_GameMap.GetComponent<Tile_Map_Manager>().m_ResetCellColours();
        m_GameMap.GetComponent<Tile_Map_Manager>().m_ResetOccupied();

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

                unit.GetComponent<Unit_Attack>().m_SetWithinAtRange(false);

                unit.GetComponent<Unit_Attack>().m_SetSelectedForAttack(false);
            }
        }
    }

    public void m_SetOwner(CurrentTurn newOwner) { m_Owner = newOwner; }

    GameObject m_ActivateUnit()
    {
        foreach (var unit in m_UnitList)
        {
            if (unit != null)
            {
                if (unit.GetComponent<Unit_Movement>().m_GetCurrentMoveRange() > 0 || unit.GetComponent<Unit_Attack>().m_GetNumberOfAttacks() > 0)
                {
                    // Debug.Log("Found Unit " + unit.name + " they are now active"); 

                    return unit;
                }
            }
        }

        Debug.Log("no more units to activate."); 

        return null;
    }

}
