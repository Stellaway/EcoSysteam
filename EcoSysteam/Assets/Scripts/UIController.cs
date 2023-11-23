using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;

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
    public GameObject nrOfSkillpoints;

    private Dictionary<Image, Color> originalColors = new Dictionary<Image, Color>();

    void Start()
    {
        // Hide the UI element at the start
        skillUi.SetActive(true);
        saveColors(heartBtn);
        saveColors(speedBtn);
        saveColors(distanceBtn);
        saveColors(herbivoreBtn);
        saveColors(sharpTeethBtn);
        saveColors(gastricAcidBtn);
        saveColors(clawsBtn);
        saveColors(carnivoreBtn);        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            // Toggle the visibility of the UI element
            skillUi.SetActive(!skillUi.activeSelf);
        }
        PlayerSkillTree st = UI_SkillTree.getOwnedSkillTree();
        if (st != null)
        {
            // itt sem a legszebb pollozni

            setButton(heartBtn, st.IsHealthUpgradeable());
            foreach (Text t in heartBtn.GetComponentsInChildren<Text>())
                t.text = st.GetHealthUpgrades().ToString();

            setButton(speedBtn, st.IsSpeedUpgradeable());
            foreach(Text t in speedBtn.GetComponentsInChildren<Text>())
                t.text = st.GetSpeedUpgrades().ToString();

            setButton(distanceBtn, st.IsViewDistanceUpgradeable());
            foreach (Text t in distanceBtn.GetComponentsInChildren<Text>())
                t.text = st.GetViewDistanceUpgrades().ToString();

            //herbivoreBtn nem kattintható

            setButton(sharpTeethBtn, st.IsFoodChainUpgradeable(PlayerSkillTree.FoodChainEnum.SharpTeeth));

            setButton(gastricAcidBtn, st.IsFoodChainUpgradeable(PlayerSkillTree.FoodChainEnum.GastricAcid));

            setButton(clawsBtn, st.IsFoodChainUpgradeable(PlayerSkillTree.FoodChainEnum.Claws));

            setButton(carnivoreBtn, st.IsFoodChainUpgradeable(PlayerSkillTree.FoodChainEnum.Carnivore));


            nrOfSkillpoints.GetComponent<TextMeshProUGUI>().text = st.GetUnusedSkillPoints().ToString();

        }
    }
    
    void saveColors(GameObject o)
    {
        foreach (Image image in o.GetComponentsInChildren<Image>())
        {
            originalColors.Add(image, image.color);
        }
    }

    void setButton(GameObject o, Boolean enabled)
    {
        o.GetComponent<Button>().enabled = enabled;
        foreach (Image image in o.GetComponentsInChildren<Image>())
        {
            image.color = enabled ? originalColors[image] : Color.Lerp(Color.white, Color.black, 0.5f);
        }

    }
}