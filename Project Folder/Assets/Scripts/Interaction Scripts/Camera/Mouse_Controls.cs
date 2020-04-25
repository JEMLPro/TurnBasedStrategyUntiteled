using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse_Controls : MonoBehaviour
{

    [SerializeField]
    float m_fSpeed = 25;

    [SerializeField]
    Vector3 m_MinZoomValue;

    [SerializeField]
    Vector3 m_MaxZoomValue;

    private void Start()
    {
        // At the start of the game assign a min and max zoom value for the attached camera. 
        // These values may need tweeking depending on the size of the map. 

        m_MinZoomValue = new Vector3(0, 0, -2);

        m_MaxZoomValue = new Vector3(0, 0, -8); 
    }

    // Update is called once per frame
    void Update()
    {
        // If the mouse wheel moves update the position of the connected game object using the direction of the movement. 
        gameObject.transform.position += new Vector3(0, 0, Input.mouseScrollDelta.y * m_fSpeed * Time.deltaTime);

        // If the position passes the min and max vales the position becomes those max values. 
        if(gameObject.transform.position.z >= m_MinZoomValue.z)
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, m_MinZoomValue.z);
        }
        else if(gameObject.transform.position.z <= m_MaxZoomValue.z)
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, m_MaxZoomValue.z);
        }
    }
}
