using System.Collections;
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
            m_bSelected = true;
        }
    }

    public int m_GetMoveRadius() => m_MoveRadius;

    public int m_GetOwner() => (int)m_Owner;

    public bool m_GetSelected() => m_bSelected; 

}
