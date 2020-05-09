using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*! \enum This is the type of attacks the unit will use and there are advantages and disadvantages to each.  */
enum UnitType
{
    Militia, // Doesn't gain any advantages during combat. 
    Spear, // Has high defence low speed and attack. 
    Sword, // has high speed and hit chance but low defence and attack power. 
    Axe, // Has high attack power yet low hit chance and defence. 
}

/*! \class This will manage the attacking and combat resolution during the player's turn. */
public class Unit_Attack : MonoBehaviour
{
    [SerializeField]
    float m_fAttack = 0; /*!< \var This is the attack power of this unit. */

    [SerializeField]
    float m_fDefence = 0; /*!< \var This is the defence of this unit. */

    [SerializeField]
    float m_fHit = 100; /*!< \var This is the base hit chance for this unit, starts at 100%. */

    [SerializeField]
    float m_fSpeed = 0; /*!< \var This is the speed of the unit, this determines the chance this unit has to dodge an attack. */

    [SerializeField]
    float m_fAttackRange = 1; /*!< \var This is the cell range this unit is able to attack, the attack target must be at this range to be targeted. */ 

    [SerializeField]
    float m_fNumberOfAttacks = 1; /*!< \var This is the base number of attacks this unit as able to do per turn. */

    [SerializeField]
    bool m_bWithinRange = false; /*!< \var This will be used to check if this unit is within range for an attack. */

    [SerializeField]
    bool m_bSelectedForAttack = false; /*!< \var This will be used to check if this unit has been targeted for an attack. */ 

    [SerializeField]
    UnitType m_UnitType = UnitType.Militia; /*!< \var This is this unit's type, used to check for advantage during combat. */

    [SerializeField]
    float m_fCombatRating;/*!< \var This is the total power of this unit, used to select targets for the AI player. */

    //----------------------------------------------------------------------------------------------------------------------------------
    //      Member Functions Start. 
    //----------------------------------------------------------------------------------------------------------------------------------

    /*! \fn This unit will attack the object passed into this function. */
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
            Debug.Log("Target Hit");

            // Target takes damage. 
            targetForAttack.GetComponent<Health_Management>().m_TakeHit(l_fDamage); 
        }
        else
        {
            Debug.Log("Target Missed");
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

    /*! \fn This will output the attack range of this unit. */
    public float m_GetAttackRange() => m_fAttackRange;

    /*! \fn This will output the number of attacks this unit has. */
    public float m_GetNumberOfAttacks() => m_fNumberOfAttacks; 

    /*! \fn This will set the number of attacks this unit can perform this turn. */
    public void m_SetNumberOfAttacks(float newNumberOfAttacks)
    {
        m_fNumberOfAttacks = newNumberOfAttacks;
    }

    /*! \fn This will change the value of within range, as well as adjusting the colours of the object. */
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

    /*! \fn This will check if this object has advantage over any given type. */
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

    /*! \fn This will check if this unit has been seleted for an attack. */
    public bool m_GetSelectedForAttack()
    {
        if(m_bSelectedForAttack == true)
        {
            m_bSelectedForAttack = false; 

            return true;
        }

        return false;
    }

    public void m_CalculateCombatRating()
    {
        m_fCombatRating = gameObject.GetComponent<Health_Management>().m_GetCurrentHealth() + m_fAttack + m_fDefence;
    }

    public float m_GetCombatRating() => m_fCombatRating; 

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
