using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public abstract class Synchronizable : NetworkBehaviour
{
    // This method sends the position over the network, and checks for bounds.
    // Must be called from the server side
    protected void UpdatePosition(Vector2 pos) {
        //határon belül marad
        pos.x = Mathf.Clamp(pos.x, -15.1f, 15.1f);
        pos.y = Mathf.Clamp(pos.y, -8.5f, 8.5f);
        transform.position = new Vector3(pos.x, pos.y, 0.0f);
    }
    // This method will be run from the server side, override it to implement the movement / action
    protected abstract void ServerUpdate();

    // Update is called once per frame
    void Update() {
        // Calling the function to calculate the new state, only on the server side
        if (NetworkManager.Singleton.IsServer)
            ServerUpdate();
    }

   
}
