using Unity.Netcode;
using UnityEngine;

public class PlantBehaviour : Synchronizable
{
    // This method will be called every frame on the server side
    protected override void ServerUpdate() {}

    // This is called only once I think
    public override void OnNetworkSpawn() {
        // we could put the plant on a random location for now
        //if (NetworkManager.Singleton.IsServer)
        //    UpdatePosition(new Vector2(Random.Range(-3f, 3f), Random.Range(-3f, 3f)));
    }
}
