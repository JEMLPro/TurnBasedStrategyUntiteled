﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

/// <summary>
/// This class will be used to activate and display a radial menu within the game. 
/// </summary>
public class Activate_Radial_Menu : MonoBehaviour
{
    #region Data Members 

    /// <summary>
    /// This is the radial menu being displayed and controlled by this function. 
    /// </summary>
    [SerializeField]
    GameObject m_RadialMenu = null;

    /// <summary>
    /// This is the Rect transform for the Radial menu to be diaplayed, allowing for it's position to be updated 
    /// and manipulated. 
    /// </summary>
    [SerializeField]
    RectTransform m_PieMenu = null;

    /// <summary>
    /// This is the main canvas used to display the UI within the game. 
    /// </summary>
    [SerializeField]
    RectTransform m_Canvas = null;

    #endregion

    #region Member Functions 

    /// <summary>
    /// This will be used to assign the components needed for the Radial menu to be functional. This will also setup 
    /// the buttons within this radial menu.
    /// </summary>
    /// <param name="radialMenu">The Radial menu to be managed. </param>
    /// <param name="pieMenu">The Transform for the radial menu allowing for it to be moved on the canvas. </param>
    /// <param name="canvas">The main canvas used to display the game's UI. </param>
    /// <param name="unitManager">The Unit manager for the game, allowing for the Pie menu to be placed on top of units. </param>
    public void m_SetRadialMenuObject(GameObject radialMenu, RectTransform pieMenu, RectTransform canvas, GameObject unitManager)
    {
        m_RadialMenu = radialMenu;
        m_PieMenu = pieMenu;
        m_Canvas = canvas;

        foreach (var button in radialMenu.GetComponentInChildren<RMF_RadialMenu>().elements)
        {
            switch (button.tag)
            {
                #region Wait Button 
                case "Wait_Button":

                    Debug.Log(button.tag + " Found");

                    if (button.GetComponentInChildren<Button>())
                    {
                        button.GetComponentInChildren<Button>().onClick.RemoveAllListeners();

                        button.GetComponentInChildren<Button>().onClick.AddListener(delegate { unitManager.GetComponent<Unit_Manager>().m_SetActionWait(); });

                        Debug.Log("Button functionality assiggned");
                    }

                    break;
                #endregion

                #region Move Button 
                case "Move_Button":

                    Debug.Log(button.tag + " Found");

                    if (button.GetComponentInChildren<Button>())
                    {
                        button.GetComponentInChildren<Button>().onClick.RemoveAllListeners();

                        button.GetComponentInChildren<Button>().onClick.AddListener(delegate { unitManager.GetComponent<Unit_Manager>().m_SetActionMove(); });

                        Debug.Log("Button functionality assiggned");
                    }

                    break;
                #endregion

                #region Attack Button
                case "Attack_Button":

                    Debug.Log(button.tag + " Found");

                    if(button.GetComponentInChildren<Button>())
                    {
                        button.GetComponentInChildren<Button>().onClick.RemoveAllListeners();

                        button.GetComponentInChildren<Button>().onClick.AddListener(delegate { unitManager.GetComponent<Unit_Manager>().m_SetActionAttack(); });

                        Debug.Log("Button functionality assiggned");
                    }

                    break;

                #endregion

                #region Build Button
                case "Build_Button":

                    Debug.Log(button.tag + " Found");

                    if (button.GetComponentInChildren<Button>())
                    {
                        button.GetComponentInChildren<Button>().onClick.RemoveAllListeners();

                        button.GetComponentInChildren<Button>().onClick.AddListener(delegate { unitManager.GetComponent<Unit_Manager>().m_SetActionBuild(); });

                        Debug.Log("Button functionality assiggned");
                    }

                    break;

                #endregion

                default:

                    Debug.Log(button.tag + " Found - Undefined functionality. ");

                    break;
            }

            if (m_RadialMenu.activeSelf == true)
            {
                m_RadialMenu.SetActive(false);
            }
        }
    }

    /// <summary>
    /// This will activate or deactivate the radial menu for the game. 
    /// </summary>
    /// <param name="attachedObject">This will be the unit to place the menu on top of. </param>
    /// <param name="menuState">This is the new state of the menu. </param>
    public void m_ActivateMenu(GameObject attachedObject, bool menuState)
    {
        m_RadialMenu.SetActive(menuState); 

        if (menuState != false)
        {
            Vector2 l_NewPos = WorldToCanvasPosition(m_Canvas, Camera.main, attachedObject.transform.position); 

            m_PieMenu.anchoredPosition = l_NewPos;
        }

        #region Set active buttons
        
        // This will be used to only display the buttons which the player will be able to use. 

        if (gameObject.GetComponent<Unit_Manager>().m_CheckAction(Action.Attack))
        {
            m_RadialMenu.GetComponent<Button_Manager>().m_GetButton("Attack").gameObject.SetActive(true);
        }
        else
        {
            m_RadialMenu.GetComponent<Button_Manager>().m_GetButton("Attack").gameObject.SetActive(false);
        }

        if (gameObject.GetComponent<Unit_Manager>().m_CheckAction(Action.Move))
        {
            m_RadialMenu.GetComponent<Button_Manager>().m_GetButton("Move").gameObject.SetActive(true);
        }
        else
        {
            m_RadialMenu.GetComponent<Button_Manager>().m_GetButton("Move").gameObject.SetActive(false);
        }

        if (gameObject.GetComponent<Unit_Manager>().m_CheckAction(Action.Build))
        {
            m_RadialMenu.GetComponent<Button_Manager>().m_GetButton("Build").gameObject.SetActive(true);
        }
        else
        {
            m_RadialMenu.GetComponent<Button_Manager>().m_GetButton("Build").gameObject.SetActive(false);
        }

        #endregion
    }

    /// <summary>
    /// This will be use to convert the world positioning to canvas positioning. 
    /// </summary>
    /// <param name="canvas">The main game canvas. </param>
    /// <param name="camera">The main canvas. </param>
    /// <param name="position">The position to convert. </param>
    /// <returns></returns>
    private Vector2 WorldToCanvasPosition(RectTransform canvas, Camera camera, Vector3 position)
    {
        //Vector position (percentage from 0 to 1) considering camera size.
        //For example (0,0) is lower left, middle is (0.5,0.5)
        Vector2 temp = camera.WorldToViewportPoint(position);

        //Calculate position considering our percentage, using our canvas size
        //So if canvas size is (1100,500), and percentage is (0.5,0.5), current value will be (550,250)
        temp.x *= canvas.sizeDelta.x;
        temp.y *= canvas.sizeDelta.y;

        //The result is ready, but, this result is correct if canvas recttransform pivot is 0,0 - left lower corner.
        //But in reality its middle (0.5,0.5) by default, so we remove the amount considering cavnas rectransform pivot.
        //We could multiply with constant 0.5, but we will actually read the value, so if custom rect transform is passed(with custom pivot) , 
        //returned value will still be correct.

        temp.x -= canvas.sizeDelta.x * canvas.pivot.x;
        temp.y -= canvas.sizeDelta.y * canvas.pivot.y;

        return temp;
    }

    #endregion
}
