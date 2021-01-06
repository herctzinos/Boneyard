using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    private GameObject levelupMenu;

    private PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
       
    }

    // Update is called once per frame
    void Update()
    {
        HandleHealthBar();
        HandleManaBar();
        HandleExpBar();
    }

    private void HandleHealthBar()
    {
        float health = player.GetHealth();
        float maxHealth = player.GetMaxHealth();
        healthBar.fillAmount = MapValue(health,maxHealth);
        healthText.text = health.ToString("0") + " / " + maxHealth.ToString("0");
    }

    private void HandleManaBar()
    {
        float mana = player.GetMana();
        float maxMana = player.GetMaxMana();
        manaBar.fillAmount = MapValue(mana, maxMana);
        manaText.text = mana.ToString("0") + " / " + maxMana.ToString("0");

    }

    private void HandleExpBar()
    {
        float exp = player.GetExp();
        float maxExp = player.GetMaxExp();
        int level = player.GetLevel();
        expBar.fillAmount = MapValue(exp, maxExp);
        expText.text = exp.ToString("0") + " / " + maxExp.ToString("0");
        levelText.text = level.ToString();

    }

    private float MapValue(float value,  float maxValue)
    {
        return value  / maxValue;
    }

    public void DisplayLevelUpMenu()
    {
        LMXinfo.SetActive(false);
        playerControls.SetActive(false);
        levelupMenu.SetActive(true);
    }

    public void HideLevelUpMenu()
    {
        levelupMenu.SetActive(false);
        LMXinfo.SetActive(true);
        playerControls.SetActive(true);

    }

}
