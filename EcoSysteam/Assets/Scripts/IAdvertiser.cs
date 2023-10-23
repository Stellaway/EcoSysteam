using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAdvertiser
{
    public abstract List<Intent> GetAdverisement();
    public enum SensoryEffects
    {
        Threath, Fruit, Meat, Protection
    }
    
    public struct Intent
    {
        SensoryEffects SensoryEffect;
        float value;
    }
        
    


}
