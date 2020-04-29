using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum Action
{
    Nothing,
    Wait, 
    Attack, 
    Move,
    Build
}

public class Unit_Manager : MonoBehaviour
{
    [SerializeField]
    GameObject m_GameMap;

    [SerializeField]
    List<GameObject> m_UnitList;

    [SerializeField]
    bool m_bCheckRange = false;

    [SerializeField]
    GameObject m_TurnManager;

    [SerializeField]
    CurrentTurn m_Owner = CurrentTurn.player;

    [SerializeField]
    bool m_bResetOnce = false;

    [SerializeField]
    bool m_bActionSelected = false; 

    [SerializeField]
    Action m_Action;

    [SerializeField]
    GameObject m_AttackTarget;

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

                                m_UnitAttack();
                                m_SetActionNull(); 
                                
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
                        // Check the distance between the current cell and the new cell. 

                        int l_DistToTarget = m_GetSelectedUnit().GetComponent<Unit_Movement>().m_GetCurrentPosition().GetComponent<Cell_Manager>().m_Distance(l_TempPosition);

                        // If the cell is within rage the unit will move towards it. 

                        if(m_GetSelectedUnit().GetComponent<Unit_Movement>().m_UpdateUnitPosition(l_TempPosition, l_DistToTarget) == true);
                        {
                            // If the unit has moved reset the cells back to their origional state. 

                            m_GameMap.GetComponent<Tile_Map_Manager>().m_ResetCellColours();

                            m_bCheckRange = false;

                            m_SetActionNull();
                        }
                    }
                    else
                    {
                        l_iResetLoop++;

                        if (l_iResetLoop >= 100)
                        {
                            m_bCheckRange = true;

                            l_iResetLoop = 0;
                        }
                    }
                }
            }
        }

        // Assign a new position to a unit without a current position. 
        if (m_GetSelectedUnit().GetComponent<Unit_Movement>().m_GetCurrentPosition() == null)
        {
            GameObject l_TempPos = m_GameMap.GetComponent<Tile_Map_Manager>().m_GetCellUsingGridPosition(0, 0);

            if (l_TempPos != null)
            {
                m_GetSelectedUnit().GetComponent<Unit_Movement>().m_SetPosition(l_TempPos);
            }
        }

    }

    void m_UnitAttack()
    {
        if (m_GetSelectedUnit() != null)
        {
            if (m_AttackTarget != null)
            {
                m_GetSelectedUnit().GetComponent<Unit_Attack>().m_AttackTarget(m_AttackTarget);
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

    GameObject m_GetSelectedUnit()
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

    public void m_SetActionNull()
    {
        m_Action = Action.Nothing;

        m_bActionSelected = false;

        Debug.Log("Action Selected - Nothing");
    }
}
