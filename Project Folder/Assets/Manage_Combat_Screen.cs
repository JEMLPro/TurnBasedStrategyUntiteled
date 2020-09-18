using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

/// <summary>
/// This class will be mainly used to display and update the values displayed by the combat screen. 
/// </summary>
public class Manage_Combat_Screen : MonoBehaviour
{
    #region Data Members 

    [Header("Player Unit Stats")]

    /// <summary>
    /// This is the text element used to display the player's health.  
    /// </summary>
    [SerializeField]
    GameObject m_PlayerHealthText;

    /// <summary>
    /// This is the text element used to display the player's attack.
    /// </summary>
    [SerializeField]
    GameObject m_PlayerAttackText;

    /// <summary>
    /// This is the text element used to display the player's hit chance.  
    /// </summary>
    [SerializeField]
    GameObject m_PlayerHitChanceText;

    /// <summary>
    /// This is the text element used to display the player's defence.
    /// </summary>
    [SerializeField]
    GameObject m_PlayerDefenceText;

    /// <summary>
    /// This is the text element used to display the player's speed. 
    /// </summary>
    [SerializeField]
    GameObject m_PlayerSpeedText;

    [Header("AI Unit Stats")]

    /// <summary>
    /// This is the text element used to display the AI's health. 
    /// </summary>
    [SerializeField]
    GameObject m_AIHealthText;

    /// <summary>
    /// This is the text element used to display the AI's attack.
    /// </summary>
    [SerializeField]
    GameObject m_AIAttackText;

    /// <summary>
    /// This is the text element used to display the AI's hit chance. 
    /// </summary>
    [SerializeField]
    GameObject m_AIHitChanceText;

    /// <summary>
    /// This is the text element used to display the AI's defence.
    /// </summary>
    [SerializeField]
    GameObject m_AIDefenceText;

    /// <summary>
    /// This is the text element used to display the AI's speed. 
    /// </summary>
    [SerializeField]
    GameObject m_AISpeedText;

    [Header("Breakdown Text")]

    /// <summary>
    /// This will be the text to display the attack breakdown.
    /// </summary>
    [SerializeField]
    GameObject m_AttackBreakdownText;

    /// <summary>
    /// This will be the text to display the counter attack breakdown.
    /// </summary>
    [SerializeField]
    GameObject m_CounterAttackBreakdownText;

    [Header("Confirmation Buttons")]

    /// <summary>
    /// This button will instagate the combat and then close the menu. 
    /// </summary>
    [SerializeField]
    Button m_ConfirmButton;

    /// <summary>
    /// This button will be used to cancel combat and close the menu. 
    /// </summary>
    [SerializeField]
    Button m_CancelButton;

    #endregion

    #region Member Functions 

    /// <summary>
    /// This will be used to toggle the visability of the combat menu. 
    /// </summary>
    /// <param name="visableValue"></param>
    public void m_ShowMenu(bool visableValue)
    {
        if(visableValue)
        {
            gameObject.SetActive(true);

            // Todo update text values
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// This will be used to check if the menu is currently active. 
    /// </summary>
    /// <returns></returns>
    public bool m_CheckVisability()
    {
        if(gameObject.activeSelf)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// This will be used to display the player's stats on the menu. 
    /// </summary>
    /// <param name="playerUnit">This is the player's unit to display it's stats. </param>
    public void m_DisplayPlayerStats(GameObject playerUnit)
    {
        if(playerUnit.GetComponent<Unit_Attack>() != null)
        {
            Debug.Log("Player Unit has required components. ");

            // Change Health Display

            string l_sHealthText = "Health : " + playerUnit.GetComponent<Health_Management>().m_GetCurrentHealth(); 

            m_PlayerHealthText.GetComponent<Text>().text  = l_sHealthText;

            // Change Attack Display

            string l_sAttackText = "Attack : " + playerUnit.GetComponent<Unit_Attack>().m_GetAttack();

            m_PlayerAttackText.GetComponent<Text>().text = l_sAttackText;

            // Change Hit Chance Display

            string l_sHitChanceText = "Hit Chance : " + playerUnit.GetComponent<Unit_Attack>().m_GetHitChance() + "%";

            m_PlayerHitChanceText.GetComponent<Text>().text = l_sHitChanceText;

            // Change Defence Display

            string l_sDefenceText = "Defence : " + playerUnit.GetComponent<Unit_Attack>().m_GetDefence();

            m_PlayerDefenceText.GetComponent<Text>().text = l_sDefenceText;

            // Change Speed Display

            string l_sSpeedText = "Speed : " + playerUnit.GetComponent<Unit_Attack>().m_GetSpeed();

            m_PlayerSpeedText.GetComponent<Text>().text = l_sSpeedText;
        }
        else 
        {
            Debug.LogWarning("Player Unit has not got the required components. ");
        }
    }

    /// <summary>
    /// This will be used to display the AI's stats on the menu. 
    /// </summary>
    /// <param name="aiUnit">This is the AI's unit to display it's stats. </param>
    public void m_DisplayAIStats(GameObject aiUnit)
    {
        if (aiUnit.GetComponent<Unit_Attack>() != null)
        {
            Debug.Log("AI Unit has required components. ");

            // Change Health Display

            string l_sHealthText = "Health : " + aiUnit.GetComponent<Health_Management>().m_GetCurrentHealth();

            m_AIHealthText.GetComponent<Text>().text = l_sHealthText;

            // Change Attack Display

            string l_sAttackText = "Attack : " + aiUnit.GetComponent<Unit_Attack>().m_GetAttack();

            m_AIAttackText.GetComponent<Text>().text = l_sAttackText;

            // Change Hit Chance Display

            string l_sHitChanceText = "Hit Chance : " + aiUnit.GetComponent<Unit_Attack>().m_GetHitChance() + "%";

            m_AIHitChanceText.GetComponent<Text>().text = l_sHitChanceText;

            // Change Defence Display

            string l_sDefenceText = "Defence : " + aiUnit.GetComponent<Unit_Attack>().m_GetDefence();

            m_AIDefenceText.GetComponent<Text>().text = l_sDefenceText;

            // Change Speed Display

            string l_sSpeedText = "Speed : " + aiUnit.GetComponent<Unit_Attack>().m_GetSpeed();

            m_AISpeedText.GetComponent<Text>().text = l_sSpeedText;
        }
        else
        {
            Debug.LogWarning("AI Unit has not got the required components. ");
        }
    }

    public void m_DisplayCombatBreakdown(GameObject playerUnit, GameObject aiUnit, bool playerAdvantage, bool aiAdvantage)
    {
        // Init local variables. 

        string l_SBaseText = "% chance to hit, dealing ";

        float l_fTotalHitChance = 0, l_fTotalDamage = 0;

        string l_sFinalText; 

        #region Attack Breakdown 

        // Calculate final hit chance. 

        l_fTotalHitChance = playerUnit.GetComponent<Unit_Attack>().m_GetHitChance() - (aiUnit.GetComponent<Unit_Attack>().m_GetSpeed() * 2);

        if (playerAdvantage == true)
        {
            l_fTotalHitChance *= 1.05f;
        }

        // Calculate final damage. 

        l_fTotalDamage = playerUnit.GetComponent<Unit_Attack>().m_GetAttack() - (aiUnit.GetComponent<Unit_Attack>().m_GetDefence() / 2);

        if (playerAdvantage == true)
        {
            l_fTotalDamage *= 2;
        }

        // Complete sentence. 

        l_sFinalText = l_fTotalHitChance + l_SBaseText + l_fTotalDamage + " damage. ";

        m_AttackBreakdownText.GetComponent<Text>().text = l_sFinalText;

        #endregion

        // Reset varaibles.

        l_fTotalHitChance = 0;
        l_fTotalDamage = 0;

        #region Counter Attack Breakdown 

        // Calculate final hit chance. 

        l_fTotalHitChance = aiUnit.GetComponent<Unit_Attack>().m_GetHitChance() - (playerUnit.GetComponent<Unit_Attack>().m_GetSpeed() * 2);

        if (aiAdvantage == true)
        {
            l_fTotalHitChance *= 1.05f;
        }

        // Calculate final damage. 

        l_fTotalDamage = aiUnit.GetComponent<Unit_Attack>().m_GetAttack() - (playerUnit.GetComponent<Unit_Attack>().m_GetDefence() / 2);

        if (playerAdvantage == true)
        {
            l_fTotalDamage *= 2;
        }

        // Complete sentence. 

        l_sFinalText = l_fTotalHitChance + l_SBaseText + l_fTotalDamage + " damage. ";

        m_CounterAttackBreakdownText.GetComponent<Text>().text = l_sFinalText;

        #endregion
    }

    /// <summary>
    /// Thsi will be used to assign the functionality of the confirmation button. 
    /// </summary>
    /// <param name="unitManager">This is the unit manager and will contain all of the functions 
    /// the button will need to work. </param>
    public void m_SetAttackConfirmation(GameObject unitManager)
    {
        m_ConfirmButton.onClick.AddListener(delegate
        {
            // When this button is pressed the attack script will be played. 

            unitManager.GetComponent<Unit_Manager>().m_UnitAttack();

            // Todo Play combat animation.

            // Then their number of attacks are set to 0.
            unitManager.GetComponent<Unit_Manager>().m_GetSelectedUnit().GetComponent<Unit_Attack>().m_SetNumberOfAttacks(0);

            // The null atcion is then selected and will end the combat phase. 
            unitManager.GetComponent<Unit_Manager>().m_SetActionNull();

            m_ShowMenu(false); 
        });
    }

    public void m_SetCancelConfirmation(GameObject unitManager)
    {
        m_CancelButton.onClick.AddListener(delegate
        {
            // The null atcion is then selected and will end the combat phase. 
            unitManager.GetComponent<Unit_Manager>().m_SetActionNull();

            // Deselect the currently selected unit thus closing the combat menu. 
            unitManager.GetComponent<Unit_Manager>().m_DeselectSelectedUnit(); 

            m_ShowMenu(false);
        });
    }

    #endregion
}
