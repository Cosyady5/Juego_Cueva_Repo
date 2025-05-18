using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon : MonoBehaviour
{
    [Header("Weapon Settings")]
    [SerializeField] private GameObject weaponOnPlayer;
    [SerializeField] private GameObject interactionUI;
    private bool isPlayerInRange = false;
    private GameObject player;
    private PlayerInput playerInput;
    private InputAction interactAction;

    private void Awake()
    {
        playerInput = GameObject.FindWithTag("Player").GetComponent<PlayerInput>();
        interactAction = playerInput.actions["Interact"]; 
    }

    private void Update()
    {
        interactionUI.transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
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
            interactionUI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            interactionUI.SetActive(false);
            player = null;
        }
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        if (isPlayerInRange)
        {
            weaponOnPlayer.SetActive(true);
            player.GetComponent<PlayerController>().SetHasWeapon(true);
            interactionUI.SetActive(false);
            gameObject.SetActive(false);
        }
    }
}
