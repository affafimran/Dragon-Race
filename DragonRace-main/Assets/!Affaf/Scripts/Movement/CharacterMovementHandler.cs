using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Dreamteck.Splines;

public class CharacterMovementHandler : NetworkBehaviour
{
    Vector2 viewInput;

    //Rotation
    float cameraRotationX = 0;

    //Other components
    NetworkCharacterControllerPrototypeCustom networkCharacterControllerPrototypeCustom;
    Camera localCamera;

    private void Awake()
    {
        networkCharacterControllerPrototypeCustom = GetComponent<NetworkCharacterControllerPrototypeCustom>();
        localCamera = GetComponentInChildren<Camera>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        cameraRotationX += viewInput.y * Time.deltaTime * networkCharacterControllerPrototypeCustom.viewUpDownRotationSpeed;
        cameraRotationX = Mathf.Clamp(cameraRotationX, -90, 90);

        localCamera.transform.localRotation = Quaternion.Euler(cameraRotationX, 0, 0);
    }

    public override void FixedUpdateNetwork()
    {
        //Get the input from the network
        if (GetInput(out NetworkInputData networkInputData))
        {
            //my comment-----------------------------------------------------
            //Rotate the view
            //networkCharacterControllerPrototypeCustom.Rotate(networkInputData.rotationInput);

            ////Move
            //Vector3 moveDirection = transform.forward * networkInputData.movementInput.y + transform.right * networkInputData.movementInput.x;
            //moveDirection.Normalize();

            //networkCharacterControllerPrototypeCustom.Move(moveDirection);

            ////Jump
            //if(networkInputData.isJumpPressed)
            //    networkCharacterControllerPrototypeCustom.Jump();
            //---------------------------------------------------------------

            //my code------------------------------------------
            
            //Debug.Log($"Inside GetInput: {networkInputData.Position} : of {gameObject.name}");
            //if (NetworkPlayer.Local.Object.HasInputAuthority) return;

            //transform.position = networkInputData.Position;
            //-------------------------------------------------
        }

    }

    void Move(Vector3 position)
    {

    }

    public void SetViewInputVector(Vector2 viewInput)
    {
        this.viewInput = viewInput;
    }
}
