using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//---------------------------------------------------------------------------------------------------------------------------\\
// File Start
//---------------------------------------------------------------------------------------------------------------------------\\

/// <summary>
/// This class will allow for a prefab to be loaded onto an object. 
/// </summary>
public class Prefab_Loader : MonoBehaviour
{
    //---------------------------------------------------------------------------------------------------------------------------\\
    // Data Members Start
    //---------------------------------------------------------------------------------------------------------------------------\\

    /// <summary>
    /// This will be used to store the loaded prefab object onto the connected class. 
    /// </summary>
    GameObject m_LoadedObject = null;

    /// <summary>
    /// This is the name of the object which has been loaded. 
    /// </summary>
    string m_sObjectName = null;

    //---------------------------------------------------------------------------------------------------------------------------\\
    // Member Functions Start
    //---------------------------------------------------------------------------------------------------------------------------\\

    /// <summary>
    /// This will load a prefab object using a file path and name and store it on within this script for external use. 
    /// </summary>
    /// <param name="filePath">The path to find the intended object. </param>
    /// <param name="objName">The name of the object being loaded. </param>
    /// <returns>A bool intedted to check if the prefab has been loaded. </returns>
    public bool m_LoadPrefabObject(string filePath, string objName)
    {
        // Debugging \\
        //Debug.Log("Attempting to load object " + objName + " at file location " + filePath);

        UnityEngine.Object l_PrefabToLoad = Resources.Load(filePath);

        if (l_PrefabToLoad != null)
        {
            // Debugging \\
            // Debug.Log(objName + " prefab found.");

            m_LoadedObject = (GameObject)GameObject.Instantiate(l_PrefabToLoad, gameObject.transform);

            if (m_LoadedObject != null)
            {
                // Debugging \\
                // Debug.Log(objName + " Loaded into game");

                m_sObjectName = objName;

                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// This will be used to load a prefab object and export it onto another object. 
    /// </summary>
    /// <param name="filePath">The path to find the intended object. </param>
    /// <param name="objName">The name of the object being loaded. Used for debugging purposes.  </param>
    /// <returns>A game object loaded from the file. </returns>
    public GameObject m_ExportPrefabObject(string filePath, string objName)
    {
        // Debug.Log("Attempting to load object " + objName);

        UnityEngine.Object l_Prefab = Resources.Load(filePath);

        if (l_Prefab != null)
        {
           // Debug.Log(objName + " prefab found.");

            GameObject l_ReturnObj = (GameObject)l_Prefab;

            if (l_ReturnObj != null)
            {
               // Debug.Log(objName + " Loaded into game");

                return l_ReturnObj;
            }
        }

        return null;
    }

    /// <summary>
    /// This will allow access to the object loaded into this class. 
    /// </summary>
    /// <returns></returns>
    public GameObject m_GetLoadedObject() => m_LoadedObject;

    /// <summary>
    /// This will return the name of the object loaded by this class.
    /// </summary>
    /// <returns></returns>
    string m_GetObjectName() => m_sObjectName;

    //---------------------------------------------------------------------------------------------------------------------------\\
    // File End
    //---------------------------------------------------------------------------------------------------------------------------\\
}
