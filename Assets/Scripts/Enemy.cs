using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private int velocity=5;
    [SerializeField]
    private float enemyHealth = 5;
    [SerializeField]
    private int attackStrength = 5;
    [SerializeField]
    private int returnedExp = 5;

    [SerializeField]
    private GameObject itemToDrop;
    [SerializeField]
    private float itemDropProbavility=0.5f;

    private PlayerController player;
    private Rigidbody rb;
    private Rigidbody playerRb;

    private Vector3 movement;

    GameObject droppedItemsLayer;
    private bool isAlive;
    // Start is called before the first frame update
    void Start()
    {
        isAlive = true;
        droppedItemsLayer = GameObject.Find("DroppedItems");
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        rb = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    void FixedUpdate()
    {
        if (isAlive) MoveEnemy(movement);
    
    }

    void MoveEnemy(Vector3 direction)
    {
        transform.LookAt(player.transform);
        transform.position += transform.forward * velocity * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag=="projectile" && isAlive)
        {
            float damage = other.GetComponent<Projectile>().GetPower();
            ReceiveDamage(damage, other.transform.forward);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player" && isAlive)
        {
            other.gameObject.GetComponent<PlayerController>().ReceiveDamage(attackStrength,this.gameObject.transform.forward);
        }
    }

    private void ReceiveDamage(float damage, Vector3 attackerDirecion)
    {
        ReceiveDamage(damage);
        rb.AddForce(attackerDirecion * damage * 5, ForceMode.Impulse);
    }

    void ReceiveDamage(float damage)
    {
        enemyHealth -= damage;
        if (enemyHealth < 1) Kill();
    }

    void Kill()
    {
        if (isAlive)
        {
            isAlive = false;
            player.GainExp(returnedExp);
            RandomDropObject(itemToDrop, itemDropProbavility);
            Destroy(this.gameObject);
        }

    }

    void RandomDropObject(GameObject obj,float probability)
    {
        float randomFactor = Random.Range(0f, 1f);
        if (randomFactor < probability)
        {
            DropObject(obj);
        }
    }

    void DropObject(GameObject obj)
    {
        GameObject newItem = Instantiate(obj,transform.position,transform.rotation);
        newItem.transform.parent = droppedItemsLayer.transform;

    }
}
