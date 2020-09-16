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

    [Header("Confirmation Buttons")]

    [SerializeField]
    Button m_ConfirmButton;

    #endregion

    #region Member Functions 

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

    public bool m_CheckVisability()
    {
        if(gameObject.activeSelf)
        {
            return true;
        }

        return false;
    }

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

    public void m_SetAttackConfirmation(GameObject m_UnitManager)
    {
        m_ConfirmButton.onClick.AddListener(delegate
        {
            // When this button is pressed the attack script will be played. 

            m_UnitManager.GetComponent<Unit_Manager>().m_UnitAttack();

            // Todo Play combat animation.

            // Then their number of attacks are set to 0.
            m_UnitManager.GetComponent<Unit_Manager>().m_GetSelectedUnit().GetComponent<Unit_Attack>().m_SetNumberOfAttacks(0);

            // The null atcion is then selected and will end the combat phase. 
            m_UnitManager.GetComponent<Unit_Manager>().m_SetActionNull();

            m_ShowMenu(false); 
        });
    }

    #endregion
}
