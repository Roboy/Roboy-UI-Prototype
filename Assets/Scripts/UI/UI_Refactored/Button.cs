﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Button : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private bool pointed;
    private bool clicked = false;

    private Menu menu;

    float startTime;
    float currentTimer;

    public float dwellTime = 2.0f;

    // This submenu will be activated if the button is clicked
    public Menu submenuActivateWithClick;

    Image dwellTimeImage;

    // Event to trigger something additional, if button is clicked, like change the state of the state manager for the transition button
    [Serializable]
    public class ButtonClicked : UnityEvent { }

    [SerializeField]
    private ButtonClicked buttonClickedTriggered = new ButtonClicked();
    public ButtonClicked buttonClickedEvent { get { return buttonClickedTriggered; } set { buttonClickedTriggered = value; } }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (pointed == false)
        {
            pointed = true;
            startTime = Time.time;
            //Debug.Log("Enter");
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (pointed == true)
        {
            pointed = false;
            //Debug.Log("Exit");
        }

        clicked = false;
    }

    // This is just a helper to reset clicked when middle button starts transition but mouse never gets moved.
    public void SetClickedFalse()
    {
        clicked = false;
    }

    private void Click()
    {
        if (clicked == false)
        {
            LogText.Instance.addToLogText("Clicked at " + this.name + " inside Button class");

            if (submenuActivateWithClick != null)
                submenuActivateWithClick.Activate();

            buttonClickedEvent.Invoke();
        }

        clicked = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        dwellTimeImage = this.GetComponent<Image>();

        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        // If submenu is active, keep the button filled
        if (submenuActivateWithClick != null && submenuActivateWithClick.GetActive())
            dwellTimeImage.fillAmount = 1;

        // If submenu is inactive and button is not pointed, reset button fill to 0
        else if (submenuActivateWithClick != null && submenuActivateWithClick.GetActive() == false && pointed == false)
            dwellTimeImage.fillAmount = 0;

        // if submenu is null and button is not pointed, reset button fill to 0
        else if (submenuActivateWithClick == null && pointed == false)
            dwellTimeImage.fillAmount = 0;

        // Draw dwell time indicator
        float f;
        if (pointed && clicked == false)
        {
            currentTimer = Time.time;
            f = (currentTimer - startTime) / dwellTime;
            dwellTimeImage.fillAmount = f;

            if (f >= 1.0f)
            {
                startTime = Time.time;
                Click();
            }

            // If the button is pointed, the menu is also pointed
            menu.Highlight();
        }
    }

    public void SetMenu(Menu menu)
    {
        this.menu = menu;
    }
}