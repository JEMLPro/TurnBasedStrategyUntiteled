using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField]
    GameObject m_AttackTarget;

    [SerializeField]
    bool m_bAttack = false;

    [SerializeField]
    Material m_AttackRangeMat;

    [SerializeField]
    int m_iNumberOfAttacks = 1; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(m_bAttack == true)
        {
            if(m_AttackTarget != null)
            {
                if (m_iNumberOfAttacks > 0)
                {
                    m_Attck();

                    m_bAttack = false;

                    m_iNumberOfAttacks--;
                }
            }
            else
            {
                m_AttackTarget = gameObject.GetComponentInParent<Find_Attack_Target>().m_FindTarget();
            }
        }
    }

    public void m_Attck()
    {
        int l_iDamage;

        // User attacks opponent 

        l_iDamage = gameObject.GetComponent<UnitStat>().m_GetAttack() - m_AttackTarget.GetComponent<UnitStat>().m_GetDefence();

        if (l_iDamage > 0)
        {
            l_iDamage = Random.Range(0, l_iDamage);

            if (l_iDamage > 0)
            {
                m_AttackTarget.GetComponent<UnitStat>().m_TakeHit(l_iDamage);
            }
            else
            {
                Debug.Log("Missed");
            }
        }
        else
        {
            Debug.Log("No Damage"); 
        }

        // Opponent counter attacks 

        l_iDamage = m_AttackTarget.GetComponent<UnitStat>().m_GetAttack() - gameObject.GetComponent<UnitStat>().m_GetDefence();

        if (l_iDamage > 0)
        {
            l_iDamage = Random.Range(0, l_iDamage);

            if (l_iDamage > 0)
            {
                gameObject.GetComponent<UnitStat>().m_TakeHit(l_iDamage);
            }
            else
            {
                Debug.Log("Missed");
            }
        }
        else
        {
            Debug.Log("No Damage");
        }
    }

    public void m_SetAttack(bool newValue)
    {
        m_bAttack = newValue; 
    }

    public void m_ResetAttackPoints()
    {
        m_iNumberOfAttacks = 1; 
    }


}
