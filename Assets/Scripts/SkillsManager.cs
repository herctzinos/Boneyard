using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkillsManager : MonoBehaviour
{

    private string healthTier;
    private string manaTier;
    private string powerTier;

    [SerializeField]
    int healthEarned;
    [SerializeField]
    int manaEarned;
    [SerializeField]
    int powerEarned;

    GameManager gameManager;
    HudManager hudManager;
    PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        hudManager = GameObject.FindGameObjectWithTag("HUD").GetComponent<HudManager>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    public GameObject FetchRandomHealthSkillForLvlUp()
    {
        healthTier = CalculateHealthEligibleTier();
        var allElligibleHealthSkills = Resources.LoadAll(healthTier + "/Health");
        GameObject randomHealthSkill = (GameObject)allElligibleHealthSkills[UnityEngine.Random.Range(0, allElligibleHealthSkills.Length)];
        return randomHealthSkill;

    }
    public GameObject FetchRandomManaSkillForLvlUp()
    {
        manaTier = CalculateManaEligibleTier();
        var allElligibleManaSkills = Resources.LoadAll(manaTier + "/Mana");
        GameObject randomManaSkill = (GameObject)allElligibleManaSkills[UnityEngine.Random.Range(0, allElligibleManaSkills.Length)];
        return randomManaSkill;

    }
    public GameObject FetchRandomPowerSkillForLvlUp()
    {
        powerTier = CalculatePowerEligibleTier();
        var allElligiblePowerSkills = Resources.LoadAll(powerTier + "/Power");
        GameObject randomPowerSkill = (GameObject)allElligiblePowerSkills[UnityEngine.Random.Range(0, allElligiblePowerSkills.Length)];
        return randomPowerSkill;

    }


    private string CalculateHealthEligibleTier()
    {
        //todo
        return healthTier = "Tier1";
    }
    private string CalculateManaEligibleTier()
    {
        //todo
        return manaTier = "Tier1";

    }
    private string CalculatePowerEligibleTier()
    {
        //todo
        return powerTier = "Tier1";

    }
    public void SkillPress()
    {
        var pressedSkilledButton = EventSystem.current.currentSelectedGameObject;
        SkillsManager buttonSkillsManager = pressedSkilledButton.GetComponent<SkillsManager>();
        if (pressedSkilledButton != null)
        {
            playerController.AttachSelectedSkillToPlayer(
            buttonSkillsManager.healthEarned,
            buttonSkillsManager.manaEarned,
            buttonSkillsManager.powerEarned);
        }
        gameManager.SaveData();
        playerController.ResetStats();  
        gameManager.HandleCloseLevelUpMenu();
        //gameManager.LoadData();
    }
}

