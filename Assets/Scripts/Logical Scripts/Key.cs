using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    [SerializeField] private Door correspondingDoor; // ������ �� �����, ������� ��������� ����

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            correspondingDoor.UnlockDoor(); // ������������ �����
            Debug.Log("Key collected, door unlocked!");
            Destroy(gameObject); // ���������� ���� ����� �������
        }
    }
}