using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is the type assigned to this current unit, each unit will have an advantage against another unit, this will allow 
/// for it to be checked and calculated. 
/// </summary>
[System.Serializable]
public enum UnitType
{
    Militia,    // Doesn't gain any advantages during combat. 
    Spear,      // Has high defence low speed and attack. 
    Sword,      // has high speed and hit chance but low defence and attack power. 
    Axe,        // Has high attack power yet low hit chance and defence. 
    Bow,        // Has low stats but has an attack range of two allowing for it to attack at a distance and not recive a counter atack. 
    Engineer    // Has low stats but is able to create new buildings. 
}

/// <summary>
/// This will be used to manage the attacking between two entities within the game, it will also contain the unit's stats
/// allowing for them to be managed easily. 
/// </summary>
public class Unit_Attack : MonoBehaviour
{
    #region Data Members 

    #region Basic Stats

    [Header("Unit Stats")]

    /// <summary>
    /// The basic attack power for the unit, this will be adjusted by the enemies defence to calculate the final 
    /// damage dealt to the enemy. The higher this value the more damage this unit will deal. 
    /// </summary>
    [SerializeField]
    float m_fAttack = 0; 

    /// <summary>
    /// This is the defence for this unit, it will determine how much of the attack from the opponent is removed. The 
    /// higher this value is the less damage the unit recives.
    /// </summary>
    [SerializeField]
    float m_fDefence = 0; 

    /// <summary>
    /// This is the percentage chance the unit will hit the target enemy. It will be affected by the enemies speed; a 
    /// faster enemy is harder to hit. 
    /// </summary>
    [SerializeField]
    float m_fHit = 100; 

    /// <summary>
    /// This is the speed of this unit, it's purpose it to reduce the oponent's chance to hit this unit. The higher this 
    /// value equates to a smaller chance of this unit being hit.
    /// </summary>
    [SerializeField]
    float m_fSpeed = 0;

    /// <summary>
    /// This is the type assined to this unit. This will determine Whether it will have advantage during the attack; advantage 
    /// during an attack will improve the units base stats allowing for a better outcome tha expected. 
    /// </summary>
    [SerializeField]
    UnitType m_UnitType = UnitType.Militia;

    #endregion

    #region Combat Resoulution Values

    [Header("Combat Resolution Values")]

    /// <summary>
    /// This is the tile range this unit has. The unit will not be able to attack outside of this distance. 
    /// </summary>
    [SerializeField]
    float m_fAttackRange = 1; 

    /// <summary>
    /// This is the limit of attacks the unit is able to make during a single turn. 
    /// </summary>
    [SerializeField]
    float m_fNumberOfAttacks = 1; 

    /// <summary>
    /// This is used to check if this unit is within the range of another unit which is making an attack. 
    /// </summary>
    [SerializeField]
    bool m_bWithinRange = false; 

    /// <summary>
    /// This will be used to check if this unit has been selected for an attack. 
    /// </summary>
    [SerializeField]
    bool m_bSelectedForAttack = false; 

    /// <summary>
    /// The summation of the unit's stats. This is mainly used by the AI to choose an attack target within the game. 
    /// </summary>
    [SerializeField]
    float m_fCombatRating;

    #endregion

    #endregion

    #region Member Functions

    #region Get Stats

    /// <summary>
    /// This will allow access to this unit's current attack stat. 
    /// </summary>
    /// <returns></returns>
    public float m_GetAttack() => m_fAttack;

    /// <summary>
    /// This will allow access to this unit's current hit chance stat. 
    /// </summary>
    /// <returns></returns>
    public float m_GetHitChance() => m_fHit;

    /// <summary>
    /// This will allow access to this unit's current speed stat. 
    /// </summary>
    /// <returns></returns>
    public float m_GetSpeed() => m_fSpeed;

    /// <summary>
    /// This will allow access to this unit's current defence stat. 
    /// </summary>
    /// <returns></returns>
    public float m_GetDefence() => m_fDefence;

    #endregion

    #region Combat Resolution

    /// <summary>
    /// This is the basic form of comabt for the game, it will be used to resolve combat between two units 
    /// at a single point in the game and will take both unit's stats into account. 
    /// </summary>
    /// <param name="targetForAttack">The unit this unit will be attacking. </param>
    public void m_AttackTarget(GameObject targetForAttack)
    {
        // Init Local Variables 

        float l_fDamage, l_fHitChance;

        bool l_bAdvantage = m_CheckForAdvantage(targetForAttack.GetComponent<Unit_Attack>().m_UnitType);

        #region Main Attack

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

        #endregion

        #region Counter Attack

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

        #endregion
    }

    /// <summary>
    /// This will allow for the unit to attck an enemy owned building within the game. 
    /// </summary>
    /// <param name="targetForAttack">The building this unit will be attacking. </param>
    public void m_AttackBuildingTarget(GameObject targetForAttack)
    {
        targetForAttack.GetComponent<Health_Management>().m_TakeHit(m_fAttack); 

        if(targetForAttack != null)
        {
            // This will check to see if the building has been defeted at the end of the combat resolution
            // and then free up the cell allowing for the cell to be moved onto again. 

            if(targetForAttack.GetComponent<Health_Management>().m_CheckForDeath())
            {
                targetForAttack.GetComponent<Building_Positioning>().m_GetPosition().GetComponent<Cell_Manager>().m_bSetObsticle(false); 
            }
        }
    }

    /// <summary>
    /// This will be used to check of this unit has advange over the opposing unit. 
    /// </summary>
    /// <param name="otherUnitType">The type the other unit has been assigned. </param>
    /// <returns></returns>
    public bool m_CheckForAdvantage(UnitType otherUnitType)
    {
        if ((m_UnitType == UnitType.Spear && otherUnitType == UnitType.Sword) ||
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

    #endregion

    #region Unit Stat Checking and Assignment

    /// <summary>
    /// This allow access to this unit's attack range. 
    /// </summary>
    /// <returns></returns>
    public float m_GetAttackRange() => m_fAttackRange;

    /// <summary>
    /// This will allow access to the number of attacks this unit can make. 
    /// </summary>
    /// <returns></returns>
    public float m_GetNumberOfAttacks() => m_fNumberOfAttacks; 

    /// <summary>
    /// This will be used to assign the number of attacks this unit can make this turn. 
    /// </summary>
    /// <param name="newNumberOfAttacks">The new number of attacks this unit can make. </param>
    public void m_SetNumberOfAttacks(float newNumberOfAttacks)
    {
        m_fNumberOfAttacks = newNumberOfAttacks;
    }

    /// <summary>
    /// This will allow access to this unit's type. 
    /// </summary>
    /// <returns></returns>
    public UnitType m_GetUnitType() => m_UnitType;

    /// <summary>
    /// Thsi will be used to assign a unit their stats, this will mainly be used during the unit spawning process. 
    /// </summary>
    /// <param name="attack">The attack value for this unit. </param>
    /// <param name="defence">The defence value for this unit. </param>
    /// <param name="hitChance">The percantage hit chance for this unit. </param>
    /// <param name="speed">The speed value for this unit. </param>
    /// <param name="attackRange">The attack radius for this unit. </param>
    /// <param name="type">The type assignment for this unit. </param>
    public void m_SetUnitStats(float attack, float defence, float hitChance, float speed, float attackRange, UnitType type)
    {
        m_fAttack = attack;
        m_fDefence = defence;
        m_fHit = hitChance;
        m_fSpeed = speed;
        m_fAttackRange = attackRange;
        m_UnitType = type;
    }

    #endregion

    #region Unit Targeting and Selection

    /// <summary>
    /// This will update the targeting status for this unit, it will allow for theis unit to be targeted 
    /// for attacks.
    /// </summary>
    /// <param name="newValue">If this unit is within attack range of the attacking unit. </param>
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

    /// <summary>
    /// This will return true if this unit is both within range and has been selected. 
    /// </summary>
    /// <returns></returns>
    public bool m_GetSelectedForAttack()
    {
        if(m_bSelectedForAttack == true)
        {
            m_bSelectedForAttack = false; 

            return true;
        }

        return false;
    }

    /// <summary>
    /// This will be used to update the selected for attack value, signifying that this unit will 
    /// be attacked by the attacking unit. 
    /// </summary>
    /// <param name="newValue">True if this unit has been selected. </param>
    public void m_SetSelectedForAttack(bool newValue)
    {
        m_bSelectedForAttack = newValue; 
    }

    /// <summary>
    /// This will be used to calculate this unit's combat rating, it will add up all of the unit's 
    /// stats. 
    /// </summary>
    public void m_CalculateCombatRating()
    {
        m_fCombatRating = gameObject.GetComponent<Health_Management>().m_GetCurrentHealth() + m_fAttack + m_fDefence;
    }

    /// <summary>
    /// This will allow access to this unit's combat rating. 
    /// </summary>
    /// <returns></returns>
    public float m_GetCombatRating() => m_fCombatRating;

    /// <summary>
    /// This will assign a new value for the combat rating, this will mainly be used to adjust the base rating with
    /// the distance between units allowing for a more accurate targeting system. 
    /// </summary>
    /// <param name="newRating">The new rating for this unit. </param>
    public void m_SetCombatRating(float newRating) { m_fCombatRating = newRating; }
    
    /// <summary>
    /// This will allow for this unit to be targted by the player using the mouse, only when the unit is within 
    /// attack range. 
    /// </summary>
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

    #endregion

    #endregion
}
