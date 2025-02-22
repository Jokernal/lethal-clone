using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 3f;
    [SerializeField] private float mouseSense = 75f;
    [SerializeField] private Camera playerCam;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Transform playerTransform;

    Vector3 mousePos = new Vector3(0, 0, 0);
    Vector3 moveDir = new Vector3();
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    
    void Update()
    {
        PlayerMovementHandler();
        PlayerMouseHandler();
        
       
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
    private void PlayerMovementHandler()
    {
        

        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        moveDir = transform.right * moveX  + transform.forward * moveY;

        

        characterController.Move(moveDir * Time.deltaTime);


         
        
    }

    
}
