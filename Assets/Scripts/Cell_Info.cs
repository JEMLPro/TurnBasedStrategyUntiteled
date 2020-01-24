using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell_Info : MonoBehaviour
{
    [SerializeField]
    GridPos m_GridPos; 

    public struct GridPos
    {
        public int x, y; 
    }

    [SerializeField]
    GameObject m_CellUp = null;

    [SerializeField]
    GameObject m_CellDown = null;

    [SerializeField]
    GameObject m_CellLeft = null;

    [SerializeField]
    GameObject m_CellRight = null; 

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void m_SetGridPos(int x, int y)
    {
        m_GridPos.x = x;
        m_GridPos.y = y;
    }

    public GridPos m_GetGridPos()
    {
        return m_GridPos;
    }

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
}
