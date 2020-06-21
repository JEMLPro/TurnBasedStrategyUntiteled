using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource_Management : MonoBehaviour
{
    /// <summary>
    /// This will hold the current value of iron held by the controlled player. 
    /// The iron will be needed to create weapons for the addition of new units.
    /// </summary>
    [SerializeField]
    float m_fIron = 0;

    /// <summary>
    /// This will hold the current value of food held by the controlled player.
    /// The food will act as a maintanence cost of current units, the more units owned 
    /// means food per turn increases. 
    /// 
    /// If there is no food at the end of the turn the units affected will begin to lose health. 
    /// </summary>
    [SerializeField]
    float m_fFood = 0;

    /// <summary>
    /// This will hold the current value of gold held by the controlled player. 
    /// The gold will act as a maintance cost of building, to own more buldings means that 
    /// more gold will need to be produced. 
    /// 
    /// If there is no gold then the buildings will slowly lose health. 
    /// </summary>
    [SerializeField]
    float m_fGold = 0; 

    // Manage Iron Resource

    /// <summary>
    /// This will increase this resource by a certain value. 
    /// </summary>
    /// <param name="addAmount"> This is the amount the resource will increase by. </param>
    public void m_AddToIron(float addAmount)
    {
        m_fIron += addAmount;

        Debug.Log(addAmount + " has been added to iron, new value = " + m_fIron); 
    }

    /// <summary>
    /// This will decrease this resource by a certain value. 
    /// </summary>
    /// <param name="removeAmount">The amount to decrease by. </param>
    public void m_RemoveFromIron(float removeAmount)
    {
        m_fIron -= removeAmount;

        if(m_fIron < 0)
        {
            m_fIron = 0; 
        }

        Debug.Log(removeAmount + " has been removed from iron, new value = " + m_fIron);
    }

    /// <summary>
    /// This will allow access to this resource. 
    /// </summary>
    /// <returns>Current Iron value</returns>
    public float m_GetIron() => m_fIron;

    // Manage Food Resource

    /// <summary>
    /// This will increase this resource by a certain value. 
    /// </summary>
    /// <param name="addAmount"> This is the amount the resource will increase by. </param>
    public void m_AddToFood(float addAmount)
    {
        m_fFood += addAmount;

        Debug.Log(addAmount + " has been added to food, new value = " + m_fFood);
    }

    /// <summary>
    /// This will decrease this resource by a certain value. 
    /// </summary>
    /// <param name="removeAmount">The amount to decrease by. </param>
    public void m_RemoveFromFood(float removeAmount)
    {
        m_fFood -= removeAmount;

        if (m_fIron < 0)
        {
            m_fIron = 0;
        }

        Debug.Log(removeAmount + " has been removed from food, new value = " + m_fFood);
    }

    /// <summary>
    /// This will allow access to this resource. 
    /// </summary>
    /// <returns>Current Food value</returns>
    public float m_GetFood() => m_fFood;

    // Manage Gold Resource

    /// <summary>
    /// This will increase this resource by a certain value. 
    /// </summary>
    /// <param name="addAmount"> This is the amount the resource will increase by. </param>
    public void m_AddToGold(float addAmount)
    {
        m_fGold += addAmount;

        if(m_fGold < 0)
        {
            m_fGold = 0; 
        }

        Debug.Log(addAmount + " has been added to gold, new value = " + m_fGold);
    }

    /// <summary>
    /// This will decrease this resource by a certain value. 
    /// </summary>
    /// <param name="removeAmount">The amount to decrease by. </param>
    public void m_RemoveFromGold(float removeAmount)
    {
        m_fGold -= removeAmount;

        if (m_fIron < 0)
        {
            m_fIron = 0;
        }

        Debug.Log(removeAmount + " has been removed from gold, new value = " + m_fGold);
    }

    /// <summary>
    /// This will allow access to this resource. 
    /// </summary>
    /// <returns>Current Gold value</returns>
    public float m_GetGold() => m_fGold;
}
