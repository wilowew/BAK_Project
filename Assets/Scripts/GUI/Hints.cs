using UnityEngine;
using UnityEngine.UI;

public class Hints : MonoBehaviour
{
    public GameObject tooltipPrefab;
    public Sprite icon;
    public string text;
    public float radius;

    private GameObject tooltipInstance;
    private Image iconImage;
    private Text tooltipText;
    private GameObject player;
    private const float tooltipVerticalOffset = -475f;
    private const float tooltipHorisontalOffset = 100f;
    private Vector2 tooltipSize = new Vector2(200f, 100f);

    private void Start()
    {
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            GameObject canvasGameObject = new GameObject("Canvas");
            canvas = canvasGameObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasGameObject.AddComponent<CanvasScaler>();
            canvasGameObject.AddComponent<GraphicRaycaster>();
        }

        player = GameObject.FindGameObjectWithTag("Player");
        tooltipInstance = Instantiate(tooltipPrefab, canvas.transform);
        tooltipInstance.SetActive(false);

        iconImage = tooltipInstance.GetComponentInChildren<Image>();
        tooltipText = tooltipInstance.GetComponentInChildren<Text>();

        iconImage.sprite = icon;
        tooltipText.text = text;

        RectTransform rectTransform = tooltipInstance.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(tooltipHorisontalOffset, tooltipVerticalOffset);
        rectTransform.sizeDelta = new Vector2(300f, 150f);

        tooltipInstance.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
    }

    private void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.transform.position);

        if (distance <= radius)
        {
            tooltipInstance.SetActive(true);
        }
        else
        {
            tooltipInstance.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        if (tooltipInstance != null)
        {
            Destroy(tooltipInstance);
        }
    }
}