using System.Collections;
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
    int m_HP = 100;

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
    bool m_bSelected = false;

    [SerializeField]
    bool m_bWithinAttackRange = false; 

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
            // Check the owner against the current turn. 

            if ((int)m_Owner == gameObject.GetComponentInParent<Unit_Manager>().m_GetCurrentTurn())
            {
                // Reset all selected units to prevent multiple units being selected at once. 

                gameObject.GetComponentInParent<Unit_Manager>().m_ResetSelectedUnits();

                // Reset the cells on the game map to prevent movemnet outside of the unit's range. 

                gameObject.GetComponent<Unit_Movement>().m_ResetMapCells();

                // When a new unit is selected reset the other unit manager. 

                gameObject.GetComponentInParent<Unit_Manager>().m_ResetOtherManager();

                // Select this unit. 

                m_bSelected = true;
            }

            if(m_bWithinAttackRange == true)
            {
                gameObject.GetComponentInParent<Unit_Manager>().m_ResetSelectedUnits();

                m_bSelected = true;
            }
        }
    }

    public int m_GetMoveRadius() => m_MoveRadius;

    public int m_GetOwner() => (int)m_Owner;

    public bool m_GetSelected() => m_bSelected; 

    public void m_SetSelected(bool newValue)
    {
        m_bSelected = newValue;
    }

    public bool m_GetWithinRange() => m_bWithinAttackRange;

    public void m_SetWithinRange(bool newValue)
    {
        m_bWithinAttackRange = newValue;
    }

    public int m_GetAttack() => m_Attack;

    public int m_GetAttackRange() => AttackRange; 

    public int m_GetDefence() => m_Defence;

    public int m_GetHP() => m_HP; 

    public void m_TakeHit(int damage)
    {
        m_HP -= damage;

        Debug.Log("Unit Hp : " + m_HP); 

        if(m_HP <= 0)
        {
            Debug.Log("Unit Killed");

            Destroy(gameObject);
        }
    }

}
