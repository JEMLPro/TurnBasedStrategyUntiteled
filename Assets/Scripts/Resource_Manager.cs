using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Resource_Manager : MonoBehaviour
{
    [SerializeField]
    GameObject m_Owner = null;

    [SerializeField]
    float m_Gold = 0;

    [SerializeField]
    float m_Food = 0;

    [SerializeField]
    float m_Iron = 0;

    [SerializeField]
    Text m_GoldText;

    [SerializeField]
    Text m_FoodText;

    [SerializeField]
    Text m_IronText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (m_GoldText != null)
        {
            string l_GoldText = "Gold : ";

            if (m_GoldText.text != l_GoldText + m_Gold.ToString())
            {
                m_GoldText.text = l_GoldText + m_Gold.ToString();
            }
        }

        if (m_FoodText != null)
        {
            string l_FoodText = "Food : ";

            if (m_FoodText.text != l_FoodText + m_Food.ToString())
            {
                m_FoodText.text = l_FoodText + m_Food.ToString();
            }
        }

        if (m_IronText != null)
        {
            string l_IronText = "Iron : ";

            if (m_IronText.text != l_IronText + m_Iron.ToString())
            {
                m_IronText.text = l_IronText + m_Iron.ToString();
            }
        }
    }

    // Manage Gold

    public void m_AddToGold(float addAmount)
    {
        m_Gold += addAmount;
    }

    public float m_GetGold() => m_Gold;

    public bool m_RemoveGold(float removeAmount)
    {
        if (m_Gold - removeAmount >= 0)
        {
            m_Gold -= removeAmount;

            return true;
        }

        return false;
    }

    // Manage Food

    public void m_AddToFood(float addAmount)
    {
        m_Food += addAmount;
    }

    public float m_GetFood() => m_Food;

    public bool m_RemoveFood(float removeAmount)
    {
        if (m_Food - removeAmount >= 0)
        {
            m_Food -= removeAmount;

            return true;
        }

        return false;
    }

    // Manage Iron 

    public void m_AddToIron(float addAmount)
    {
        m_Iron += addAmount;
    }

    public float m_GetIron() => m_Iron;

    public bool m_RemoveIron(float removeAmount)
    {
        if(m_Iron - removeAmount >= 0)
        {
            m_Iron -= removeAmount;

            return true; 
        }

        return false;
    }
}
