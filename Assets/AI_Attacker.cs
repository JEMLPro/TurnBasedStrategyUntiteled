using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Attacker : MonoBehaviour
{

    [SerializeField]
    GameObject m_AttackTarget;

    [SerializeField]
    GameObject m_PlayerUnitManager;

    [SerializeField]
    Color m_WithinRangeColour;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < gameObject.GetComponent<Unit_Manager>().m_UnitList.Count; i++)
        {
            if (gameObject.GetComponent<Unit_Manager>().m_UnitList[i].GetComponent<Attack>().m_GetAttackPoints() > 0)
            {
                m_RangeFinder(gameObject.GetComponent<Unit_Manager>().m_UnitList[i]);

                m_AttackTarget = m_GetAttackTarget();

                if (m_AttackTarget != null)
                {
                    gameObject.GetComponent<Unit_Manager>().m_UnitList[i].GetComponent<Attack>().m_Attck(m_AttackTarget);

                    gameObject.GetComponent<Unit_Manager>().m_ResetOtherManager();
                }
            }
        }
    }

    void m_RangeFinder(GameObject currentUnit)
    {
        if (currentUnit.GetComponent<UnitStat>().m_GetOwner() == gameObject.GetComponent<Unit_Manager>().m_GetCurrentTurn())
        {
            GameObject[] l_PotentialTargets;
            l_PotentialTargets = GameObject.FindGameObjectsWithTag("PlayerUnit");

            // Check enemies are in cell range

            // Check Up 

            for (int j = 0; j <= 3; j++)
            {
                GameObject l_CellToCheck = null;

                string l_Direction = " ";

                switch (j)
                {
                    case 0:
                        l_CellToCheck = currentUnit.GetComponent<Unit_Movement>().m_GetCurrentCell().GetComponent<Cell_Info>().m_GetCellNeighbour("Up");
                        l_Direction = "Up";
                        break;

                    case 1:
                        l_CellToCheck = currentUnit.GetComponent<Unit_Movement>().m_GetCurrentCell().GetComponent<Cell_Info>().m_GetCellNeighbour("Down");
                        l_Direction = "Down";
                        break;

                    case 2:
                        l_CellToCheck = currentUnit.GetComponent<Unit_Movement>().m_GetCurrentCell().GetComponent<Cell_Info>().m_GetCellNeighbour("Left");
                        l_Direction = "Left";
                        break;

                    case 3:
                        l_CellToCheck = currentUnit.GetComponent<Unit_Movement>().m_GetCurrentCell().GetComponent<Cell_Info>().m_GetCellNeighbour("Right");
                        l_Direction = "Right";
                        break;

                    default:
                        l_CellToCheck = null;
                        break;
                }

                if (l_CellToCheck != null)
                {
                    for (int i = 0; i < l_PotentialTargets.Length; i++)
                    {
                        if (l_CellToCheck == l_PotentialTargets[i].GetComponent<Unit_Movement>().m_GetCurrentCell())
                        {
                            Debug.Log("Player Is In Cell " + l_Direction);

                            l_PotentialTargets[i].GetComponent<UnitStat>().m_SetWithinRange(true);

                            l_PotentialTargets[i].GetComponent<Renderer>().material.color = m_WithinRangeColour;
                        }
                    }
                }
            }
        }

    }

    GameObject m_GetAttackTarget()
    {
        List <GameObject> l_OtherUnitManager = m_PlayerUnitManager.GetComponent<Unit_Manager>().m_UnitList;

        GameObject l_Temp = null;

        for(int i = 0; i < l_OtherUnitManager.Count; i++)
        {
            if (l_OtherUnitManager[i] != null)
            {
                if (l_OtherUnitManager[i].GetComponent<UnitStat>().m_GetWithinRange() == true)
                {
                    if (l_Temp == null)
                    {
                        l_Temp = l_OtherUnitManager[i];
                    }

                    if (l_Temp.GetComponent<UnitStat>().m_GetHP() > l_OtherUnitManager[i].GetComponent<UnitStat>().m_GetHP())
                    {
                        l_Temp = l_OtherUnitManager[i];
                    }
                }
            }
        }

        return l_Temp; 
    }


}
