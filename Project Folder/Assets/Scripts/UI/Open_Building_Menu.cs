using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

/// <summary>
/// This class is used to open the buiding menu, allowing for the player to select a building to be built. 
/// </summary>
public class Open_Building_Menu : MonoBehaviour
{
    #region Data Members 

    [Header("Dynamic Creation Of Buttons")]

    /// <summary>
    /// This is the list of buttons that needs to be created dynammically, the string name in this list 
    /// will directly corrispond to the name of the Button and the text it displays. 
    /// </summary>
    [SerializeField]
    List<string> m_sButtonOptions;
    
    /// <summary>
    /// This is the list of buttons created at the start of this class's existance. This list will 
    /// allow for them to be controlled and updated. 
    /// </summary>
    [SerializeField]
    List<GameObject> m_Buttons;

    /// <summary>
    /// The background for the buttons, this is needed to ensure they are set the correct size and 
    /// positions. 
    /// </summary>
    [SerializeField]
    GameObject m_ButtonBackground;

    /// <summary>
    /// This is a basic button, this will be cloned to allow for new buttons to be created. 
    /// </summary>
    [SerializeField]
    GameObject m_BasicButton;

    #endregion

    #region Member Functions 

    /// <summary>
    /// This is called before the first update for this object, used to initilize and create all 
    /// the buttons in the list. 
    /// </summary>
    void Start()
    {
        // Using the list of buttons create a set of buttons for each option. 

        if (m_sButtonOptions.Count > 0)
        {
            Debug.Log("\nThere are " + m_sButtonOptions.Count + " options for buttons. Creating now. \n");

            foreach (var option in m_sButtonOptions)
            {
                Debug.Log("\n Creating " + option + ",");

                // Instantiate the button using a button prefab. 
                GameObject l_Button = Instantiate(m_BasicButton);

                l_Button.AddComponent<LayoutElement>();

                // Give the button a name and a parent. 
                l_Button.transform.SetParent(m_ButtonBackground.transform);
                l_Button.name = option + " Button";

                // Set the componentsto have the base properties. 

                l_Button.GetComponentInChildren<Text>().text = option; //< Update the text displayed on the button. 

                // Add The new button into the list of buttons. 

                m_Buttons.Add(l_Button); 
            }
        }

        m_HideAll();
    }

    /// <summary>
    /// This will allow access to the full list of buttons created by this class. 
    /// </summary>
    /// <returns>All buttons managed by this class. </returns>
    public List<GameObject> m_GetButtons() => m_Buttons;

    /// <summary>
    /// This will be used to hide the buttons when they don't need to be displayed. 
    /// </summary>
    public void m_HideAll()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// This will be used to check if this is currently visable within the game. 
    /// </summary>
    /// <returns>True if the buttons are visable in the game. </returns>
    public bool m_Visable()
    {
        if (gameObject.activeSelf == true)
        {
            return true;
        }

        return false;
    }

    #endregion
}
