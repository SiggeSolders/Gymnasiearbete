using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Camera cam;
    private  CameraHolder camHolder;
    void Start()
    {
        cam = GetComponentInChildren<Camera>();
        camHolder = GetComponentInChildren<CameraHolder>();
    }

    // Update is called once per frame
    void RotateCamera()
    {

    }
}
