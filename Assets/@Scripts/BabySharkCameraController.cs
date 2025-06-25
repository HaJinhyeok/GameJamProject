using Unity.Cinemachine;
using UnityEngine;

public class BabySharkCameraController : MonoBehaviour
{
    CinemachineCamera _cam;
    public Transform followTarget;
    public Transform lookAtTarget;

    void Start()
    {
        _cam = GetComponent<CinemachineCamera>();
        _cam.Follow = followTarget;
        _cam.LookAt = lookAtTarget;
        //_cam.GetComponent<CinemachineThirdPersonFollow>().CameraDistance = 0f;
    }
}
