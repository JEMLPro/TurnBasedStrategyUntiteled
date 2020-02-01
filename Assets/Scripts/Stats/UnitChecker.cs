using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitChecker : MonoBehaviour
{       //unit is selected is true




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 l_MousePos = Camera.main.ScreenToWorldPoint(
                 new Vector3(Input.mousePosition.x,
                 Input.mousePosition.y,
                 Camera.main.nearClipPlane));

        if ((l_MousePos.x >= gameObject.transform.position.x && l_MousePos.y >= gameObject.transform.position.y) && 
            (l_MousePos.x <= gameObject.transform.position.x + gameObject.GetComponent<Renderer>().bounds.size.x && l_MousePos.y <= gameObject.transform.position.y + gameObject.GetComponent<Renderer>().bounds.size.y))
        {
            Debug.Log("unit has mouse over it");

            //open options menu
        }

        Debug.Log("Mouse : " + l_MousePos.x + " " + l_MousePos.y);
        Debug.Log("Unit : " + gameObject.transform.position.x + " " + gameObject.transform.position.y);

    }
}
