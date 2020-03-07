﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStat : MonoBehaviour
{
    public enum unitType
    {
        
    }

    enum currentPlayer
    {
        player = 0x00,
        opponent = 0x01
    }

    [SerializeField]
    currentPlayer m_Owner = currentPlayer.player; 

    [SerializeField]
    int HP = 100;

    [SerializeField]
    int Attack = 10;

    [SerializeField]
    int AttackRange = 1;

    [SerializeField]
    int Defence = 5;

    [SerializeField]
    int BuildTime = 2;

    [SerializeField]
    int BuildCost = 10;

    [SerializeField]
    int m_MoveRadius = 4;

    [SerializeField]
    unitType Type;

    [SerializeField]
    bool m_bSelected = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            m_bSelected = false; 
        }
    }

    private void OnMouseOver()
    {
        if(Input.GetMouseButtonDown(0))
        {
            // Reset all selected units to prevent multiple units being selected at once. 

            gameObject.GetComponentInParent<Unit_Manager>().m_ResetSelectedUnits();

            // Reset the cells on the game map to prevent movemnet outside of the unit's range. 

            gameObject.GetComponent<Unit_Movement>().m_ResetMapCells();

            // Select this unit. 

            m_bSelected = true;
        }
    }

    public int m_GetMoveRadius() => m_MoveRadius;

    public int m_GetOwner() => (int)m_Owner;

    public bool m_GetSelected() => m_bSelected; 

    public void m_SetSelected(bool newValue)
    {
        m_bSelected = newValue;
    }

}
