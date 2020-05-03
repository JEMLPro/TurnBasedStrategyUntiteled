using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_Object : MonoBehaviour
{
    [SerializeField]
    float m_fSpeed = 10.0f; /*!< Used to define the speed at which the connected object will move. */

    [SerializeField]
    bool m_bXAsisMovement = true; /*!< Used to make the object move Left and Right. */

    [SerializeField]
    bool m_bYAxisMovement = true; /*!< Used to make the object mobe Up and Down. */

    [SerializeField]
    bool m_bZAxisMovement = false; /*!< Used to make the object move Backwards and Forwards. */

    [SerializeField]
    Vector3 m_MinMoveValue;

    [SerializeField]
    Vector3 m_MaxMoveValue;

    private void Start()
    {
        // Assign Min and max position values, May need tweeking depending upon map. 
        m_MinMoveValue = new Vector3(-10f, -10f, 0);

        m_MaxMoveValue = new Vector3(0, 0, 0); 
    }

    // Update is called once per frame
    void Update()
    {
        // This will check which directional controls have been assigned to the object and move in the desired directions. 

        if(m_bXAsisMovement)
        {
            m_XAxisMovement();
        }

        if (m_bYAxisMovement)
        {
            m_YAxisMovement();
        }

        if (m_bZAxisMovement)
        {
            m_ZAxisMovement();
        }

        
    }

    // When added into update will allow for the player to move the object in the X axis using the defined Horizontal keys.
    void m_XAxisMovement()
    {
        float l_xDirect = Input.GetAxis("Horizontal") * m_fSpeed * Time.deltaTime;

        transform.Translate(new Vector3(l_xDirect, 0, 0));

        // Limits the movement to a min and maximum possible value. 
        if (transform.position.x >= m_MaxMoveValue.x)
        {
            transform.position = new Vector3(m_MaxMoveValue.x, transform.position.y, transform.position.z);
        }
        else if(transform.position.x <= m_MinMoveValue.x)
        {
            transform.position = new Vector3(m_MinMoveValue.x, transform.position.y, transform.position.z);
        }
    }

    // When added into update will allow for the player to move the object in the Y axis using the defined Vertical keys.
    void m_YAxisMovement()
    {
        float l_yDirect = Input.GetAxis("Vertical") * m_fSpeed * Time.deltaTime;

        transform.Translate(new Vector3(0, l_yDirect, 0));

        if (transform.position.y >= m_MaxMoveValue.y)
        {
            transform.position = new Vector3(transform.position.x, m_MaxMoveValue.y, transform.position.z);
        }
        else if (transform.position.y <= m_MinMoveValue.y)
        {
            transform.position = new Vector3(transform.position.x, m_MinMoveValue.y, transform.position.z);
        }
    }

    // When added into update will allow for the player to move the object in the Z axis using the defined Vertical keys.
    void m_ZAxisMovement()
    {
        float l_zDirect = Input.GetAxis("Vertical") * m_fSpeed * Time.deltaTime;

        transform.Translate(new Vector3(0, 0, l_zDirect));
    }

}
