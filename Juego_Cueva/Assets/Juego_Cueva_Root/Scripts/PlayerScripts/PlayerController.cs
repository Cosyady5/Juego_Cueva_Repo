using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement & Look Stats")]
    [SerializeField] GameObject camHolder;
    [SerializeField] private float rotationSpeed = 4f;
    public float speed;
    public float maxForce = 1; //Límite de aceleración máxima
    public float sensitivity = 0.1f; //Sensibilidad aplicada al input de observar
    private Transform cameraFollowTransform;

    [Header("Jumping Stats")]
    public float jumpForce;
    [SerializeField] GameObject groundCheck;
    [SerializeField] bool isGrounded;
    [SerializeField] float groundCheckRadius;
    [SerializeField] LayerMask groundLayer;

    //Referencias privadas (GetComponent)
    private Rigidbody playerRb;
    private Animator anim;
    //Referencias privadas del input
    Vector2 moveInput;
    Vector2 lookInput;
    float lookRotation; //Valor de rotación que puede ser utilizado para la dirección de movimiento

    private void Awake()
    {
        playerRb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        //camHolder = GameObject.Find("CameraHolder");
        groundCheck = GameObject.Find("GroundCheck");
        cameraFollowTransform = Camera.main.transform;
    }
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleAnimations();
        isGrounded = Physics.CheckSphere(groundCheck.transform.position, groundCheckRadius, groundLayer);
    }
    private void FixedUpdate()
    {
        Movement();
    }

    private void LateUpdate()
    {
        //CameraLook();
    }

    void Movement()
    {
        Vector3 currentVelocity = playerRb.velocity; //Velocidad actual del player
        /*Vector3 targetVelocity = new Vector3(moveInput.x, 0, moveInput.y); //Velocidad hacia la que queremos que se mueva el player
        targetVelocity *= speed;
        //Alinear la dirección con la orientación correcta (de local a global)
        targetVelocity = transform.TransformDirection(targetVelocity);*/
        Vector3 inputDir = new Vector3(moveInput.x, 0, moveInput.y);
        Vector3 camForward = cameraFollowTransform.forward;
        Vector3 camRight = cameraFollowTransform.right;

        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        // Movimiento relativo a la cámara
        Vector3 targetVelocity = (camForward * inputDir.z + camRight * inputDir.x) * speed;

        //Calcular las fuerzas que afectan al movimiento
        Vector3 velocityChange = (targetVelocity - currentVelocity);
        velocityChange = new Vector3(velocityChange.x, 0, velocityChange.z); //Hace que la aceleración no afecte en vertical
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

    /*void CameraLook()
    {
        //Girar (Gira el personaje en horizontal)
        transform.Rotate(Vector3.up * lookInput.x * sensitivity);
        //Mirar (Gira la cámara en vertical)
        lookRotation += (-lookInput.y * sensitivity);
        lookRotation = Mathf.Clamp(lookRotation, -90, 90); //Restringe el valor de lookRotation entre dos valores mínimo/máximo
        camHolder.transform.eulerAngles = new Vector3(lookRotation, camHolder.transform.eulerAngles.y, camHolder.transform.eulerAngles.z);
    }*/

    void Jump()
    {
        if (isGrounded)
        {
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
    void HandleAnimations()
    {
        anim.SetBool("isJumping", !isGrounded);
        anim.SetFloat("VelocityY", playerRb.velocity.y);
        anim.SetBool("isRunning", Mathf.Abs(moveInput.x) > 0.1f || Mathf.Abs(moveInput.y) > 0.1f);
    }

    #region Input Methods

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Jump();
        }
    }
    #endregion
}
