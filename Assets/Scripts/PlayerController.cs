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

    private float levelUpFactor = 1.2f;
    private float health;
    private float mana;
    private float exp;
    private int level=1;
    private GameObject weapon;
    private Rigidbody playerRB;
    private float timePassed = 0f;
    private float keyDelay = 0.1f;
    private Weapon activeWeaponScript;
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        GameObject gM = GameObject.FindGameObjectWithTag("GameManager");
        if (gM) gameManager = gM.GetComponent<GameManager>();

        playerRB = GetComponent<Rigidbody>();
        weapon = GameObject.FindGameObjectWithTag("active_weapon");
        activeWeaponScript = this.weapon.GetComponent<Weapon>();

        ResetStats();
    }

    private void FixedUpdate()
    {
        CheckExpLevelUp();
        RegenMana();

        Vector3 newPlayerVelocity = GetPlayerMovement();
        Vector3 newPlayerRotation = GetPlayerRotation();
        
        MoveAndRotatePlayer(newPlayerVelocity,newPlayerRotation);
        CheckFire(newPlayerRotation);
        CheckHealth();

    }

    private void RegenMana()
    {
        if (mana < maxMana)
        {
            mana += manaRegenRate;
        }
    }
    private void CheckFire(Vector3 rotation)
    {
        timePassed += Time.deltaTime;

        if (rotation.x!=0f || rotation.z!=0f)
        {
            if (timePassed >= keyDelay)
            {
                timePassed = 0f;
                Attack();
            }
        }
    }

    Vector3 GetPlayerMovement()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");
        

        //float moveHorizontal = leftJoystick.Horizontal;
        //float moveVertical = leftJoystick.Vertical;

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        return movement;
    }

    Vector3 GetPlayerRotation()
    {
       
        //float rotateHorizontal = rightJoystick.Horizontal;
        //float rotateVertical = rightJoystick.Vertical;

        float rotateHorizontal = Input.GetAxis("RightHorizontal");
        float rotateVertical = Input.GetAxis("RightVertical");

        Vector3 rotation = new Vector3(rotateHorizontal, 0.0f, rotateVertical);

        return rotation;
    }

    void MoveAndRotatePlayer(Vector3 movement, Vector3 rotation)
    {

        if (rotation.x != 0f || rotation.z != 0f) { 
            transform.rotation = Quaternion.LookRotation(Vector3.Lerp(transform.forward, rotation, smoothRotate));
        }
        else if (movement.x != 0f || movement.z != 0f)
        {
            transform.rotation = Quaternion.LookRotation(Vector3.Lerp(transform.forward, movement, smoothRotate));
        }


        //transform.Translate(movement * moveSpeed * Time.deltaTime, Space.World);
        playerRB.velocity = movement * moveSpeed * Time.deltaTime;
    }

    void Attack()
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
    }

    private void CheckHealth()
    {
        if (health <= 0) Die();
    }

    private void Die()
    {
        gameManager.EndGame();
    }

    public GameObject GetActiveWeapon()
    {
        return weapon;
    }
}
