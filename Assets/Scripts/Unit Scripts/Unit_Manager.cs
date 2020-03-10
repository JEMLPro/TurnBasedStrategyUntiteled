using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_Manager : MonoBehaviour
{
    [SerializeField]
    List<GameObject> m_UnitList;

    [SerializeField]
    GameObject m_UnitMenu; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (m_UnitMenu != null)
        {
            if (m_GetSelectedUnit() != null)
            {
                m_UnitMenu.SetActive(true);
            }
            else
            {
                m_UnitMenu.SetActive(false);
            }
        }
    }

    public void m_Wait()
    {
        GameObject l_CurrentObj = m_GetSelectedUnit();

        l_CurrentObj.GetComponent<Unit_Movement>().m_Wait();
    }

    public void m_Attack()
    {
        GameObject l_CurrentObj = m_GetSelectedUnit();

        l_CurrentObj.GetComponent<Attack>().m_SetAttack(true); 
    }

    public GameObject m_GetSelectedUnit()
    {
        GameObject l_ReturnValue = null; 

        for(int i = 0; i < m_UnitList.Count; i++)
        {
            if (m_UnitList[i] != null)
            {
                if (m_UnitList[i].GetComponent<UnitStat>().m_GetSelected())
                {
                    l_ReturnValue = m_UnitList[i];

                    return l_ReturnValue;
                }
            }
        }

        return l_ReturnValue;
    }

    public void m_ResetSelectedUnits()
    {
        for (int i = 0; i < m_UnitList.Count; i++)
        {
            if (m_UnitList[i] != null)
            {
                if (m_UnitList[i].GetComponent<UnitStat>().m_GetSelected())
                {
                    m_UnitList[i].GetComponent<UnitStat>().m_SetSelected(false);
                }
            }
        }
    }

    public void m_ResetUnitMovementPoints()
    {
        for (int i = 0; i < m_UnitList.Count; i++)
        {
            if (m_UnitList[i] != null)
            {
                m_UnitList[i].GetComponent<Unit_Movement>().m_ResetMovementPoints();
            }
        }
    }
}
