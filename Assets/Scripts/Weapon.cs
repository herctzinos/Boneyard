using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;


public class Weapon : MonoBehaviour
{
    public GameObject projectile;

    public int projectileCount;
    public int projectileSpread = 20;
    public int fireSpeed = 100;
    public int fireRate = 1;

    private int initialFireSpeed;
    private int initialProjectileSpread;
    private int initialProjectileCount;


    public float basicManaRequirement = 1;
    public float requiredMana;

    private Rigidbody playerRB;

    private float timePassed = 0f;



    // Start is called before the first frame update
    void Start()
    {
        SetInitialValues();
        SetRequiredMana();
    }

    // Update is called once per frame
    void Update()
    {
        timePassed += Time.deltaTime;
    }

    private void FixedUpdate()
    {
    }

    private void SetInitialValues()
    {
        initialFireSpeed = fireSpeed;
        initialProjectileCount = projectileCount;
        initialProjectileSpread = projectileSpread;
        playerRB = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
    }

    public bool fire(float receivedMana)
    {
        if (timePassed >= 1 / fireRate && CheckRequiredMana(receivedMana))
        {

            fireGO();
            timePassed = 0f;
            return true;
        }
        else
        {
            return false;
        }

    }

    private void fireGO()
    {

        float pspr = projectileSpread / (projectileCount + 1f);

        float initRot = -projectileSpread / 2;
        for (int i = 1; i <= projectileCount; i++)
        {
            Vector3 spawnPosition = transform.position;
            Quaternion spawnRotation = transform.rotation;
            float yRotation = initRot + (pspr * i);
            spawnRotation *= Quaternion.Euler(0, yRotation, 0);
            GameObject newProjectile = Instantiate(projectile, spawnPosition, spawnRotation);
            //newProjectile.GetComponent<Projectile>().SetVelocity(fireSpeed);
            Rigidbody newProjectileRB = newProjectile.GetComponent<Rigidbody>();
            newProjectileRB.AddForce(newProjectileRB.transform.forward* fireSpeed, ForceMode.Impulse);
            //newProjectile.transform.rotation = Quaternion.LookRotation(vForce);
            //Vector3 projectileDirection = new Vector3(transform.rotation.x, -projectileSpread / 2 + pspr * (i + 1), transform.rotation.z);
            //newProjectile.transform.Rotate(projectileDirection);
            //newProjectileRB.velocity = transform.forward*fireSpeed * Time.deltaTime;
        }
    }


    private void SetRequiredMana()
    {
        requiredMana = CalculateCurrentRequiredMana();
    }

    public float GetRequiredMana()
    {
        return requiredMana;
    }

    private float CalculateCurrentRequiredMana()
    {
        float reqMana;

        reqMana = basicManaRequirement *
            (projectileCount/initialProjectileCount) *
            (basicManaRequirement + ((projectileSpread/initialProjectileSpread)/10)) *
            (basicManaRequirement + ((fireSpeed / initialFireSpeed)/10));

        return reqMana;
    }

    private bool CheckRequiredMana(float manaToCheck)
    {
        //Debug.Log("CheckRequiredMana: " + manaToCheck + " provided and " + requiredMana + " needed.");
        if (manaToCheck >= requiredMana) return true;
        else return false;
    }

    public void UpgradeStats(int projectileCountInc, int spreadInc, int speed)
    {
        IncreaseProjectileCount(projectileCountInc);
        IncreaseSpread(spreadInc);
        IncreaseSpeed(speed);
        SetRequiredMana();
    }

    private int IncreaseProjectileCount(int projectileCountInc)
    {
        projectileCount += projectileCountInc;

        return projectileCount;
    }

    private int IncreaseSpread(int spreadInc)
    {
        projectileSpread += spreadInc;
        return projectileSpread;
    }

    private int IncreaseSpeed(int speed)
    {
        fireSpeed += speed;
        return fireSpeed;
    }
}
