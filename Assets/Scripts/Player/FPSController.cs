using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSController : MonoBehaviour
{
    [Header("Hareket Ayarları")]
    public float walkSpeed = 6f;
    public float sprintSpeed = 12f;
    public float jumpHeight = 2f;
    public float gravity;
    public float walkingGravity = -9.81f -2; 
    public CharacterController controller;

    [Header("Kamera Ayarları")]
    public Transform playerCamera;
    public float mouseSensitivity = 100f;
    public float zoomFOV = 30f;
    public float normalFOV = 60f;
    public float zoomSpeed = 10f;

    [Header("Zemin Kontrolü")]
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    private Vector3 velocity;
    private bool isGrounded;
    private float xRotation = 0f;
    float YRotation = 0f;
    private float currentSpeed;
    private Camera cam;
    private PlayerState playerState;

    private Vector3 lastPosition = new Vector3 (0f, 0f,0f);
    public bool isMoving;
    
    // Swimming
    public bool isSwimming;
    public bool isUnderwater;
    public float swimmingGravity = -0.5f;

    
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Fareyi ekranın ortasına kilitle
        cam = playerCamera.GetComponent<Camera>(); // Kamera bileşenini al
        
    }

    void Update()
    {   
        //if (DialogSystem.Instance.dialogUIActivate == false && StorageManager.Instance.storageUIOpen == false && CampfireUIManager.Instance.isUiOpen == false)
       // {
      //      Movement();
       // }
        
        if (MovmentManager.Instance.canMove)
        {
            Movement();
        }
        
        if(!InventorySystem.Instance.isOpen && !CraftingSystem.Instance.isOpen && !MenuManager.Instance.isMenuOpen && !DialogSystem.Instance.dialogUIActivate && !QuestManager.Instance.isQuestMenuOpen && !StorageManager.Instance.storageUIOpen && !CampfireUIManager.Instance.isUiOpen)
        {
                if (MovmentManager.Instance.canLookAround){
                    float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
                    float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
                
                    //control rotation around x axis (Look up and down)
                    xRotation -= mouseY;
                
                    //we clamp the rotation so we cant Over-rotate (like in real life)
                    xRotation = Mathf.Clamp(xRotation, -90f, 90f);
                
                    //control rotation around y axis (Look up and down)
                    YRotation += mouseX;
                
                    //applying both rotations
                    transform.localRotation = Quaternion.Euler(xRotation, YRotation, 0f);
                    }
                    // Zoom (Sağ Tık)
                    float targetFOV = Input.GetKey(KeyCode.V) ? zoomFOV : normalFOV;
                    cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetFOV, zoomSpeed * Time.deltaTime);
                }
    }

    public void Movement()
    {
        if (isSwimming)
        {
            if (isUnderwater)
            {
                gravity = swimmingGravity;
            }
            else
            {
                velocity.y = 0;
            }
            
        }
        else
        {
            gravity = walkingGravity;
        }
        // Zemin kontrolü
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Hareket Kontrolleri
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Shift basılıysa hızlı koş
        currentSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed;

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * currentSpeed * Time.deltaTime);

        if (lastPosition != gameObject.transform.position && isGrounded == true)
        {
            isMoving = true;

            SoundManager.Instance.PlaySound(SoundManager.Instance.grassWalkSound);
        }
        else
        {
            isMoving = false;
            SoundManager.Instance.grassWalkSound.Stop();
        }
        lastPosition = gameObject.transform.position;
        

        


        // Zıplama
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Yerçekimi Uygula
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    

        
}
