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
    }

    public void m_SetCurrentAIUnit(GameObject newObject)
    {
        m_CurrentAIUnit = newObject;
    }
}
