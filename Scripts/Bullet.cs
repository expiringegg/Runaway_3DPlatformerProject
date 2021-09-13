using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public LayerMask whatisEnemies;
    public float attackRange;
    public int damage = 1;

    public AudioSource hit;
        void Update()
        {
            gameObject.transform.position += new Vector3(0f, 0f, 0.2f);

            Collider[] enemiestodamage = Physics.OverlapCapsule(gameObject.transform.position, gameObject.transform.position, attackRange, whatisEnemies);
            for (int i = 0; i < enemiestodamage.Length; i++)
            {
                enemiestodamage[i].GetComponent<Enemy>().TakeDamage(damage); //the amount of damage caused
                //hit.Play();

                StartCoroutine(FasterDespawn());
            }

        StartCoroutine(Despawn());
    }
    
    IEnumerator Despawn()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

    IEnumerator FasterDespawn()
    {
        yield return new WaitForSeconds(0.02f);
        Destroy(gameObject);
    }
}
