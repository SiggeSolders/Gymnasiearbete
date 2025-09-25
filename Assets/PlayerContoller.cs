using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerContoller : MonoBehaviour
{
    #region Variables

    [Header("Inputs")]
    private Vector2 move;
    private Vector2 cameraRotation;
    private bool leftMouse;
    private bool rightMouse;

    [HideInInspector] public Vector3 moveDirection;
    private Hip hip;
    private ConfigurableJoint hipJoint;
    private Movement_Direction movementDirection;
    [SerializeField] private int moveSpeed;
    private RagdollPart grabDirection_R;
    private RagdollPart grabDirection_L;
    private Hand leftHand;
    private Hand rightHand;

    [Header("Camera")]
    private CameraHolder camHolder;
    private float yRotation = 0;
    private float xRotation = 0;
    public float xSens = 30f;
    public float ySens = 30f;
    #endregion

    private void Awake()
    {
        hip = GetComponentInChildren<Hip>();
        hipJoint = hip.GetComponent<ConfigurableJoint>();
        movementDirection = GetComponentInChildren<Movement_Direction>();
        camHolder = GetComponentInChildren<CameraHolder>();
        Cursor.lockState = CursorLockMode.Locked;
        grabDirection_L = GetComponentInChildren<RagdollPart>();
        grabDirection_R = GetComponentInChildren<RagdollPart>();
        leftHand = GetComponentInChildren<Hand>();
        rightHand = GetComponentInChildren<Hand>();

    }
    private void Update()
    {
        Move();
        RotateCamera();
        if(leftMouse) { Grab(grabDirection_L.gameObject, leftHand.gameObject); }
        if(rightMouse) { Grab(grabDirection_R.gameObject, rightHand.gameObject); }

    }

    #region Inputs
    public void OnTopDownMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }
    public void OnCamera(InputAction.CallbackContext context)
    {
        cameraRotation = context.ReadValue<Vector2>();
    }
    public void OnLeftClick(InputAction.CallbackContext context)
    {
        if (context.ReadValue<float>() < 1) { leftMouse = false; }
    }
    public void OnRightClick(InputAction.CallbackContext context)
    {
        if (context.ReadValue<float>() < 1) { rightMouse = false; }
    }

    #endregion

    #region PlayerMove

    public void Move()
    {
        moveDirection = Vector3.zero;
        moveDirection.x = move.x * 3;
        moveDirection.z = move.y * 3;

        Vector3 targetPosition = new Vector3(moveDirection.x, movementDirection.transform.localPosition.y, moveDirection.z);
        movementDirection.transform.localPosition = Vector3.MoveTowards(movementDirection.transform.localPosition, targetPosition, moveSpeed * Time.deltaTime);
    }

    public void RotateCamera()
    {
        xRotation -= (cameraRotation.x * Time.deltaTime) * xSens;
        yRotation -= (cameraRotation.y * Time.deltaTime) * xSens;
        yRotation = Mathf.Clamp(yRotation, -80, 80);

        hipJoint.targetRotation = Quaternion.Euler(0, xRotation, 0);

        camHolder.gameObject.transform.rotation = Quaternion.Euler(0, -xRotation + 90, -yRotation);
    }

    public void Grab(GameObject grabTarget, GameObject hand)
    {
        Vector3 direction = grabTarget.gameObject.transform.position - transform.position;
        direction.Normalize();
        Rigidbody rb = hand.GetComponent<Rigidbody>();

        // Apply the force towards the target
        rb.AddForce(direction * 100);
    }

    #endregion
}
