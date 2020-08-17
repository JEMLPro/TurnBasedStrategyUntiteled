using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class Open_Building_Menu : MonoBehaviour
{
    #region Data Members 

    [SerializeField]
    bool m_bopenMenu = false;

    [Header("Dynamic Creation Of Buttons")]

    [SerializeField]
    List<string> m_sButtonOptions;

    [SerializeField]
    List<GameObject> m_Buttons;

    [SerializeField]
    GameObject m_ButtonBackground;

    [SerializeField]
    GameObject m_BasicButton;

    #endregion

    // Start is called before the first frame update
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

    public List<GameObject> m_GetButtons() => m_Buttons;

    public void m_HideAll()
    {
        gameObject.SetActive(false);
    }

    public bool m_Visable()
    {
        if (gameObject.activeSelf == true)
        {
            return true;
        }

        return false;
    }

}
