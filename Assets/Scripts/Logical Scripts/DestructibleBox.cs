using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] private int _health = 10;
    [SerializeField] private List<GameObject> _lootTable; 
    [SerializeField] private Transform _lootSpawnPoint;

    public void TakeDamage(int damageAmount)
    {
        _health -= damageAmount;

        if (_health <= 0)
        {
            DestroyBox();
        }
    }

    private void DestroyBox()
    {
        int randomIndex = Random.Range(0, _lootTable.Count);

        GameObject lootItem = Instantiate(_lootTable[randomIndex], _lootSpawnPoint.position, Quaternion.identity);
        lootItem.GetComponent<ItemPickup>().itemType = (ItemPickup.ItemType)randomIndex;

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Sword>(out Sword sword))
        {
            TakeDamage(sword.GetDamageAmount());
        }
    }
}
