using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

[SerializeField]
public enum UnitTraningRating
{
    Simple = 0x101, 
    Advanced = 0x201, 
    Expert = 0x301
}

[System.Serializable]
public struct ButtonVisibility
{
    public GameObject thisObject;

    public UnitTraningRating visibilityRating; 
}

public class Open_Unit_Spawn_Menu : MonoBehaviour
{
    #region Data Members 

    [SerializeField]
    bool m_bopenMenu = false;

    [Header("Dynamic Creation Of Buttons")]

    [SerializeField]
    List<string> m_sButtonOptions;

    [SerializeField]
    List<ButtonVisibility> m_Buttons;

    [SerializeField]
    GameObject m_ButtonBackground;

    [SerializeField]
    GameObject m_BasicButton;

    #endregion

    private void Start()
    {
        // Using the list of buttons create a set of buttons for each option. 

        if(m_sButtonOptions.Count > 0)
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

                ButtonVisibility l_ComboButton = new ButtonVisibility();

                l_ComboButton.thisObject = l_Button;

                switch (option) //< Set the object's traning rating and the tag for the object.
                {
                    case "Militia":
                        l_ComboButton.visibilityRating = UnitTraningRating.Simple;
                        l_ComboButton.thisObject.tag = "Militia_Button";
                        break;

                    case "Engineer":
                        l_ComboButton.visibilityRating = UnitTraningRating.Simple;
                        l_ComboButton.thisObject.tag = "Engineer_Button";
                        break;

                    case "Swordsman":
                        l_ComboButton.visibilityRating = UnitTraningRating.Advanced;
                        l_ComboButton.thisObject.tag = "Swordsman_Button";
                        break;

                    case "Lancer":
                        l_ComboButton.visibilityRating = UnitTraningRating.Advanced;
                        l_ComboButton.thisObject.tag = "Lancer_Button";
                        break;

                    case "Marauder":
                        l_ComboButton.visibilityRating = UnitTraningRating.Advanced;
                        l_ComboButton.thisObject.tag = "Marauder_Button";
                        break;

                    default:
                        break;
                }

                m_Buttons.Add(l_ComboButton); 
            }
        }

        m_HideAll(); 
    }

    public void m_HQSelected()
    {
        m_ButtonBackground.SetActive(true);

        gameObject.SetActive(true);

        foreach (var button in m_Buttons)
        {
            if(button.visibilityRating == UnitTraningRating.Simple)
            {
                button.thisObject.SetActive(true); 
            }
        }
    }

    public void m_BarracksSelected()
    {
        m_ButtonBackground.SetActive(true);

        gameObject.SetActive(true);

        foreach (var button in m_Buttons)
        {
            if (button.visibilityRating == UnitTraningRating.Advanced)
            {
                button.thisObject.SetActive(true);
            }
        }
    }

    public void m_HideAll()
    {
        m_ButtonBackground.SetActive(false); 

        gameObject.SetActive(false);
        
        foreach (var button in m_Buttons)
        {
            button.thisObject.SetActive(false); 
        }

    }

    public bool m_Visable()
    {
        if(gameObject.activeSelf == true)
        {
            return true; 
        }

        return false;
    }

    public List<ButtonVisibility> m_GetButtons() => m_Buttons; 
}
