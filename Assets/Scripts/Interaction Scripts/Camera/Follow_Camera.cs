using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow_Camera : MonoBehaviour
{
    [SerializeField]
    GameObject m_FollowObject = null;

    [SerializeField]
    float m_fXOffset = 0;

    [SerializeField]
    float m_fYOffset = 0;

    [SerializeField]
    float m_fZOffset = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.transform.position != m_FollowObject.transform.position + new Vector3(m_fXOffset, m_fYOffset, m_fZOffset))
        {
            gameObject.transform.position = m_FollowObject.transform.position + new Vector3(m_fXOffset, m_fYOffset, m_fZOffset);

            gameObject.transform.LookAt(m_FollowObject.transform.position); 

        }
    }
}
