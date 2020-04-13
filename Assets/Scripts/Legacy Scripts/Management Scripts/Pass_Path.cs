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

                    if (false)
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
            int l_iPlaceTomove = 0;

            for(int i = m_CurrentAIUnit.GetComponent<UnitStat>().m_GetMoveRadius(); i > 0; i--)
            {
                if(m_GetPointInPath(i) != null)
                {
                    l_iPlaceTomove = i; 

                    break;
                }
            }


            Debug.Log("New Place To Move " + l_iPlaceTomove);

            if (l_iPlaceTomove > 0)
            {
                m_CurrentAIUnit.GetComponent<Unit_Movement>().m_SetNewCell(m_GetPointInPath(l_iPlaceTomove));

                m_CurrentAIUnit.GetComponent<Unit_Movement>().m_SetUsedPoints(m_CurrentAIUnit.GetComponent<UnitStat>().m_GetMoveRadius());
            }
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

        Debug.Log("Possible Spaces to move " + spacesToMove + "\n Current Path Size " + l_CurrentPath.Count);

        if(spacesToMove >= l_CurrentPath.Count)
        {
            return null;
        }

        if (l_CurrentPath != null)
        {
            if (l_CurrentPath[spacesToMove].GetComponent<Cell_Info>().m_GetOccpied() == false)
            {
                if (l_CurrentPath.Count >= spacesToMove)
                {
                    l_ReturnCell = l_CurrentPath[spacesToMove];
                }
            }
        }

        return l_ReturnCell;
    }
}
