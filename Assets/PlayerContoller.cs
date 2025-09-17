using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerContoller : MonoBehaviour
{
    #region Variables

    private CharacterController controller;
    private Vector2 move;
    [HideInInspector] public Vector3 moveDirection;
    private Hip hip;
    [SerializeField] private int moveSpeed;
    #endregion

    private void Awake()
    {
        hip = GetComponentInChildren<Hip>();
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
        moveDirection.x = move.x;
        moveDirection.z = move.y;

        Vector3 targetPosition = new Vector3(moveDirection.x, hip.transform.localPosition.y, moveDirection.z);
        hip.transform.localPosition = Vector3.MoveTowards(hip.transform.localPosition, targetPosition, moveSpeed * Time.deltaTime);
    }

    #endregion
}
