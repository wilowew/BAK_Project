using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] private int _health = 10;
    [SerializeField] private List<GameObject> _lootTable; 
    [SerializeField] private Transform _lootSpawnPoint;
    [SerializeField] private int _lootChance = 10;
    [SerializeField] private int _multiplicator = 1;

    private bool _lootSpawned = false;

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
        if (!_lootSpawned)
        {
            int randomChance = Random.Range(0, _lootChance);
            Debug.Log(randomChance);
            if (randomChance <= _lootChance / _multiplicator || randomChance == 0)
            {
                int randomIndex = Random.Range(0, _lootTable.Count);

                GameObject lootItem = Instantiate(_lootTable[randomIndex], _lootSpawnPoint.position, Quaternion.identity);

                _lootSpawned = true; 
            }

            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Sword>(out Sword sword))
        {
            TakeDamage(sword.GetDamageAmount());
        }
        if (collision.TryGetComponent<MagicProjectile>(out MagicProjectile magicProjectile))
        {
            TakeDamage(magicProjectile.GetDamageAmount());
        }
    }

}
