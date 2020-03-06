using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_Attack : MonoBehaviour
{
    [SerializeField]
    GameObject m_AttackTarget;

    [SerializeField]
    bool m_bAttack = false;

    // Update is called once per frame
    void Update()
    {
        if(m_bAttack == true)
        {
            m_AttackTarget = gameObject.GetComponentInParent<Get_Attack_Target>().m_GetAttackTarget();

            if (m_AttackTarget != null)
            {
                m_Attack();

                m_bAttack = false;
            }
        }
    }

    void m_Attack()
    {
        int l_iDamageDealt, l_iDamageRecived;

        // Attack Opponent.

        if (m_AttackTarget != null)
        {

            l_iDamageDealt = gameObject.GetComponent<UnitStat>().m_GetAttack() - m_AttackTarget.GetComponent<UnitStat>().m_GetDefence();

            l_iDamageDealt = Random.Range(0, l_iDamageDealt);

            if (l_iDamageDealt > 0)
            {
                m_AttackTarget.GetComponent<UnitStat>().m_GetHit(l_iDamageDealt);
            }

            // Opponent Attack 

            l_iDamageRecived = m_AttackTarget.GetComponent<UnitStat>().m_GetAttack() - gameObject.GetComponent<UnitStat>().m_GetDefence();

            l_iDamageRecived = Random.Range(0, l_iDamageRecived);

            if (l_iDamageRecived > 0)
            {
                gameObject.GetComponent<UnitStat>().m_GetHit(l_iDamageDealt);
            }
        }
    }

}
