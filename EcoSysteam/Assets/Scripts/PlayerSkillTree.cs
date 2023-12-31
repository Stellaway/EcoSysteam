using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;

public class PlayerSkillTree : NetworkBehaviour
{
    private NetworkVariable<int> SkillPoints = new NetworkVariable<int>(
        2,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server
    );
    public void AddSkillPoint() => SkillPoints.Value++; // csak a szerverről
    private int UsedSkillPoints = 0; // kliensen tartjuk számon, mivel a SkillPointsot csak a szerverről lehet frissíteni
    
    private NetworkVariable<int> Speed = new NetworkVariable<int>(
        0,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner
    );
    public void UpgradeSpeed() { // csak ownerből hívni (azaz az owner guijából pl)
        Speed.Value++;
        UsedSkillPoints++; // a gomb csak akkor engedélyezett, ha van elég skillpoint
    }
    public float GetSpeed() => 0.2f + 0.4f * (float)Math.Log10((Speed.Value+1)*10); // TODO itt kell majd balanszolni
    public bool IsSpeedUpgradeable() => SkillPoints.Value > UsedSkillPoints;

    private NetworkVariable<int> Health = new NetworkVariable<int>(
        0,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner
    );
    public void UpgradeHealth() {
        Health.Value++;
        UsedSkillPoints++;
    }
    public float GetHealth() => 50.0f + 50.0f * (float)Math.Log10((Health.Value+1)*10);
    public bool IsHealthUpgradeable() => SkillPoints.Value > UsedSkillPoints;

    private NetworkVariable<int> ViewDistance = new NetworkVariable<int>(
        0,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner
    );
    public void UpgradeViewDistance() {
        ViewDistance.Value++;
        UsedSkillPoints++;
    }
    public float GetViewDistance() => 6.0f + 3.0f * (float)Math.Log10((ViewDistance.Value+1)*10);
    public bool IsViewDistanceUpgradeable() => SkillPoints.Value > UsedSkillPoints;

    // 0: növényevő, 1: SharpTeeth, 2: GastricAcid, 3: Claws, 4: Carnivore
    public enum FoodChainEnum 
    {
        Herbivore = 0,
        SharpTeeth = 1,
        GastricAcid = 2,
        Claws = 3,
        Carnivore = 4,
    }

    private NetworkVariable<int> FoodChainPosition = new NetworkVariable<int>(
        0,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner
    );
    public void UpgradeFoodChainPosition(FoodChainEnum toPos) {
        int posInt = (int)toPos;
        if (FoodChainPosition.Value == posInt - 1 && posInt <= 4) {
            FoodChainPosition.Value = posInt;
            UsedSkillPoints++;
        }
    }
    public FoodChainEnum GetFoodChainPosition() => (FoodChainEnum)FoodChainPosition.Value;
    public bool IsFoodChainUpgradeable(FoodChainEnum toPos)
        => SkillPoints.Value > UsedSkillPoints
        && FoodChainPosition.Value == (int)toPos - 1 && (int)toPos <= 4;

    public int GetSpeedUpgrades()
    {
        return Speed.Value;
    }

    public int GetHealthUpgrades()
    {
        return Health.Value;
    }

    public int GetViewDistanceUpgrades()
    {
        return ViewDistance.Value;
    }
    
    public int GetUnusedSkillPoints()
    {
        return SkillPoints.Value - UsedSkillPoints;
    }


    // these are for display
    private NetworkVariable<float> CurrentHealth = new NetworkVariable<float>(
        0.0f,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server
    );
    public void UpdateCurrentHealth(float v) => CurrentHealth.Value = v;
    public int GetCurrentHealth() => (int)(CurrentHealth.Value + 0.5f);
    private NetworkVariable<float> CurrentHunger = new NetworkVariable<float>(
        0.0f,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server
    );
    public void UpdateCurrentHunger(float v) => CurrentHunger.Value = v;
    public int GetCurrentHunger() => (int)(CurrentHunger.Value + 0.5f);
}
