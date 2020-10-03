using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Open_Main_Menu : MonoBehaviour
{

    #region Data Members 

    [SerializeField]
    GameObject m_MiniMenuBackground = null;

    [SerializeField]
    GameObject m_GameManager = null;

    #endregion

    #region Member Functions

    private void Start()
    {
        if(m_GameManager == null)
        {
            m_GameManager = GameObject.FindGameObjectWithTag("Game Manager");
        }
    }

    void Update()
    {
        if(m_GameManager != null)
        {
            if (m_GameManager.GetComponent<Start_Up_Script>().m_bGetInGame() == true)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    if (m_MiniMenuBackground.activeSelf == true)
                    {
                        m_MiniMenuBackground.SetActive(false);

                        Time.timeScale = 1;
                    }
                    else
                    {
                        Time.timeScale = 0;

                        m_MiniMenuBackground.SetActive(true);
                    }
                }
            }
        }
    }

    #endregion

}
