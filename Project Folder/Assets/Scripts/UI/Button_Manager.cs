using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This will allow for a button to be managed, this will have two key parts. All buttons managed by this class 
/// will have a name, so that it can be found easily, and the button element itself. 
/// </summary>
[System.Serializable]
public struct buttonPair
{
    /// <summary>
    /// The name given to this button. 
    /// </summary>
    public string name;

    /// <summary>
    /// The button element being managed. 
    /// </summary>
    public Button buttonElement; 
};

public class Button_Manager : MonoBehaviour
{
    [SerializeField]
    List<buttonPair> m_ManagedButtons; 

    public void m_AddButton(string name, Button newElement)
    {
        buttonPair l_NewPair = new buttonPair();

        l_NewPair.name = name;
        l_NewPair.buttonElement = newElement;
        
        m_ManagedButtons.Add(l_NewPair); 
    }

    public Button m_GetButton(string reference)
    {
        foreach (var pair in m_ManagedButtons)
        {
            if(pair.name == reference)
            {
                return pair.buttonElement; 
            }
        }

        Debug.LogWarning("Unable to find button in list of managed buttons. ");

        return null;
    }

}
