using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class Projectile : MonoBehaviour,IConvertGameObjectToEntity
{
    //Projectile configurable properties
    public float lifespan = 1f;
    public float startTrackingTime = 0.5f;
    public int power = 3;
    public bool canTrackEnemies = true;
    public int trackingEase = 1;

    private bool trackingStarted = false;
    private Rigidbody projectileRB;

    //Projectile properties set by weapon
    private float velocity;

    //Global variables
    private GameObject closestEnemy;

    // Start is called before the first frame update
    void Start()
    {
        projectileRB = GetComponent<Rigidbody>();

        //Start tracking enemy after interval
        if (canTrackEnemies) StartCoroutine(StartTrackEnemy());
        //Set the lifespan
        StartCoroutine(KillDelay());
        projectileRB.AddRelativeForce(Vector3.forward * velocity,ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        CheckTrackEnemy();
        MoveProjectile();
    }

    private void MoveProjectile()
    {
        //transform.position += transform.forward * velocity * Time.deltaTime;
        //transform.Translate(Vector3.forward * velocity * Time.deltaTime);
        //projectileRB.velocity = transform.forward * velocity * Time.deltaTime;


    }
    private void Kill()
    {
        Destroy(this.gameObject);
    }

    private IEnumerator KillDelay()
    {
        yield return new WaitForSeconds(lifespan);
        Kill();
    }

    private IEnumerator StartTrackEnemy()
    {
        yield return new WaitForSeconds(startTrackingTime);
        trackingStarted = true;
        closestEnemy = FindClosestEnemy();
    }

    public void SetVelocity(float vel)
    {
        velocity = vel;
    }

    public int GetPower()
    {
        return power;
    }

    private void CheckTrackEnemy()
    {
        if (trackingStarted)
        {
            if (!closestEnemy)
            {
                closestEnemy = FindClosestEnemy();
            }
            if (closestEnemy)
            {

                Quaternion targetRotation = Quaternion.LookRotation(closestEnemy.transform.position - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, trackingEase * Time.deltaTime);

                //transform.LookAt(closestEnemy.transform);
            }
        }
        
    }

    public GameObject FindClosestEnemy()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("enemy");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "enemy")
        {
            Kill();
        }
    }

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.AddComponent(entity, typeof(MoveForward));

        MoveSpeed moveSpeed = new MoveSpeed { Value = velocity };
        dstManager.AddComponentData(entity, moveSpeed);

        TimeToLive ttl = new TimeToLive { Value = lifespan };
        dstManager.AddComponentData(entity, ttl);

    }
}
