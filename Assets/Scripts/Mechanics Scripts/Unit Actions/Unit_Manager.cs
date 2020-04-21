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

    void Start()
    {
        m_GameMap = GameObject.FindGameObjectWithTag("Map"); 
    }

    private void Update()
    {
        m_UpdateUnitPosition(); 
    }

    void m_UpdateUnitPosition()
    {
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
                    }
                }

                // Assign a new position to a unit without a current position. 

                if(unit.GetComponent<Unit_Movement>().m_GetCurrentPosition() == null)
                {
                    GameObject l_TempPos = m_GameMap.GetComponent<Tile_Map_Manager>().m_GetCellUsingGridPosition(0, 0);

                    if(l_TempPos != null)
                    {
                        unit.GetComponent<Unit_Movement>().m_SetPosition(l_TempPos);
                    }
                }
            }
        }
    }

}
