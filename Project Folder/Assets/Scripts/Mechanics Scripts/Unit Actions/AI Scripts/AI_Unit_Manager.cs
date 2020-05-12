﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Unit_Manager : MonoBehaviour
{
    // Movement Management.

    [SerializeField]
    GameObject m_GameMap; /*!< \var This will be the game object holding the game's map. */

    // Unit Management.

    [SerializeField]
    List<GameObject> m_UnitList; /*!< \var This will hold all of the controlled units. */

    [SerializeField]
    GameObject m_OtherUnitManger; 

    [SerializeField]
    GameObject m_TurnManager; /*!< \var The turn manager for the game, used to check the current turn. */

    [SerializeField]
    CurrentTurn m_Owner = CurrentTurn.ai; /*!< \var The owner ofthis set of units. */

    [SerializeField]
    bool m_bResetOnce = false; /*!< \var Used to reset the list of units at the end of the turn. */

    GameObject m_ActiveUnit = null; 

    //----------------------------------------------------------------------------------------------------------------------------
    //  Member Functions Start 
    //----------------------------------------------------------------------------------------------------------------------------

    private void Start()
    {
        m_GameMap = GameObject.FindGameObjectWithTag("Map");
        m_TurnManager = GameObject.FindGameObjectWithTag("Turn_Manager");
        m_OtherUnitManger = GameObject.FindGameObjectWithTag("Unit_Manager_Player");
    }

    private void Update()
    {
        // Init temp locations for units.
        foreach (var unit in m_UnitList)
        {
            if (unit != null)
            {
                if (unit.GetComponent<Unit_Movement>().m_GetCurrentPosition() == null)
                {
                    Debug.Log("Set Random Pos"); 

                    GameObject l_TempPos = m_GameMap.GetComponent<Tile_Map_Manager>().m_GetRandomCell();

                    if (l_TempPos != null)
                    {
                        unit.GetComponent<Unit_Movement>().m_SetPosition(l_TempPos);
                    }
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

                GameObject l_TargetUnit = m_OtherUnitManger.GetComponent<Unit_Manager>().m_GetLowestCombatRating(m_ActiveUnit);

                //-------------------------------------------------------
                // Unit Move.

                if (l_TargetUnit != null)
                {
                    // Debug.Log("This Target : " + l_TargetUnit.name);

                    if (m_ActiveUnit.GetComponent<Unit_Movement>().m_GetCurrentMoveRange() > 0)
                    {
                        // Set path finding requirements. 

                        if (gameObject.GetComponent<Find_Path>().m_CheckRequirements() == false)
                        {
                            gameObject.GetComponent<Find_Path>().m_SetEndCell(l_TargetUnit.GetComponent<Unit_Movement>().m_GetCurrentPosition());

                            gameObject.GetComponent<Find_Path>().m_SetStartCell(m_ActiveUnit.GetComponent<Unit_Movement>().m_GetCurrentPosition());
                        }

                        // Update Pathfinding. 

                        gameObject.GetComponent<Find_Path>().m_FindPath();

                        // If path is found begin moving. 

                        if (gameObject.GetComponent<Find_Path>().m_CheckStateOfPath() == true)
                        {
                            List<GameObject> l_PathToTarget = gameObject.GetComponent<Find_Path>().m_GetFinalPath();

                            if (l_PathToTarget.Count > 0)
                            {
                                // Move as close to target as possible. 

                                if (l_PathToTarget.Count > m_ActiveUnit.GetComponent<Unit_Movement>().m_GetCurrentMoveRange())
                                {
                                    // If the target is farther than this unit can move. 

                                    m_ActiveUnit.GetComponent<Unit_Movement>().m_UpdateUnitPosition(l_PathToTarget[m_ActiveUnit.GetComponent<Unit_Movement>().m_GetCurrentMoveRange()], 0);

                                    m_ActiveUnit.GetComponent<Unit_Movement>().m_UnitWait();
                                }
                                else
                                {
                                    // If the target is closer than the full movement points required. 

                                    m_ActiveUnit.GetComponent<Unit_Movement>().m_UpdateUnitPosition(l_PathToTarget[l_PathToTarget.Count - 1], 0);

                                    m_ActiveUnit.GetComponent<Unit_Movement>().m_UnitWait();
                                }
                            }
                        }
                    }
                }

                //------------------------------------------------------
                // Unit Attack. 

                if(l_TargetUnit != null)
                {
                    // Check the unit can attack
                    if(m_ActiveUnit.GetComponent<Unit_Attack>().m_GetNumberOfAttacks() > 0)
                    {
                        // If the target unit is within range attack them. 
                        if(l_TargetUnit.GetComponent<Unit_Movement>().m_GetCurrentPosition().GetComponent<Cell_Manager>().m_Distance(m_ActiveUnit.GetComponent<Unit_Movement>().m_GetCurrentPosition()) 
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

                // End Turn. 

                m_bResetOnce = true;
            }
            else
            {
                m_ActiveUnit = m_ActivateUnit(); 

                if(m_ActiveUnit == null)
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

    // This will be ued to check which trun it is. If the turn is the owners.
    public bool m_CheckTurn()
    {
        if (m_Owner == m_TurnManager.GetComponent<Turn_Manager>().m_GetCurrentTurn())
        {
            return true;
        }

        return false;
    }

    void m_ResetUnits()
    {
        // Reset Map

        // Reset the map cells allowing for proper move representation. 
        m_GameMap.GetComponent<Tile_Map_Manager>().m_ResetCellColours();

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

    GameObject m_ActivateUnit()
    {
        foreach (var unit in m_UnitList)
        {
            if (unit != null)
            {
                if (unit.GetComponent<Unit_Movement>().m_GetCurrentMoveRange() > 0 && unit.GetComponent<Unit_Attack>().m_GetNumberOfAttacks() > 0)
                {
                    return unit;
                }
            }
        }

        Debug.Log("no more units to activate."); 

        return null;
    }

}
