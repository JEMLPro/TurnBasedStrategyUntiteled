using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum  CellTile
{
        none, 
        grass, 
        water
}

public class Cell_Manager : MonoBehaviour
{
    
    [SerializeField]
    Material m_CellMaterial;

    [SerializeField]
    CellTile m_TileType; 

    // This will be used to set a new tile to this cell, allowing for a tile map to be cretaed. 
    public void m_SetTile(CellTile newTile, Material newMaterial)
    {
        m_TileType = newTile; 

        m_CellMaterial = newMaterial;

        gameObject.GetComponent<Renderer>().material = newMaterial; 
    }

}
