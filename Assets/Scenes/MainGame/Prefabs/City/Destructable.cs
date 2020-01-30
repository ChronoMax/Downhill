using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructable : MonoBehaviour
{
    public GameObject brokenHouse;
    public GameObject brokenHouseBig;
    public GameObject brokenTree;

    public bool huisje;
    public bool huis;
    public bool tree;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && huisje)
        {
            Instantiate(brokenHouse, transform.position, transform.rotation);
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Player") && huis)
        {
            Instantiate(brokenHouseBig, transform.position, transform.rotation);
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Player") && tree)
        {
            Instantiate(brokenTree, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
