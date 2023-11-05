using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public GameObject skillUi; // Reference to your UI element

    void Start()
    {
        // Hide the UI element at the start
        skillUi.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            // Toggle the visibility of the UI element
            skillUi.SetActive(!skillUi.activeSelf);
        }
    }
}