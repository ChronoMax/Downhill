using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollFloor : MonoBehaviour
{
    private UI UIscript;
    public bool zone1;
    public bool zone2;
    public bool zone3;

    private void Start()
    {
        UIscript = GameObject.Find("UIController").GetComponent<UI>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("ShardDestroy"))
        {
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("Shard") && zone1) //zone1 
        {
            UIscript.ScoreAdd();
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("Shard") &&  zone2) //zone2
        {
            UIscript.ScoreAdd5();
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("Shard") && zone3) //zone 3
        {
            UIscript.ScoreAdd7();
            Destroy(collision.gameObject);
        }
    }
}
