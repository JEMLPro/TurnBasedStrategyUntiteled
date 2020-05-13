using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health_Bar_On_Object : MonoBehaviour
{
    [SerializeField]
    GameObject m_ConnectedObject = null;

    [SerializeField]
    RectTransform m_HealthBar = null;

    [SerializeField]
    RectTransform m_Canvas = null;

    private void Update()
    {

        Vector2 temp = Camera.main.WorldToViewportPoint(m_ConnectedObject.transform.position);

        
        temp.x *= m_Canvas.sizeDelta.x;
        temp.y *= m_Canvas.sizeDelta.y;
        
        temp.x -= m_Canvas.sizeDelta.x * m_Canvas.pivot.x;
        temp.y -= m_Canvas.sizeDelta.y * m_Canvas.pivot.y;

        Vector2 l_NewPos = temp; 

        if (m_HealthBar.anchoredPosition != l_NewPos)
        {
            m_HealthBar.anchoredPosition = l_NewPos;
        }
    }

    
}
