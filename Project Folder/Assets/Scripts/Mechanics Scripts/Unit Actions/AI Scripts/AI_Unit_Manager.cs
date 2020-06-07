using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Unit_Manager : Unit_Manager
{
    // Unit Management.

    [SerializeField]
    GameObject m_OtherManger; 

    [SerializeField]
    GameObject m_ActiveUnit = null; 

    //----------------------------------------------------------------------------------------------------------------------------
    //  Member Functions Start 
    //----------------------------------------------------------------------------------------------------------------------------

    private void Update()
    {
        // Init temp locations for units.
        foreach (var unit in m_UnitList)
        {
            if (unit != null)
            {
                if (unit.GetComponent<Unit_Movement>().m_GetCurrentPosition().GetComponent<Cell_Manager>().m_bCheckForOccupied() == false)
                {
                    unit.GetComponent<Unit_Movement>().m_GetCurrentPosition().GetComponent<Cell_Manager>().m_bSetOccupied(true);
                }
            }
        }

        if(m_CheckTurn())
        {
            // Start of AI Turn

            // Debug.Log("AI Turn");

            if (m_ActiveUnit != null)
            {
                // Find enemy target.

                if (m_OtherManger != null)
                {
                    GameObject l_TargetUnit = m_OtherManger.GetComponentInChildren<Unit_Manager>().m_GetLowestCombatRating(m_ActiveUnit);

                    m_ActiveUnit.GetComponent<AI_Unit_Movement>().m_bCheckForTurn = true;

                    //-------------------------------------------------------
                    // Unit Move.

                    if (l_TargetUnit != null)
                    {
                        // Debug.Log("This Target : " + l_TargetUnit.name);

                        if(m_ActiveUnit.GetComponent<Unit_Movement>().m_GetCurrentPosition().GetComponent<Cell_Manager>().m_Distance(l_TargetUnit.GetComponent<Unit_Movement>().m_GetCurrentPosition()) > 1)
                        {
                            if (m_ActiveUnit.GetComponent<Unit_Movement>().m_GetCurrentMoveRange() > 0)
                            {
                                // Set path finding requirements. 

                                if (m_ActiveUnit.GetComponent<AI_Unit_Movement>().m_GetPathfinding().m_CheckRequirements() == false)
                                {
                                    if (m_ActiveUnit.GetComponent<AI_Unit_Movement>().m_GetPathfinding().m_CheckStateOfPath() == false)
                                    {
                                        Debug.Log("Setting variables");

                                        // Check the target unit's position for a free space next to it. 

                                        GameObject l_TargetPosition = l_TargetUnit.GetComponent<Unit_Movement>().m_GetCurrentPosition().GetComponent<Cell_Neighbours>().m_GetClosestNeighbour(m_ActiveUnit.GetComponent<AI_Unit_Movement>().m_GetCurrentPosition());

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

                    //------------------------------------------------------
                    // Unit Attack. 

                    if (l_TargetUnit != null)
                    {
                        // Check the unit can attack
                        if (m_ActiveUnit.GetComponent<Unit_Attack>().m_GetNumberOfAttacks() > 0)
                        {
                            // If the target unit is within range attack them. 
                            if (l_TargetUnit.GetComponent<Unit_Movement>().m_GetCurrentPosition().GetComponent<Cell_Manager>().m_Distance(m_ActiveUnit.GetComponent<Unit_Movement>().m_GetCurrentPosition())
                                == m_ActiveUnit.GetComponent<Unit_Attack>().m_GetAttackRange())
                            {
                                m_ActiveUnit.GetComponent<Unit_Attack>().m_AttackTarget(l_TargetUnit);
                            }

                            // End of combat set the number of attacks to 0. 
                            m_ActiveUnit.GetComponent<Unit_Attack>().m_SetNumberOfAttacks(0);
                        }
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
                    Debug.LogWarning("Unable to find Other unit manager. ");
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

                unit.GetComponent<AI_Unit_Movement>().m_bCheckForTurn = false;
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
