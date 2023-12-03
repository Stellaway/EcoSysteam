using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class DestructiveInteraction : SimpleInteraction
{
    public override void Perform(PlayerBehaviour performer, UnityAction<BaseInteraction> onCompleted)
    {
        base.Perform(performer, onCompleted);
        this.gameObject.GetComponent<NetworkObject>().Despawn();
    }
}
