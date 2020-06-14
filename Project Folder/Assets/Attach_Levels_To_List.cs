using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Attach_Levels_To_List : MonoBehaviour
{
    [SerializeField]
    Dropdown m_LevelDropDown;

    int m_iLevelToLoad = -1;

    bool m_bLoadnewLevel = false; 

    public void m_StartUp()
    {
        if (m_LevelDropDown == null)
        {
            m_LevelDropDown = gameObject.GetComponent<Dropdown>();
        }

        m_LevelDropDown.onValueChanged.AddListener(delegate { m_ValueChanged(); });

        m_LevelDropDown.ClearOptions();
    }

    public void m_AddItemIntoDropdown(string nameToAdd, int index)
    {

        if (index < m_LevelDropDown.options.Count)
        {
            Dropdown.OptionData l_NewItem = new Dropdown.OptionData(nameToAdd);

            m_LevelDropDown.options.RemoveAt(index);

            m_LevelDropDown.options.Insert(index, l_NewItem);

            Debug.Log(nameToAdd + " has been added to the dropdown list at position " + index + ".");
        }
        else
        {
            Dropdown.OptionData l_NewItem = new Dropdown.OptionData(nameToAdd);

            m_LevelDropDown.options.Add(l_NewItem);

            Debug.Log(nameToAdd + " has been onto the end of the dropdown list.");

        }
    }

    private void m_ValueChanged()
    {
        m_iLevelToLoad = m_LevelDropDown.value;

        m_bLoadnewLevel = true; 
    }

    public bool m_GetLoadNewLevel() => m_bLoadnewLevel;

    public int m_GetLevelToLoad() => m_iLevelToLoad;

    public void m_LevelHasBeenLoaded()
    {
        m_bLoadnewLevel = false;
    }

}
