using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class LeverInteract : MonoBehaviour
{
    [Header("Lever Settings")]
    [SerializeField] private GameObject interactionUILever;
    private bool isPlayerInRange = false;
    private GameObject player;
    private PlayerInput playerInput;
    private InputAction interactAction;
    public GameObject Activate;

    public int points;
    public int winPoints;


    private void Start()
    {
        transform.localEulerAngles = new Vector3(-120f, 0f, 0f);
        points = 0;
        winPoints = 2;
    }
    private void Awake()
    {
        playerInput = GameObject.FindWithTag("Player").GetComponent<PlayerInput>();
        interactAction = playerInput.actions["Interact"];
    }

    private void Update()
    {
        interactionUILever.transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
    }

    private void OnEnable()
    {
        interactAction.performed += OnInteract;
    }

    private void OnDisable()
    {
        interactAction.performed -= OnInteract;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.gameObject;
            isPlayerInRange = true;
            interactionUILever.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            interactionUILever.SetActive(false);
            player = null;
        }
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        if (isPlayerInRange)
        {
            points += 1;
            isPlayerInRange = false;
            interactionUILever.SetActive(false);
            // Destroy(interactionUILever);
            transform.localEulerAngles = new Vector3(-50f, 0f, 0f);
            //Quaternion targetRot = Quaternion.Euler(-50f, transform.eulerAngles.y, transform.eulerAngles.z);
            //transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, Time.deltaTime * 2f);

            if (points >= winPoints)
            {
                Destroy(Activate);
            }
         
           

        }

    }
}
