using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Get_Attack_Target : MonoBehaviour
{
    [SerializeField]
    GameObject m_EnemyManager;

    [SerializeField]
    bool m_bSelectUnit = false;

    [SerializeField]
    GameObject m_Target; 

    // Update is called once per frame
    void Update()
    {
        if(m_bSelectUnit == true)
        {
            if (m_EnemyManager.GetComponent<Unit_Manager>().m_GetSelectedUnit() != null)
            {
                m_Target = m_EnemyManager.GetComponent<Unit_Manager>().m_GetSelectedUnit();
            }

            m_bSelectUnit = false; 
        }
    }

    public GameObject m_GetAttackTarget()
    {
        return m_Target;
    }

    public void m_SeletUnit()
    {

    }


}
