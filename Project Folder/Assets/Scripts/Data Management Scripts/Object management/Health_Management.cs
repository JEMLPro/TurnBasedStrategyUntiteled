using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

/*! /class This class will allow for an object to be assigned a health value, it will also manage the updates needed for that health value.  */
public class Health_Management : MonoBehaviour
{
    [SerializeField]
    float m_fHealth = 0; /*!< \var This will be used to keep track of the object's current health value. */

    [SerializeField]
    float m_fMaxHealth = 0; /*!< \var This will store the object's maximum health value, this will prevent the health from reaching greater than this. */

    [SerializeField]
    Slider m_HealthBar = null;

    private void Start()
    {
        // Init Object 

        if (m_HealthBar != null)
        {
            // If the object has a health bar attached to it, update their values to be correct. 

            if (m_HealthBar.maxValue != m_fMaxHealth)
            {
                m_HealthBar.maxValue = m_fMaxHealth;
            }

            m_HealthBar.value = m_fHealth;
        }
    }

    // This will change the maimum health value.
    public void m_SetMaxHealth(float newMaxHealth)
    {
        // Used to assign a new max health for this game object. 

        m_fMaxHealth = newMaxHealth; 

        if(m_fHealth > m_fMaxHealth)
        {
            m_fHealth = m_fMaxHealth;
        }

        if(m_HealthBar != null)
        {
            m_HealthBar.maxValue = newMaxHealth; 
        }
    }

    // This will set the new current health. 
    public void m_SetCurrentHealth(float newHealth)
    {
        if(newHealth <= m_fMaxHealth)
        {
            // If the new health is less than the max possible health, the new health become the current health. 

            m_fHealth = newHealth;
        }
        else
        {
            // If the new health is greater than the max health the new health becomes the max. 

            m_fHealth = m_fMaxHealth;
        }

        if (m_HealthBar != null)
        {
            m_HealthBar.value = m_fHealth;

            if(m_HealthBar.maxValue != m_fMaxHealth)
            {
                m_HealthBar.maxValue = m_fMaxHealth; 
            }
        }
    }

    // Allows access to the current health value. 
    public float m_GetCurrentHealth() => m_fHealth; 

    // This will reduce the object's health by the value provided into this function. 
    public void m_TakeHit(float damage)
    {
        // This will take an amount off the health value. 

        m_fHealth -= damage;

        // If the new health woud equal less tahn 0 it becomes 0.

        if(m_fHealth < 0)
        {
            m_fHealth = 0; 

            if(m_fHealth == 0)
            {
                Destroy(gameObject); 
            }
        }

        if (m_HealthBar != null)
        {
            m_HealthBar.value = m_fHealth;
        }
    }

    // This will be used to check if this object's health value is at 0 or not. 
    public bool m_CheckForDeath()
    {
        // If the object's health has reached 0 (or less) this will return true for it being dead. 
        // This will allow for interaction outside this function. 

        if (m_fHealth <= 0)
        {
            return true;
        }

        return false;
    }

}
