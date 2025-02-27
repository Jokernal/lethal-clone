using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface Interactable
{
    public void Interact();
}

public class PlayerController : MonoBehaviour
{
   

    [SerializeField] private float walkSpeed = 3f;
    [SerializeField] private float runSpeed = 6f;
    [SerializeField] private float jumpForce = 3f;
    //[SerializeField] private float dodgeForce = 20f;
    [SerializeField] private float mouseSense = 1000f;
    [SerializeField] private Camera playerCam;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private LayerMask jumpable;

    Vector3 mousePos = new Vector3(0, 0, 0);
    Vector3 moveDir = new Vector3();
    Vector3 groundedBoxCheck = new Vector3(.1f, .1f,.1f);
    Vector3 velocity;

    bool grounded;
    bool isCrouching = false;
    bool isDodge = false;

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
        //Dodge();       
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
      
        if(Input.GetKey(KeyCode.LeftShift))
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

            isCrouching = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            characterController.height = 2f;
            characterController.center = new Vector3(0, 1, 0);
            playerCam.transform.position = playerCam.transform.position - new Vector3(0, -0.5f, 0f);

            isCrouching = false;
        }   
    }

    private void PlayerJump()
    {
        if (Physics.CheckBox(playerTransform.position, groundedBoxCheck, new Quaternion(0,0,0,0), jumpable))
            grounded = true;
        else
            grounded = false;

        if (grounded && !isCrouching && !isDodge && Input.GetKeyDown(KeyCode.Space))
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

    private void Hit()
    {
        Ray r = new Ray(playerCam.transform.position, playerCam.transform.forward);

        if (Physics.Raycast(r, out RaycastHit hitInfo, 1.5f))
            if(hitInfo.collider.gameObject.TryGetComponent(out Hurtable hurtableObj))
            {
                if(Input.GetMouseButtonDown(0))
                {
                    hurtableObj.Hurt();
                }
            }

    }
    //Revisit dodge
    /*private void Dodge()
    {
        if (Input.GetMouseButton(1))
            isDodge = true;
        else
            isDodge = false;
        
        if (isDodge && Input.GetKeyDown(KeyCode.Space))
           
            characterController.Move(moveDir * dodgeForce * Time.deltaTime);
        

    }*/
}
