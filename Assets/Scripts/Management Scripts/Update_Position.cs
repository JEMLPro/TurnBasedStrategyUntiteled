using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Update_Position : MonoBehaviour
{
    float m_speed = 0.05f;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 0, -10);
    }

    // Update is called once per frame
    void Update()
    {
        float l_xDirect = Input.GetAxis("Horizontal") * m_speed;
        float l_yDirect = Input.GetAxis("Vertical") * m_speed;

        transform.Translate(new Vector3(l_xDirect, l_yDirect, 0)); 

    }
}
