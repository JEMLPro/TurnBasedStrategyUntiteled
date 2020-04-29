using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health_Management : MonoBehaviour
{
    [SerializeField]
    float m_fHealth = 0;

    [SerializeField]
    float m_fMaxHealth = 0; 

    public void m_SetMaxHealth(float newMaxHealth)
    {
        // Used to assign a new max health for this game object. 

        m_fMaxHealth = newMaxHealth; 
    }

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
    }

    // Allows access to the current health value. 
    public float m_GetCurrentHealth() => m_fHealth; 

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
    }

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
