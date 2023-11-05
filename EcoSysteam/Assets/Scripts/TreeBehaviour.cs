using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Netcode;
using UnityEngine;

public class TreeBehaviour : PlantBehaviour
{
    private int timePeriodTillFruit;
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
            timeCounter = 0;
        }
    }
}
