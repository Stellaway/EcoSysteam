using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using UnityEngine;


public class UI_SkillTree : MonoBehaviour
{
    // visszaadhat nullt is ha még nem fut pl a játék!
    private PlayerSkillTree getOwnedSkillTree() {
        var playerObject = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject();
        if (playerObject == null)
            return null;
        return playerObject.GetComponent<PlayerSkillTree>();
    }

    public void HealthButton()
    {
        Debug.Log("CLIKEQ");
    }

    public void SpeedButton()
    {
        Debug.Log("Speed$$$");
        getOwnedSkillTree().UpgradeSpeed();
    }
}
