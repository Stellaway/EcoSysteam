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
        Vector2 speed = new Vector2(2, 0); // pixel / sec
        // előző frissítés óta eltelt idő (HASZNÁLJÁTOK PLS, HOGY FPS-FÜGGETLEN LEGYEN):
        float delta = Time.deltaTime; // másodpercben
        // ez itt egy egyenes vonalú egyenletes mozgás
        Vector2 newPos = currentPos + speed * delta;
        // elküldjük a hálózaton az új pozíciót (TODO ez lehet majd változik)
        UpdatePosition(newPos);
    }

    // This will be called when the prefab spawns, but we only want to randomize the position
    // from the "owner": only from the client that corresponds to the player
    // (at least if I understand it well xd)
    public override void OnNetworkSpawn() {
        if (IsOwner)
            Move();
    }

    // Ask the server to place the bogyesz on a random position
    // it is called from the GUI, and from OnNetworkSpawn
    // and it may be called from the server from the GUI
    public void Move() {
        if (NetworkManager.Singleton.IsServer) MoveServer();
        else SubmitRandomPositionRequestServerRpc();
    }

    // This can be called from the clients, and will be run on the server
    [ServerRpc]
    private void SubmitRandomPositionRequestServerRpc(ServerRpcParams rpcParams = default) {
        MoveServer();
    }

    private void MoveServer() {
        spawnTest();
        UpdatePosition(new Vector2(Random.Range(-3f, 3f), Random.Range(-3f, 3f)));
    }


    // a spawnolás teszthez (inspectorban drag-n-droppal be lehet tenni)
    public GameObject BushPrefab;
    public GameObject TreePrefab;
    public GameObject MousePrefab;
    public GameObject WormPrefab;

    private void spawnTest() {
        // TODO teszt, spawnolásra, most akkor, ha move-ra nyomunk
        // https://docs-multiplayer.unity3d.com/netcode/current/basics/object-spawning/
        // szerveren lehet csak spawnolni (a randomot biztos lehetne szebben)
        // illetve a pozíciót is itt kéne megadni, és nem spawnolás után TODO xd!
        // most úgy csináltam, hogy a teleport előtti pozícióra teszi a dolgot
        // itt tudjuk majd, hogy hová is szeretnénk tenni
        // persze ez állatoknál nem ilyen egyszerű, ők úgyis mozognak később is
        int rnd = (int)Random.Range(0f, 3.99f);
        GameObject mitAkarokSpawnolni = BushPrefab;
        switch(rnd) {
            case 0: mitAkarokSpawnolni = BushPrefab; break;
            case 1: mitAkarokSpawnolni = TreePrefab; break;
            case 2: mitAkarokSpawnolni = MousePrefab; break;
            case 3: mitAkarokSpawnolni = WormPrefab; break;
            default: break;
        }

        //GameObject go = Instantiate(mitAkarokSpawnolni, Vector3.zero, Quaternion.identity);
        Vector3 newPos = new Vector3(transform.position.x, transform.position.y, 0);
        GameObject go = Instantiate(mitAkarokSpawnolni, newPos, Quaternion.identity);
        go.GetComponent<NetworkObject>().Spawn();
    }
}
