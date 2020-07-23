using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//---------------------------------------------------------------------------------------------------------------------------\\
// File Start
//---------------------------------------------------------------------------------------------------------------------------\\

/// <summary>
/// This will be used to contain all UI elements and allow for them to be connected to the appropriate GameObjects within 
/// the game. 
/// </summary>
public class Interface_Controller : MonoBehaviour
{
    //---------------------------------------------------------------------------------------------------------------------------\\
    // Data Members Start 
    //---------------------------------------------------------------------------------------------------------------------------\\

    // Other and Utility Elements \\ 

    /// <summary>
    /// This is the main canvas used by the UI to draw onto the screen. 
    /// </summary>
    [SerializeField]
    RectTransform m_MainCanvas;

    // In-Game UI Elements \\ 

    /// <summary>
    /// The End Turn button within the main game, this will be used for the player to end their turn. 
    /// </summary>
    [SerializeField]
    Button m_EndTurnButton;

    /// <summary>
    /// This is the player's action menu, it will allow for the player to direct their units within the game. 
    /// </summary>
    [SerializeField]
    GameObject m_RadialMenu;

    /// <summary>
    /// Once either of the HQs are destroyed this will be displayed and updated to display the correct info. 
    /// </summary>
    [SerializeField]
    GameObject m_GameOverScreen;

    /// <summary>
    /// This is the info panel which will display the important info to the player such as turn, and resource into.
    /// </summary>
    [SerializeField]
    GameObject m_InfoPanel;

    /// <summary>
    /// This is a menu of buttons which will allow for the player to build new units within the game, differnt buttons 
    /// will be displayed depending on which building has been selected. 
    /// </summary>
    [SerializeField]
    GameObject m_UnitBuildMenu;

    // Menu UI Elements \\

    /// <summary>
    /// This is the level select dropdown list, it will contain a full list of all of the levels within the game, it will 
    /// be populated upon start-up and is the player's main navigation for starting games as new option needs to be selected 
    /// for a new game to start.
    /// </summary>
    [SerializeField]
    GameObject m_LevelSelectDropdownMenu;

    //---------------------------------------------------------------------------------------------------------------------------\\
    // Member Functions Start
    //---------------------------------------------------------------------------------------------------------------------------\\

    // Set up UI functionality. 

    /// <summary>
    /// This will be used to set up and linked all of the user interface items within the game. 
    /// </summary>
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

    /// <summary>
    /// This will be used to assign the functionality of the end turn button. 
    /// </summary>
    /// <param name="turnManager">The manager of the turn based system in the game. </param>
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

    /// <summary>
    /// This will be used to set up the player's info panel. 
    /// </summary>
    public void m_SetUpInfoPanel()
    {
        m_InfoPanel.SetActive(true); 
    }

    /// <summary>
    /// This will be used to set up the unit spawning buttons. 
    /// </summary>
    /// <param name="playerUnitManager">The player's unit manager. </param>
    /// <param name="playerBuildingManager">The player's building manager. </param>
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

    /// <summary>
    /// This will allow access to the Iron Text element. 
    /// </summary>
    /// <returns></returns>
    public Text m_GetIronText()
    {
        return GameObject.FindGameObjectWithTag("Iron_Text").GetComponentInChildren<Text>();
    }

    /// <summary>
    /// This will allow access to the Food Text element. 
    /// </summary>
    /// <returns></returns>
    public Text m_GetFoodText()
    {
        return GameObject.FindGameObjectWithTag("Food_Text").GetComponentInChildren<Text>();
    }

    /// <summary>
    /// This will allow access to the Gold Text element. 
    /// </summary>
    /// <returns></returns>
    public Text m_GetGoldText()
    {
        return GameObject.FindGameObjectWithTag("Gold_Text").GetComponentInChildren<Text>();
    }

    /// <summary>
    /// This will allow access to the player's radial action menu. 
    /// </summary>
    /// <returns></returns>
    public GameObject m_GetRadialMenu() => m_RadialMenu;

    /// <summary>
    /// This will allow access to the game over screen. 
    /// </summary>
    /// <returns></returns>
    public GameObject m_GetGameOverScreen() => m_GameOverScreen; 

    /// <summary>
    /// This will allow access to the main canvas for the game, which all of the other elements will be drawn onto. 
    /// </summary>
    /// <returns></returns>
    public RectTransform m_GetMainCanvas() => m_MainCanvas;

    /// <summary>
    /// This will allow access to the drop-down menu for the level select. 
    /// </summary>
    /// <returns></returns>
    public GameObject m_GetLevelSelectDropDown() => m_LevelSelectDropdownMenu;

    /// <summary>
    /// This will allow access to the unit build menu.
    /// </summary>
    /// <returns></returns>
    public GameObject m_GetUnitBuildMenu() => m_UnitBuildMenu;

    //---------------------------------------------------------------------------------------------------------------------------\\
    // File End
    //---------------------------------------------------------------------------------------------------------------------------\\

}
