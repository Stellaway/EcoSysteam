using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using UnityEngine.UI;



public class SimpleInteraction : BaseInteraction
{
    public override bool CanPerform()
    {
        return true;
    }

    public override void Perform(PlayerBehaviour performer, UnityAction<BaseInteraction> onCompleted)
    {
        onCompleted.Invoke(this);
    }
}
