using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] private int _health = 10;
    [SerializeField] private List<GameObject> _lootTable; 
    [SerializeField] private Transform _lootSpawnPoint;
    [SerializeField] private int _lootChance = 10;

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
        int randomChance = Random.Range(0, _lootChance);
        Debug.Log(randomChance);
        if (randomChance < _lootChance / 2 || randomChance == 0)
        {
            int randomIndex = Random.Range(0, _lootTable.Count);

            GameObject lootItem = Instantiate(_lootTable[randomIndex], _lootSpawnPoint.position, Quaternion.identity);
            lootItem.GetComponent<ItemPickup>().itemType = (ItemPickup.ItemType)randomIndex;

            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Sword>(out Sword sword))
        {
            TakeDamage(sword.GetDamageAmount());
        }
    }

}
