using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_Spwaning : MonoBehaviour
{
    [SerializeField]
    GameObject m_BaseUnit = null;
    
    public void m_SetBaseUnit(GameObject newPrefab) { m_BaseUnit = newPrefab; }

    public GameObject m_SpawnMilitiaUnit(GameObject spawnCell)
    {
        // Militia cost will be a small amount of gold, to represent the small stats. This could be shown by them 
        // having basic weapons or simple hand tools in their sprite models. 

        const float l_fGoldCost = 3; /*!< \var This will be the gold cost required to spawn a new militia unit.  */ 

        // Check resource requirement before spawning. 

        if (gameObject.transform.parent.GetComponentInChildren<Resource_Management>().m_CheckGoldRequirement(l_fGoldCost))
        {
            gameObject.transform.parent.GetComponentInChildren<Resource_Management>().m_RemoveFromGold(l_fGoldCost); 

            GameObject l_NewMilitiaUnit = Instantiate(m_BaseUnit);

            float l_fNewAttack = Random.Range(10, 20);
            float l_fNewDefence = Random.Range(10, 20);
            float l_fNewHitChance = Random.Range(90, 110);
            float l_fNewSpeed = Random.Range(8, 16);
            float l_fNewAttackRange = 1;
            UnitType l_NewType = UnitType.Militia;

            l_NewMilitiaUnit.GetComponent<Unit_Attack>().m_SetUnitStats(l_fNewAttack, l_fNewDefence, l_fNewHitChance, l_fNewSpeed, l_fNewAttackRange, l_NewType);
            l_NewMilitiaUnit.GetComponent<Unit_Movement>().m_SetPosition(spawnCell);

            l_NewMilitiaUnit.name = "Militia";

            return l_NewMilitiaUnit;
        }

        return null; 
    }

    public GameObject m_SpawnEngineerUnit(GameObject spawnCell)
    {
        // Militia cost will be a small amount of gold, to represent the small stats. This could be shown by them 
        // having basic weapons or simple hand tools in their sprite models. 

        const float l_fGoldCost = 3; /*!< \var This will be the gold cost required to spawn a new militia unit.  */

        // Check resource requirement before spawning. 

        if (gameObject.transform.parent.GetComponentInChildren<Resource_Management>().m_CheckGoldRequirement(l_fGoldCost))
        {
            gameObject.transform.parent.GetComponentInChildren<Resource_Management>().m_RemoveFromGold(l_fGoldCost);

            GameObject l_NewEngineerUnit = Instantiate(m_BaseUnit);

            float l_fNewAttack = Random.Range(5, 15);
            float l_fNewDefence = Random.Range(8, 16);
            float l_fNewHitChance = Random.Range(100, 110);
            float l_fNewSpeed = Random.Range(12, 22);
            float l_fNewAttackRange = 1;
            UnitType l_NewType = UnitType.Engineer;

            l_NewEngineerUnit.GetComponent<Unit_Attack>().m_SetUnitStats(l_fNewAttack, l_fNewDefence, l_fNewHitChance, l_fNewSpeed, l_fNewAttackRange, l_NewType);
            l_NewEngineerUnit.GetComponent<Unit_Movement>().m_SetPosition(spawnCell);

            l_NewEngineerUnit.name = "Engineer";

            return l_NewEngineerUnit;
        }

        return null;
    }
}
