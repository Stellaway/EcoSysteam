using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class SimpleInteraction : BaseInteraction
{
    public override bool CanPerform()
    {
        return true;
    }

    public override void Perform(MonoBehaviour performer, UnityAction<BaseInteraction> onCompleted)
    {
        Debug.Log($"Action is being performed {this}");
        onCompleted.Invoke(this);
    }
}
