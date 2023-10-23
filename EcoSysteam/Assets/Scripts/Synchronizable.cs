using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

// ÚJRAGONDOLANDÓ, INKÁBB NETWORKTRANSFORM-OT HASZNÁLUNK, MERT TUDJA AZ INTERPOLÁCIÓT
// LETÖRLENDŐ, EGYELŐRE (amennyiben később sem találunk neki hasznot)
// Base class for classes that need to update their position across the network
public abstract class Synchronizable : NetworkBehaviour, IAdvertiser
{
    // This variable can be written from the server side, and can be read from everywhere
    //private NetworkVariable<Vector2> NetPosition = new NetworkVariable<Vector2>();
    // This method sends the position over the network. Must be called from the server side
    protected void UpdatePosition(Vector2 pos) {
        //NetPosition.Value = pos;
        // we update the transform on the server,
        // so it can be immediately used for calculations
        transform.position = new Vector3(pos.x, pos.y, 0.0f);
    }
    // This method will be run from the server side, override it to implement the movement
    protected abstract void ServerUpdate();

    // Update is called once per frame
    void Update() {
        // Calling the function to calculate the new state, only on the server side
        if (NetworkManager.Singleton.IsServer)
            ServerUpdate();
        // Updating the position based on the network variable. This is run everywhere,
        // including on the server, the owner, and every other client as well
        //transform.position = new Vector3(NetPosition.Value.x, NetPosition.Value.y, 0.0f);
    }

    virtual public List<IAdvertiser.Intent> GetAdverisement()
    {
        return new List<IAdvertiser.Intent>();
    }
}
