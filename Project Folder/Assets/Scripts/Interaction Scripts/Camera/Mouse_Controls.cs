using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//---------------------------------------------------------------------------------------------------------------------------\\
// File Start
//---------------------------------------------------------------------------------------------------------------------------\\

/// <summary>
/// This script will allow for additional mouse controls added onto the camera.
/// </summary>
public class Mouse_Controls : MonoBehaviour
{
    //---------------------------------------------------------------------------------------------------------------------------\\
    // Data Members Start
    //---------------------------------------------------------------------------------------------------------------------------\\

    /// <summary>
    /// This is the speed at which the mouse controls will occur at. 
    /// </summary>
    [SerializeField]
    float m_fSpeed = 25;

    /// <summary>
    /// This is the minimum zoom for the camera. 
    /// </summary>
    [SerializeField]
    Vector3 m_MinZoomValue;

    /// <summary>
    /// This is the maximum zoom for the camera.
    /// </summary>
    [SerializeField]
    Vector3 m_MaxZoomValue;

    //---------------------------------------------------------------------------------------------------------------------------\\
    // Member Functions Start
    //---------------------------------------------------------------------------------------------------------------------------\\

    /// <summary>
    /// This will happen at the start of the game, it will init a base set of min and max bounds for the camera. 
    /// </summary>
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
