using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Netcode;

public class TreeBehaviour : PlantBehaviour
{
    // Reference to the Prefab. Drag a Prefab into this field in the Inspector.
    public GameObject myPrefab;

    private int timePeriodTillFruit=5000;
    private int timeCounter = 0;

    // This method will be called every frame on the server side
    protected override void ServerUpdate() {
        base.ServerUpdate();
        if(timeCounter < timePeriodTillFruit)
        {
            timeCounter++;//ez így viszont sztem nem lesz fps-független, inkább a delta-t kéne összeadogatni
        }
        else
        {
            //create fruit
            // Instantiate at position (0, 0, 0) and zero rotation.
            GameObject go = Instantiate(myPrefab, this.transform.position, Quaternion.identity);
            go.GetComponent<NetworkObject>().Spawn(); // hogy mindenkinél megjelenjen

            timeCounter = 0;
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //TODO ezt inkább a ServerUpdate-ben, hogy csak a szerver spawnoljon, és szinkronban maradjon a kliensekkel
    }
}
