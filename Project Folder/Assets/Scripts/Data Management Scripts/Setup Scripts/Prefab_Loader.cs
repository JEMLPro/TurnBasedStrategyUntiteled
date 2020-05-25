using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Prefab_Loader : MonoBehaviour
{
    [SerializeField]
    GameObject m_LoadedObject = null;

    [SerializeField]
    string m_sObjectName = null;

    public bool m_LoadPrefabObject(string filePath, string objName)
    {
        Debug.Log("Attempting to load object " + objName);

        UnityEngine.Object l_MapPrefab = Resources.Load(filePath);

        if (l_MapPrefab != null)
        {
            Debug.Log(objName + " prefab found.");

            m_LoadedObject = (GameObject)GameObject.Instantiate(l_MapPrefab, gameObject.transform);

            if (m_LoadedObject != null)
            {
                Debug.Log(objName + " Loaded into game");

                m_sObjectName = objName;

                return true;
            }
        }

        return false;
    }

    public GameObject m_ExportPrefabObject(string filePath, string objName)
    {
        Debug.Log("Attempting to load object " + objName);

        UnityEngine.Object l_Prefab = Resources.Load(filePath);

        if (l_Prefab != null)
        {
            Debug.Log(objName + " prefab found.");

            GameObject l_ReturnObj = (GameObject)GameObject.Instantiate(l_Prefab, gameObject.transform);

            if (l_ReturnObj != null)
            {
                Debug.Log(objName + " Loaded into game");

                return l_ReturnObj;
            }
        }

        return null;
    }

    public GameObject m_GetLoadedObject() => m_LoadedObject;

    string m_GetObjectName() => m_sObjectName;
}
