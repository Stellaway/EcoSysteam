using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;


public class PlayerSkillTree : NetworkBehaviour
{

    private NetworkVariable<bool> Speed = new NetworkVariable<bool>(
        false,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner
    );
    public void UpgradeSpeed() => Speed.Value = true; // CSAK OWNERBŐL HÍVNI!! (azaz az owner guijából pl)
    public bool GetSpeed() => Speed.Value;

    //kövi skill:
    /*
    private NetworkVariable<bool> Health = new NetworkVariable<bool>(
        false,
        NetworkVariableReadPermission.Owner,
        NetworkVariableWritePermission.Owner
    );
    public void UpgradeHealth() => Health.Value = true;
    public bool GetHealth() => Health.Value;
    */
    //...

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
