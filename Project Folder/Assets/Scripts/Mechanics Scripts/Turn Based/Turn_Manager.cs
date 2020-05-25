using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum CurrentTurn
{
    player = 0x10, 
    ai = 0x11
}

public class Turn_Manager : MonoBehaviour
{
    [SerializeField]
    CurrentTurn m_ThisTurn = CurrentTurn.player;

    public void m_SwitchTurn()
    {
        if(m_ThisTurn == CurrentTurn.player)
        {
            m_ThisTurn = CurrentTurn.ai;
        }
        else
        {
            m_ThisTurn = CurrentTurn.player; 
        }
    }

    public CurrentTurn m_GetCurrentTurn() => m_ThisTurn;

    public void m_Startup()
    {
        m_ThisTurn = CurrentTurn.player; 
    }
}
