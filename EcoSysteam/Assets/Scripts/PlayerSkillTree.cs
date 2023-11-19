using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;


public class PlayerSkillTree : NetworkBehaviour
{
    private NetworkVariable<int> SkillPoints = new NetworkVariable<int>(
        0,
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
    public float GetSpeed() => 1.0f + 0.8f * Speed.Value; // TODO itt kell majd balanszolni
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
    public float GetHealth() => 100.0f + 50.0f * Health.Value;
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
    public float GetViewDistance() => 6.0f + 3.0f * ViewDistance.Value;
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
}
