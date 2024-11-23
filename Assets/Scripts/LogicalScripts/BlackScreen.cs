using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackScreen : MonoBehaviour
{
    [SerializeField] private Door door;

    private void Update()
    {
        if (!door.IsLocked()) 
        {
            Collider2D[] doorColliders = door.GetComponents<Collider2D>();
            bool allCollidersDisabled = true;

            foreach (Collider2D col in doorColliders)
            {
                if (col.enabled)
                {
                    allCollidersDisabled = false;
                    break;
                }
            }

            if (allCollidersDisabled)
            {
                Destroy(gameObject); 
            }
        }
    }
}