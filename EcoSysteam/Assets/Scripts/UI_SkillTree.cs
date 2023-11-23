using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;


public class UI_SkillTree : MonoBehaviour
{
    // visszaadhat nullt is ha még nem fut pl a játék!
    public static PlayerSkillTree getOwnedSkillTree() {
        if (NetworkManager.Singleton.SpawnManager == null)
            return null;
        var playerObject = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject();
        if (playerObject == null)
            return null;
        return playerObject.GetComponent<PlayerSkillTree>();
    }

    public void HealthButton()
    {
        getOwnedSkillTree().UpgradeHealth();
    }

    public void SpeedButton()
    {
        getOwnedSkillTree().UpgradeSpeed();
    }
    
    public void DistanceButton()
    {
        getOwnedSkillTree().UpgradeViewDistance();
    }

    public void SharpTeethButton()
    {
        getOwnedSkillTree()
            .UpgradeFoodChainPosition(PlayerSkillTree.FoodChainEnum.SharpTeeth);
    }

    public void GastricAcidButton()
    {
        getOwnedSkillTree()
            .UpgradeFoodChainPosition(PlayerSkillTree.FoodChainEnum.GastricAcid);
    }

    public void ClawsButton()
    {
        getOwnedSkillTree()
            .UpgradeFoodChainPosition(PlayerSkillTree.FoodChainEnum.Claws);
    }

    public void CarnivoreButton()
    {
        getOwnedSkillTree()
            .UpgradeFoodChainPosition(PlayerSkillTree.FoodChainEnum.Carnivore);
    }
}
