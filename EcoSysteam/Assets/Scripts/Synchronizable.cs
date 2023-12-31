using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public abstract class Synchronizable : NetworkBehaviour
{
    public static Vector3 ClampPos(Vector3 pos) {
        pos.x = Mathf.Clamp(pos.x, -15f, 15f);
        pos.y = Mathf.Clamp(pos.y, -7.5f, 7.5f);
        //határon belül marad
        return pos;
    }
    // This method sends the position over the network, and checks for bounds.
    // Must be called from the server side
    protected void UpdatePosition(Vector2 pos) {
        transform.position = ClampPos(new Vector3(pos.x, pos.y, 0.0f));
    }
    // This method will be run from the server side, override it to implement the movement / action
    protected abstract void ServerUpdate();

    // Update is called once per frame
    void Update() {
        // Calling the function to calculate the new state, only on the server side
        if (NetworkManager.Singleton.IsServer) {
            // update the position to be inside bounds
            UpdatePosition(new Vector2(transform.position.x, transform.position.y));
            ServerUpdate();
        }
    }

   
}
