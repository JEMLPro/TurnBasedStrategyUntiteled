using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Open_Unit_Spawn_Menu : MonoBehaviour
{
    [SerializeField]
    bool m_bopenMenu = false;

    [SerializeField]
    GameObject m_MilitiaButton;

    [SerializeField]
    GameObject m_EngineerButton;

    [SerializeField]
    GameObject m_SwordsmanButton;

    [SerializeField]
    GameObject m_LancerButton;

    [SerializeField]
    GameObject m_MarauderButton;

    private void Start()
    {
        // Upon startup find all of the buttons if they are not already set. 

        if(m_MilitiaButton == null)
        {
            m_MilitiaButton = GameObject.FindGameObjectWithTag("Militia_Button");
        }

        if(m_EngineerButton == null)
        {
            m_EngineerButton = GameObject.FindGameObjectWithTag("Engineer_Button");
        }

        if(m_SwordsmanButton == null)
        {
            m_SwordsmanButton = GameObject.FindGameObjectWithTag("Swordsman_Button");
        }

        if(m_LancerButton == null)
        {
            m_LancerButton = GameObject.FindGameObjectWithTag("Lancer_Button");
        }

        if(m_MarauderButton == null)
        {
            m_MarauderButton = GameObject.FindGameObjectWithTag("Marauder_Button");
        }

        m_HideAll(); 
    }

    public void m_HQSelected()
    {
        gameObject.SetActive(true);

        m_MilitiaButton.SetActive(true);

        m_EngineerButton.SetActive(true);
    }

    public void m_HideAll()
    {
        m_MilitiaButton.SetActive(false);
        m_EngineerButton.SetActive(false);
        m_SwordsmanButton.SetActive(false);
        m_LancerButton.SetActive(false);
        m_MarauderButton.SetActive(false);

        gameObject.SetActive(false);
    }

    public bool m_Visable()
    {
        if(gameObject.activeSelf == true)
        {
            return true; 
        }

        return false;
    }
}
