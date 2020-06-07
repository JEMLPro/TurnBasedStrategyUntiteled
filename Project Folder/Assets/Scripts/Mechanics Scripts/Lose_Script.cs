using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lose_Script : MonoBehaviour
{
    [SerializeField]
    GameObject m_GameOverScreen;

    [SerializeField]
    bool m_GameOver;

    CurrentTurn m_Owner; 

    private void Update()
    {
        if(m_GameOver == true)
        {
            if(m_GameOverScreen.activeSelf == false)
            {
                m_GameOverScreen.SetActive(true);

                Time.timeScale = 0;

                switch (m_Owner)
                {
                    case CurrentTurn.player:
                        m_GameOverScreen.GetComponentInChildren<Text>().text = "AI has Won.";
                        break;
                    case CurrentTurn.ai:
                        m_GameOverScreen.GetComponentInChildren<Text>().text = "Player Has Won.";
                        break;
                    default:
                        m_GameOverScreen.GetComponentInChildren<Text>().text = "Someone has won!";
                        break;
                }
            }
        }
    }

    public void m_SetGameOver()
    {
        m_GameOver = true;

        Time.timeScale = 0;

        m_GameOverScreen.SetActive(true);

        m_GameOverScreen.GetComponentInChildren<Text>().text = "Someone has won!"; 
    }

    public void m_SetGameOverScreen(GameObject gameOverScreen) { m_GameOverScreen = gameOverScreen; }

    public void m_SetOwner(CurrentTurn newOwner) { m_Owner = newOwner; }

}
