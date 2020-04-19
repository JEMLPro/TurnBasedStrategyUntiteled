using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_Manager : MonoBehaviour
{
    [SerializeField]
    GameObject m_GameMap;

    [SerializeField]
    List<GameObject> m_UnitList; 

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
                    GameObject l_TempPosition = m_GameMap.GetComponent<Tile_Map_Manager>().m_GetSelectedCell();

                    if (l_TempPosition != null)
                    {
                        unit.GetComponent<Unit_Movement>().m_SetPosition(l_TempPosition);
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
