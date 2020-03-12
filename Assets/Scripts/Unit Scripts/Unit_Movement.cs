using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_Movement : MonoBehaviour
{
    [SerializeField]
    GameObject m_CurrentCell = null;

    [SerializeField]
    GameObject m_GameMap;

    [SerializeField]
    GameObject m_GameManager;

    [SerializeField]
    Material m_MoveRangeMat;

    [SerializeField]
    int m_UsedPoints;

    int m_ScaledMoveRadius;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (m_CurrentCell != null)
        {
            if (gameObject.transform.position != m_CurrentCell.GetComponent<Cell_Info>().m_GetCellPosition())
            {
                gameObject.transform.position = m_CurrentCell.GetComponent<Cell_Info>().m_GetCellPosition();
            }
        }
        else
        {
            m_CurrentCell = m_GameMap.GetComponent<Create_Map>().m_GetRandomCell();
        }

        if (gameObject.GetComponent<UnitStat>().m_GetSelected() == true)
        {
            if (gameObject.GetComponent<UnitStat>().m_GetOwner() == m_GameManager.GetComponent<Turn_Management>().m_GetTurn())
            {
                m_ScaledMoveRadius = gameObject.GetComponent<UnitStat>().m_GetMoveRadius() - m_UsedPoints;

                if (m_ScaledMoveRadius >= 0)
                {
                    m_CellRange();

                    GameObject l_MoveDestination = m_GameMap.GetComponent<Create_Map>().m_GetSelectedCell();

                    if (l_MoveDestination != null)
                    {
                        m_UsedPoints += m_CalculateUsedPoints(m_CurrentCell.GetComponent<Cell_Info>().m_GetGridPos().x, m_CurrentCell.GetComponent<Cell_Info>().m_GetGridPos().y,
                            l_MoveDestination.GetComponent<Cell_Info>().m_GetGridPos().x, l_MoveDestination.GetComponent<Cell_Info>().m_GetGridPos().y);

                        m_CurrentCell = l_MoveDestination;

                        m_ResetMapCells();
                    }
                }

                if(gameObject.GetComponent<UnitStat>().m_GetMoveRadius() == m_UsedPoints)
                {
                    m_ResetMapCells();
                }
            }
        }
    }

    public void m_Wait()
    {
        m_UsedPoints = gameObject.GetComponent<UnitStat>().m_GetMoveRadius(); 
    }

    public void m_ResetMapCells()
    {
        m_GameMap.GetComponent<Create_Map>().m_ResetCells();
    }

    public void m_ResetMovementPoints()
    {
        m_UsedPoints = 0; 
    }

    public GameObject m_GetCurrentCell() => m_CurrentCell; 

    private int m_CalculateUsedPoints(int xOne, int yOne, int xTwo, int yTwo)
    {
        int l_ReturnValue = 0;

        int newX, newY;

        // Calculate points used in the X axis.

        if(xOne > xTwo)
        {
            newX = xOne - xTwo;
        }
        else
        {
            newX = xTwo - xOne;
        }

        // Calculate points in the Y axis

        if (yOne > yTwo)
        {
            newY = yOne - yTwo;
        }
        else
        {
            newY = yTwo - yOne;
        }

        l_ReturnValue = newX + newY; 

        return l_ReturnValue; 
    }

    public void m_CellRange()
    {
        // Debug.Log("Entered Cell Range"); 

        m_CurrentCell.GetComponent<Cell_Info>().m_SetWithinRange(true);

        // Cell Range Up 

        m_CheckMoveDirection("Up");

        // Cell Range Down 

        m_CheckMoveDirection("Down");

        // Cell Range Left

        m_CheckMoveDirection("Left");

        // Cell Range Right 

        m_CheckMoveDirection("Right");

        // Cell Range Diagonal

        for(int i = 0; i < m_ScaledMoveRadius; i++)
        {
            m_CheckMoveDirectionDiagonal("Left", i);

            m_CheckMoveDirectionDiagonal("Right", i);
        }
    }

    public void m_CellRange(Material newMat, int range)
    {
        // Debug.Log("Entered Cell Range"); 

        m_CurrentCell.GetComponent<Cell_Info>().m_SetWithinRange(true);

        // Cell Range Up 

        m_CheckMoveDirection("Up", range, newMat);

        // Cell Range Down 

        m_CheckMoveDirection("Down", range, newMat);

        // Cell Range Left

        m_CheckMoveDirection("Left", range, newMat);

        // Cell Range Right 

        m_CheckMoveDirection("Right", range, newMat);
    }

    void m_CheckMoveDirection(string direction)
    {
        GameObject l_CurrentCellInRange = m_CurrentCell;

        for (int i = 0; i < m_ScaledMoveRadius; i++)
        {
            if (l_CurrentCellInRange.GetComponent<Cell_Info>().m_GetCellNeighbour(direction) != null)
            {
                l_CurrentCellInRange = l_CurrentCellInRange.GetComponent<Cell_Info>().m_GetCellNeighbour(direction);

                l_CurrentCellInRange.GetComponent<Cell_Info>().m_SetWithinRange(true);

                l_CurrentCellInRange.GetComponent<Renderer>().material = m_MoveRangeMat;
            }
            else
            {
                break; 
            }
        }
    }

    void m_CheckMoveDirection(string direction, int range, Material newMat)
    {
        GameObject l_CurrentCellInRange = m_CurrentCell;

        for (int i = 0; i < range; i++)
        {
            if (l_CurrentCellInRange.GetComponent<Cell_Info>().m_GetCellNeighbour(direction) != null)
            {
                l_CurrentCellInRange = l_CurrentCellInRange.GetComponent<Cell_Info>().m_GetCellNeighbour(direction);

                l_CurrentCellInRange.GetComponent<Cell_Info>().m_SetWithinRange(true);

                l_CurrentCellInRange.GetComponent<Renderer>().material = newMat;
            }
            else
            {
                break;
            }
        }
    }

    void m_CheckMoveDirectionDiagonal(string direction)
    {
        GameObject l_CurrentCellInRange = m_CurrentCell;

        l_CurrentCellInRange = m_CurrentCell.GetComponent<Cell_Info>().m_GetCellNeighbour(direction);

        string l_Direction = "Up";

        for (int j = 0; j <= 1; j++)
        {
            for (int i = 0; i < m_ScaledMoveRadius - 1; i++)
            {
                l_CurrentCellInRange = l_CurrentCellInRange.GetComponent<Cell_Info>().m_GetCellNeighbour(l_Direction);

                if (l_CurrentCellInRange != null)
                {
                    l_CurrentCellInRange.GetComponent<Cell_Info>().m_SetWithinRange(true);

                    l_CurrentCellInRange.GetComponent<Renderer>().material = m_MoveRangeMat;
                }
            }

            l_CurrentCellInRange = m_CurrentCell.GetComponent<Cell_Info>().m_GetCellNeighbour(direction);

            l_Direction = "Down";
        }
    }

    void m_CheckMoveDirectionDiagonal(string direction, int steps)
    {
        GameObject l_CurrentCellInRange = m_CurrentCell;

        bool l_bExit = false; 

        string l_Direction = "Up";

        for (int l = 0; l < 2; l++)
        {
            l_CurrentCellInRange = m_CurrentCell.GetComponent<Cell_Info>().m_GetCellNeighbour(direction);

            if (l_CurrentCellInRange != null)
            {
                for (int k = 0; k < steps; k++)
                {
                    if (l_CurrentCellInRange.GetComponent<Cell_Info>().m_GetCellNeighbour(direction) != null)
                    {
                        l_CurrentCellInRange = l_CurrentCellInRange.GetComponent<Cell_Info>().m_GetCellNeighbour(direction);
                    }
                    else
                    {
                        l_bExit = true;

                        break;
                    }
                }

                if (l_bExit != true)
                {
                    for (int j = 0; j <= 1; j++)
                    {
                        if (l_CurrentCellInRange != null)
                        {
                            for (int i = 0; i < m_ScaledMoveRadius - 1 - steps; i++)
                            {
                                if (l_CurrentCellInRange.GetComponent<Cell_Info>().m_GetCellNeighbour(l_Direction) != null)
                                {
                                    l_CurrentCellInRange = l_CurrentCellInRange.GetComponent<Cell_Info>().m_GetCellNeighbour(l_Direction);

                                    l_CurrentCellInRange.GetComponent<Cell_Info>().m_SetWithinRange(true);

                                    l_CurrentCellInRange.GetComponent<Renderer>().material = m_MoveRangeMat;
                                }
                            }
                        }
                        else
                        {
                            break;
                        }

                        if (l_CurrentCellInRange.GetComponent<Cell_Info>().m_GetCellNeighbour(l_Direction) != null)
                        {
                            l_CurrentCellInRange = m_CurrentCell.GetComponent<Cell_Info>().m_GetCellNeighbour(direction);
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }

            l_Direction = "Down";

            l_bExit = false; 

        }
    }
}
