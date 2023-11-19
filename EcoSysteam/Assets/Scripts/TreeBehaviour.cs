using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Netcode;
using UnityEngine;

public class TreeBehaviour : PlantBehaviour
{
    // Reference to the Prefab. Drag a Prefab into this field in the Inspector.
    public GameObject myPrefab;

    private int timePeriodTillFruit=5000;
    private int timeCounter = 0;

    // This method will be called every frame on the server side
    protected override void ServerUpdate() { }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(timeCounter < timePeriodTillFruit)
        {
            timeCounter++;
        }
        else
        {
            //create fruit
            // Instantiate at position (0, 0, 0) and zero rotation.
            Instantiate(myPrefab, this.transform.position, Quaternion.identity);

            timeCounter = 0;
        }
    }
}
