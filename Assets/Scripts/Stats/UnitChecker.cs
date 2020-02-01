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
        if ((Input.mousePosition.x >= gameObject.transform.position.x && Input.mousePosition.y >= gameObject.transform.position.y && (Input.mousePosition.x <= gameObject.transform.position.x + gameObject.GetComponent<Renderer>().bounds.size.x && Input.mousePosition.y <= gameObject.transform.position.y + gameObject.GetComponent<Renderer>().bounds.size.y)))
        {
            Debug.Log("unit has mouse over it");

            //open options menu
        }

    }
}
