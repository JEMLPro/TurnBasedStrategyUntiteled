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
    bool m_bWithinRange = false;

    [SerializeField]
    bool m_bSelectedForAttack = false; 

    [SerializeField]
    UnitType m_UnitType = UnitType.Militia; 

    public void m_AttackTarget(GameObject targetForAttack)
    {
        // Init Local Variables 

        float l_fDamage = 0, l_fHitChance = 0;

        // Attacking Unit Attacks. 

        // Calculate damage by reducing attack value by target's defence value. 
        l_fDamage = m_fAttack - (targetForAttack.GetComponent<Unit_Attack>().m_fDefence / 2);

        // Debug.Log("Total Damage : " + l_fDamage); 

        // Calculate hit chance by reducing starting hit chance using target's speed. 
        l_fHitChance = m_fHit - (targetForAttack.GetComponent<Unit_Attack>().m_fSpeed * 2);

        // Debug.Log("Total Chance To Hit : " + l_fHitChance);

        // Generate random number, if the number is within the hit chance then the attack hits. 
        if (Random.Range(0, 100) <= l_fHitChance)
        {
            // Debug.Log("Target Hit");

            // Target takes damage. 
            targetForAttack.GetComponent<Health_Management>().m_TakeHit(l_fDamage); 
        }
        else
        {
            // Debug.Log("Target Missed");
        }

        // Counter Attack. 

        // Same as above but targets are backwards. 

        l_fDamage = targetForAttack.GetComponent<Unit_Attack>().m_fAttack - (m_fDefence / 2);

        // Debug.Log("Total Damage : " + l_fDamage);

        l_fHitChance = targetForAttack.GetComponent<Unit_Attack>().m_fHit - (m_fSpeed * 2);

        // Debug.Log("Total Chance To Hit : " + l_fHitChance);

        if (Random.Range(0, 100) <= l_fHitChance)
        {
            // Debug.Log("Target Hit");

            gameObject.GetComponent<Health_Management>().m_TakeHit(l_fDamage);
        }
        else
        {
            // Debug.Log("Target Missed");
        }
    }

    public float m_GetAttackRange() => m_fAttackRange; 

    public void m_SetWithinAtRange(bool newValue)
    {
        m_bWithinRange = newValue;

        if (newValue == true)
        {
            gameObject.GetComponent<Renderer>().material.color = Color.gray;
        }
        else
        {
            gameObject.GetComponent<Renderer>().material.color = Color.white;
        }
    }

    public bool m_GetSelectedForAttack()
    {
        if(m_bSelectedForAttack == true)
        {
            m_bSelectedForAttack = false; 

            return true;
        }

        return false;
    }

    private void OnMouseOver()
    {
        if (m_bWithinRange == true)
        {
            if (Input.GetMouseButton(0))
            {
                m_bSelectedForAttack = true; 
            }
        }
    }
}
