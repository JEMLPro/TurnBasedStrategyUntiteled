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
        if(m_GetSelectedUnit() != null)
        {
            m_UnitMenu.SetActive(true);
        }
        else
        {
            m_UnitMenu.SetActive(false);
        }
    }

    public void m_Wait()
    {
        for (int i = 0; i < m_UnitList.Count; i++)
        {
            if (m_UnitList[i].GetComponent<UnitStat>().m_GetSelected())
            {
                m_UnitList[i].GetComponent<Unit_Movement>().m_Wait(); 
            }
        }
    }


    public GameObject m_GetSelectedUnit()
    {
        GameObject l_ReturnValue = null; 

        for(int i = 0; i < m_UnitList.Count; i++)
        {
            if(m_UnitList[i].GetComponent<UnitStat>().m_GetSelected())
            {
                l_ReturnValue = m_UnitList[i];

                return l_ReturnValue; 
            }
        }

        return l_ReturnValue;
    }
}
