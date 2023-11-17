using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public GameObject skillUi; // Reference to your UI element
    public GameObject heartBtn;
    public GameObject speedBtn;
    public GameObject distanceBtn;
    public GameObject herbivoreBtn;
    public GameObject sharpTeethBtn;
    public GameObject gastricAcidBtn;
    public GameObject clawsBtn;
    public GameObject carnivoreBtn;


    void Start()
    {
        // Hide the UI element at the start
        skillUi.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            // Toggle the visibility of the UI element
            skillUi.SetActive(!skillUi.activeSelf);
        }
        PlayerSkillTree st = UI_SkillTree.getOwnedSkillTree();
        if (st != null) {
            // itt sem a legszebb pollozni
            heartBtn.GetComponent<Button>().enabled = st.IsHealthUpgradeable();
            speedBtn.GetComponent<Button>().enabled = st.IsSpeedUpgradeable();
            distanceBtn.GetComponent<Button>().enabled = st.IsViewDistanceUpgradeable();
            //herbivoreBtn.GetComponent<Button>().enabled = st // ez nem kattintható
            //    .IsFoodChainUpgradeable(PlayerSkillTree.FoodChainEnum.Herbivore);
            sharpTeethBtn.GetComponent<Button>().enabled =
                st.IsFoodChainUpgradeable(PlayerSkillTree.FoodChainEnum.SharpTeeth);
            gastricAcidBtn.GetComponent<Button>().enabled =
                st.IsFoodChainUpgradeable(PlayerSkillTree.FoodChainEnum.GastricAcid);
            clawsBtn.GetComponent<Button>().enabled =
                st.IsFoodChainUpgradeable(PlayerSkillTree.FoodChainEnum.Claws);
            carnivoreBtn.GetComponent<Button>().enabled =
                st.IsFoodChainUpgradeable(PlayerSkillTree.FoodChainEnum.Carnivore);
        }
    }
}