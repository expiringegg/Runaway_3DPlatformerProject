using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour
{
    public GameObject player;
    public GameObject enemy;
    public int damage = 1;

    void Start()
    {
        player = GameObject.Find("Player");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            player.GetComponent<Player>().TakeDamage(damage);
            GameObject newenemy = Instantiate(enemy, gameObject.transform.position - new Vector3(0, 0, 6), Quaternion.identity);
        }
    }
}