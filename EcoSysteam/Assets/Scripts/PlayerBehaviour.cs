using Unity.Netcode;
using UnityEngine;

public class PlayerBehaviour : Synchronizable
{
    // This method will be called every frame on the server side
    protected override void ServerUpdate() {
        // TODO itt kell varázsolni az új pozíció kiszámításához

        // így el lehet érni az éppen aktuális pozíciót
        // transform.position.z == 0.0f, sztem szerencsésebb Vector2-t használni
        Vector2 currentPos = new Vector2(transform.position.x, transform.position.y);
        Vector2 speed = new Vector2(5, 0); // pixel / sec
        // előző frissítés óta eltelt idő (HASZNÁLJÁTOK PLS, HOGY FPS-FÜGGETLEN LEGYEN):
        float delta = Time.deltaTime; // másodpercben
        // ez itt egy egyenes vonalú egyenletes mozgás
        Vector2 newPos = currentPos + speed * delta;
        // elküldjük a hálózaton az új pozíciót
        UpdatePosition(newPos);
    }

    // This will be called when the prefab spawns, but we only want to randomize the position
    // from the "owner": the client that corresponds to the player
    // (at least if I understand it well xd)
    public override void OnNetworkSpawn() {
        if (IsOwner)
            Move();
    }

    // Ask the server to place the bogyesz on a random position
    public void Move() => SubmitRandomPositionRequestServerRpc();

    // This can be called from the clients, and will be run on the server
    [ServerRpc]
    void SubmitRandomPositionRequestServerRpc(ServerRpcParams rpcParams = default) {
        UpdatePosition(new Vector2(Random.Range(-3f, 3f), Random.Range(-3f, 3f)));
    }
}
