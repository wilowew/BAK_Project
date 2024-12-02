using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapLayerSwitch : MonoBehaviour
{
    [Header("Tilemap Layer Settings")]
    [SerializeField] private List<Tilemap> layersToShow;
    [SerializeField] private List<Tilemap> layersToHide;

    [Header("Additional Settings")]
    [SerializeField] private GameObject additionalCollider;
    [SerializeField] private GameObject additionalBlackScreen;
    [SerializeField] private GameObject additionalCamera;

    private CameraTrigger cameraScript; // Ссылка на скрипт движения камеры

    private void Awake()
    {
        // Проверка, есть ли в объекте additionalCamera компонент CameraTrigger
        if (additionalCamera != null)
        {
            cameraScript = additionalCamera.GetComponent<CameraTrigger>();
            if (cameraScript == null)
            {
                Debug.LogWarning("CameraTrigger script not found on the additionalCamera object.");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            // Если есть скрипт камеры и камера еще не достигла нужной позиции
            if (cameraScript != null)
            {
                StartCoroutine(WaitForCameraToReachPosition());
            }
            else
            {
                // Если скрипт камеры отсутствует, выполняем смену слоев сразу
                ChangeLayers();
            }
        }
    }

    private IEnumerator WaitForCameraToReachPosition()
    {
        // Ждем, пока камера не достигнет нужной позиции
        while (cameraScript != null && !cameraScript.AtPosition())
        {
            yield return null; // Ожидание следующего кадра
        }

        // Камера достигла нужной позиции, выполняем смену слоев
        ChangeLayers();
    }

    private void ChangeLayers()
    {
        foreach (var layer in layersToShow)
        {
            if (layer != null)
            {
                layer.gameObject.GetComponent<Tilemap>().color = new Color(1, 1, 1, 1);
            }
        }

        foreach (var layer in layersToHide)
        {
            if (layer != null)
            {
                layer.gameObject.GetComponent<Tilemap>().color = new Color(1, 1, 1, 0);
            }
        }

        if (additionalCollider != null)
        {
            Destroy(additionalCollider);
        }

        if (additionalBlackScreen != null)
        {
            Destroy(additionalBlackScreen);
        }
    }
}