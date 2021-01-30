using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class HudManager : MonoBehaviour
{
    private GameManager gameManager;

    [SerializeField]
    private GameObject LMXinfo;

    [SerializeField]
    private GameObject playerControls;

    [SerializeField]
    private float healthFillAmount;

    [SerializeField]
    private Image healthBar;

    [SerializeField]
    private Image manaBar;

    [SerializeField]
    private Image expBar;

    [SerializeField]
    private Text healthText;

    [SerializeField]
    private Text manaText;

    [SerializeField]
    private Text expText;

    [SerializeField]
    private Text levelText;

    [SerializeField]
    private Text goldText;
    private float goldAmount;

    [SerializeField]
    public GameObject levelupPanel;

    [SerializeField]
    private SkillsManager skillManager;

    private PlayerController player;

    [Header("Fields for Stats Screen")]
    [SerializeField]
    private GameObject stats;

    [SerializeField]
    private Text maxHealthEarnedText, maxManaEarnedText, maxPowerEarnedText, experienceTotalText, goldTotalText;

    GameObject oldHealthButton;
    GameObject oldManaButton;
    GameObject oldPowerButton;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        HandleHealthBar();
        HandleManaBar();
        HandleExpBar();
        HandleGold();
        HandleHealthStat();
        HandleManaStat();
        HandleExpStat();
        HandleGoldStat();

    }

    private void HandleHealthBar()
    {
        float currentHealth = player.GetCurrentHealth();
        float maxHealthEarned = player.GetMaxHealthEarned();
        healthBar.fillAmount = MapValue(currentHealth, maxHealthEarned);
        healthText.text = currentHealth.ToString("0") + " / " + maxHealthEarned.ToString("0");
    }

    private void HandleManaBar()
    {
        float currentMana = player.GetCurrentMana();
        float maxManaEarned = player.GetMaxManaEarned();
        manaBar.fillAmount = MapValue(currentMana, maxManaEarned);
        manaText.text = currentMana.ToString("0") + " / " + maxManaEarned.ToString("0");

    }

    private void HandleExpBar()
    {
        float exp = player.GetCurrentExp();
        float expToLevelUp = player.GetExpToLevelUp();
        int level = player.GetLevel();
        expBar.fillAmount = MapValue(exp, expToLevelUp);
        expText.text = exp.ToString("0") + " / " + expToLevelUp.ToString("0");
        levelText.text = level.ToString();

    }

    private void HandleGold()
    {
        float goldSmooth = Mathf.Lerp(goldAmount , player.GetGold(),Time.deltaTime*5);
        goldAmount = goldSmooth;
        goldText.text = Mathf.Round(goldSmooth).ToString();
    }
    private void HandleHealthStat()
    {
        float maxHealthEarned = player.GetMaxHealthEarned();
        maxHealthEarnedText.text = maxHealthEarned.ToString();
    }

    private void HandleManaStat()
    {
        float maxManaEarned = player.GetMaxManaEarned();
        maxManaEarnedText.text = maxManaEarned.ToString();

    }

    private void HandleExpStat()
    {
        float currentExp = player.GetCurrentExp();
        float expToLevelUp = player.GetExpToLevelUp();
        int level = player.GetLevel();
        experienceTotalText.text = "Level: " + level + ". Next level:" + currentExp.ToString() +"/" + expToLevelUp.ToString();

    }

    private void HandleGoldStat()
    {
        goldAmount = player.GetGold();
        goldTotalText.text = goldAmount.ToString();
    }

    private float MapValue(float value,  float maxValue)
    {
        return value  / maxValue;
    }

    public void DisplayLevelUpMenu()
    {
        PrepareButtonSkills();

 
        LMXinfo.SetActive(false);
        playerControls.SetActive(false);
        levelupPanel.SetActive(true);
    }

    private void PrepareButtonSkills() {
 

        foreach (Transform oldSkillButton in levelupPanel.transform)
        {
            if (oldSkillButton.gameObject.tag == "Health") {
                 oldHealthButton = oldSkillButton.gameObject;
            }
            else if (oldSkillButton.gameObject.tag == "Mana"){
                oldManaButton = oldSkillButton.gameObject;
            }
            else if (oldSkillButton.gameObject.tag == "Power")
            {
                oldPowerButton = oldSkillButton.gameObject;
            }
        }

        PrepareHealthButtonSkill(oldHealthButton);
        PrepareManaButtonSkill(oldManaButton);
        PreparePowerButtonSkill(oldPowerButton);
    }

    private void PrepareHealthButtonSkill(GameObject oldHealthButton)
    { 
        Vector3 healthSkillPosition = oldHealthButton.transform.position;
        Transform healthTransform = oldHealthButton.transform;
        Destroy(oldHealthButton);
        GameObject prefabHealthSkillForLvlUp = skillManager.FetchRandomHealthSkillForLvlUp();
        GameObject healthSkillForLvlUp = Instantiate(prefabHealthSkillForLvlUp, healthTransform);
        healthSkillForLvlUp.transform.SetParent(levelupPanel.transform);
        healthSkillForLvlUp.transform.position = healthSkillPosition;
    }
    private void PrepareManaButtonSkill(GameObject oldManaButton)
    {
        Vector3 manaSkillPosition = oldManaButton.transform.position;
        Transform manaTransform = oldManaButton.transform;
        Destroy(oldManaButton);
        GameObject PrefabManaSkillForLvlUp = skillManager.FetchRandomManaSkillForLvlUp();
        GameObject manaSkillForLvlUp = Instantiate(PrefabManaSkillForLvlUp, manaTransform);
        manaSkillForLvlUp.transform.SetParent(levelupPanel.transform);
        manaSkillForLvlUp.transform.position = manaSkillPosition;
    }
    private void PreparePowerButtonSkill(GameObject oldPowerButton)
    {
        Vector3 powerSkillPosition = oldPowerButton.transform.position;
        Transform powerTransform = oldPowerButton.transform;
        Destroy(oldPowerButton);
        GameObject prefabPowerSkillForLvlUp = skillManager.FetchRandomPowerSkillForLvlUp();
        GameObject powerSkillForLvlUp = Instantiate(prefabPowerSkillForLvlUp, powerTransform);
        powerSkillForLvlUp.transform.SetParent(levelupPanel.transform);
        powerSkillForLvlUp.transform.position = powerSkillPosition;
    }



    public void HideLevelUpMenu()
    {
        levelupPanel.SetActive(false);
        LMXinfo.SetActive(true);
        playerControls.SetActive(true);

    } 
    public void DisplayStats()
    {
        LMXinfo.SetActive(false);
        playerControls.SetActive(false);
        stats.SetActive(true);
    }

    public void HideStats()
    {
        stats.SetActive(false);
        LMXinfo.SetActive(true);
        playerControls.SetActive(true);

    }
}
