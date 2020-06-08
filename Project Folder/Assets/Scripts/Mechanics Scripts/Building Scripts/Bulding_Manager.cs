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

    [SerializeField]
    GameObject m_TurnManager = null;

    private void FixedUpdate()
    {
        bool l_bCheckForHQDestruction = true;

        foreach (var building in m_BuildingList)
        {
            if (building != null)
            {
                if (building.tag == "HQ")
                {
                    l_bCheckForHQDestruction = false;
                }
            }
        }

        if (l_bCheckForHQDestruction == true)
        {
            gameObject.GetComponentInParent<Lose_Script>().m_SetGameOver(); 
        }
    }

    public void m_SetTurnManager(GameObject turnManager) { m_TurnManager = turnManager; }

    public void m_SetBasicBuilding(GameObject newPrefab) { m_BasicBuilding = newPrefab; }

    public void m_SetOwner(CurrentTurn newOwner) { m_Owner = newOwner; }

    public bool m_CheckTurn()
    {
        if (m_Owner == m_TurnManager.GetComponent<Turn_Manager>().m_GetCurrentTurn())
        {
            return true;
        }

        return false;
    }

    public List<GameObject> m_GetBuildigList() => m_BuildingList; 

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

    public GameObject m_GetLowestCombatRating(GameObject unitOfFocus)
    {
        if (unitOfFocus != null)
        {
            GameObject l_Targetbuilding = null;

            foreach (var building in m_BuildingList)
            {
                if (building != null)
                {
                    // Start by getting the first building which isn't a null object. 

                    l_Targetbuilding = building;

                    break;
                }
            }

            if (l_Targetbuilding != null)
            {
                // Start by calculating the combat raing for the first building. 

                l_Targetbuilding.GetComponent<Building_Combat_Rating>().m_GetCombatRating();

                float l_fPrevRating, l_fNewRating;

                float l_fCurrDist = l_Targetbuilding.GetComponent<Building_Positioning>().m_GetPosition().GetComponent<Cell_Manager>().m_Distance(unitOfFocus.GetComponent<Unit_Movement>().m_GetCurrentPosition());

                // Using the distance we can judge the viability of this target. 

                l_fPrevRating = l_Targetbuilding.GetComponent<Building_Combat_Rating>().m_GetCombatRating() + (l_fCurrDist * 10);

                l_Targetbuilding.GetComponent<Building_Combat_Rating>().m_SetCombatRating(l_fPrevRating);

                foreach (var building in m_BuildingList)
                {
                    if (building != null)
                    {
                        if (building.GetComponent<Building_Positioning>().m_GetPosition().GetComponent<Cell_Neighbours>().m_NeighboursFree())
                        {

                            l_fCurrDist = building.GetComponent<Building_Positioning>().m_GetPosition().GetComponent<Cell_Manager>().m_Distance(unitOfFocus.GetComponent<Unit_Movement>().m_GetCurrentPosition());

                            l_fNewRating = building.GetComponent<Building_Combat_Rating>().m_GetBaseCombatRating() + (l_fCurrDist * 10);

                            building.GetComponent<Building_Combat_Rating>().m_SetCombatRating(l_fNewRating);

                            // Debug.Log(unit.name + " has a combat rating of " + l_fNewRating);

                            if (l_fNewRating < l_fPrevRating)
                            {
                                l_fPrevRating = l_fNewRating;

                                l_Targetbuilding = building;

                                // Debug.Log("New Unit Selected"); 
                            }
                        }
                        else
                        {
                            Debug.Log("Unable to move to this position"); 
                        }
                    }
                }

                return l_Targetbuilding;
            }
        }

        return null;
    }

}
