using UnityEngine;

public class CameraTrigger : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] private Camera targetCamera; // Ссылка на камеру
    [SerializeField] private Vector2 targetPosition; // Заданные координаты для перемещения камеры
    [SerializeField] private float transitionSpeed = 2.0f; // Скорость перемещения камеры
    [SerializeField] private float returnDelay = 2.0f; // Задержка перед возвратом камеры к игроку

    [Header("Player Settings")]
    [SerializeField] private GameObject player; // Ссылка на объект игрока

    private MonoBehaviour cameraFollowScript; // Универсальная ссылка на скрипт следования камеры
    private bool isReturning = false; // Флаг возврата камеры

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

        // Найти скрипт следования камеры
        cameraFollowScript = targetCamera.GetComponent<MonoBehaviour>();
        if (cameraFollowScript == null)
        {
            Debug.LogWarning("Follow script not found on the target camera.");
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject == player)
        {
            if (targetCamera == null)
            {
                Debug.LogError("Camera is null when trying to move it!");
                return;
            }

            // Отключаем скрипт следования камеры, если он найден
            if (cameraFollowScript != null)
            {
                cameraFollowScript.enabled = false;
            }

            // Прерываем все корутины и начинаем перемещение камеры
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
        while ((targetCamera.transform.position - position).sqrMagnitude > 0.001f) // Используем квадрат расстояния
        {
            targetCamera.transform.position = Vector3.MoveTowards(targetCamera.transform.position, position, transitionSpeed * Time.deltaTime);
            yield return null;
        }

        targetCamera.transform.position = position; // Убедимся, что камера точно достигла цели
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

        while ((targetCamera.transform.position - playerPosition).sqrMagnitude > 0.001f) // Используем квадрат расстояния
        {
            targetCamera.transform.position = Vector3.MoveTowards(targetCamera.transform.position, playerPosition, transitionSpeed * Time.deltaTime);
            playerPosition = new Vector3(player.transform.position.x, player.transform.position.y + 2, targetCamera.transform.position.z);
            yield return null;
        }

        targetCamera.transform.position = playerPosition; // Убедимся, что камера точно достигла позиции игрока
        isReturning = false;

        // Включаем скрипт следования камеры, если он найден
        if (cameraFollowScript != null)
        {
            cameraFollowScript.enabled = true;
        }

        Debug.Log("Calling DestroyTrigger...");
        DestroyTrigger(); // Вызываем метод для уничтожения
    }

    private void DestroyTrigger()
    {
        Debug.Log("Destroying object...");
        Destroy(gameObject); // Уничтожаем текущий объект
    }
}