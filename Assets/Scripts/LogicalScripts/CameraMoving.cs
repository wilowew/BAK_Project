using UnityEngine;
public class CameraTrigger : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] private Camera targetCamera; 
    [SerializeField] private Vector2 targetPosition; 
    [SerializeField] private float transitionSpeed = 2.0f; 
    [SerializeField] private float returnDelay = 2.0f; 
    [Header("Player Settings")]
    [SerializeField] private GameObject player;
    private MonoBehaviour cameraFollowScript;
    private bool isReturning = false;

    private void Awake()
    {
        if (targetCamera == null)
        {
            Debug.LogError("Camera not assigned in CameraTrigger!");
        }
        if (player == null)
        {
            Debug.LogError("Player GameObject not assigned in CameraTrigger!");
        }
        cameraFollowScript = targetCamera.GetComponent<MonoBehaviour>();
        if (cameraFollowScript == null)
        {
            Debug.LogWarning("Follow script not found on the target camera.");
        }
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject == player && !isReturning)
        {
            if (targetCamera == null)
            {
                Debug.LogError("Camera is null when trying to move it!");
                return;
            }

            if (cameraFollowScript != null)
            {
                cameraFollowScript.enabled = false;
            }

            StopAllCoroutines();
            Debug.Log("Starting camera movement...");
            Vector3 targetPos = new Vector3(targetPosition.x, targetPosition.y, targetCamera.transform.position.z);
            StartCoroutine(MoveCameraToPosition(targetPos, () =>
            {
                Debug.Log("Camera reached target position.");
                StartCoroutine(ReturnCameraAfterDelay());
            }));
        }
    }
    private System.Collections.IEnumerator MoveCameraToPosition(Vector3 position, System.Action onComplete = null)
    {
        while ((targetCamera.transform.position - position).sqrMagnitude > 0.001f) 
        {
            targetCamera.transform.position = Vector3.MoveTowards(targetCamera.transform.position, position, transitionSpeed * Time.deltaTime);
            yield return null;
        }
        targetCamera.transform.position = position; 
        Debug.Log("Camera position set to target.");
        onComplete?.Invoke();
    }
    private System.Collections.IEnumerator ReturnCameraAfterDelay()
    {
        Debug.Log("Waiting for return delay...");
        yield return new WaitForSeconds(returnDelay);
        if (targetCamera == null || player == null)
        {
            Debug.LogError("Camera or player is null during return!");
            yield break;
        }
        isReturning = true;
        Debug.Log("Returning camera to player.");
        Vector3 playerPosition = new Vector3(player.transform.position.x, player.transform.position.y + 2, targetCamera.transform.position.z);
        while ((targetCamera.transform.position - playerPosition).sqrMagnitude > 0.001f) 
        {
            targetCamera.transform.position = Vector3.MoveTowards(targetCamera.transform.position, playerPosition, transitionSpeed * Time.deltaTime);
            playerPosition = new Vector3(player.transform.position.x, player.transform.position.y + 2, targetCamera.transform.position.z);
            yield return null;
        }
        targetCamera.transform.position = playerPosition; 
        isReturning = false;
        
        if (cameraFollowScript != null)
        {
            cameraFollowScript.enabled = true;
        }
        Debug.Log("Calling DestroyTrigger...");
        DestroyTrigger(); 
    }
    private void DestroyTrigger()
    {
        Debug.Log("Destroying object...");
        Destroy(gameObject); 
    }
}