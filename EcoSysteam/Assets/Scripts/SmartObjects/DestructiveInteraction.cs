using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DestructiveInteraction : SimpleInteraction
{
    public override void Perform(PlayerBehaviour performer, UnityAction<BaseInteraction> onCompleted)
    {
        base.Perform(performer, onCompleted);
        Destroy(this.gameObject);
    }
}
