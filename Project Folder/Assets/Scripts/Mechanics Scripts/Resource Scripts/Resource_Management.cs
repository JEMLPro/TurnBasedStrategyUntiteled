using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class will allow for resouces to be added and managed by a plaer in the game. 
/// </summary>
public class Resource_Management : MonoBehaviour
{
    //--------------------------------------------------------------------------------------------------------------\\
    // Data Members 
    //--------------------------------------------------------------------------------------------------------------\\

    /// <summary>
    /// This will hold the current value of iron held by the controlled player. 
    /// The iron will be needed to create weapons for the addition of new units.
    /// </summary>
    [SerializeField]
    float m_fIron = 0;

    /// <summary>
    /// This will be used to display the current iron value onto the screen. 
    /// </summary>
    [SerializeField]
    Text m_IronText;

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
    /// This will display the current food value onto the screen. 
    /// </summary>
    [SerializeField]
    Text m_FoodText;

    /// <summary>
    /// This will hold the current value of gold held by the controlled player. 
    /// The gold will act as a maintance cost of building, to own more buldings means that 
    /// more gold will need to be produced. 
    /// 
    /// If there is no gold then the buildings will slowly lose health. 
    /// </summary>
    [SerializeField]
    float m_fGold = 0;

    /// <summary>
    /// This will display the current gold value onto the screen. 
    /// </summary>
    [SerializeField]
    Text m_GoldText;

    //--------------------------------------------------------------------------------------------------------------\\
    // Member Functions
    //--------------------------------------------------------------------------------------------------------------\\

    //--------------------------------------------------------------------------------------------------------------\\
    // Manage Iron Resource
    //--------------------------------------------------------------------------------------------------------------\\

    /// <summary>
    /// This will increase this resource by a certain value. 
    /// </summary>
    /// <param name="addAmount"> This is the amount the resource will increase by. </param>
    public void m_AddToIron(float addAmount)
    {
        m_fIron += addAmount;

        Debug.Log(addAmount + " has been added to iron, new value = " + m_fIron);

        if (m_IronText != null)
        {
            string l_sText = "Iron : " + m_fIron.ToString();

            m_IronText.text = l_sText;
        }
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

    /// <summary>
    /// This will be used to set the text element used to display the iron value.
    /// </summary>
    /// <param name="newTextElement">This is the new text element controlled by the Interface manager.</param>
    public void m_SetIronText(Text newTextElement)
    {
        m_IronText = newTextElement;

        m_IronText.text = "Iron : " + m_fIron.ToString();
    }

    //--------------------------------------------------------------------------------------------------------------\\
    // Manage Food Resource
    //--------------------------------------------------------------------------------------------------------------\\

    /// <summary>
    /// This will increase this resource by a certain value. 
    /// </summary>
    /// <param name="addAmount"> This is the amount the resource will increase by. </param>
    public void m_AddToFood(float addAmount)
    {
        m_fFood += addAmount;

        Debug.Log(addAmount + " has been added to food, new value = " + m_fFood);

        if (m_FoodText != null)
        {
            string l_sText = "Food : " + m_fFood.ToString();

            m_FoodText.text = l_sText;
        }
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

    /// <summary>
    /// This will be used to set the text element used to display the food value.
    /// </summary>
    /// <param name="newTextElement">This is the new text element controlled by the Interface manager.</param>
    public void m_SetFoodText(Text newTextElement)
    {
        m_FoodText = newTextElement;

        m_FoodText.text = "Food : " + m_fFood.ToString();
    }

    //--------------------------------------------------------------------------------------------------------------\\
    // Manage Gold Resource
    //--------------------------------------------------------------------------------------------------------------\\

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

        if (m_GoldText != null)
        {
            string l_sText = "Gold : " + m_fGold.ToString();

            m_GoldText.text = l_sText;
        }
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

    /// <summary>
    /// This will be used to set the text element used to display the gold value.
    /// </summary>
    /// <param name="newTextElement">This is the new text element controlled by the Interface manager.</param>
    public void m_SetGoldText(Text newTextElement)
    {
        m_GoldText = newTextElement;

        m_GoldText.text = "Gold : " + m_fGold.ToString();
    }

    //--------------------------------------------------------------------------------------------------------------\\
    // File End 
    //--------------------------------------------------------------------------------------------------------------\\
}
