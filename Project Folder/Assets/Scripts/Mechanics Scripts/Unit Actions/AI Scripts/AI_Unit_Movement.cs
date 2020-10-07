using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Unit_Movement : Unit_Movement
{

    public Find_Path m_GetPathfinding() => gameObject.GetComponent<Find_Path>();

    public void m_SetStartandEndPoints(GameObject endPoint)
    {
        Debug.Log("Setting Requirements"); 

        m_GetPathfinding().m_SetStartCell(gameObject.GetComponent<AI_Unit_Movement>().m_GetCurrentPosition());
        m_GetPathfinding().m_SetEndCell(endPoint);
    }

    public void FixedUpdate()
    {
        if (gameObject.GetComponentInParent<AI_Unit_Manager>().m_CheckTurn())
        {
            Debug.Log(gameObject.name + "'s turn.");

            if (m_GetPathfinding().m_CheckStateOfPath() == true)
            {
                Debug.Log(gameObject.name + "'s found a path.");

                List<GameObject> l_PathToTarget = gameObject.GetComponent<Find_Path>().m_GetFinalPath();

                if (l_PathToTarget.Count > 0)
                {

                    for (int i = l_PathToTarget.Count - 1; i > 0; i--)
                    {
                        Debug.Log("Pathfinding checking movement."); 

                        if (l_PathToTarget[i].GetComponent<Cell_Manager>().m_CanBeMovedTo())
                        {
                            if (l_PathToTarget[i].GetComponent<Cell_Manager>().m_Distance(gameObject.GetComponent<Unit_Movement>().m_GetCurrentPosition()) > gameObject.GetComponent<Unit_Movement>().m_GetCurrentMoveRange())
                            {
                                Debug.Log("Pathfinding - Cell " + i + " out of range.");

                                continue; 
                            }
                            else if((l_PathToTarget[i].GetComponent<Cell_Manager>().m_Distance(gameObject.GetComponent<Unit_Movement>().m_GetCurrentPosition()) <= gameObject.GetComponent<Unit_Movement>().m_GetCurrentMoveRange())
                                && (i > gameObject.GetComponent<Unit_Movement>().m_GetCurrentMoveRange()))
                            {
                                Debug.Log("Pathfinding - Cell " + i + " out of range.");

                                continue;
                            }
                            else
                            {
                                Debug.Log("Pathfinding - Cell " + i + " within range and moving.");

                                GameObject l_TempPosition = l_PathToTarget[i];

                                gameObject.GetComponent<Unit_Movement>().m_UpdateUnitPosition(l_TempPosition, i);

                                gameObject.GetComponent<Unit_Movement>().m_UnitWait();

                                gameObject.GetComponent<Find_Path>().m_ResetPathfinding();

                                break; 
                            }
                        }
                    }
                }
            }
            else
            {
                Debug.Log(gameObject.name + "'s not found its path.");

                if (m_GetPathfinding().m_CheckRequirements() == true)
                {
                    Debug.Log(gameObject.name + "'s got what it needs to search for a path.");

                    m_GetPathfinding().m_FindPath();
                }
            }
        }

        base.Update();

    } 

}
