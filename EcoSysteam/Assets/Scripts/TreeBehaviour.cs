using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class TreeBehaviour : PlantBehaviour
{
    private int timePeriodTillFruit;
    private int timeCounter = 0;

    [SerializeField] public GameObject _fruitPrefab;

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
            /*GameObject fruit = (GameObject)*/Instantiate(MousePrefab);
            //fruit.name = "Fruit" + noFruit++;
            //NetworkServer.Spawn(fruit);
            timeCounter = 0;
        }
    }
}
