using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bulding_Manager : MonoBehaviour
{
    [SerializeField]
    GameObject m_BasicBuilding = null;

    [SerializeField]
    CurrentTurn m_Owner = CurrentTurn.player;

    [SerializeField]
    List<GameObject> m_BuildingList = new List<GameObject>();

    
    public void m_SetBasicBuilding(GameObject newPrefab) { m_BasicBuilding = newPrefab; }

    public void m_SetOwner(CurrentTurn newOwner) { m_Owner = newOwner; }

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

                    l_TempHQ.name = "HQ Building";

                    l_TempHQ.tag = "HQ"; 

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

    public GameObject m_GetHQObject()
    {
        foreach (var building in m_BuildingList)
        {
            if(building.tag == "HQ")
            {
                return building;
            }
        }

        Debug.LogWarning("Unable to find HQ item, check the list to see if it exists and contains the tag 'HQ'.");

        return null;
    }

}
