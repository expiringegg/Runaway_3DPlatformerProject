using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public Animator animator;
    public Transform movePoint;
    public GameObject block;
    public Transform player;
    public bool inRange;
    public int speed;

    public int dialogueNum;
    public List<string> dialogueLines;
    public bool talked;

    public float smoothTime = 0.4f;
    public float turnSmoothVelocity;

    public bool keyDown;

    public float timer;

    private void FixedUpdate()
    {
        timer += Time.deltaTime;
    }

    void Update()
    {
        if (!talked && player.GetComponent<Player>().interactInput == 1 && inRange)
        {
            FindObjectOfType<UIManager>().dialogueBox.SetActive(true);
            FindObjectOfType<UIManager>().Dialogue(dialogueLines[dialogueNum]);
            if (player.GetComponent<Player>().interactInput == 1 && !keyDown)
            {
                keyDown = true;
                dialogueNum++;
            }

            if (dialogueNum >= dialogueLines.Count)
            {
                talked = true;
                FindObjectOfType<UIManager>().dialogueBox.SetActive(false);
                block.SetActive(false);
            }
        }

        if (player.GetComponent<Player>().interactInput == 0)
        {
            keyDown = false;
        }
        
        if(inRange && talked)
        {
            timer = 0f;
            transform.position = Vector3.MoveTowards(transform.position, movePoint.transform.position, speed * Time.deltaTime);
            animator.SetBool("Running", true);
            float targetangle = Mathf.Atan2(0, 1) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.position.y, targetangle, ref turnSmoothVelocity, smoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }

        if (!inRange && timer > 0.8f)
        {
            animator.SetBool("Running", false);
            float targetangle = Mathf.Atan2(0, -1) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetangle, ref turnSmoothVelocity, smoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }
    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            inRange = true;
        }
    }

    private void OnTriggerExit(Collider collision)
    { 
        if (collision.gameObject.tag == "Player")
        {
            inRange = false;
        }
    }
}
