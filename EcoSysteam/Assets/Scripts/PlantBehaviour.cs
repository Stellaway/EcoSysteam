using Unity.Netcode;
using UnityEngine;

public class PlantBehaviour : Synchronizable
{
    // Reference to the Prefab. Drag a Prefab into this field in the Inspector.
    public GameObject myPrefab;

    private float timePeriodTillFruit = 10;
    private float timeCounter = 0;

    // This method will be called every frame on the server side
    protected override void ServerUpdate()
    {
        if (timeCounter < timePeriodTillFruit)
        {
            timeCounter += Time.deltaTime;//ez �gy viszont sztem nem lesz fps-f�ggetlen, ink�bb a Time.deltaTime-t k�ne �sszeadogatni (float), m�sodpercben van elvileg
        }
        else
        {
            //create fruit
            // Instantiate at position (0, 0, 0) and zero rotation.
            GameObject go = Instantiate(myPrefab, this.transform.position, Quaternion.identity);
            go.GetComponent<NetworkObject>().Spawn(); // hogy mindenkin�l megjelenjen

            timeCounter = 0;
        }
    }
}
