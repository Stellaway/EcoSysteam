using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class BaseInteraction : MonoBehaviour
{
    [SerializeField] protected string _DisplayName;
    public string DisplayName => _DisplayName;


    public abstract bool CanPerform();
    public abstract void Perform(MonoBehaviour performer, UnityAction<BaseInteraction> onCompleted);
}
