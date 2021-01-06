using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private int velocity=5;
    [SerializeField]
    private float health = 5;
    [SerializeField]
    private int attackStrength = 5;
    [SerializeField]
    private int returnedExp = 5;

    private PlayerController player;
    private Rigidbody rb;
    private Rigidbody playerRb;

    private Vector3 movement;



    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        rb = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    void FixedUpdate()
    {
        MoveEnemy(movement);
    
    }

    void MoveEnemy(Vector3 direction)
    {
        transform.LookAt(player.transform);
        transform.position += transform.forward * velocity * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag=="projectile")
        {
            float damage = other.GetComponent<Projectile>().GetPower();
            ReceiveDamage(damage, other.transform.forward);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
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
        health -= damage;
        if (health < 1) Kill();
    }

    void Kill()
    {
        player.GainExp(returnedExp);
        Destroy(this.gameObject);
    }
}
