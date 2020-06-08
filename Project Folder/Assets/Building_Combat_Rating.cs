using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building_Combat_Rating : MonoBehaviour
{
    [SerializeField]
    float m_fCombatRating = 100; /*!< \var This is the base combat rating for all buildings.it will allow for enemy units to slect a target. */

    const float m_fBaseCombatRating = 100;

    public float m_GetCombatRating() => m_fCombatRating;

    public float m_GetBaseCombatRating() => m_fBaseCombatRating;

    public void m_SetCombatRating(float newRating) { m_fCombatRating = newRating; }
}
