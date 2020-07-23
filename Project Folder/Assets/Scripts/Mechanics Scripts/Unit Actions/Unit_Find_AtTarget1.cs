using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_Find_AtTarget1 : MonoBehaviour
{
    [SerializeField]
    GameObject m_AtTarget = null;

    [SerializeField]
    GameObject m_OtherManager;

    private void Start()
    {
        m_OtherManager = GameObject.FindGameObjectWithTag("AI_Manager");
    }

    public void m_AtRangeFinder()
    {
        GameObject l_SelectedUnit = gameObject.GetComponent<Unit_Manager>().m_GetSelectedUnit();

        if (l_SelectedUnit != null)
        {
            // Check Units

            foreach (var unit in m_OtherManager.GetComponentInChildren<AI_Unit_Manager>().m_GetUnitList())
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

            // Check Buildings

            foreach (var building in m_OtherManager.GetComponentInChildren<Bulding_Manager>().m_GetBuildigList())
            {
                if (building != null)
                {
                    if (l_SelectedUnit.GetComponent<Unit_Movement>().m_GetCurrentPosition().GetComponent<Cell_Manager>().m_Distance(building.GetComponent<Building_Positioning>().m_GetPosition()) ==
                        l_SelectedUnit.GetComponent<Unit_Attack>().m_GetAttackRange())
                    {
                        // Debug.Log("Theres a Building within range");

                        building.GetComponent<Select_Building>().m_SetWithinRange(true);
                    }
                    else
                    {
                        building.GetComponent<Select_Building>().m_SetWithinRange(false);
                    }
                }
            }
        }
    }

    public void m_SelectAttackTarget()
    {
        foreach (var unit in m_OtherManager.GetComponentInChildren<AI_Unit_Manager>().m_GetUnitList())
        {
            if (unit != null)
            {
                if (unit.GetComponent<Unit_Attack>().m_GetSelectedForAttack() == true)
                {
                    m_AtTarget = unit;
                }
            }
        }

        // If no target has been set then check fr buidlings

        if (m_AtTarget == null)
        {
            foreach (var building in m_OtherManager.GetComponentInChildren<Bulding_Manager>().m_GetBuildigList())
            {
                if (building.GetComponent<Select_Building>().m_GetSelected() == true)
                {
                    m_AtTarget = building; 
                }
            }
        }
    }

    public void m_SetAtTarget(GameObject newObject)
    {
        if (m_OtherManager != null)
        {
            m_AtTarget = newObject;

            if (newObject == null)
            {
                foreach (var unit in m_OtherManager.GetComponentInChildren<AI_Unit_Manager>().m_GetUnitList())
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
    }

    public GameObject m_GetAtTarget() => m_AtTarget;
}
