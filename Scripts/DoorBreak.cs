using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBreak : MonoBehaviour
{
    public Player player;
    public Rigidbody body;
    public Animator animator;
    public ParticleSystem fumes;
    public bool destroyed;

    public AudioSource open;
    public AudioSource smoke;
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        body = gameObject.GetComponent<Rigidbody>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (player.interactInput == 1 && other.gameObject.tag == "Player" && !destroyed)
        {
            destroyed = true;
            animator.SetTrigger("Destroyed");
            open.Play();
            StartCoroutine(Delay());
        }
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(1f);
        fumes.gameObject.SetActive(true);
        fumes.Play();
        smoke.Play();
    }
}
