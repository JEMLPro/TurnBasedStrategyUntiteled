﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStat : MonoBehaviour
{
    public enum unitType
    {
        militia = 0x00
    }

    enum currentPlayer
    {
        player = 0x00,
        opponent = 0x01
    }

    [SerializeField]
    currentPlayer m_Owner = currentPlayer.player; 

    [SerializeField]
    int m_Attack = 10;

    [SerializeField]
    int AttackRange = 1;

    [SerializeField]
    int m_Defence = 5;

    [SerializeField]
    int BuildTime = 2;

    [SerializeField]
    int BuildCost = 10;

    [SerializeField]
    int m_MoveRadius = 4;

    [SerializeField]
    unitType Type = unitType.militia;

    [SerializeField]
    bool m_bWithinAttackRange = false; 

    private void OnMouseOver()
    {
        if(Input.GetMouseButtonDown(0))
        {
            // Check the owner against the current turn. 

            if ((int)m_Owner == gameObject.GetComponentInParent<Unit_Manager>().m_GetCurrentTurn())
            {
                // Reset all selected units to prevent multiple units being selected at once. 

                gameObject.GetComponentInParent<Unit_Manager>().m_ResetSelectedUnits();

                // Reset the cells on the game map to prevent movemnet outside of the unit's range. 

                gameObject.GetComponent<Unit_Movement>().m_ResetMapCells();

                // When a new unit is selected reset the other unit manager. 

                gameObject.GetComponentInParent<Unit_Manager>().m_ResetOtherManager();

            }

           
        }
    }

    public int m_GetMoveRadius() => m_MoveRadius;

    public void m_SetMoveRadius(int newValue)
    {
        m_MoveRadius = newValue; 
    }

    public int m_GetOwner() => (int)m_Owner;

    public bool m_GetWithinRange() => m_bWithinAttackRange;

    public void m_SetWithinRange(bool newValue)
    {
        m_bWithinAttackRange = newValue;
    }

    public int m_GetAttack() => m_Attack;

    public int m_GetAttackRange() => AttackRange; 

    public int m_GetDefence() => m_Defence;

}