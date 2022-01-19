using System;
using System.Collections;
using UnityEngine;

public class FPSMovement : MonoBehaviour, IMovement
{
    [SerializeField] private bool drawGizmos;

    [SerializeField] private float gravityForce = -60f;

    [SerializeField] private float normalSpeed=10f;

    [SerializeField] private bool useSmoothMove=true;

    [SerializeField] private float smoothTime = 0.3f;

    [Header("Jump")]

    [SerializeField] private float jumpForce = 3f;

    [Header("Ground Check")]

    [SerializeField] private RayCheck rayCheck;

    [SerializeField] private float groundCheckRadius=0.2f;

    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private Transform groundCheckPosition;

    [Header("Slope Surfaces")]

    [SerializeField] private float normalSlopeForce = -500f;

    [SerializeField] private float rayCastLenght=1f;

    [Header("Rotation")]
    [SerializeField] private Transform camTransform;

    [Range(0, 1)]
    [SerializeField] private float rotationSpeed;

    [SerializeField] private bool useLocalsRotations=true;

    [Header("Run")]

    [SerializeField] private float runSpeed=17f;

    [SerializeField] private float runningSlopeForce=-800f;

    [SerializeField] private bool stopRunMovingBack=true;

    [SerializeField] private bool useSmoothSpeedTransition=true;

    [SerializeField] private float normalToRunning;

    [SerializeField] private float runningToNormal;

    [Header("Mesh Rotation")]

    [SerializeField] private bool useIndividualMeshRotation=true;

    [SerializeField] private Transform mesh;

    [Range(70,90)]
    [SerializeField] private float maxRotation=80f;

    [Range(0, 1)]
    [SerializeField] private float meshRotationSpeed;

    //Events
    public Action<Vector2> OnNewInput;
    public Action<float> OnMove;
    public Action<bool> OnRun;
    public Action<bool> OnJumping;

    private PlayerState playerState;

    private CharacterController characterController;

    private InputController inputController;

    private Transform _transform;

    private Vector3 lastPosition;

    private Vector2 currentInputDirection = Vector2.zero;

    private Vector2 currentVelocity = Vector2.zero;

    private Vector2 movementValue;

    private RaycastHit hitResult;

    private Coroutine speedTransitionCoroutine;

    private bool canRun=true;

    private bool canJump=true;

    private float jumpTime;

    private float velocityY = 0f;

    private float playerLength;

    private float speed;

    private float slopeForce;

    private float defaltSlopeLimit;

    private float lastSign;

    public enum PlayerState
    {
        ONGROUND, JUMPING, ONAIR
    }

    private void Awake()
    {
        
        speed = normalSpeed;

        slopeForce = normalSlopeForce;

        characterController = GetComponent<CharacterController>();

        playerLength = characterController.height / 2;

        playerState = PlayerState.ONGROUND;
      

        inputController = FindObjectOfType<InputController>();

        inputController.OnMoveEvent += InputController_OnMove;
        //inputController.OnJumpEvent += InputController_OnJump;
        inputController.OnRun += InputController_OnRun;

        _transform = GetComponent<Transform>();
        lastPosition = _transform.position;

        defaltSlopeLimit = characterController.slopeLimit;

        if (camTransform == null)
            camTransform = Camera.main.transform;

    }

    public void FixedUpdate()
    {
        if (stopRunMovingBack && speed==runSpeed)
        {
            if(movementValue.y < 0)
            {
                speed = normalSpeed;

                OnRun?.Invoke(false);
            }
        }

        PlayerMove();

        PlayerRotation();

        if (useIndividualMeshRotation)
        {
            MeshRotation();
        }
    }


    //Movement
    private void PlayerMove()
    {
        lastPosition = _transform.position;

        if (playerState == PlayerState.JUMPING)
        {
            velocityY = Mathf.Sqrt(jumpForce * -2f * gravityForce);

            playerState = PlayerState.ONAIR;

            //event
            OnJumping?.Invoke(true);
        }
        else
        {
            GroundCheck();
        }
      
        Vector3 forward;
        Vector3 rigth;

        if (useSmoothMove)
        {
            currentInputDirection = Vector2.SmoothDamp(currentInputDirection, movementValue, ref currentVelocity, smoothTime);         

            if (currentInputDirection.magnitude < 0.003f)
            {
                //print("Valor baixo");
                currentInputDirection = Vector2.zero;
            }
           
            forward = _transform.forward * currentInputDirection.y;

            rigth = _transform.right * currentInputDirection.x;
        }
        else
        {
            forward = _transform.forward * movementValue.y;

            rigth = _transform.right * movementValue.x;

           
        }
        //print(speed);
        //XZ
        characterController.Move(Vector3.ClampMagnitude(forward + rigth, 1f) * (speed * Time.deltaTime));

        //Y
        characterController.Move(Vector3.up * (velocityY * Time.deltaTime));

        //Event

        OnMove?.Invoke((_transform.position - lastPosition).magnitude);
    }


    //Rotations
    private void PlayerRotation()
    {
        Quaternion targetRotation;

        if (useLocalsRotations)
        {
            targetRotation = Quaternion.Lerp(_transform.localRotation, camTransform.localRotation, rotationSpeed);
            targetRotation.x = 0;
            targetRotation.z = 0;

            _transform.localRotation = targetRotation;
        }
        else
        {
            targetRotation = Quaternion.Lerp(_transform.rotation, camTransform.rotation, rotationSpeed);
            targetRotation.x = 0;
            targetRotation.z = 0;

            _transform.rotation = targetRotation;
        }
        


    }

    private void MeshRotation()
    {
        float rotationValue;
        if (useSmoothMove)
        {

            if (movementValue.y == 0)
            {
                rotationValue = maxRotation * currentInputDirection.x;/** Math.Sign(movementValue.y);*/
            }
            else
            {
                rotationValue = maxRotation * currentInputDirection.x * Math.Sign(movementValue.y);
            }
           
                  
       
            mesh.localRotation = Quaternion.Lerp(mesh.localRotation, Quaternion.Euler(Vector3.up * rotationValue), meshRotationSpeed);
        }
        else
        {
            if (movementValue.y == 0)
            {
                rotationValue = maxRotation * movementValue.x;/** Math.Sign(movementValue.y);*/
            }
            else
            {
                rotationValue = maxRotation * movementValue.x * Math.Sign(movementValue.y);
            }

            mesh.localRotation = Quaternion.Lerp(mesh.localRotation, Quaternion.Euler(Vector3.up * rotationValue), meshRotationSpeed);

        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;

        // no rigidbody
        if (body == null || body.isKinematic)
        {
            return;
        }

        // We dont want to push objects below us
        if (hit.moveDirection.y < -0.3)
        {
            return;
        }

        // Calculate push direction from move direction,
        // we only push objects to the sides never up and down
        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

        // If you know how fast your character is trying to move,
        // then you can also multiply the push velocity by that.

        // Apply the push
        body.velocity = pushDir * (speed/2) ;
    }

    #region Inputs
    public void InputController_OnMove(Vector2 inputValue)
    {
        //Event
        OnNewInput?.Invoke(inputValue);

        movementValue = inputValue;
    }

    public void InputController_OnJump()
    {
       if(playerState == PlayerState.ONGROUND && canJump)
       {
            characterController.slopeLimit = 90;

            playerState = PlayerState.JUMPING;
       }
    }

    public void InputController_OnRun(float inputValue)
    {
        if (canRun)
        {
            if (inputValue > 0 && playerState == PlayerState.ONGROUND)
            {
                //Event
                OnRun?.Invoke(true);

                SetNewSpeed(runSpeed, runningSlopeForce, normalToRunning);

            }
            else
            {
                //Event
                OnRun?.Invoke(false);

                SetNewSpeed(normalSpeed, normalSlopeForce, runningToNormal);

            }
        }
    }

    private void SetNewSpeed(float newSpeed,float newSlopeForce, float time)
    {
        if (useSmoothSpeedTransition)
        {
            if (speedTransitionCoroutine != null)
            {
                StopCoroutine(speedTransitionCoroutine);
            }

            slopeForce = newSlopeForce;

            speedTransitionCoroutine = StartCoroutine(SpeedTransition(newSpeed, time));
        }
        else
        {
            speed = newSpeed;

            slopeForce = newSlopeForce;
        }
    }

    IEnumerator SpeedTransition(float newSpeed, float time)
    {
        float currentSpeed = speed;
        float currentTime = 0;
        float value = 0;
        float addValue = 1 / time;
        float difference = newSpeed-speed;
        do
        {
            currentTime += Time.deltaTime;

            value += addValue * Time.deltaTime;
          
            speed = currentSpeed + (difference * value);

            yield return null;

        } while (currentTime<time);

        speed = newSpeed;

        //print(speed);
    }
    #endregion


    #region Raycats
    private void GroundCheck()
    {
        if (rayCheck.SphereCheck(groundCheckPosition.position,groundCheckRadius,groundLayer))
        {
            //print("OnGround");
            if (playerState != PlayerState.ONGROUND)
            {
                OnJumping?.Invoke(false);
            }

            playerState = PlayerState.ONGROUND;

            characterController.slopeLimit = defaltSlopeLimit;

            if (useSmoothMove)
            {
                if (currentInputDirection.magnitude > 0 && OnSlopeSurface())
                {
                    //Y
                    //print("SlopeSurface");
                    velocityY = playerLength * slopeForce * Time.deltaTime;
                }
                else
                {
                    velocityY = 0;
                }
            }
            else
            {
                if (movementValue.magnitude > 0 && OnSlopeSurface())
                {
                    //Y
                    //print("SlopeSurface");
                    velocityY = playerLength * slopeForce * Time.deltaTime;
                }
                else
                {
                    velocityY = 0;
                }
            }
            
                
        }
        else
        {
            playerState = PlayerState.ONAIR;

            velocityY += gravityForce * Time.deltaTime;

        }
    }

    private bool OnSlopeSurface()
    {    
        if (rayCheck.RayCastCheck(out hitResult,groundCheckPosition.position,Vector3.down,rayCastLenght,groundLayer))
        {
            if (hitResult.normal != Vector3.up)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }

    #endregion


    #region Gets/Sets
    public FPSMovement.PlayerState GetPlayerState()
    {
        return playerState;
    }

    public void EnableMeshRotation()
    {
        useIndividualMeshRotation = true;
    }

    public void DisableMeshRotation(bool disable)
    {
        print("MESH: " + disable);
        useIndividualMeshRotation = !disable;
    }

    public void BackSpeedToNormal()
    {
       speed = normalSpeed;
    }

    public void SetNewSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    public void CanJump(bool canJump)
    {
        this.canJump = canJump;
    }

    public void CanRun(bool canRun)
    {
        this.canRun = canRun;
    }
    #endregion



    private void OnDrawGizmos()
    {
        if (drawGizmos)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(groundCheckPosition.position, groundCheckRadius);

            Gizmos.DrawLine(groundCheckPosition.position, groundCheckPosition.position + Vector3.down * rayCastLenght);
        }
    }
}
