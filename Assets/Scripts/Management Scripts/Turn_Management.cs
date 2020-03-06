using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Turn_Management : MonoBehaviour
{
    enum CurrentPlayer
    {
        player = 0x00,
        opponent = 0x01
    }

    [SerializeField]
    CurrentPlayer m_Turn = CurrentPlayer.player;

    [SerializeField]
    bool m_bEndTurn = false;

    [SerializeField]
    Text m_TurnDisplay;

    [SerializeField]
    GameObject m_PlayerUnitManager;

    [SerializeField]
    GameObject m_EnemyUnitManager;

    // Update is called once per frame
    void Update()
    {
        if(m_bEndTurn == true)
        {
            m_SwitchTurn();

            m_PlayerUnitManager.GetComponent<Unit_Manager>().m_ResetUnitMovePoints();

            m_EnemyUnitManager.GetComponent<Unit_Manager>().m_ResetUnitMovePoints();

            m_bEndTurn = false; 
        }
    }

    public void m_EndTurn()
    {
        m_bEndTurn = true; 
    }

    void m_SwitchTurn()
    {
        if(m_Turn == CurrentPlayer.player)
        {
            m_Turn = CurrentPlayer.opponent;

            m_TurnDisplay.text = "Opponent's Turn";
        }
        else
        {
            m_Turn = CurrentPlayer.player;

            m_TurnDisplay.text = "Player's Turn";
        }
    }

    public int m_GetTurn() => (int)m_Turn;
}
