using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Unit_Movement : Unit_Movement
{

    public Find_Path m_GetPathfinding() => gameObject.GetComponent<Find_Path>();

    public bool m_bCheckForTurn = false; 

    public void m_SetStartandEndPoints(GameObject endPoint)
    {
        Debug.Log("Setting Requirements"); 

        m_GetPathfinding().m_SetStartCell(gameObject.GetComponent<AI_Unit_Movement>().m_GetCurrentPosition());
        m_GetPathfinding().m_SetEndCell(endPoint);
    }

    public override void Update()
    {
        if (m_bCheckForTurn)
        {
            if (m_GetPathfinding().m_CheckStateOfPath() == true)
            {
                List<GameObject> l_PathToTarget = gameObject.GetComponent<Find_Path>().m_GetFinalPath();

                if (l_PathToTarget.Count > 0)
                {
                    if (l_PathToTarget.Count >= gameObject.GetComponent<Unit_Movement>().m_GetCurrentMoveRange())
                    {
                        Debug.Log(l_PathToTarget.Count);

                        // Move as close to target as possible. 

                        if (l_PathToTarget.Count > gameObject.GetComponent<Unit_Movement>().m_GetCurrentMoveRange())
                        {
                            // If the target is farther than this unit can move. 

                            for (int i = gameObject.GetComponent<Unit_Movement>().m_GetCurrentMoveRange(); i > 0; i--)
                            {
                                GameObject l_TempPosition = l_PathToTarget[i];

                                if (l_TempPosition.GetComponent<Cell_Manager>().m_bcheckForObsticle() == false && l_TempPosition.GetComponent<Cell_Manager>().m_bCheckForOccupied() == false)
                                {
                                    gameObject.GetComponent<Unit_Movement>().m_UpdateUnitPosition(l_TempPosition, i);

                                    break; 
                                }
                            }

                            gameObject.GetComponent<Unit_Movement>().m_UnitWait();

                            gameObject.GetComponent<Find_Path>().m_ResetPathfinding();
                        }
                        else if (l_PathToTarget.Count < gameObject.GetComponent<Unit_Movement>().m_GetCurrentMoveRange())
                        {
                            Debug.Log(l_PathToTarget.Count);

                            // If the target is closer than the full movement points required. 

                            for (int i = l_PathToTarget.Count - 1; i > 0; i--)
                            {
                                GameObject l_TempPosition = l_PathToTarget[i];

                                if (l_TempPosition.GetComponent<Cell_Manager>().m_bcheckForObsticle() == false && l_TempPosition.GetComponent<Cell_Manager>().m_bCheckForOccupied() == false)
                                {
                                    gameObject.GetComponent<Unit_Movement>().m_UpdateUnitPosition(l_TempPosition, i);

                                    break;
                                }
                            }

                            gameObject.GetComponent<Unit_Movement>().m_UnitWait();

                            gameObject.GetComponent<Find_Path>().m_ResetPathfinding();
                        }
                    }
                    else
                    {
                        Debug.Log(l_PathToTarget.Count);

                        gameObject.GetComponent<Unit_Movement>().m_UnitWait();

                        gameObject.GetComponent<Find_Path>().m_ResetPathfinding();
                    }
                    
                }
            }
            else
            {
                if (m_GetPathfinding().m_CheckRequirements() == true)
                {
                    Debug.Log("Looking for path");

                    m_GetPathfinding().m_FindPath();
                }
            }
        }

        base.Update();

    } 

}
