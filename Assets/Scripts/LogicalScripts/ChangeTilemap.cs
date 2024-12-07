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

    private CameraTrigger cameraScript;

    private void Awake()
    {

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
            if (cameraScript != null)
            {
                StartCoroutine(WaitForCameraToReachPosition());
            }
            else
            {
                ChangeLayers();
            }
        }
    }

    private IEnumerator WaitForCameraToReachPosition()
    {
        while (cameraScript != null && !cameraScript.AtPosition())
        {
            yield return null;
        }

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