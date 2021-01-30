using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [Header("Player Initial Stats")]
    [SerializeField]
    private float maxHealthEarned = 100;
    [SerializeField]
    private float maxManaEarned = 100;
    private float maxPowerEarned = 20;
    public float expToLevelUp = 1;
    [SerializeField]
    private float moveSpeed = 1;
    [SerializeField]
    private float smoothRotate = 1;
    [SerializeField]
    private float manaRegenRate = 0.1f;

    [Header("Player Controls")]
    [SerializeField]
    private Joystick leftJoystick;
    [SerializeField]
    private Joystick rightJoystick;
    [SerializeField]
    private JoyButton joybutton;

    [Header("Leveling data")]

    [SerializeField]
    private GameObject playerModel;
    
    [Header("Player Current Stats")]
    [SerializeField]
   // private float levelUpFactor = 1.2f;
    private float currentHealth;
    private int gold;
    private float currentMana;
    private float currentPower;
    private float currentExp;
    private int level=1;
    private GameObject weapon;
    private Rigidbody playerRB;
    private float timePassed = 0f;
    private float keyDelay = 0.1f;
    private Weapon activeWeaponScript;
    private GameManager gameManager;
    private GameObject gM;
    private Animator playerAnimator;
    private bool isMoving=false;
    private bool isAttacking = true;

    private InputActions inputActions;
    Vector3 movementInput;
    bool attackInput;
    Vector3 lookInput;

    // Start is called before the first frame update

    private void Awake()
    {
        MapControls();
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }
    void Start()
    {
        gM=GameObject.FindGameObjectWithTag("GameManager");
        if (gM) gameManager = gM.GetComponent<GameManager>();
        
        playerRB = GetComponent<Rigidbody>();
        weapon = GameObject.FindGameObjectWithTag("active_weapon");
        activeWeaponScript = this.weapon.GetComponent<Weapon>();
        playerAnimator = playerModel.GetComponent<Animator>();
        LoadStats();      
    }



    private void FixedUpdate()
    {
        CheckExpLevelUp();
        RegenMana();
        MoveAndRotatePlayer();
        CheckFire();
        CheckHealth();
        HandleAnimations();
    }


    private void RegenMana()
    {
        if (currentMana < maxManaEarned)
        {
            currentMana += manaRegenRate;
        }
    }
    private void CheckFire()
    {
        timePassed += Time.deltaTime;

        if (attackInput)
        {
            isAttacking = true;
            if (timePassed >= keyDelay)
            {
                timePassed = 0f;
                Attack();
            }
        }
        else isAttacking = false;
    }
    void MoveAndRotatePlayer()
    {

        if (lookInput.x != 0f || lookInput.z != 0f)
        {
            transform.rotation = Quaternion.LookRotation(Vector3.Lerp(transform.forward, lookInput, smoothRotate));
        }
        else if (movementInput.x != 0f || movementInput.z != 0f)
        {
            transform.rotation = Quaternion.LookRotation(Vector3.Lerp(transform.forward, movementInput, smoothRotate));
            isMoving = true;
        }
        else isMoving = false;
        playerRB.velocity = movementInput * moveSpeed * Time.deltaTime;
    }

    public void Attack()
    {
        float requiredMana = activeWeaponScript.GetRequiredMana();
        //Debug.Log("Mana: " + mana + " required: " + requiredMana);

        if (requiredMana <= currentMana)
        {
            if (activeWeaponScript.fire(requiredMana))
            {
                currentMana -= requiredMana;
            }
        }
        else
        {
           // Debug.Log("not enough mana...");
        }

    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Gold")
        {
            int goldAmmount = other.gameObject.GetComponent<GoldScript>().GetGold();
            GaintGold(goldAmmount);
        }
    }
    public void ReceiveDamage(float damage,Vector3 attackerDirection)
    {
        currentHealth -= damage;
        playerRB.AddForce(attackerDirection * damage * 5, ForceMode.Impulse);
    }

/*    public void ReceiveDamage(float damage)
    {
        health -= damage;
    }*/

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public float GetCurrentMana()
    {
        return currentMana;
    }

    public float GetCurrentExp()
    {
        return currentExp;
    }

    public int GetGold()
    {
        return gold;
    }  
    public float GetMaxPowerEarned()
    {
        return maxPowerEarned;
    }

    public void SetGold(int goldToSet)
    {
        gold = goldToSet;
    }

    public void GaintGold(int goldToGain)
    {
        gold += goldToGain;
    }

    public int GetLevel()
    {
        return level;
    }

    public float GetMaxHealthEarned()
    {
        return maxHealthEarned;
    }

    public float GetMaxManaEarned()
    {
        return maxManaEarned;
    }
    public float GetExpToLevelUp()
    {
        return expToLevelUp;
    }

    public void GainExp(int gainedExp)
    {
        currentExp+= gainedExp;
    }

    private int CheckExpLevelUp()
    {
        if (CheckLevelUp()) return LevelUp();
        return level;
    }

    private bool CheckLevelUp()
    {
        if (currentExp >= expToLevelUp) return true;
        return false;
    }

    private int LevelUp()
    {
        gameManager.HandleOpenLevelUpMenu();
        level += 1;
       // LevelUpStats();
       // gameManager.SaveData();
       // ResetStats();
        return level;
    }

    private void LevelUpStats()
    {
/*        maxHealth *= levelUpFactor;
        maxMana *= levelUpFactor;
        maxExp *= levelUpFactor;*/
        //Debug.Log("levelUpFactor: " + levelUpFactor.ToString() + " maxMana: " + maxMana.ToString());
    }
    public void AttachSelectedSkillToPlayer(int extraHealthEarned, int extraManaEarned, int extraPowerEarned)
    {
        maxHealthEarned += extraHealthEarned;
        maxManaEarned += extraManaEarned;
        maxPowerEarned += extraPowerEarned;
    }

    public void ResetStats()
    {
        currentHealth = maxHealthEarned;
        currentMana = maxManaEarned;
        currentExp = 0;
       // Debug.Log(gameManager.gd.playerGold);
        gold = gameManager.gd.playerGold;
        //currentHealth = gameManager.gd.maxHealthReached;
       // currentMana = gameManager.gd.maxManaReached;
        //power = gameManager.gd.maxPower;
    }

    public void LoadStats()
    {
        gameManager.LoadData();

        Debug.Log("Max level " + gameManager.gd.maxLevelReached);
        if (gameManager.gd.maxLevelReached > 1)
        {
            gold = gameManager.gd.playerGold;
            currentHealth = gameManager.gd.maxHealthReached;
            currentMana = gameManager.gd.maxManaReached;
           currentPower = gameManager.gd.maxPowerReached;
        }
        else
        {
            currentExp = 0;
            currentHealth = maxHealthEarned;
            currentMana = maxManaEarned;
            currentExp = 0;
            // Debug.Log(gameManager.gd.playerGold);

        }
    }

    private void CheckHealth()
    {
        if (currentHealth <= 0) Die();
    }

    private void Die()
    {
        gameManager.LoadGameOverScene();
    }

    public GameObject GetActiveWeapon()
    {
        return weapon;
    }

    private void HandleAnimations()
    {
        playerAnimator.SetBool("isWalking", isMoving);
        playerAnimator.SetBool("isAttacking", isAttacking);

    }

    private void MapControls()
    {
        inputActions = new InputActions();
        inputActions.Player.Move.performed += ctx =>
        {
            movementInput.x = ctx.ReadValue<Vector2>().x;
            movementInput.z = ctx.ReadValue<Vector2>().y;
        };
        inputActions.Player.Move.canceled += ctx => movementInput = Vector3.zero;
        inputActions.Player.Attack.performed += ctx =>
        {
            attackInput = true;
        };
        inputActions.Player.Attack.canceled += ctx => attackInput = false;
        inputActions.Player.Look.performed += ctx =>
        {
            lookInput.x = ctx.ReadValue<Vector2>().x;
            lookInput.z = ctx.ReadValue<Vector2>().y;
        };
        inputActions.Player.Look.canceled += ctx => lookInput = Vector3.zero;
    }
}
