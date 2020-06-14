using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Close_Game : MonoBehaviour
{
    public void m_CloseGame()
    {
        Debug.LogError("Game is closed. But due to the editor doesn't close. "); 

        Application.Quit(); 
    }
}
