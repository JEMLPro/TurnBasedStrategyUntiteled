using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow_Camera : MonoBehaviour
{
    [SerializeField]
    GameObject m_FollowObject = null; /*!< This is the object the camera will follow. */ 

    [SerializeField]
    float m_fXOffset = 0; /*!< This is the offset for the X axis, will allow for camera positioning. */

    [SerializeField]
    float m_fYOffset = 0; /*!< This is the offset for the Y axis, will allow for camera positioning. */

    [SerializeField]
    float m_fZOffset = 0; /*!< This is the offset for the Z axis, will allow for camera positioning. */

    // Update is called once per frame
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
}
