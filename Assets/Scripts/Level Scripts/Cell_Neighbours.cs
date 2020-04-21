using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell_Neighbours : MonoBehaviour
{
    [SerializeField]
    List<GameObject> m_CellNeighbours = new List<GameObject>(); /*! < /var This will hold all of the cells adjacent to this one. */

    // This will be used to add a new game obejct to it's list of neighbours. 
    public void m_SetCellNeighbour(GameObject newNeighbour)
    {
        m_CellNeighbours.Add(newNeighbour); 
    }

    // This will be used to get the current adjacent cells. 
    public List<GameObject> m_GetCellNeighbours()
    {
        return m_CellNeighbours; 
    }


}
