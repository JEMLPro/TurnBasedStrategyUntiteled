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

    [SerializeField]
    GameObject m_InfoPanel;

    [SerializeField]
    GameObject m_UnitBuildMenu; 

    // Set up UI functionality. 

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

        if(m_InfoPanel == null)
        {
            Debug.Log("Finding Info Panel.");

            m_LevelSelectDropdownMenu = GameObject.FindGameObjectWithTag("Info_Panel");
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

            m_EndTurnButton.gameObject.SetActive(true);

            m_EndTurnButton.onClick.RemoveAllListeners();

            m_EndTurnButton.onClick.AddListener(delegate { turnManager.GetComponent<Turn_Manager>().m_SwitchTurn(); }); 
        }
        else
        {
            Debug.LogError("Unable to access End Turn Button"); 
        }
    }

    public void m_SetUpInfoPanel()
    {
        m_InfoPanel.SetActive(true); 
    }

    public void m_SetUpUnitSpanwing(GameObject playerUnitManager, GameObject playerBuildingManager)
    {
        m_UnitBuildMenu.SetActive(true); 

        GameObject l_buttonOfFocus = null;

        // Setup militia spawing 

        l_buttonOfFocus = GameObject.FindGameObjectWithTag("Militia_Button");

        if (l_buttonOfFocus != null)
        {
            l_buttonOfFocus.GetComponent<Button>().onClick.RemoveAllListeners();

            l_buttonOfFocus.GetComponent<Button>().onClick.AddListener(delegate
            {

                GameObject l_NewUnit = null;

                GameObject l_SpawnPoint = playerBuildingManager.GetComponent<Bulding_Manager>().m_GetSpawnPointForNewSpawnedUnit();

                if (l_SpawnPoint != null)
                {
                    l_NewUnit = playerUnitManager.GetComponent<Unit_Spwaning>().m_SpawnMilitiaUnit(l_SpawnPoint);

                    if (l_NewUnit != null)
                    {
                        playerUnitManager.GetComponent<Unit_Manager>().m_AddUnitIntoList(l_NewUnit);
                    }
                }
                else
                {
                    Debug.Log("No Free Spawn Points"); 
                }
            });
        }
        else
        {
            Debug.LogError("Error code 1010-0 - Problem setting up the button 'Militia Button'.");
        }
    }

    // Getters for UI elements 

    public Text m_GetIronText()
    {
        return GameObject.FindGameObjectWithTag("Iron_Text").GetComponentInChildren<Text>();
    }

    public Text m_GetFoodText()
    {
        return GameObject.FindGameObjectWithTag("Food_Text").GetComponentInChildren<Text>();
    }

    public Text m_GetGoldText()
    {
        return GameObject.FindGameObjectWithTag("Gold_Text").GetComponentInChildren<Text>();
    }

    public GameObject m_GetRadialMenu() => m_RadialMenu;

    public GameObject m_GetGameOverScreen() => m_GameOverScreen; 

    public RectTransform m_GetMainCanvas() => m_MainCanvas;

    public GameObject m_GetLevelSelectDropDown() => m_LevelSelectDropdownMenu;

    public GameObject m_GetUnitBuildMenu() => m_UnitBuildMenu;

    

}
