using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pass_Path : MonoBehaviour
{
    [SerializeField]
    GameObject m_Pathfinder;

    [SerializeField]
    GameObject m_CurrentAIUnit = null;

    [SerializeField]
    bool m_bGenerateNewPath = false;

    [SerializeField]
    GameObject l_TargetUnit = null;

    [SerializeField]
    List<GameObject> l_PlayerUnitList;

    // Start is called before the first frame update
    void Start()
    {
        l_PlayerUnitList = gameObject.GetComponent<AI_Attacker>().m_PlayerUnitManager.GetComponent<Unit_Manager>().m_UnitList;
    }

    // Update is called once per frame
    void Update()
    {
        if(m_bGenerateNewPath == true)
        {
            for (int i = 0; i < l_PlayerUnitList.Count; i++)
            {
                if (l_TargetUnit == null && l_PlayerUnitList[i] != null)
                {
                    l_TargetUnit = l_PlayerUnitList[i]; 
                }
                else if(l_TargetUnit != null)
                {
                    int l_iNewTargetValue, l_iOldTargetValue;

                    l_iNewTargetValue = l_PlayerUnitList[i].GetComponent<UnitStat>().m_GetUnitCombatValue() +
                        l_PlayerUnitList[i].GetComponent<Unit_Movement>().m_Distance(m_CurrentAIUnit.GetComponent<Unit_Movement>().m_GetCurrentCell().GetComponent<Cell_Info>().m_GetGridPos().x,
                        m_CurrentAIUnit.GetComponent<Unit_Movement>().m_GetCurrentCell().GetComponent<Cell_Info>().m_GetGridPos().y);

                    l_iOldTargetValue = l_TargetUnit.GetComponent<UnitStat>().m_GetUnitCombatValue() +
                        l_TargetUnit.GetComponent<Unit_Movement>().m_Distance(m_CurrentAIUnit.GetComponent<Unit_Movement>().m_GetCurrentCell().GetComponent<Cell_Info>().m_GetGridPos().x,
                        m_CurrentAIUnit.GetComponent<Unit_Movement>().m_GetCurrentCell().GetComponent<Cell_Info>().m_GetGridPos().y);

                    if (l_iNewTargetValue < l_iOldTargetValue)
                    {
                        l_TargetUnit = l_PlayerUnitList[i];
                    }
                }
            }

            if(l_TargetUnit != null && m_CurrentAIUnit != null)
            {
                m_Pathfinder.GetComponent<Pathfinding>().m_SetStartCell(m_CurrentAIUnit.GetComponent<Unit_Movement>().m_GetCurrentCell());
                m_Pathfinder.GetComponent<Pathfinding>().m_SetEndCell(l_TargetUnit.GetComponent<Unit_Movement>().m_GetCurrentCell());
                m_Pathfinder.GetComponent<Pathfinding>().m_SetFoundPath(false); 
            }

            m_bGenerateNewPath = false;

        }
        else
        {
            if (m_Pathfinder.GetComponent<Pathfinding>().m_GetPath().Count > 0 && m_Pathfinder.GetComponent<Pathfinding>().m_GetFoundPath() == true)
            {
                m_MoveUnit();
            }
        }
    }

    public void m_MoveUnit()
    {
        if (m_CurrentAIUnit.GetComponent<Unit_Movement>().m_GetMovementPoints() == 0)
        {
            m_CurrentAIUnit.GetComponent<Unit_Movement>().m_SetNewCell(m_GetPointInPath(m_CurrentAIUnit.GetComponent<UnitStat>().m_GetMoveRadius()));

            m_CurrentAIUnit.GetComponent<Unit_Movement>().m_SetUsedPoints(m_CurrentAIUnit.GetComponent<UnitStat>().m_GetMoveRadius());
        }
    }

    public void m_SetCurrentAIUnit(GameObject newObject)
    {
        m_CurrentAIUnit = newObject;
    }

    public GameObject m_GetPointInPath(int spacesToMove)
    {
        GameObject l_ReturnCell = null;

        List<GameObject> l_CurrentPath = m_Pathfinder.GetComponent<Pathfinding>().m_GetPath();

        if (l_CurrentPath.Count >= spacesToMove)
        {
            l_ReturnCell = l_CurrentPath[spacesToMove];
        }
        else
        {
            l_ReturnCell = l_CurrentPath[l_CurrentPath.Count - 1];
        }


        return l_ReturnCell;
    }
}
