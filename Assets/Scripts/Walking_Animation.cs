using UnityEngine;
using System.Collections;

public class Walking_Animation : MonoBehaviour
{
    [SerializeField] private GameObject L_Foot;
    [SerializeField] private GameObject R_Foot;
    [SerializeField] private GameObject L_TargetPoint;
    [SerializeField] private GameObject R_TargetPoint;

    [Header("Animation Settings")]
    [SerializeField] private float stepDuration = 0.3f;
    [SerializeField] private float stepHeight = 0.5f;
    [SerializeField] private float stepDistance = 0.5f;
    [SerializeField] private AnimationCurve stepCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [SerializeField] private AnimationCurve heightCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(0.5f, 1), new Keyframe(1, 0));

    private Foot L_Foot_;
    private Foot R_Foot_;
    private bool L_isMoving = false;
    private bool R_isMoving = false;

    private void Start()
    {
        L_Foot_ = L_Foot.GetComponent<Foot>();
        R_Foot_ = R_Foot.GetComponent<Foot>();
    }

    void Update()
    {
        CheckFootMovement(L_Foot, L_TargetPoint, R_Foot_, L_isMoving);
        CheckFootMovement(R_Foot, R_TargetPoint, L_Foot_, R_isMoving);
    }

    private void CheckFootMovement(GameObject foot, GameObject targetPoint, Foot otherFoot, bool isMoving)
    {
        float distance = Vector3.Distance(foot.transform.position, targetPoint.transform.position);

        if (distance > stepDistance && otherFoot.isGrounded && !isMoving)
        {
            if (foot == L_Foot)
                StartCoroutine(MoveFoot(foot, targetPoint, true));
            else
                StartCoroutine(MoveFoot(foot, targetPoint, false));
        }
    }

    private IEnumerator MoveFoot(GameObject foot, GameObject targetPoint, bool isLeftFoot)
    {
        // Set the appropriate moving flag
        if (isLeftFoot)
            L_isMoving = true;
        else
            R_isMoving = true;

        Vector3 startPosition = foot.transform.position;
        Vector3 targetPosition = targetPoint.transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < stepDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / stepDuration;

            // Apply animation curve for horizontal movement
            float curveValue = stepCurve.Evaluate(progress);
            Vector3 horizontalPosition = Vector3.Lerp(startPosition, targetPosition, curveValue);

            // Apply height curve for vertical lift
            float heightValue = heightCurve.Evaluate(progress);
            float currentHeight = Mathf.Max(startPosition.y, targetPosition.y) + (heightValue * stepHeight);

            foot.transform.position = new Vector3(horizontalPosition.x, currentHeight, horizontalPosition.z);

            yield return null;
        }

        // Ensure final position is exact
        foot.transform.position = targetPosition;

        // Clear the appropriate moving flag
        if (isLeftFoot)
            L_isMoving = false;
        else
            R_isMoving = false;
    }
}