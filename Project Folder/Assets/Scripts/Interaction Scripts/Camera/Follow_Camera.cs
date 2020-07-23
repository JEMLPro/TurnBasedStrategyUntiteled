using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//---------------------------------------------------------------------------------------------------------------------------\\
// File Start
//---------------------------------------------------------------------------------------------------------------------------\\

/// <summary>
/// This will allow for the game camera to be attached to a game object and follow it. The main use for this script will be 
/// to create a camera which follows a moving object. 
/// </summary>
public class Follow_Camera : MonoBehaviour
{
    //---------------------------------------------------------------------------------------------------------------------------\\
    // Data Members Start
    //---------------------------------------------------------------------------------------------------------------------------\\

    /// <summary>
    /// This is the object the camera will be following. 
    /// </summary>
    [SerializeField]
    GameObject m_FollowObject = null; 

    /// <summary>
    /// The X offset for the camera, used to position the camera in the correct place while the connected object is moving. 
    /// </summary>
    [SerializeField]
    float m_fXOffset = 0;

    /// <summary>
    /// The Y offset for the camera, used to position the camera in the correct place while the connected object is moving. 
    /// </summary>
    [SerializeField]
    float m_fYOffset = 0; 

    /// <summary>
    /// The Z offset for the camera, used to position the camera in the correct place while the connected object is moving. 
    /// </summary>
    [SerializeField]
    float m_fZOffset = 0; 

    //---------------------------------------------------------------------------------------------------------------------------\\
    // Member Functions Start
    //---------------------------------------------------------------------------------------------------------------------------\\

    /// <summary>
    /// This is called once per frame and will be used to update the position of the camera. 
    /// </summary>
    void Update()
    {
        // If this game object is not at the target location, move to target location. 

        if (gameObject.transform.position != m_FollowObject.transform.position + new Vector3(m_fXOffset, m_fYOffset, m_fZOffset))
        {
            gameObject.transform.position = m_FollowObject.transform.position + new Vector3(m_fXOffset, m_fYOffset, m_fZOffset);

            // This will make this gameobject look in the direction of the target object. 

            gameObject.transform.LookAt(m_FollowObject.transform.position); 

        }
    }

    //---------------------------------------------------------------------------------------------------------------------------\\
    // File End
    //---------------------------------------------------------------------------------------------------------------------------\\
}
