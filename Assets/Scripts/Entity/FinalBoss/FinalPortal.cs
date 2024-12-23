using UnityEngine;

public class ColorCycling : MonoBehaviour
{
    [Header("Color Cycling Settings")]
    [SerializeField] private float colorChangeSpeed = 1f;
    private SpriteRenderer spriteRenderer;
    private CapsuleCollider2D capsuleCollider;
    private bool isBossAlive = true;

    private readonly Color initialColor = Color.HSVToRGB(121f / 360f, 38f / 100f, 45f / 100f);

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        capsuleCollider = gameObject.AddComponent<CapsuleCollider2D>();
        capsuleCollider.isTrigger = true;
        capsuleCollider.size = new Vector2(1f, 2f);
        capsuleCollider.enabled = false; 
    }

    void Start()
    {
        spriteRenderer.color = initialColor;
    }

    void Update()
    {
        bool bossExists = GameObject.FindWithTag("Muhomor") != null;

        if (!bossExists && isBossAlive)
        {
            isBossAlive = false;

            capsuleCollider.enabled = true;
            StartCoroutine(CycleColor());
        }
        else if (bossExists && !isBossAlive)
        {
            isBossAlive = true;

            capsuleCollider.enabled = false;
            StopCoroutine(nameof(CycleColor));
            spriteRenderer.color = initialColor;
        }
    }

    private System.Collections.IEnumerator CycleColor()
    {
        float hue = initialColor.r;

        while (!isBossAlive)
        {
            Color color = Color.HSVToRGB(hue, 1f, 1f);
            spriteRenderer.color = color;

            hue += Time.deltaTime * colorChangeSpeed;
            if (hue > 1f) hue = 0f;

            yield return null;
        }
    }
}
