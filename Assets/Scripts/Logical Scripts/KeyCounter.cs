using System;
using UnityEngine;
using UnityEngine.UI;

public class TextCounter : MonoBehaviour
{
    public TextMesh counter;
    [SerializeField] private Door correspondingDoor;

    private void Update()
    {
        counter.text = $"{correspondingDoor.CheckUnlockPower()}/{correspondingDoor.CheckLockPower()}";
        if (correspondingDoor.IsLocked() == false)
        {
            Destroy(gameObject);
        }
    }
}
