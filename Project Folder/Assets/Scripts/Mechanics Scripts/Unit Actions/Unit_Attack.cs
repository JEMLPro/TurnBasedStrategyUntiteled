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
    float m_fNumberOfAttacks = 1; 

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

        bool l_bAdvantage = m_CheckForAdvantage(targetForAttack.GetComponent<Unit_Attack>().m_UnitType); 

        // Attacking Unit Attacks. 

        // Calculate damage by reducing attack value by target's defence value. 
        l_fDamage = m_fAttack - (targetForAttack.GetComponent<Unit_Attack>().m_fDefence / 2);

        if(l_bAdvantage == true)
        {
            l_fDamage *= 2; 
        }

        Debug.Log("Total Damage : " + l_fDamage); 

        // Calculate hit chance by reducing starting hit chance using target's speed. 
        l_fHitChance = m_fHit - (targetForAttack.GetComponent<Unit_Attack>().m_fSpeed * 2);

        if (l_bAdvantage == true)
        {
            l_fHitChance *= 1.05f;
        }

        Debug.Log("Total Chance To Hit : " + l_fHitChance);

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

        l_bAdvantage = m_CheckForAdvantage(gameObject.GetComponent<Unit_Attack>().m_UnitType);

        l_fDamage = targetForAttack.GetComponent<Unit_Attack>().m_fAttack - (m_fDefence / 2);

        if (l_bAdvantage == true)
        {
            l_fDamage *= 2;
        }

        Debug.Log("Total Damage : " + l_fDamage);

        l_fHitChance = targetForAttack.GetComponent<Unit_Attack>().m_fHit - (m_fSpeed * 2);

        if (l_bAdvantage == true)
        {
            l_fHitChance *= 1.05f;
        }

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

    public float m_GetAttackRange() => m_fAttackRange;

    public float m_GetNumberOfAttacks() => m_fNumberOfAttacks; 

    public void m_SetNumberOfAttacks(float newNumberOfAttacks)
    {
        m_fNumberOfAttacks = newNumberOfAttacks;
    }

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

    bool m_CheckForAdvantage(UnitType otherUnitType)
    {
        if((m_UnitType == UnitType.Spear && otherUnitType == UnitType.Sword) ||
            (m_UnitType == UnitType.Sword && otherUnitType == UnitType.Axe) ||
            (m_UnitType == UnitType.Axe && otherUnitType == UnitType.Spear))
        {
            Debug.Log("This Unit has advantage");

            return true;
        }
        else
        {
            Debug.Log("This Unit has disadvantage");

            return false; 
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
        // Used to check if the mouse is hovering over this. 

        if (m_bWithinRange == true)
        {
            // If the unit is within range of an attack they can attack. 

            if (Input.GetMouseButton(0))
            {
                m_bSelectedForAttack = true; 
            }
        }
    }
}
