using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class will handle the spawning of the new units into the game. 
/// </summary>
public class Unit_Spwaning : MonoBehaviour
{
    #region Data Members

    /// <summary>
    /// This is the base prefab for a unit, this will allow for all of the other units to be spawned using a single base. 
    /// </summary>
    [SerializeField]
    GameObject m_BaseUnit = null;

    #endregion

    #region Member Functions 

    /// <summary>
    /// This will be used to assign the basic unit variale, storing this locally. 
    /// </summary>
    /// <param name="newPrefab">The prefab object used to create all other units. </param>
    public void m_SetBaseUnit(GameObject newPrefab) { m_BaseUnit = newPrefab; }

    #region Units To Spawn

    /// <summary>
    /// This will spawn a militia unit onto the spawn cell prvided into this function. The militia is the weakest unit in the game, but 
    /// is the cheapest unit to spawn. 
    /// </summary>
    /// <param name="spawnCell">The cell the unit will be spawned onto. </param>
    /// <returns>The newly spawned unit to be added into the unit list. </returns>
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

    /// <summary>
    /// This will spawn an engineer unit. The engineer is the main unit which can build within the game. 
    /// </summary>
    /// <param name="spawnCell">The cell the unit will be spawned onto. </param>
    /// <returns>The newly spawned unit to be added into the unit list. </returns>
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

    // Todo Add Swordsman spawn function. 

    // Todo Add Lancer spawn function. 

    // Todo Add Maurauder spawn function.

    #endregion

    #endregion
}
