using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Turn_Management : MonoBehaviour
{
    enum currentPlayer
    {
        player = 0x00,
        opponent = 0x01
    }

    [SerializeField]
    int m_TurnCount = 1; 

    [SerializeField]
    currentPlayer m_Turn = currentPlayer.opponent;

    [SerializeField]
    bool m_bEndTurn = true;

    [SerializeField]
    Text m_TurnDisplay;

    [SerializeField]
    GameObject m_UnitManagerPlayer;

    [SerializeField]
    GameObject m_UnitManagerOpponent;

    // Start is called before the first frame update
    void Start()
    {
        if (m_Turn == currentPlayer.player)
        {
            m_Turn = currentPlayer.opponent;

            m_TurnDisplay.text = "Opponent's Turn\n" + "Turn : " + m_TurnCount;
        }
        else
        {
            m_Turn = currentPlayer.player;

            m_TurnDisplay.text = "Player's Turn\n" + "Turn : " + m_TurnCount;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(m_bEndTurn == true)
        {
            m_SwitchTurn();

            m_bEndTurn = false; 
        }
    }

    public void m_SwitchTurn()
    {
        if(m_Turn == currentPlayer.player)
        {
            m_Turn = currentPlayer.opponent;

            m_TurnDisplay.text = "Opponent's Turn\n" + "Turn : " + m_TurnCount;
        }
        else
        {
            m_Turn = currentPlayer.player;

            m_TurnDisplay.text = "Player's Turn\n" + "Turn : " + m_TurnCount;
        }

        // Reset Player Units 

        m_ResetUnitManager(m_UnitManagerPlayer);

        // Reset Opponent Units

        m_ResetUnitManager(m_UnitManagerOpponent);

        m_TurnCount += 1;
    }

    public int m_GetTurn() => (int)m_Turn;

    public void m_ResetOtherManager(GameObject unitManager)
    {
        if (unitManager == m_UnitManagerPlayer)
        {
            m_ResetUnitManager(m_UnitManagerOpponent);
        }
        else
        {
            m_ResetUnitManager(m_UnitManagerPlayer);
        }
    }

    void m_ResetUnitManager(GameObject unitManager)
    {
        unitManager.GetComponent<Unit_Manager>().m_ResetUnitMovementPoints();

        unitManager.GetComponent<Unit_Manager>().m_ResetAttackPoints();

        unitManager.GetComponent<Unit_Manager>().m_ResetWithinRange();

        unitManager.GetComponent<Unit_Manager>().m_ResetSelectedUnits();
    }
}
