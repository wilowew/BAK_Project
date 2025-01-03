using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveWeapon : MonoBehaviour
{
    public static ActiveWeapon Instance { get; private set; }
    [SerializeField] private Sword sword;

    private bool isSwordActive = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        isSwordActive = false;
        sword.SetActive(isSwordActive);
    }

    private void Update()
    {
        if (Player.Instance.IsAlive() && !PauseMenu._isPaused)
        {
            FollowMousePosition();
            if (Input.GetKeyDown(KeyCode.E))
            {
                isSwordActive = !isSwordActive;
                sword.SetActive(isSwordActive); 

                if (isSwordActive)
                {
                    sword.ResetAttack();
                }
            }

            if (isSwordActive && Input.GetMouseButtonDown(0))
            {
                sword.Attack();
            }
        }
    }

    public Sword GetActiveWeapon()
    {
        return sword;
    }

    private void FollowMousePosition()
    {
        if (PauseMenu._isPaused) return;

        Vector3 mousePos = GameInput.Instance.GetMousePosition();
        Vector3 playerPosition = Player.Instance.GetPlayerScreenPosition();

        if (mousePos.x < playerPosition.x)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}