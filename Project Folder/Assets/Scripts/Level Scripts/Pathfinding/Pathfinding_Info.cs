using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding_Info : MonoBehaviour
{
    [SerializeField]
    float m_fGScore = 0; /*!< \var This is the global cost of getting from the current cell and this one. */

    [SerializeField]
    float m_fHScore = 0; /*!< \var This is the huristic cost of getting from this cell to the end cell. */

    [SerializeField]
    float m_fFScore = 0; /*!< \var This is the total of both the G and H scores. */

    [SerializeField]
    float m_fMovementCost = 10; /*!< \var The cost of moving one tile in the game. */

    /*! \fn This will calculate the G-Score for this cell, The G scorre is the global cost of getting from the start cell to this cell. */
    public void m_CalculateGScore(GameObject startCell)
    {
        m_fGScore = m_fMovementCost * startCell.GetComponent<Cell_Manager>().m_Distance(gameObject);
    }

    public float m_GetGScore() => m_fGScore; /*!< \fn This will return this cell's G score. */

    /*! \fn This will calculate the H-Score for this cell, The H score is the Huristic cost of getting from this cell to the end cell. */
    public void m_CalculateHScore(GameObject endCell)
    {
        m_fGScore = m_fMovementCost * gameObject.GetComponent<Cell_Manager>().m_Distance(endCell); 
    }

    public float m_GetHScore() => m_fHScore; /*!< \fn This will return this cell's H score. */

    /*! \fn This will calculate the F-Score for this cell, The F score is the final cost, the total of both the G and H scores. */
    public void m_CalculateFScore()
    {
        m_fFScore = m_fGScore + m_fHScore;
    }

    public float m_GetFScore() => m_fFScore; /*!< \fn This will return this cell's F score. */

}
