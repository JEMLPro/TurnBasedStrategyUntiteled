using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//---------------------------------------------------------------------------------------------------------------------------\\
// File Start 
//---------------------------------------------------------------------------------------------------------------------------\\

/// <summary>
/// This will add a health variable onto an object, this will alow for an object to both live and die. 
/// </summary>
public class Health_Management : MonoBehaviour
{
    //---------------------------------------------------------------------------------------------------------------------------\\
    // Data Members Start
    //---------------------------------------------------------------------------------------------------------------------------\\

    /// <summary>
    /// This is the object's current helath value and will be the main one which will be manipulated. 
    /// </summary>
    [SerializeField]
    float m_fHealth = 0;

    /// <summary>
    /// This is the object's maximum helath value and is mainly used at both object start-up or if the object is healed at 
    /// any point. 
    /// </summary>
    [SerializeField]
    float m_fMaxHealth = 0; 

    /// <summary>
    /// A slider can be used as a visual display for the current health value, the Maximum health acting as it's max value 
    /// and the health acting as its current value. Whenever the health value is changed make sure to update this to ensure 
    /// the UI displays proper values. 
    /// </summary>
    [SerializeField]
    Slider m_HealthBar = null;

    //---------------------------------------------------------------------------------------------------------------------------\\
    // Member Function Start 
    //---------------------------------------------------------------------------------------------------------------------------\\

    /// <summary>
    /// On object start the start script will be used to set up the object's helath bar slider, if one is attached. 
    /// </summary>
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

    /// <summary>
    /// This will be used to dynamically set the maximum health for this object. 
    /// </summary>
    /// <param name="newMaxHealth">This is the new value for the object's max health. </param>
    public void m_SetMaxHealth(float newMaxHealth)
    {
        // Used to assign a new max health for this game object. 

        m_fMaxHealth = newMaxHealth; 

        if(m_fHealth > m_fMaxHealth)
        {
            // This will be used to ensure the current health isn't already higher than the maximum health. 

            m_fHealth = m_fMaxHealth;
        }

        // Update the health slider if one is connected. 

        if(m_HealthBar != null)
        {
            m_HealthBar.maxValue = newMaxHealth; 
        }
    }

    /// <summary>
    /// This will be used to dynamically set the currnt health value; this should't be used too often.
    /// </summary>
    /// <param name="newHealth">The new health value. </param>
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

        // If one is connected update the health slider. 

        if (m_HealthBar != null)
        {
            m_HealthBar.value = m_fHealth;

            if(m_HealthBar.maxValue != m_fMaxHealth)
            {
                m_HealthBar.maxValue = m_fMaxHealth; 
            }
        }
    }

    /// <summary>
    /// Allows access to the current health value. 
    /// </summary>
    /// <returns>Current health value.</returns>
    public float m_GetCurrentHealth() => m_fHealth; 

    /// <summary>
    /// This function represents incoming damage to the connected object; this is the most common manipulation for the 
    /// current health value. 
    /// 
    /// This function will also check if the object has run out of health, in that case the object will then be destroyed. 
    /// </summary>
    /// <param name="damage">The incoming damage to this object. This is removed from the current health value. </param>
    public void m_TakeHit(float damage)
    {
        // This will take the damage off the health value. 

        m_fHealth -= damage;

        // If the new health woud equal less than 0 it becomes 0.

        if(m_fHealth < 0)
        {
            m_fHealth = 0; 

            // If the object's health is at 0 then the object will be destroyed. 

            if(m_fHealth == 0)
            {
                Destroy(gameObject); 
            }
        }

        // IF present update the health slider. 

        if (m_HealthBar != null)
        {
            m_HealthBar.value = m_fHealth;
        }
    }

    /// <summary>
    /// This will be used to check if the object's health value is at 0, allowing for external functions to check if the object
    /// needs to be removed or proccessed in other ways.
    /// </summary>
    /// <returns>True if health is at 0, otherwise false. </returns>
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

    //---------------------------------------------------------------------------------------------------------------------------\\
    // File End
    //---------------------------------------------------------------------------------------------------------------------------\\
}
