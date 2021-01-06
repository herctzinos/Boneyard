using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawn : MonoBehaviour
{
    [SerializeField]
    private bool active = true;
    private bool globalyEnabled=true;

    [Header("Item to spawn settings")]
    [SerializeField]
    private GameObject objectToSpawn;
    [SerializeField]
    private float spawnRate;
    [SerializeField]
    private int itemsToSpawn=1;
    [SerializeField]
    private int itemsInitialSpeed = 0;

    private Vector3 spawnPosition;
    private float nextSpawn = 0f;

    // Start is called before the first frame update
    void Start()
    {
        nextSpawn = spawnRate;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (active&&globalyEnabled) Spawn();
    }
    private void Spawn()
    {
        if (Time.time > nextSpawn)
        {
            nextSpawn = Time.time + spawnRate;
            spawnPosition = transform.position;
            GameObject newItemy = Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);
            //Rigidbody newItemRB = newItemy.GetComponent<Rigidbody>();
        }
    }

    public bool SetGlobalEnabling(bool enbl)
    {
        globalyEnabled = enbl;
        return globalyEnabled;
    }

}
