using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_Manager : MonoBehaviour
{
    [SerializeField]
    GameObject m_GameMap;

    [SerializeField]
    List<GameObject> m_UnitList;

    bool m_bCheckRange = false;

    [SerializeField]
    GameObject m_TurnManager;

    [SerializeField]
    CurrentTurn m_Owner = CurrentTurn.player;

    [SerializeField]
    bool m_bResetOnce = false; 

    void Start()
    {
        // If the game objects are not set find the objects through a assigned tag. 
        m_GameMap = GameObject.FindGameObjectWithTag("Map");

        if(m_GameMap == null)
        {
            Debug.LogError("Error code 0010 - Unable to assign game object in game: " + "Game Map. ");
        }

        m_TurnManager = GameObject.FindGameObjectWithTag("Turn_Manager"); 

        if(m_TurnManager == null)
        {
            Debug.LogError("Error code 0010 - Unable to assign game object in game: " + "Turn Manager. ");
        }
    }

    private void Update()
    {
        // This will check if the over of the unit manager is the same as current turn player. 
        if (m_CheckTurn())
        {
            m_UpdateUnitPosition();

            m_bResetOnce = true; 
        }
        else
        {
            // If it is not the owner's turn reset the units once. 

            if(m_bResetOnce == true)
            {
                m_ResetUnits();

                m_bResetOnce = false; 
            }
        }
    }

    void m_UpdateUnitPosition()
    {
        int l_iResetLoop = 0;

        // Loops through each unit in the list. 
        foreach (var unit in m_UnitList)
        {
            if (unit != null)
            {
                // Update the position of the current selected unit. 
                if (unit.GetComponent<Unit_Active>().m_GetActiveUnit() == true)
                {
                    // If the unit can still has movemet points they can still move. 
                    if (unit.GetComponent<Unit_Movement>().m_GetCurrentMoveRange() > 0)
                    {
                        // If the range hasn't been checked for this unit, check their movement range. 
                        if (m_bCheckRange == false)
                        {
                            m_GameMap.GetComponent<Tile_Map_Manager>().m_CheckCellRange(unit.GetComponent<Unit_Movement>().m_GetCurrentMoveRange(),
                                unit.GetComponent<Unit_Movement>().m_GetCurrentPosition());

                            m_bCheckRange = true;
                        }

                        // Store the selected cell locally to check it exists. 
                        GameObject l_TempPosition = m_GameMap.GetComponent<Tile_Map_Manager>().m_GetSelectedCell();

                        // If the cell exists. 
                        if (l_TempPosition != null)
                        {
                            // Check the distance between the current cell and the new cell. 

                            int l_DistToTarget = unit.GetComponent<Unit_Movement>().m_GetCurrentPosition().GetComponent<Cell_Manager>().m_Distance(l_TempPosition);

                            // If the cell is within rage the unit will move towards it. 

                            unit.GetComponent<Unit_Movement>().m_SetPosition(l_TempPosition, l_DistToTarget);

                            // If the unit has moved reset the cells back to their origional state. 

                            m_GameMap.GetComponent<Tile_Map_Manager>().m_ResetCellColours();

                            m_bCheckRange = false;
                        }
                        else
                        {
                            l_iResetLoop++;

                            if(l_iResetLoop >= 100)
                            {
                                m_bCheckRange = true;

                                l_iResetLoop = 0; 
                            }
                        }
                    }
                }

                // Assign a new position to a unit without a current position. 
                if (unit.GetComponent<Unit_Movement>().m_GetCurrentPosition() == null)
                {
                    GameObject l_TempPos = m_GameMap.GetComponent<Tile_Map_Manager>().m_GetCellUsingGridPosition(0, 0);

                    if (l_TempPos != null)
                    {
                        unit.GetComponent<Unit_Movement>().m_SetPosition(l_TempPos);
                    }
                }
            }
        }
    }
    
    void m_ResetUnits()
    {
        // Reset Map

        // Reset the map cells allowing for proper move representation. 
        m_GameMap.GetComponent<Tile_Map_Manager>().m_ResetCellColours();

        foreach (var unit in m_UnitList)
        {
            if(unit != null)
            {
                // Reset Units

                // Deselect Unit
                unit.GetComponent<Unit_Active>().m_SetUnitActive(false); 

                // Reset Unit's Movememt points allowing for new movement.
                unit.GetComponent<Unit_Movement>().m_ResetUsedPoints();

            }
        }
    }

    public bool m_CheckTurn()
    {
        if(m_Owner == m_TurnManager.GetComponent<Turn_Manager>().m_GetCurrentTurn())
        {
            return true;
        }

        return false; 
    }
}
