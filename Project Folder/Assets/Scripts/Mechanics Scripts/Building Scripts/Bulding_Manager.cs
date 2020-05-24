using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bulding_Manager : MonoBehaviour
{
    [SerializeField]
    GameObject m_BasicBuilding = null;

    [SerializeField]
    GameObject m_GameMap = null; 

    [SerializeField]
    CurrentTurn m_Owner = CurrentTurn.player;

    [SerializeField]
    List<GameObject> m_BuildingList;

    private void Start()
    {
        m_GameMap = GameObject.FindGameObjectWithTag("Map");
    }

    [SerializeField]
    GameObject l_SpawnPoint = null;

    private void Update()
    {
        if(m_BuildingList.Count == 0)
        {
            l_SpawnPoint = m_GameMap.GetComponent<Tile_Map_Manager>().m_GetHQSpawnPoint(0);

            m_SpawnHQ(l_SpawnPoint); 
        }
    }

    public void m_SpawnHQ(GameObject cellToSpawn)
    {
        if (m_BasicBuilding != null)
        {
            if (cellToSpawn != null)
            {
                GameObject l_TempHQ = Instantiate(m_BasicBuilding, cellToSpawn.transform.position, Quaternion.identity);

                if (l_TempHQ != null)
                {
                    l_TempHQ.GetComponent<Building_Positioning>().m_SetPosition(cellToSpawn);

                    l_TempHQ.transform.parent = gameObject.transform; 

                    m_BuildingList.Add(l_TempHQ);

                    Debug.Log("HQ Spawned");
                }
                else
                {
                    Debug.LogWarning("Unable to spawn HQ - Error instantiating.");
                }
            }
            else
            {
                Debug.LogWarning("Unable to spawn HQ - Spawn location not found."); 
            }
        }
        else
        {
            Debug.LogError("Unable to spawn HQ - Basic building not found."); 
        }
    }

}
