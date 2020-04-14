using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum  CellTile
{
        none, 
        grass, 
        forrest
}

public class Cell_Manager : MonoBehaviour
{
    
    [SerializeField]
    Material m_CellMaterial;

    [SerializeField]
    CellTile m_TileType; 

    public void m_SetTile(CellTile newTile, Material newMaterial)
    {
        m_TileType = newTile; 

        m_CellMaterial = newMaterial;

        gameObject.GetComponent<Renderer>().material = newMaterial; 
    }

}
