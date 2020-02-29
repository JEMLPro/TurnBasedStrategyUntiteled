using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePosToCell : MonoBehaviour
{
    [SerializeField]
    Vector3 m_MousePos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        m_MousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));

        
    }

    public Vector3 m_GetMousePos() => m_MousePos; 
}
