using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform player;

    private float smoothSpeed = 0.125f;

    public  Vector3 offset;

    private void Start()
    {
        GameObject playerObj = GameObject.Find("Player");
        player = playerObj.GetComponent<Transform>();
    }
    void LateUpdate()
    {
        Vector3 desiredPosition = player.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(player.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        player.Rotate(0,0,0);
    }
}
