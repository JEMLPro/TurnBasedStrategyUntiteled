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

    [SerializeField]
    GameObject m_GameOverScreen;

    [SerializeField]
    GameObject m_LevelSelectDropdownMenu; 

    public void m_StartUp()
    {
        // Find Objects. 

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

        if(m_GameOverScreen == null)
        {
            Debug.Log("Finding Game Over Screen.");

            m_GameOverScreen = GameObject.FindGameObjectWithTag("Game_Over_Screen");

            if(m_GameOverScreen.activeSelf == true)
            {
                m_GameOverScreen.SetActive(false);
            }
        }

        if(m_LevelSelectDropdownMenu == null)
        {
            Debug.Log("Finding Level Select Dropdown list.");

            m_LevelSelectDropdownMenu = GameObject.FindGameObjectWithTag("Level_Select_DropDown");
        }

        // Set-Up Basic Ui functionality

        if(m_LevelSelectDropdownMenu != null)
        {
            m_LevelSelectDropdownMenu.GetComponentInChildren<Attach_Levels_To_List>().m_StartUp();
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

    public GameObject m_GetGameOverScreen() => m_GameOverScreen; 

    public RectTransform m_GetMainCanvas() => m_MainCanvas;

    public GameObject m_GetLevelSelectDropDown() => m_LevelSelectDropdownMenu; 

}
