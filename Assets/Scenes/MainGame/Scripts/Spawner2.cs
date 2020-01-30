using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner2 : MonoBehaviour
{
    public GameObject[] toys;

    private void Start()
    {
        Instantiate(toys[Random.Range(0, toys.Length)], transform.position, Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0)));              
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Invoke("SpawnRandom", 3f);
            gameObject.GetComponent<BoxCollider>().enabled = false;
        }
    }

    private void SpawnRandom()
    {
        Instantiate(toys[Random.Range(0, toys.Length)], transform.position, Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0)));
        gameObject.GetComponent<BoxCollider>().enabled = true;
    }
}
