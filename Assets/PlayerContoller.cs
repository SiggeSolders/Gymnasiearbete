using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerContoller : MonoBehaviour
{
    #region Variables

    private Vector2 move;
    [HideInInspector] public Vector3 moveDirection;
    private Hip hip;
    private Movement_Direction movementDirection;
    [SerializeField] private int moveSpeed;
    #endregion

    private void Awake()
    {
        hip = GetComponentInChildren<Hip>();
        movementDirection = GetComponentInChildren<Movement_Direction>();
    }
    private void Update()
    {
        Move();
    }
    public void OnTopDownMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }

    #region PlayerMove

    public void Move()
    {
        moveDirection = Vector3.zero;
        moveDirection.x = move.x * 3;
        moveDirection.z = move.y * 3;

        Vector3 targetPosition = new Vector3(moveDirection.x, movementDirection.transform.localPosition.y, moveDirection.z);
        movementDirection.transform.localPosition = Vector3.MoveTowards(movementDirection.transform.localPosition, targetPosition, moveSpeed * Time.deltaTime);
    }

    #endregion
}
