using System;
using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    public float speed = 5.0f;
    public float sprintSpeed = 10.0f; // Shift tuşuna basınca hızlanma
    public float mouseSensitivity = 2.0f;
    public float jumpHeight = 2.0f;
    public CharacterController controller;

    private float gravity = -9.81f;
    private Vector3 velocity;
    private float xRotation = 0f;
    [SerializeField] GameObject userInventory;
    bool isInventoryOpen;

    public Transform cameraTransform;

    void Start()
    {
        userInventory.SetActive(false);
        //Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            if (isInventoryOpen == false)
            {
                userInventory.SetActive(true);
                isInventoryOpen = true;

            }
            else if(isInventoryOpen==true)
            {
                userInventory.SetActive(false);
                isInventoryOpen = false;
            }
        }
        // Fare ile bakış kontrolü
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);

        // Klavye ile hareket kontrolü
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // Shift tuşuna basınca hızlanma
        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : speed;

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        controller.Move(move * currentSpeed * Time.deltaTime);

        // Yer çekimi uygulama
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Zıplama
        if (Input.GetButtonDown("Jump") && controller.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }
}
