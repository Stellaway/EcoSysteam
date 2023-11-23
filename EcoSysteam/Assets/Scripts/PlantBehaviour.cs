using Unity.Netcode;
using UnityEngine;

public class PlantBehaviour : Synchronizable
{
    // Reference to the Prefab. Drag a Prefab into this field in the Inspector.
    public GameObject myPrefab;

    private float timePeriodTillFruit = 5;
    private float timeCounter = 0;

    // This method will be called every frame on the server side
    protected override void ServerUpdate()
    {
        if (timeCounter < timePeriodTillFruit)
        {
            timeCounter += Time.deltaTime;//ez így viszont sztem nem lesz fps-független, inkább a Time.deltaTime-t kéne összeadogatni (float), másodpercben van elvileg
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

    // This is called only once I think
    public override void OnNetworkSpawn() {
        // we could put the plant on a random location for now
        //if (NetworkManager.Singleton.IsServer)
        //    UpdatePosition(new Vector2(Random.Range(-3f, 3f), Random.Range(-3f, 3f)));
    }
}
