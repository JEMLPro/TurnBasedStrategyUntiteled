using UnityEngine;

public class Characterstat : MonoBehaviour
{
    public int maxHP = 100;
    public int currentHP { get; private set; }



    public Stat HP;

    public Stat Attack;

    public Stat AttackRange;

    public Stat Defence;

    public Stat BuildTime;

    public Stat BuildCost;

    public Stat Type;

    public void TakeDamage(int damage)
    {

        damage -= Defence.GetValue();
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



}
