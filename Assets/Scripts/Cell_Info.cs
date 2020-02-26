using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell_Info : MonoBehaviour
{
    [SerializeField]
    GridPos m_GridPos; // This cells current grid postion, in two coordinates (X and Y). 

    [SerializeField]
    GameObject m_CellUp = null; // The cell above this one (Null if none exists). 

    [SerializeField]
    GameObject m_CellDown = null; // The cell bellow this one (Null if none exists). 

    [SerializeField]
    GameObject m_CellLeft = null; // The cell left of this one (Null if none exists). 

    [SerializeField]
    GameObject m_CellRight = null; // The cell right of this one (Null if none exists). 

    [SerializeField]
    bool m_Obsticle = false; 

    [SerializeField]
    float m_HScore = 0; // The Cost to get from the currnt node to this one. 

    [SerializeField]
    float m_GScore = 0; // The cost to get from this cell to the end cell. 

    [SerializeField]
    float m_FScore = 0; // The combination of G and H scores to get final score. 

    [SerializeField]
    bool m_WithinRange = false; 

    public Vector3 m_GetCellPosition()
    {
        return gameObject.transform.position; 
    }

    public struct GridPos // Struct Creates a set of data for X and Y positions. 
    {
        public int x, y;
    }

    // Used to set the coords for this cell. 
    public void m_SetGridPos(int x, int y)
    {
        m_GridPos.x = x;
        m_GridPos.y = y;
    }

    // Used to get the coords for this cell.
    public GridPos m_GetGridPos()
    {
        return m_GridPos;
    }

    // Used to set a neighbour for this cell. Use a direction to set the game object; (up, down, left, right). 
    public void m_SetCellNeighbour(string direction, GameObject neighbour)
    {
        if (neighbour != null)
        {
            if (direction == "Down" || direction == "down")
            {
                // Debug.Log("Set Neighbour in direction: Down");

                m_CellDown = neighbour;
            }
            else if (direction == "Up" || direction == "Up")
            {
                // Debug.Log("Set Neighbour in direction: Up");

                m_CellUp = neighbour;
            }
            else if (direction == "Left" || direction == "left")
            {
                m_CellLeft = neighbour;
            }
            else if (direction == "Right" || direction == "right")
            {
                m_CellRight = neighbour;
            }
            else
            {
                Debug.Log("Unable to find direction \"" + direction + "\"");
            }
        }
        else
        {
            Debug.Log("Neighbour is null, Unable to assign");
        }
    }

    // Used to get this cells neighbours, using the same directions it returns taht game object in that direction. 
    public GameObject m_GetCellNeighbour(string direction)
    {
        if (direction == "Up" || direction == "up")
        {
            return m_CellUp;
        }
        else if (direction == "Down" || direction == "down")
        {
            return m_CellDown;
        }
        else if (direction == "Left" || direction == "left")
        {
            return m_CellLeft;
        }
        else if (direction == "Right" || direction == "right")
        {
            return m_CellRight;
        }
        else
        {
            Debug.Log("Unable to find direction \"" + direction + "\"");
        }

        return null;
    }

    // Used to set the G Score for this cell.
    public void m_SetGScore(float newG)
    {
        m_GScore = newG; 
    }

    // Used to get the G Score for this cell.
    public float m_GetGScore() => m_GScore;

    // Used to set the H Score for this cell.
    public void m_SetHScore(float newH)
    {
        m_HScore = newH;
    }

    // Used to get the H Score for this cell.
    public float m_GetHScore => m_HScore;

    // Used to set the F Score for this cell.
    public void m_SetFScore(float newF)
    {
        m_FScore = newF;
    }

    // Used to get the F Score for this cell.
    public float m_GetFScore => m_FScore;

    public void m_SetObsticle(bool newSetting)
    {
        m_Obsticle = newSetting; 
    }

    public bool m_GetObsticle => m_Obsticle;


    public void m_SetWithinRange(bool newSetting)
    {
        m_WithinRange = newSetting;
    }

    public bool m_GetWithinRange => m_WithinRange;

}
