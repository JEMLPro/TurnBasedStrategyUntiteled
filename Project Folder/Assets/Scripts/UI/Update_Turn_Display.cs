﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Update_Turn_Display : MonoBehaviour
{
    [SerializeField]
    Text m_TurnText;

    const string m_sPlayerTurnText = "Player's Turn";

    const string m_sAITurnText = "Opponent's Turn";

    [SerializeField]
    GameObject m_TurnManager = null;


    private void Update()
    {
        if (m_TurnManager != null)
        {
            if (m_TurnManager.GetComponent<Turn_Manager>().m_GetCurrentTurn() == CurrentTurn.player)
            {
                if (m_TurnText.text != m_sPlayerTurnText)
                {
                    m_TurnText.text = m_sPlayerTurnText;
                }
            }
            else
            {
                if (m_TurnText.text != m_sAITurnText)
                {
                    m_TurnText.text = m_sAITurnText;
                }
            }
        }
        else
        {
            m_TurnManager = GameObject.FindGameObjectWithTag("Turn_Manager");

            if(m_TurnText == null)
            {
                m_TurnText = GameObject.FindGameObjectWithTag("Turn_Text").GetComponent<Text>();
            }
        }
    }
}
