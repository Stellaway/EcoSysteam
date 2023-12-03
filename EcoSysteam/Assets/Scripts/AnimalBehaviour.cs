using System;
using Unity.Netcode;
using UnityEngine;

public class AnimalBehaviour : Synchronizable
{
    // inspectorban állítható:
    public float speed=1;

    // teszt, hogy hogy lehet kommunikálni
    private PlayerBehaviour connectedPlayer;
    private float playerRefreshmentTimer = 0;


    // This method will be called every frame on the server side
    protected override void ServerUpdate() {
        Reproduce();
        
        
        if (connectedPlayer == null || playerRefreshmentTimer > 1.0f)
        {
            refreshClosestPlayer();
            playerRefreshmentTimer = 0;
        }
        if(connectedPlayer == null) { return; }

        // megpróbálunk a connectedplayer felé menni

        // így el lehet érni az éppen aktuális pozíciót
        // transform.position.z == 0.0f, sztem szerencsésebb Vector2-t használni
        Vector2 currentPos = new Vector2(transform.position.x, transform.position.y);
        Vector2 playerPos = new Vector2(connectedPlayer.transform.position.x, connectedPlayer.transform.position.y);
        // ebbe az irányba szeretnénk menni
        Vector2 dir = -(playerPos - currentPos);
        dir.Normalize();
        // előző frissítés óta eltelt idő (HASZNÁLJÁTOK PLS, HOGY FPS-FÜGGETLEN LEGYEN):
        float delta = Time.deltaTime; // másodpercben
        // a játékos felé történő mozgás
        Vector2 newPos = currentPos + dir * speed * delta;

        playerRefreshmentTimer += Time.deltaTime;



        // elküldjük a hálózaton az új pozíciót (TODO ez lehet majd változik)
        if (!PlayerScarilyCloseTestOrFarAf())
            UpdatePosition(newPos);
    }

    private bool PlayerScarilyCloseTestOrFarAf()
    {
        return ((connectedPlayer.transform.position - transform.position).magnitude < 0.4) || ((connectedPlayer.transform.position - transform.position).magnitude > 7);
    }

    public override void OnNetworkSpawn() {
        refreshClosestPlayer();
            
    }

    public void refreshClosestPlayer()
    {
        // valahogyan szerzünk egy referenciát
        // (itt most a spawnoláskor legközelebbi játékost célzom meg)
        if (!NetworkManager.Singleton.IsServer)
            return;
        foreach (ulong uid in NetworkManager.Singleton.ConnectedClientsIds)
        {
            if (NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(uid) == null)
            {
                continue;
            }
            PlayerBehaviour player = NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(uid).GetComponent<PlayerBehaviour>();

            if (connectedPlayer == null ||
            Vector3.Distance(connectedPlayer.transform.position, transform.position)
            > Vector3.Distance(player.transform.position, transform.position))
                connectedPlayer = player;
        }
    }

    // Reference to the Prefab. Drag a Prefab into this field in the Inspector.
    public GameObject childPrefab;

    private float timePeriodTillChild = 15;
    private float timeCounter = 0;
    private System.Random rnd = new System.Random();

    private static int maxSpawnedCount = 100;

    // This method will be called every frame on the server side
    protected void Reproduce()
    {
        if (timeCounter < timePeriodTillChild)
        {
            timeCounter += Time.deltaTime;//ez �gy viszont sztem nem lesz fps-f�ggetlen, ink�bb a Time.deltaTime-t k�ne �sszeadogatni (float), m�sodpercben van elvileg
        }
        else if (GameObject.FindGameObjectsWithTag("Advertiser").Length < maxSpawnedCount)
        {
            //create child
            // Instantiate at position (0, 0, 0) and zero rotation.
            Vector3 pos = this.transform.position;
            pos.y -= 4;
            var add = 1;
            if (rnd.Next(1) == 0) add *= -1;
            pos.x += add * rnd.Next(3);
            if (rnd.Next(1) == 0) add *= -1;
            pos.y += add * rnd.Next(5) * (float)0.33;
            pos = Synchronizable.ClampPos(pos);
            GameObject go = Instantiate(childPrefab, pos, Quaternion.identity);
            go.GetComponent<NetworkObject>().Spawn(); // hogy mindenkin�l megjelenjen

            timeCounter = 0;
        }
    }
}
