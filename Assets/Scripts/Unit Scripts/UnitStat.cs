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
    int HP;

    [SerializeField]
    int Attack;

    [SerializeField]
    int AttackRange;

    [SerializeField]
    int Defence;

    [SerializeField]
    int BuildTime;

    [SerializeField]
    int BuildCost;

    [SerializeField]
    int m_MoveRadius = 4;

    [SerializeField]
    unitType Type;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int maxHP = 100;
    public int currentHP { get; private set; }

    public void TakeDamage(int damage)
    {

        damage -= Defence;
        damage = Mathf.Clamp(damage, 0, int.MaxValue);


        currentHP -= damage;
        Debug.Log(transform.name + " Takes " + damage + " damage ");

        if (currentHP <= 0)
        {
            Die();
        }


    }

    public virtual void Die()
    {
        //die in some way
        //overwritten code
        Debug.Log(transform.name + " died ");
    }

    public int m_GetMoveRadius() => m_MoveRadius;

    public int m_GetOwner() => (int)m_Owner;

}
