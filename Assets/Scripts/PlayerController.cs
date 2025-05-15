using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Это автоматически добавит контроллер персонажа к игровому объекту, если он еще не применен:
[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{

    public Camera playerCam;

    // Настройки передвижения
    public float walkSpeed = 3f;
    public float runSpeed = 5f;
    public float gravity = 10f;

    // Настройки камеры
    public float lookSpeed = 2f;
    public float lookXLimit = 75f;

    // Звуки
    public AudioClip[] FootstepSoundsLeft; 
    public AudioClip[] FootstepSoundsRight; 
    public Transform footstepAudioPosition;
    public AudioSource audioSource;

    private bool isWalking = false;
    private bool isLeftStep = true;

    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;


    private bool canMove = true;

    // Движение камеры во время передвижения
    public float walkBobFrequency = 1.5f; 
    public float runBobFrequency = 2.5f;  
    public float bobAmplitude = 0.05f;    
    private float bobFrequency;          
    private float bobTimer = 0f;
    private bool playFootstepAtValley = false; 

    CharacterController characterController;

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Хотьба/Бег:
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        bool isRunning = Input.GetKey(KeyCode.LeftShift);

        bobFrequency = isRunning ? runBobFrequency : walkBobFrequency;

        float targetSpeed = isRunning ? runSpeed : walkSpeed;

        float curSpeedX = canMove ? targetSpeed * Input.GetAxis("Vertical") : 0; // Forward/backward movement
        float curSpeedY = canMove ? targetSpeed * Input.GetAxis("Horizontal") : 0; // Left/right movement

        float movementDirectionY = moveDirection.y;

        moveDirection = (forward * curSpeedX) + (right * curSpeedY);


        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime* 1000;
        }

        characterController.Move(moveDirection * Time.deltaTime);

        // Передвижение камеры
        if (canMove)
        {
            rotationX -= Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCam.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }

        // Синхронизация движений камеры и звуков шагов
        if (curSpeedX != 0f || curSpeedY != 0f)
        {
            bobTimer += Time.deltaTime * bobFrequency;

            float baseBobOffset = Mathf.Sin(bobTimer) * bobAmplitude;

            float sharpBobOffset = Mathf.Sin(bobTimer) * bobAmplitude * 0.5f;

            float totalBobOffset = baseBobOffset + sharpBobOffset;

            if (Mathf.Sin(bobTimer) < -0.99f && !playFootstepAtValley)
            {
                playFootstepAtValley = true;
                PlayFootstep();
            }
            else if (Mathf.Sin(bobTimer) > -0.99f)
            {
                playFootstepAtValley = false;
            }

        
            playerCam.transform.localPosition = new Vector3(0, 1 + totalBobOffset, 0);

        }
       
        //Увеличение фов во время бега
        if (isRunning)
        {
            playerCam.fieldOfView = Mathf.Lerp(playerCam.fieldOfView,60f, Time.deltaTime*5);
            
        }
        else if (!isRunning) 
        {
            playerCam.fieldOfView = Mathf.Lerp(playerCam.fieldOfView, 50f, Time.deltaTime*5);
        }

    }

    // Звуки передвижение
    private void PlayFootstep()
    {
        AudioClip[] selectedFootstepSounds = isLeftStep ? FootstepSoundsLeft : FootstepSoundsRight;

        if (selectedFootstepSounds.Length > 0)
        {
            int randomIndex = Random.Range(0, selectedFootstepSounds.Length);
            audioSource.transform.position = footstepAudioPosition.position;
            audioSource.clip = selectedFootstepSounds[randomIndex];
            audioSource.Play();
        }

        isLeftStep = !isLeftStep; // Чередования шагов
    }
}
