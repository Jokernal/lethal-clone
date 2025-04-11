using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

interface Interactable
{
    public void Interact();
}

public class PlayerController : MonoBehaviour
{
   

    [SerializeField] private float walkSpeed = 3f;
    [SerializeField] private float runSpeed = 6f;
    [SerializeField] private float jumpForce = 3f;
    [SerializeField] private float mouseSense = 1000f;
    [SerializeField] private Camera playerCam;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private LayerMask jumpable;
    [SerializeField] private GameObject punch;

    Vector3 mousePos = new Vector3(0, 0, 0);
    Vector3 moveDir = new Vector3();
    Vector3 groundedBoxCheck = new Vector3(.5f, .1f,.5f);
    Vector3 velocity;

    bool grounded;
    bool isCrouching = false;
    
    float gravity = -9.81f;
    
    void Start()
    {
        
        Cursor.lockState = CursorLockMode.Locked;
    }

    
    void Update()
    {
        PlayerLocomotionHandlerNormalized();
        PlayerMouseHandler();
        PlayerCrouch();
        PlayerJump();
        Gravity();
        InteractHandler();
        Hit();
              
    }

    private void PlayerMouseHandler()
    {
        float xInput = Input.GetAxis("Mouse X") * Time.deltaTime * mouseSense;
        float yInput = Input.GetAxis("Mouse Y") * Time.deltaTime * mouseSense;

        mousePos.y -= yInput;

        mousePos.y = Mathf.Clamp(mousePos.y, -90, 90);

        transform.Rotate(Vector3.up * xInput);
        Vector3 camRotationY = new Vector3(mousePos.y, 0, 0);
        playerCam.transform.localEulerAngles = camRotationY;
    }
    private void PlayerLocomotionHandlerNormalized()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        moveDir = transform.right * moveX + transform.forward * moveY;

        moveDir = moveDir.normalized;
      
        if(Input.GetKey(KeyCode.LeftShift)  && !isCrouching)
        characterController.Move(moveDir * runSpeed * Time.deltaTime);
        
        characterController.Move(moveDir * walkSpeed * Time.deltaTime);
    }

    private void PlayerCrouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            characterController.height = characterController.height/2;
            characterController.center = new Vector3(0,.5f,0);
            playerCam.transform.position = playerCam.transform.position - new Vector3(0, 0.5f, 0f);

            walkSpeed -= 1f;
            isCrouching = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            characterController.height = 2f;
            characterController.center = new Vector3(0, 1, 0);
            playerCam.transform.position = playerCam.transform.position - new Vector3(0, -0.5f, 0f);

            walkSpeed += 1f;
            isCrouching = false;
        }   
    }

    private void PlayerJump()
    {
        if (Physics.CheckBox(playerTransform.position, groundedBoxCheck, new Quaternion(0,0,0,0), jumpable))
            grounded = true;
        else
            grounded = false;

        if (grounded && !isCrouching && Input.GetKeyDown(KeyCode.Space))
        {
            velocity.y = -6f;
            velocity.y += jumpForce * 2;
            characterController.Move(velocity * Time.deltaTime);  
        }
    }

    private void Gravity()
    {    
        velocity.y += gravity * 1.5f * Time.deltaTime;

        characterController.Move(velocity * Time.deltaTime);

        if (grounded && velocity.y < - 13.81f)
            velocity.y = -2;
    }

    private void InteractHandler()
    {
        Ray r = new Ray(playerCam.transform.position, playerCam.transform.forward);

        if (Physics.Raycast(r, out RaycastHit hitInfo, 1.5f))
        {
        
            if(hitInfo.collider.gameObject.TryGetComponent(out Interactable interactObj))
            {
                if(Input.GetKeyDown(KeyCode.E))
                {
                    interactObj.Interact();
                }
            }
        }  
    }

    private async void Hit()
    {
        
        
        
                if(Input.GetMouseButtonDown(0) && !punch.activeInHierarchy)
                {
                    await Task.Delay(75);
                    punch.SetActive(true);
            
                    if (punch.activeInHierarchy)
                    {
                        await Task.Delay(150);
                        Debug.Log("Puanch");
                        punch.SetActive(false);
                    }
                }

        
    }
   
}
