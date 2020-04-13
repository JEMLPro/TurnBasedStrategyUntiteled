using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Find_Attack_Target : MonoBehaviour
{
    [SerializeField]
    GameObject m_OtherUnitManager; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject m_FindTarget()
    {


        return m_OtherUnitManager.GetComponent<Unit_Manager>().m_GetSelectedUnit();
    }
}
