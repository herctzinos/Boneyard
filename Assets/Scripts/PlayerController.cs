using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [Header("Player Initial Stats")]
    [SerializeField]
    private float maxHealth = 100;
    [SerializeField]
    private float maxMana = 100;
    private float maxExp = 100;
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

    [SerializeField]
    private float levelUpFactor = 1.2f;
    private float health;
    private int gold;
    private float mana;
    private float exp;
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
    Vector3 attackInput;

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
        ResetStats();      
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
        if (mana < maxMana)
        {
            mana += manaRegenRate;
        }
    }
    private void CheckFire()
    {
        timePassed += Time.deltaTime;

        if (attackInput.x != 0f || attackInput.z != 0f)
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

        if (attackInput.x != 0f || attackInput.z != 0f)
        {
            transform.rotation = Quaternion.LookRotation(Vector3.Lerp(transform.forward, attackInput, smoothRotate));
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

        if (requiredMana <= mana)
        {
            if (activeWeaponScript.fire(requiredMana))
            {
                mana -= requiredMana;
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
    public void ReceiveDamage(float damage,Vector3 attackerDirecion)
    {
        health -= damage;
        playerRB.AddForce(attackerDirecion * damage * 5, ForceMode.Impulse);
    }

    public void ReceiveDamage(float damage)
    {
        health -= damage;
    }

    public float GetHealth()
    {
        return health;
    }

    public float GetMana()
    {
        return mana;
    }

    public float GetExp()
    {
        return exp;
    }

    public int GetGold()
    {
        return gold;
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

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public float GetMaxMana()
    {
        return maxMana;
    }
    public float GetMaxExp()
    {
        return maxExp;
    }

    public void GainExp(int gainedExp)
    {
        exp+= gainedExp;
    }

    private int CheckExpLevelUp()
    {
        if (CheckLevelUp()) return LevelUp();
        return level;
    }

    private bool CheckLevelUp()
    {
        if (exp >= maxExp) return true;
        return false;
    }

    private int LevelUp()
    {
        gameManager.HandleOpenLevelUpMenu();
        level += 1;
        LevelUpStats();
        gameManager.SaveData();
        ResetStats();
        return level;
    }

    private void LevelUpStats()
    {
        maxHealth *= levelUpFactor;
        maxMana *= levelUpFactor;
        maxExp *= levelUpFactor;
        //Debug.Log("levelUpFactor: " + levelUpFactor.ToString() + " maxMana: " + maxMana.ToString());
    }

    private void ResetStats()
    {
        mana = maxMana;
        health = maxHealth;
        exp = 0;
        Debug.Log(gameManager.gd.playerGold);
        gold = gameManager.gd.playerGold;
    }

    private void CheckHealth()
    {
        if (health <= 0) Die();
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
            attackInput.x = ctx.ReadValue<Vector2>().x;
            attackInput.z = ctx.ReadValue<Vector2>().y;
        };
        inputActions.Player.Attack.canceled += ctx => attackInput = Vector3.zero;
    }
}
