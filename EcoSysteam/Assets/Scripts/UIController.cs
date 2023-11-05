using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public GameObject yourUIElement; // Reference to your UI element

    void Start()
    {
        // Hide the UI element at the start
        yourUIElement.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            // Toggle the visibility of the UI element
            yourUIElement.SetActive(!yourUIElement.activeSelf);
        }
    }
}