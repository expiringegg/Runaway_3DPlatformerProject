using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Player : MonoBehaviour
{
    public Controls controls;

    public Checkpoint checkPoint;
    public BoxCollider boxCollider;
    [SerializeField] private LayerMask platform;
    public GameObject enemy;

    int damage = 1;
    public bool dead;

    public bool angleZoom;

    public Vector3 movementInput;
    public float moveForce;
    public bool walking;
    public float smoothTime = 0.1f;
    public float turnSmoothVelocity;
    public bool turned;

    public Transform cam;
    public Rigidbody body;
    public Animator animator;

    public float pauseInput;
    public GameObject pauseScreen;
    public bool gamePaused;

    public float jumpInput;
    public float jumpPower;
    public int gravity;
    public float verticalVelocity;
    public float jumpAllow;
    public int jumpCount;
    public int jumpMax = 1;

    public float attackRange;
    public float allowAttackTimer;

    public float attackInput;
    public GameObject bullet;

    public float interactInput;

    public int currentHealth;
    public GameObject deadUI;

    public float dashInput;

    public GameObject levelComplete;

    public GameObject memories;
    public GameObject memory1;
    public GameObject memory2;
    public GameObject memory3;

    public GameObject memory1Screen;
    public GameObject memory2Screen;
    public GameObject memory3Screen;

    public AudioSource jumpSound;
    public AudioSource walkSound;
    public AudioSource attackSound;
    public AudioSource hurtSound;
    public AudioSource collectible;

    private void Awake()
    {
        #region input code
        controls = new Controls();
        controls.Input.Move.performed += ctx => movementInput = ctx.ReadValue<Vector2>();
        controls.Input.Move.canceled += ctx => movementInput = Vector2.zero;

        controls.Input.Jump.performed += OnJump;
        controls.Input.Jump.canceled += OnJump => jumpInput = 0; //on letting the key go, it sets the float to 0

        controls.Input.Dash.performed += OnDash;
        controls.Input.Dash.canceled += OnDash => dashInput = 0;

        controls.Input.Attack.performed += OnAttack;
        controls.Input.Attack.canceled += OnAttack => attackInput = 0;

        controls.Input.Interact.performed += OnInteract;
        controls.Input.Interact.canceled += OnInteract => interactInput = 0;

        controls.Input.Pause.performed += OnPause;
        controls.Input.Pause.canceled += OnPause => pauseInput = 0;
        #endregion

        body = gameObject.GetComponent<Rigidbody>();
        checkPoint = GameObject.Find("CheckPointHandle").GetComponent<Checkpoint>();

        transform.position = checkPoint.savedTransform;
        Time.timeScale = 1f;

        if (checkPoint.savedTransform != new Vector3(0, 0, -2))
        {
            animator.SetTrigger("StandUp");
        }

        if (checkPoint.memory2 == true)
        {
            memory1Screen.SetActive(false);
        }

        if (checkPoint.memory1 == true)
        {
            memory2Screen.SetActive(false);
        }

        if (checkPoint.memory3 == true)
        {
            memory3.SetActive(false);
            memory3Screen.SetActive(false);
        }
    }

    private void OnEnable()
    {
        controls.Input.Enable();
    }

    private void OnDisable()
    {
        controls.Input.Disable();
    }

    private void FixedUpdate()
    {
        allowAttackTimer += Time.deltaTime;

        if (jumpInput == 1)
        {
            jumpAllow += Time.deltaTime;
        }
        if (jumpAllow > 0.1 && jumpAllow < 0.15)
        {
            jumpAllow = 0;
            jumpInput = 0;
        }

        if (GroundCheck() == true)
        {
            jumpCount = 0;
            verticalVelocity = -gravity * Time.deltaTime;
            Vector3 orignaloffset = new Vector3(0, 0.77f, 0.11f);
            boxCollider.center = Vector3.MoveTowards(boxCollider.center, orignaloffset, moveForce * Time.deltaTime);
        }
        if (jumpInput == 0)
        {
            jumpAllow = 0;
            verticalVelocity = -gravity * Time.deltaTime;
        }

        if (currentHealth != 0)
        {
            body.AddForce(movementInput.x * moveForce, verticalVelocity, movementInput.y * moveForce);
        }

        if (movementInput.x != 0 && Time.timeScale != 0 || movementInput.y != 0 && Time.timeScale != 0)
        {
            float targetangle = Mathf.Atan2(movementInput.x, movementInput.y) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetangle, ref turnSmoothVelocity, smoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            animator.SetBool("Running", true);
            if (walking == false)
            {
                walking = true;
                walkSound.Play();
            }
        }
        else
        {
            animator.SetBool("Running", false);
            walking = false;
            walkSound.Stop();
        }

        if (currentHealth < 1 && dead == false)
        {
            dead = true;
            animator.SetTrigger("death");
            hurtSound.Play();
            StartCoroutine(setUI());
        }
    }

    IEnumerator setUI()
    {
        yield return new WaitForSeconds(1.8f);
        deadUI.SetActive(true);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        jumpInput = context.ReadValue<float>();

        if (GroundCheck() && jumpInput == 1)
        {
            verticalVelocity = jumpPower;
            animator.SetTrigger("Jump");
            boxCollider.center = new Vector3(0, 1.5f, 0.11f);
            jumpSound.Play();
        }
        else if (checkPoint.DoubleJumpUnlocked == true && jumpInput == 1 && jumpCount < jumpMax)
        {
            verticalVelocity = jumpPower;
            jumpCount++;
            animator.SetTrigger("Jump");
            jumpSound.Play();
        }
    }
    bool GroundCheck()
    {
        float distToGround = 2.5f;
        return Physics.Raycast(boxCollider.bounds.center, Vector3.down, distToGround, platform);
    }
    public void OnAttack(InputAction.CallbackContext context)
    {
        attackInput = context.ReadValue<float>();

        if (checkPoint.attackUnlocked && attackInput == 1 && allowAttackTimer > 0.5f)
        {
            allowAttackTimer = 0f;
            animator.SetTrigger("Shoot");
            Instantiate(bullet, gameObject.transform.position + new Vector3(0f, 1.6f, 0.5f), Quaternion.identity); //spawns bolt away from player instead of on top of them
            attackSound.Play();
        }
    }
    public void OnDash(InputAction.CallbackContext context)
    {
        dashInput = context.ReadValue<float>();
    }
    public void OnPause(InputAction.CallbackContext context)
    {
        pauseInput = context.ReadValue<float>();

        if (pauseInput == 1 && Time.timeScale == 1f)
        {
            pauseScreen.SetActive(true);
        }
        if (pauseInput == 1 && Time.timeScale == 0f)
        {
            pauseScreen.SetActive(false);
            Time.timeScale = 1f; //makes game run normally 
        }
    }
    public void TakeDamage(int damage) => currentHealth -= damage;

    public void OnInteract(InputAction.CallbackContext context) => interactInput = context.ReadValue<float>();

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Hazard")
        {
            TakeDamage(damage);
            GameObject newenemy = Instantiate(enemy, gameObject.transform.position - new Vector3(0, 0, 6), Quaternion.identity);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "DoubleJump")
        {
            checkPoint.DoubleJumpUnlocked = true;
        }
        if (other.gameObject.tag == "Range")
        {
            checkPoint.attackUnlocked = true;
        }
        if (other.gameObject.name == "Dash")
        {
            checkPoint.dashUnlocked = true;
        }
        if (other.gameObject.tag == "NextLevelTrigger")
        {
            levelComplete.SetActive(true);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "CameraTriggerAngle")
        {
            angleZoom = true;
            cam.GetComponent<CameraController>().moveTo = other.gameObject;
        }

        if (other.gameObject == memory1)
        {
            memory1.SetActive(false);
            memories.SetActive(true);
            memory2Screen.SetActive(false);
            checkPoint.memory2 = true;
            collectible.Play();
        }
        if (other.gameObject == memory2)
        {
            checkPoint.memory1 = true;
            memories.SetActive(true);
            memory2.SetActive(false);
            memory1Screen.SetActive(false);
            checkPoint.memory1 = true;
            collectible.Play();
        }
        if (other.gameObject == memory3)
        {
            checkPoint.memory3 = true;
            memory3.SetActive(false);
            checkPoint.memory3 = true;
            memory3Screen.SetActive(false);
            memories.SetActive(true);
            other.gameObject.SetActive(false);
            collectible.Play();
            if (checkPoint.memory2 == true)
            {
                memory1Screen.SetActive(false);
            }
            if (checkPoint.memory1 == true)
            {
                memory2Screen.SetActive(false);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "CameraTriggerAngle")
        {
            angleZoom = false;
        }
    }
}