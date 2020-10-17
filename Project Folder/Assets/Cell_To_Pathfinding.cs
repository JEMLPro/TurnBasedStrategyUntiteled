using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell_To_Pathfinding : MonoBehaviour
{
    #region Data Members 

    /// <summary>
    /// This is the Global Cost of getting to this cell using the current cell. This tentative GScore 
    /// will allow for this to check if using this new current cell will result in a better path, so
    /// if the new FScore is lower using this new new tentative GScore than this is a better path and 
    /// the preveous cell should be updated to use this new current cell.
    /// </summary>
    [SerializeField]
    protected float m_fGScore = float.PositiveInfinity;

    /// <summary>
    ///  This is the Huristic Cost for getting from this cell to the end cell using raw distance,
    ///  it will merge with the G-Score to create the F-Score. 
    /// </summary>
    [SerializeField]
    protected float m_fHScore;

    /// <summary>
    /// This is the Final Score for this cell and using this the algorithm will be able to select the 
    /// next cell to be checked. It is created using both G and H Scrores where F = G + H; and the cell
    /// with the lowest F score after at the end of the this pathfinding step becomes the new current cell 
    /// to be checked. 
    /// </summary>
    [SerializeField]
    protected float m_fFScore;

    /// <summary>
    /// This is the cell needed to get to this one from the start cell. Its main function will be for the 
    /// path tracing step, I will loop through previous cells until I reach the start cell to form the final
    /// path. If none is set when added into the open set the current cell should be set and it's G, H and F 
    /// scores calculated. 
    /// </summary>
    [SerializeField]
    protected GameObject m_PreviousCell = null;

    #endregion

    #region Member Functions 

    #region G-Score 

    /// <summary>
    /// This will be used to update the G-Score associated with this cell, it will be a sum of the movement from the 
    /// start cell to the current cell, and the distance to get to this cell. This will update the curret G-Score if 
    /// the new tentative G is less than the current G-Score. 
    /// </summary>
    /// <param name="currentCell">The cell currently being checked. </param>
    public void m_CalculateGScore(GameObject currentCell)
    {
        // This will calculate the new tentative G-Score usingthe distance required to move to the current cell and 
        // the distance to move to this new neighbour. 

        float l_fTentativeGScore = currentCell.GetComponent<Cell_To_Pathfinding>().m_fGScore + 
            gameObject.GetComponent<Cell_Manager>().m_Distance(currentCell); 

        // If the new tentative G-Score is less than the preveous one then this is a better path and the variables need
        // to be updated. 

        if(l_fTentativeGScore < m_fGScore)
        {
            m_fGScore = l_fTentativeGScore;

            m_PreviousCell = currentCell; 
        }

    }

    /// <summary>
    /// This will be used to access the G-Score outside this class. 
    /// </summary>
    /// <returns></returns>
    public float m_GetGScore() => m_fGScore;

    #endregion

    #region H-Score

    /// <summary>
    /// This will be used to calcuate the H-Score for this cell. It's main funationality will be to 
    /// guage the distance between this cell and the end cell, allowing for the algorithm to check 
    /// the closest cell to the end point.
    /// </summary>
    /// <param name="endCell">This is the target cell for the algoritm, and will allow for the 
    /// algorithm to check the most relevent cells. </param>
    public void m_CalculateHScore(GameObject endCell)
    {
        // Thsi will be used to set the H-Score for the algorithm, it wil use the raw distance from this 
        // cell to the end cell.

        m_fHScore = gameObject.GetComponent<Cell_Manager>().m_Distance(endCell); 
    }

    /// <summary>
    /// This will be used to access the H-Score outside this class. 
    /// </summary>
    /// <returns></returns>
    public float m_GetHScore() => m_fHScore;

    #endregion

    #region F-Score 

    /// <summary>
    /// This will be used to add the G and H scores to form the final score for the pathfinding.
    /// </summary>
    public void m_CalculateFScore()
    {
        m_fFScore = m_fGScore + m_fHScore; 
    }

    /// <summary>
    /// This will be used to access the F-Score outside this class. 
    /// </summary>
    /// <returns></returns>
    public float m_GetFScore() => m_fFScore;

    #endregion

    #region path Tracing

    /// <summary>
    /// This will allow access to this cell's previous cell and will allow for path tracing. 
    /// </summary>
    /// <returns></returns>
    public GameObject m_GetPreviousCell() => m_PreviousCell; 

    #endregion

    #endregion
}
