using UnityEngine;

public class Walking_Target : MonoBehaviour
{
    private Hip hip;
    [SerializeField] private float maxRaycastDistance = 10f; // Maximum raycast distance

    void Start()
    {
        // Find the Hip component in parent objects
        hip = GetComponentInParent<Hip>();
        if (hip == null)
        {
            Debug.LogError("Hip component not found in parent objects!");
        }
    }

    void Update()
    {
        if (hip == null) return;

        // ============ RAYCAST ALL TO CONTINUE THROUGH IGNORED OBJECTS ============
        RaycastHit[] hits = Physics.RaycastAll(hip.transform.position, Vector3.down, maxRaycastDistance);

        // Find the first valid hit (not tagged with ignoreTag)
        RaycastHit validHit = new RaycastHit();
        bool foundValidHit = false;

        foreach (RaycastHit hit in hits)
        {
            // Optional: Skip objects that are children of same parent
            if (IsChildOfSameParent(hit.transform))
            {
                continue; // Skip this hit and check the next one
            }

            // Found a valid hit - use the closest one
            if (!foundValidHit || hit.distance < validHit.distance)
            {
                validHit = hit;
                foundValidHit = true;
            }
        }

        // Apply the valid hit if found
        if (foundValidHit)
        {
            transform.position = new Vector3(transform.position.x, validHit.point.y, transform.position.z);
        }
        // =========================================================================
    }

    private bool IsChildOfSameParent(Transform hitTransform)
    {
        // Check if the hit object shares the same root parent
        Transform rootParent = GetRootParent(transform);
        Transform hitRootParent = GetRootParent(hitTransform);
        return rootParent == hitRootParent;
    }

    private Transform GetRootParent(Transform child)
    {
        Transform current = child;
        while (current.parent != null)
        {
            current = current.parent;
        }
        return current;
    }
}