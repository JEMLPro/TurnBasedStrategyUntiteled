using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_Spwaning : MonoBehaviour
{
    [SerializeField]
    GameObject m_BaseUnit = null;
    
    public GameObject m_SpawnMilitiaUnit(GameObject spawnCell)
    {
        GameObject l_NewMilitiaUnit = Instantiate(m_BaseUnit);

        float l_fNewAttack = Random.Range(10, 20); 
        float l_fNewDefence = Random.Range(10, 20);
        float l_fNewHitChance = Random.Range(90, 110);
        float l_fNewSpeed = Random.Range(8, 16);
        float l_fNewAttackRange = 1;
        UnitType l_NewType = UnitType.Militia; 

        l_NewMilitiaUnit.GetComponent<Unit_Attack>().m_SetUnitStats(l_fNewAttack, l_fNewDefence, l_fNewHitChance, l_fNewSpeed, l_fNewAttackRange, l_NewType);
        l_NewMilitiaUnit.GetComponent<Unit_Movement>().m_SetPosition(spawnCell); 

        return l_NewMilitiaUnit; 
    }
}
