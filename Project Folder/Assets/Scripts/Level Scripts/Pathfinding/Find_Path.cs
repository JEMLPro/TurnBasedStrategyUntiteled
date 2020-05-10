using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*! \class This will be used to generate a path between two cells on a grid based map. */
public class Find_Path : MonoBehaviour
{
    [SerializeField]
    GameObject m_StartCell = null; /*!< \var This is the starting point for the algorithm. */

    [SerializeField]
    GameObject m_EndCell = null; /*! \var This is the target location for the pathfinding. */

    List<GameObject> m_OpenSet; /*!< \var This is the list of objects which still need to be checked. */ 

    List<GameObject> m_ClosedSet; /*!< \var This is the list of objects which have already been checked. */



}
