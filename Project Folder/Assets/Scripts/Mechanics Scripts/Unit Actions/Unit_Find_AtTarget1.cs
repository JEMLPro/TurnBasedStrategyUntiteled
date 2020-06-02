using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_Find_AtTarget1 : MonoBehaviour
{
    [SerializeField]
    GameObject m_AtTarget = null;

    [SerializeField]
    GameObject m_OtherUnitManager;

    private void Start()
    {
        m_OtherUnitManager = GameObject.FindGameObjectWithTag("Unit_Manager_AI");
    }

    public void m_AtRangeFinder()
    {
        GameObject l_SelectedUnit = gameObject.GetComponent<Unit_Manager>().m_GetSelectedUnit();

        if (l_SelectedUnit != null)
        {
            foreach (var unit in m_OtherUnitManager.GetComponent<AI_Unit_Manager>().m_GetUnitList())
            {
                if (unit != null)
                {
                    if (l_SelectedUnit.GetComponent<Unit_Movement>().m_GetCurrentPosition().GetComponent<Cell_Manager>().m_Distance(unit.GetComponent<Unit_Movement>().m_GetCurrentPosition()) ==
                        l_SelectedUnit.GetComponent<Unit_Attack>().m_GetAttackRange())
                    {
                        Debug.Log("Theres a unit within range");

                        unit.GetComponent<Unit_Attack>().m_SetWithinAtRange(true);
                    }
                    else
                    {
                        unit.GetComponent<Unit_Attack>().m_SetWithinAtRange(false);
                    }
                }
            }
        }
    }

    public void m_SelectAttackTarget()
    {
        foreach (var unit in m_OtherUnitManager.GetComponent<AI_Unit_Manager>().m_GetUnitList())
        {
            if (unit != null)
            {
                if (unit.GetComponent<Unit_Attack>().m_GetSelectedForAttack() == true)
                {
                    m_AtTarget = unit;
                }
            }
        }
    }

    public void m_SetAtTarget(GameObject newObject)
    {
        m_AtTarget = newObject; 

        if(newObject == null)
        {
            foreach (var unit in m_OtherUnitManager.GetComponent<AI_Unit_Manager>().m_GetUnitList())
            {
                if (unit != null)
                {
                    if (unit.GetComponent<Unit_Attack>().m_GetSelectedForAttack() == true)
                    {
                        unit.GetComponent<Unit_Attack>().m_SetSelectedForAttack(false);
                    }
                }
            }
        }
    }

    public GameObject m_GetAtTarget() => m_AtTarget;
}
