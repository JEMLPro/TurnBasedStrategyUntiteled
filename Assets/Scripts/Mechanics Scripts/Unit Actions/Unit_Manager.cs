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
                GameObject l_TempPosition = m_GameMap.GetComponent<Tile_Map_Manager>().m_GetSelectedCell();

                if (l_TempPosition != null)
                {
                    unit.GetComponent<Unit_Movement>().m_SetPosition(l_TempPosition);
                }
            }
        }
    }

}
