using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class Activate_Build_Menu : MonoBehaviour
{
    [SerializeField]
    GameObject m_BuildMenu;

    public bool m_ActiveBuildMenu()
    {
        return m_BuildMenu.activeSelf;
    }

    public void m_SetBuildMenu(bool newValue)
    {
        m_BuildMenu.SetActive(newValue);
    }

    public void m_SetButtonFunctions(GameObject unitManager, GameObject tileMap, GameObject buildingManager)
    {
        Button l_FocusButton;

        l_FocusButton = m_BuildMenu.GetComponent<Open_Building_Menu>().m_GetButtons()[0].GetComponent<Button>();

        l_FocusButton.onClick.AddListener(delegate {
            
            if(unitManager.GetComponent<Unit_Manager>().m_GetSelectedUnit().GetComponent<Unit_Movement>().m_GetCurrentPosition().GetComponent<Cell_Neighbours>().m_NeighboursFree())
            {
                tileMap.GetComponent<Tile_Map_Manager>().m_SetSelectable(true);

                buildingManager.GetComponent<Bulding_Manager>().m_SetBuildingToSpawn(BuildingToSpawn.Farm); 
            }

        }); 
    }

    public void m_SetBuildMenu(GameObject newMenu)
    {
        m_BuildMenu = newMenu;
    }
}
