using UnityEngine;

public class CameraFollow : MonoBehaviour {
    [SerializeField] private Transform target; 
    [SerializeField] private float smoothSpeed = 0.125f; 
    [SerializeField] private Vector3 offset; 

    [SerializeField] private Vector2 minPosition; 
    [SerializeField] private Vector2 maxPosition; 

    private void LateUpdate() {
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        smoothedPosition.x = Mathf.Clamp(smoothedPosition.x, minPosition.x, maxPosition.x);
        smoothedPosition.y = Mathf.Clamp(smoothedPosition.y, minPosition.y, maxPosition.y);

        transform.position = smoothedPosition;
        }
    }
