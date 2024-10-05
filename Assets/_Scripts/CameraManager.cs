using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform targetTransform;
    public Transform cameraPivot;
    private Vector3 cameraFollowVelocty = Vector3.zero;

    public float cameraFollowSpeed = 0.2f;
    public float cameraLookSpeed = 2;
    public float cameraPivotSpeed = 2;

    public float minPivotAngle = -35f;
    public float maxPivotAngle = 35;

    public float lookAngle;
    public float pivotAngle;

    public void HandleAllCameraMovement()
    {
        FollowTarget();
        RotateCamera();
    }

    private void FollowTarget()
    {
        Vector3 targetPosition = Vector3.SmoothDamp(transform.position, targetTransform.position, ref cameraFollowVelocty, cameraFollowSpeed);

        transform.position = targetPosition;
    }

    private void RotateCamera()
    {
        lookAngle = lookAngle + (Input.GetAxis("Mouse X") * cameraLookSpeed);
        pivotAngle = pivotAngle - (Input.GetAxis("Mouse Y") * cameraPivotSpeed);
        pivotAngle = Mathf.Clamp(pivotAngle, minPivotAngle, maxPivotAngle);

        Vector3 rotation = Vector3.zero;
        rotation.y = lookAngle;
        Quaternion targetRotation = Quaternion.Euler(rotation);
        transform.rotation = targetRotation;

        rotation = Vector3.zero;
        rotation.x = pivotAngle;
        targetRotation = Quaternion.Euler(rotation);
        cameraPivot.localRotation = targetRotation;
    }
}
