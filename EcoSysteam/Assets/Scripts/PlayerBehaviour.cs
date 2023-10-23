using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerBehaviour : Synchronizable
{
    public static int STARTINGMAXHEALTH = 100;
    public static int STARTINGSENSORDISTANCE = 10; //??? majd kitaláljuk mennyi

    private int Hunger { get; set; } = 0;
    private int Health { get; set; } = STARTINGMAXHEALTH;
    private int MaxHealth { get; set; } = STARTINGMAXHEALTH;

    private int SensorDistance { get; set; } = STARTINGSENSORDISTANCE;

    private IAdvertiser.Intent? CurrentIntent { get; set; } = null;
    
    //TODO ez csak ideiglenesen van itt
    private GameObject currTarget {  get; set; }



    // This method will be called every frame on the server side
    protected override void ServerUpdate() {
        // TODO itt kell varázsolni az új pozíció kiszámításához
        Act();

        

        
    }

    // visszaadja az érzékelt gameobjectecet, azoknak a collider-ét használva
    private List<GameObject> getNearGameObjects()
    {
        var list = new List<GameObject>();

        Collider[] colliders = Physics.OverlapSphere(transform.position, SensorDistance);

        foreach (Collider collider in colliders)
        {
            list.Add(collider.gameObject);
        }

        return list;
    }

    // Kőkorszaki egyszerűségű döntő script
    private void Evaluate(GameObject gameObject)
    {
        if(currTarget != null)
        {
            //choose closest object
            if ((this.transform.position -  currTarget.transform.position).sqrMagnitude > (this.transform.position - gameObject.transform.position).sqrMagnitude) {
                currTarget = gameObject;
            }
        } else
        {
            currTarget = gameObject;
        }
    }

    // Megnézi a környezetet és beforog a megfelelő irányba
    // Lép a megfelelő irányba

    //TODO Valahogy tag-ekből kivadászni minden prefabnak egy "advertisement scriptet" és akkor onnan meg lehet szerezni az intentet
    private void Act()
    {
        var nearGameObjects = getNearGameObjects();


        foreach (var nearGameObject in nearGameObjects)
        {
            Evaluate(nearGameObject);
        }
        




        // így el lehet érni az éppen aktuális pozíciót
        // transform.position.z == 0.0f, sztem szerencsésebb Vector2-t használni
        Vector2 currentPos = new Vector2(transform.position.x, transform.position.y);
        Vector2 speed = (currTarget.transform.position - this.transform.position).normalized * 2; // pixel / sec
        // előző frissítés óta etlelt idő (HASZNÁLJÁTOK PLS, HOGY FPS-FÜGGETLEN LEGYEN):
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

     override public List<IAdvertiser.Intent> GetAdverisement()
    {
        throw new System.NotImplementedException();
    }
}
