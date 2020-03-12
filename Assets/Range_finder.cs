using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Range_finder : MonoBehaviour
{
    [SerializeField]
    bool m_bCheckRange = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (m_bCheckRange == true)
        {
            m_CheckEnemyRange();

            m_bCheckRange = false; 
        }
    }

    public void m_CheckEnemyRange()
    {
        // Find enemies with opponent tag. 

        GameObject[] l_PotentialTargets;
        l_PotentialTargets = GameObject.FindGameObjectsWithTag("OpponentUnit");

        // Check enemies are in cell range

        // Check Up 

        for (int j = 0; j <= 3; j++)
        {
            GameObject l_CellToCheck = null;

            string l_Direction = " "; 

            switch (j)
            {
                case 0:
                    l_CellToCheck = gameObject.GetComponent<Unit_Movement>().m_GetCurrentCell().GetComponent<Cell_Info>().m_GetCellNeighbour("Up");
                    l_Direction = "Up";
                    break;

                case 1:
                    l_CellToCheck = gameObject.GetComponent<Unit_Movement>().m_GetCurrentCell().GetComponent<Cell_Info>().m_GetCellNeighbour("Down");
                    l_Direction = "Down";
                    break;

                case 2:
                    l_CellToCheck = gameObject.GetComponent<Unit_Movement>().m_GetCurrentCell().GetComponent<Cell_Info>().m_GetCellNeighbour("Left");
                    l_Direction = "Left";
                    break;

                case 3:
                    l_CellToCheck = gameObject.GetComponent<Unit_Movement>().m_GetCurrentCell().GetComponent<Cell_Info>().m_GetCellNeighbour("Right");
                    l_Direction = "Right";
                    break;

                default:
                    l_CellToCheck = null;
                    break;
            }

            if (l_CellToCheck != null)
            {
                for (int i = 0; i < l_PotentialTargets.Length; i++)
                {
                    if (l_CellToCheck == l_PotentialTargets[i].GetComponent<Unit_Movement>().m_GetCurrentCell())
                    {
                        Debug.Log("Enemy Is In Cell " + l_Direction);

                        l_PotentialTargets[i].GetComponent<UnitStat>().m_SetWithinRange(true);
                    }
                }
            }
        }

        // Check if they are clicked on. 
    }
}
