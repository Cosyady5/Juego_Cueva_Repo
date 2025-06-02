using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement & Look Stats")]
    [SerializeField] private float rotationSpeed = 4f;
    public float speed;
    public float maxForce = 1; //L?mite de aceleraci?n m?xima
    private Transform cameraFollowTransform;

    [Header("Interactable Stats")]
    public float shootingCooldown;
    public int damage;
    [SerializeField] Collider attackCollider;


    [Header("State Bools")]
    [SerializeField] bool isAttacking; //Verdadero cuando ESTAMOS DISPARANDO
    [SerializeField] bool canAttack; //Verdadero cuando PODEMOS DISPARAR
    public bool isDead = false;
    

    [Header("Jumping Stats")]
    public float jumpForce;
    [SerializeField] GameObject groundCheck;
    [SerializeField] bool isGrounded;
    [SerializeField] float groundCheckRadius;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] int maxJumps = 1;
    int jumpCount;
    float timeAir;
    bool firstjump = false;

    //Referencias privadas (GetComponent)
    private Rigidbody playerRb;
    private Animator anim;
    //Referencias privadas del input
    Vector2 moveInput;
    //Vector2 lookInput;
    private AudioSource stepSound;

    private void Awake()
    {
        playerRb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        groundCheck = GameObject.Find("GroundCheck");
        cameraFollowTransform = Camera.main.transform;
        canAttack = false;
        stepSound = GetComponent<AudioSource>();
    }
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        attackCollider.enabled = false;

    }

    void Update()
    {
        HandleAnimations();
        HandleFootsteps();
        isGrounded = Physics.CheckSphere(groundCheck.transform.position, groundCheckRadius, groundLayer);
        if (isGrounded && jumpCount > 0) jumpCount = 0;
        /*if (isGrounded && jumpCount > 0)
        {
            timeAir = 0f;
            jumpCount = 0;
        }
        else timeAir += Time.deltaTime;*/
        if (canAttack && isAttacking) Attack();
    }

    void HandleFootsteps()
    {
        bool isMoving = moveInput.magnitude > 0.1f && isGrounded && !isDead;

        if (isMoving && !stepSound.isPlaying)
        {
            stepSound.Play();
        }
        else if (!isMoving && stepSound.isPlaying)
        {
            stepSound.Stop();
        }
    }
    private void FixedUpdate()
    {
        Movement();
    }

    void Movement()
    {
        if (isDead) return;
        Vector3 currentVelocity = playerRb.velocity;
        Vector3 inputDir = new Vector3(moveInput.x, 0, moveInput.y);
        Vector3 camForward = cameraFollowTransform.forward;
        Vector3 camRight = cameraFollowTransform.right;
        
        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        // Movimiento relativo a la c?mara
        Vector3 targetVelocity = (camForward * inputDir.z + camRight * inputDir.x) * speed;

        //Calcular las fuerzas que afectan al movimiento
        Vector3 velocityChange = (targetVelocity - currentVelocity);
        velocityChange = new Vector3(velocityChange.x, 0, velocityChange.z); //Hace que la aceleraci?n no afecte en vertical
        Vector3.ClampMagnitude(velocityChange, maxForce);

        //Aplicamos el movimiento
        playerRb.AddForce(velocityChange, ForceMode.VelocityChange);

        if (moveInput != Vector2.zero)
        {
            float targetAngle = Mathf.Atan2(moveInput.x, moveInput.y) * Mathf.Rad2Deg + cameraFollowTransform.eulerAngles.y;
            Quaternion rotation = Quaternion.Euler(0f, targetAngle, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.fixedDeltaTime * rotationSpeed);
        }
    }

    void Attack()
    {
        if (isDead) return;

        AudioManager.Instance.PlaySFX(7);
        canAttack = false;
        anim.SetTrigger("isAttacking");
        Invoke(nameof(ResetAttack), shootingCooldown);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!attackCollider.enabled) return;

        if (other.CompareTag("Enemy") || other.CompareTag("SpawnedEnemy"))
        {
            other.GetComponent<EnemyHealth>().TakeDamage(damage);
            attackCollider.enabled = false;
        }
        if (other.CompareTag("Destructible"))
        {
            other.GetComponent<FracturedObjects>().Explode();
        }
        if (other.CompareTag("Crystal"))
        {
            other.GetComponent<FracturedObjects>().Explode();
        }
    }

    public void SetHasWeapon(bool value)
    {
        canAttack = value;
    }
    void ResetAttack()
    {
        canAttack = true;
    }

    void Jump()
    {
        if (isDead) return;
        if (jumpCount < maxJumps)
        {
            AudioManager.Instance.PlaySFX(6);
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumpCount++;
        }

        /*if ((isGrounded || timeAir < 0.25f) && jumpCount == 0)
        {
            AudioManager.Instance.PlaySFX(6);
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumpCount = 1;
        }
        else
        {
            if (jumpCount == 1)
            {
                AudioManager.Instance.PlaySFX(6);
                playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                jumpCount = 2;
            }
        }*/
    }

    void HandleAnimations()
    {
        anim.SetBool("isJumping", !isGrounded);
        anim.SetFloat("VelocityY", playerRb.velocity.y);
        anim.SetBool("isRunning", Mathf.Abs(moveInput.x) > 0.1f || Mathf.Abs(moveInput.y) > 0.1f);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + transform.forward * 1f, 1.2f);
    }
    public void EnableAttackCollider()
    {
        attackCollider.enabled = true;
    }

    public void DisableAttackCollider()
    {
        attackCollider.enabled = false;
    }

    #region Input Methods

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
    /*public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }*/

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Jump();
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isAttacking = true;
        }
        if (context.canceled)
        {
            isAttacking = false;
        }
    }
    #endregion
}
