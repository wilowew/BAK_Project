using UnityEngine;
using UnityEngine.UI;

public class Hints : MonoBehaviour
{
    public GameObject tooltipPrefab; 
    public Sprite icon;
    public string text;
    public float radius;

    [SerializeField]
    private Canvas canvas;
    private GameObject tooltipInstance;
    private Image iconImage;
    private Text tooltipText;

    public GameObject player;

    private void Start()
    {
        tooltipInstance = Instantiate(tooltipPrefab, canvas.transform);
        tooltipInstance.SetActive(false);

        iconImage = tooltipInstance.GetComponentInChildren<Image>();
        tooltipText = tooltipInstance.GetComponentInChildren<Text>();

        iconImage.sprite = icon;
        tooltipText.text = text;
    }

    private void Update()
    {
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
