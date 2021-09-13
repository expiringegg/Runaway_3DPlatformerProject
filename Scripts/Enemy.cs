using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health;
    public int damage;
    public int speed = 10;
    public Animator animator;

    public bool inRange;
    public float allowAttackTimer;

    public Vector3[] movePoints;
    public int moveAmount;
    public bool reachedPoint;
    public int randomPoint;
    public Transform player;
    public Vector3 originalPos;

    public AudioSource attack;
    public AudioSource growl;

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<Transform>();
        randomPoint = Random.Range(1, movePoints.Length);
        originalPos = transform.position;
        movePoints[0] = new Vector3(originalPos.x + moveAmount, originalPos.y, originalPos.z);
        movePoints[1] = new Vector3(originalPos.x, originalPos.y, originalPos.z + moveAmount);
        movePoints[2] = new Vector3(originalPos.x - moveAmount, originalPos.y, originalPos.z);
        movePoints[3] = new Vector3(originalPos.x, originalPos.y, originalPos.z - moveAmount);
    }

    public void TakeDamage(int damage)
    {
        health -= damage; //decreases health when hit
    }

    private void FixedUpdate()
    {
        allowAttackTimer += Time.deltaTime;
    }

    private void Update()
    {
        if (health > 0)
        {
            if (reachedPoint == true)
            {
                randomPoint = Random.Range(1, movePoints.Length);
            }

            if (transform.position == movePoints[randomPoint])
            {
                reachedPoint = true;
            }
            else
            {
                reachedPoint = false;
            }

            if (inRange)
            {
                transform.position = Vector3.MoveTowards(transform.position, player.position, (speed + 3) * Time.deltaTime);
                animator.SetBool("Run", true);
                growl.Stop();
                transform.LookAt(player);
            }
            if (!inRange)
            {
                animator.SetBool("Run", false);
                transform.position = Vector3.MoveTowards(transform.position, movePoints[randomPoint], speed * Time.deltaTime);
                transform.LookAt(movePoints[randomPoint]);
            }
        }
        if (health <= 0)
        {
            animator.SetTrigger("Death");
            StartCoroutine(Despawn()); //I delayed the destruction so the animation can finish playing before it's destoryed 
        }
    }

    public IEnumerator Despawn()
    {
        yield return new WaitForSeconds(0.6f);
        Destroy(gameObject);
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            player.GetComponent<Player>().TakeDamage(damage);
            animator.SetBool("Attack", true);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            animator.SetBool("Attack", false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        {
            if (other.gameObject.tag == "Player")
            {
                inRange = true;
                attack.Play();
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            inRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            inRange = false;
            growl.Play();
            attack.Stop();
        }
    }
}

