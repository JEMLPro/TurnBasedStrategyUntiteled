using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

/// <summary>
/// This class will be used to activate a build menu for the game. 
/// </summary>
public class Activate_Build_Menu : MonoBehaviour
{
    #region Data Members 

    /// <summary>
    /// This will be the build menu which will be managed by this class. 
    /// </summary>
    [SerializeField]
    GameObject m_BuildMenu;

    #endregion

    #region Member Functions 

    /// <summary>
    /// This will be used to check if the menu is currently active. 
    /// </summary>
    /// <returns></returns>
    public bool m_ActiveBuildMenu()
    {
        return m_BuildMenu.activeSelf;
    }

    /// <summary>
    /// This will be used to set the state of the build menu. 
    /// </summary>
    /// <param name="newValue">To either activate or deative the build menu. </param>
    public void m_SetBuildMenu(bool newValue)
    {
        m_BuildMenu.SetActive(newValue);
    }

    /// <summary>
    /// This will be used to set the button functionality for the build menu. 
    /// </summary>
    /// <param name="unitManager">The unit manager for the player, allowing for the engineers to create new buildings. </param>
    /// <param name="tileMap">The map in which to spawn the new buildings. </param>
    /// <param name="buildingManager">The building manager the new building will be added into. </param>
    public void m_SetButtonFunctions(GameObject unitManager, GameObject tileMap, GameObject buildingManager)
    {
        Button l_FocusButton;

        // Todo add the functionality for the other buttons, allowing for the other buildings to be spawned into the game. 

        l_FocusButton = m_BuildMenu.GetComponent<Open_Building_Menu>().m_GetButtons()[0].GetComponent<Button>();

        l_FocusButton.onClick.AddListener(delegate {
            
            if(unitManager.GetComponent<Unit_Manager>().m_GetSelectedUnit().GetComponent<Unit_Movement>().m_GetCurrentPosition().GetComponent<Cell_Neighbours>().m_NeighboursFree())
            {
                tileMap.GetComponent<Tile_Map_Manager>().m_SetSelectable(true);

                buildingManager.GetComponent<Bulding_Manager>().m_SetBuildingToSpawn(BuildingToSpawn.Farm); 
            }

        }); 
    }

    /// <summary>
    /// This will be used to assign the build menu onto this class allowing for it to be managed. 
    /// </summary>
    /// <param name="newMenu">The new build menu. </param>
    public void m_SetBuildMenu(GameObject newMenu)
    {
        m_BuildMenu = newMenu;
    }

    #endregion
}
