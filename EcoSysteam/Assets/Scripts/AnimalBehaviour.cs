using System;
using Unity.Netcode;
using UnityEngine;

public class AnimalBehaviour : Synchronizable
{
    // inspectorban állítható:
    public float speed=1;

    // teszt, hogy hogy lehet kommunikálni
    private PlayerBehaviour connectedPlayer;


    // This method will be called every frame on the server side
    protected override void ServerUpdate() {
        // TODO itt kell varázsolni az új pozíció kiszámításához

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

        //határon belül marad
        newPos.x = Mathf.Clamp(newPos.x, -7f,7f);
        newPos.y = Mathf.Clamp(newPos.y, -3f,3f);
        
        // elküldjük a hálózaton az új pozíciót (TODO ez lehet majd változik)
        UpdatePosition(PlayerScarilyCloseTest()?transform.position:newPos);
    }

    private bool PlayerScarilyCloseTest()
    {
        return (connectedPlayer.transform.position - transform.position).magnitude > 3;
    }

    public override void OnNetworkSpawn() {
        // valahogyan szerzünk egy referenciát
        // (itt most a spawnoláskor legközelebbi játékost célzom meg)

        foreach (ulong uid in NetworkManager.Singleton.ConnectedClientsIds) {
            PlayerBehaviour player = NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(uid).GetComponent<PlayerBehaviour>();

            if (connectedPlayer == null ||
            Vector3.Distance(connectedPlayer.transform.position, transform.position)
            > Vector3.Distance(player.transform.position, transform.position))
                connectedPlayer = player;
        }
            
    }
}
