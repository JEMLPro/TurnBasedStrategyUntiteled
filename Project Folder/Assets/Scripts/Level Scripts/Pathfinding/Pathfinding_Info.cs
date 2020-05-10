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

    /*! \fn This will calculate the G-Score for this cell, The G scorre is the global cost of getting from the start cell to this cell. */
    public void m_CalculateGScore()
    {

    }

    /*! \fn This will calculate the H-Score for this cell, The H score is the Huristic cost of getting from this cell to the end cell. */
    public void m_CalculateHScore()
    {

    }

    /*! \fn This will calculate the F-Score for this cell, The F score is the final cost, the total of both the G and H scores. */
    public void m_CalculateFScore()
    {
        m_fFScore = m_fGScore + m_fHScore;
    }

}
