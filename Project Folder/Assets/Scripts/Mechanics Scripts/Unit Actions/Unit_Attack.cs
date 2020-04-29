using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum UnitType
{
    Militia,
    Spear, 
    Sword, 
    Axe,
}

public class Unit_Attack : MonoBehaviour
{
    [SerializeField]
    float m_fAttack = 0;

    [SerializeField]
    float m_fDefence = 0;

    [SerializeField]
    float m_fHit = 0;

    [SerializeField]
    float m_fSpeed = 0;

    [SerializeField]
    float m_fAttackRange = 0;

    [SerializeField]
    UnitType m_UnitType = UnitType.Militia; 

    public void m_AttackTarget(GameObject targetForAttack)
    {
        // Init Local Variables 

        float l_fDamage = 0, l_fHitChance = 0;

        // Attacking Unit Attacks. 

        l_fDamage = m_fAttack - (targetForAttack.GetComponent<Unit_Attack>().m_fDefence / 2);

        Debug.Log("Total Damage : " + l_fDamage); 

        l_fHitChance = m_fHit - (targetForAttack.GetComponent<Unit_Attack>().m_fSpeed * 2);

        Debug.Log("Total Chance To Hit : " + l_fHitChance);

        if (Random.Range(0, 100) <= l_fHitChance)
        {
            Debug.Log("Target Hit");

            targetForAttack.GetComponent<Health_Management>().m_TakeHit(l_fDamage); 
        }
        else
        {
            Debug.Log("Target Missed");
        }

        // Counter Attack. 

        l_fDamage = targetForAttack.GetComponent<Unit_Attack>().m_fAttack - (m_fDefence / 2);

        Debug.Log("Total Damage : " + l_fDamage);

        l_fHitChance = targetForAttack.GetComponent<Unit_Attack>().m_fHit - (m_fSpeed * 2);

        Debug.Log("Total Chance To Hit : " + l_fHitChance);

        if (Random.Range(0, 100) <= l_fHitChance)
        {
            Debug.Log("Target Hit");

            gameObject.GetComponent<Health_Management>().m_TakeHit(l_fDamage);
        }
        else
        {
            Debug.Log("Target Missed");
        }
    }
}
