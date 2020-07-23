using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//---------------------------------------------------------------------------------------------------------------------------\\
// File Start
//---------------------------------------------------------------------------------------------------------------------------\\

/// <summary>
/// This class will alow for the connected object to be moved in the game world in three different possible directions; (X, Y, Z).
/// </summary>
public class Move_Object : MonoBehaviour
{
    //---------------------------------------------------------------------------------------------------------------------------\\
    // Data Members Start
    //---------------------------------------------------------------------------------------------------------------------------\\

    /// <summary>
    /// This is the speed of movement the connected object will move at. This can only be changed within this script. 
    /// </summary>
    [SerializeField]
    const float m_fSpeed = 10.0f; 

    /// <summary>
    /// If this is set to true, the connected object can be moved Left and Right. 
    /// </summary>
    [SerializeField]
    bool m_bXAsisMovement = true;

    /// <summary>
    /// If this is set to true, the connected object can be moved Up and Down. 
    /// </summary>
    [SerializeField]
    bool m_bYAxisMovement = true;

    /// <summary>
    /// If this is set to true, the connected object can be moved Forwards and Backwards. 
    /// </summary>
    [SerializeField]
    bool m_bZAxisMovement = false; 

    /// <summary>
    /// This is the lower bounds of the object's movement. If there is no level bounds created then this can be used to hard 
    /// limit the position the connected object can move, combined with the other bound this can form a box the object cannot 
    /// leave. 
    /// </summary>
    [SerializeField]
    Vector3 m_MinMoveValue;

    /// <summary>
    /// This is the upper bounds of the object's movement. If there is no level bounds created then this can be used to hard 
    /// limit the position the connected object can move, combined with the other bound this can form a box the object cannot 
    /// leave. 
    /// </summary>
    [SerializeField]
    Vector3 m_MaxMoveValue; 

    //---------------------------------------------------------------------------------------------------------------------------\\
    // Member Functions Start 
    //---------------------------------------------------------------------------------------------------------------------------\\

    /// <summary>
    /// This will be called at the start of the game, it will be used to set the starting position of the connected object.
    /// </summary>
    private void Start()
    {
        gameObject.transform.position = new Vector3(0, 0, 0);
    }

    /// <summary>
    /// Called once per frame, this will update the position of the connectd object. 
    /// </summary>
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

    /// <summary>
    /// If X axis movement is selected this function will allow for the connected object to move along the X axis. 
    /// </summary>
    void m_XAxisMovement()
    {
        float l_xDirect = Input.GetAxis("Horizontal") * m_fSpeed * Time.deltaTime;

        transform.Translate(new Vector3(l_xDirect, 0, 0));


        // Limits the movement to a min and maximum possible value. 

        if (transform.position.x >= m_MinMoveValue.x)
        {
            transform.position = new Vector3(m_MinMoveValue.x, transform.position.y, transform.position.z);
        }
        else if (transform.position.x <= m_MaxMoveValue.x)
        {
            transform.position = new Vector3(m_MaxMoveValue.x, transform.position.y, transform.position.z);
        }
    }

    /// <summary>
    /// If Y axis movement is selected this function will allow for the connected object to move along the Y axis. 
    /// </summary>
    void m_YAxisMovement()
    {
        float l_yDirect = Input.GetAxis("Vertical") * m_fSpeed * Time.deltaTime;

        transform.Translate(new Vector3(0, l_yDirect, 0));

        if (transform.position.y >= m_MinMoveValue.y)
        {
            transform.position = new Vector3(transform.position.x, m_MinMoveValue.y, transform.position.z);
        }
        else if (transform.position.y <= m_MaxMoveValue.y)
        {
            transform.position = new Vector3(transform.position.x, m_MaxMoveValue.y, transform.position.z);
        }
    }

    /// <summary>
    /// If Z axis movement is selected this function will allow for the connected object to move along the Z axis. 
    /// </summary>
    void m_ZAxisMovement()
    {
        float l_zDirect = Input.GetAxis("Vertical") * m_fSpeed * Time.deltaTime;

        transform.Translate(new Vector3(0, 0, l_zDirect));
    }

    /// <summary>
    /// This will be used to assign a new min point for the movement script. 
    /// </summary>
    /// <param name="newbound">The new minimum position for the connected object. </param>
    public void m_SetMinBounds(Vector3 newbound) { m_MinMoveValue = newbound; }

    /// <summary>
    /// This will be used to assign a new max poit for the movement script. 
    /// </summary>
    /// <param name="newbound">The new maximum point for the connected object. </param>
    public void m_SetMaxBounds(Vector3 newbound) { m_MaxMoveValue = newbound; }

    //---------------------------------------------------------------------------------------------------------------------------\\
    // File End
    //---------------------------------------------------------------------------------------------------------------------------\\

}
