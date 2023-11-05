using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

public enum EInteractionType
{
    fruit = 0,
    meat = 1,
    danger = 2,
    safety = 3 
}

[System.Serializable]
public class InteractionStatChange
{
    public EInteractionType Type;
    public float Value;
}



public abstract class BaseInteraction : MonoBehaviour
{
    [SerializeField] protected string _DisplayName;
    public string DisplayName => _DisplayName;

    [SerializeField, FormerlySerializedAs("StatChanges")] protected InteractionStatChange[] _StatChanges;
    public InteractionStatChange[] StatChanges => _StatChanges;


    public abstract bool CanPerform();
    public abstract void Perform(PlayerBehaviour performer, UnityAction<BaseInteraction> onCompleted);

    public void ApplyStatChanges(PlayerBehaviour performer)
    {
        foreach (var stat in StatChanges)
        {
            performer.UpdateIndividualStat(stat.Type, stat.Value);
        }
    }
 
}
