using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interface_Controller : MonoBehaviour
{
    [SerializeField]
    Button m_EndTurnButton;

    [SerializeField]
    GameObject m_RadialMenu;

    [SerializeField]
    RectTransform m_MainCanvas; 

    private void Start()
    {
        if (m_MainCanvas == null)
        {
            Debug.Log("Finding Main Canvas.");

            m_MainCanvas = GameObject.FindGameObjectWithTag("Main_Canvas").GetComponent<RectTransform>();
        }

        if (m_EndTurnButton == null)
        {
            Debug.Log("Finding End turn button.");

            m_EndTurnButton = GameObject.FindGameObjectWithTag("End_Turn_Button").GetComponent<Button>();
        }

        if (m_RadialMenu == null)
        {
            Debug.Log("Finding Action Radial Menu.");

            m_RadialMenu = GameObject.FindGameObjectWithTag("Action_Radial_Menu");
        }
    }

    public void m_SetEndTurnFunctionality(GameObject turnManager)
    {
        if (m_EndTurnButton != null)
        {
            Debug.Log("Setting up End Turn button");

            m_EndTurnButton.onClick.AddListener(delegate { turnManager.GetComponent<Turn_Manager>().m_SwitchTurn(); }); 
        }
        else
        {
            Debug.LogError("Unable to access End Turn Button"); 
        }
    }

    public GameObject m_GetRadialMenu() => m_RadialMenu;

    public RectTransform m_GetMainCanvas() => m_MainCanvas; 

}
