using UnityEngine;
using System.Collections;

public class Grib : MonoBehaviour 
{
    public float yLift = 2.1f;
    public string gribTag = "grib1"; 
    private bool isBouncing;
    private bool hasBounced;
    private Transform grib1;

    private void Start() 
    {
        hasBounced = false;
    }

    private void OnCollisionEnter2D(Collision2D collision) 
    {
        if (collision.gameObject.CompareTag("Player") && !isBouncing && !hasBounced) 
        {
            foreach (ContactPoint2D contact in collision.contacts) 
            {
                if (contact.normal.y > 0.5f) 
                {
                    isBouncing = true;
                    hasBounced = true;
                    grib1 = GameObject.FindGameObjectWithTag(gribTag).transform;
                    if (grib1 != null) 
                    {
                        StartCoroutine(MoveAndEnable(grib1));
                    }
                }
            }
        }
    }

    private IEnumerator MoveAndEnable(Transform grib1) 
    {
        Vector3 targetPositionY = grib1.position + new Vector3(0, yLift, 0);
        float elapsedTime = 0f;
        float duration = 1f;

        while (elapsedTime < duration) 
        {
            grib1.position = Vector3.Lerp(grib1.position, targetPositionY, (elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        grib1.position = targetPositionY;

        GribMovement gribMovement = grib1.GetComponent<GribMovement>();
        if (gribMovement != null) 
        {
            gribMovement.EnableMovement();
        }
    }
}
