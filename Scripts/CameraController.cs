using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject moveTo;
    private GameObject player;
    public int moveSpeed;
    public Vector3 originalTransform;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player"); 
    }

    void FixedUpdate()
    {
        if (player.GetComponent<Player>().angleZoom == false)
        {
            gameObject.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 4, player.transform.position.z - 4);
            gameObject.transform.rotation = Quaternion.Euler(20.34f, 0f, 0f);
        }

        if (player.GetComponent<Player>().angleZoom == true)
        {
            transform.position = moveTo.transform.position;
            gameObject.transform.rotation = moveTo.transform.rotation;
        }
    }
}


