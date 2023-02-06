using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public class ObjectSpawner : MonoBehaviour
    {
        [Header("Unit")]
        [SerializeField] private UnitManager _unityManager;
        [SerializeField] private Unit _unitPrefab;
        [SerializeField] private int _unitAmount = 100;
        [SerializeField] private int _spawnRange = 100;

        [Header("Items")]
        [SerializeField] private List<GameObject> _items;
        [SerializeField] private float _itemSpawnTime = 1f;

        private void Awake()
        {
            for (int i = 0; i < _unitAmount; ++i)
            {
                int spawnX = UnityEngine.Random.Range(-_spawnRange, _spawnRange);
                int spawnY = UnityEngine.Random.Range(-_spawnRange, _spawnRange);

                Vector3 randomPosition = new Vector3(spawnX, 2, spawnY);
                if (!IsAbleToSpawnAtPosition(randomPosition))
                {
                    --i;
                    continue;
                }

                var unit = Instantiate(_unitPrefab, randomPosition, Quaternion.identity);
                _unityManager.AddUnit(unit);
            }
        }

        private void Start()
        {
            StartCoroutine(SpawnRandomItem());
        }

        private IEnumerator SpawnRandomItem()
        {
            while (true)
            {
                yield return new WaitForSeconds(_itemSpawnTime);

                int randomIndex = UnityEngine.Random.Range(0, _items.Count);
                var randomItem = _items[randomIndex];

                Instantiate(randomItem, GetPositionForItem(), Quaternion.identity);
            }
        }

        private bool IsAbleToSpawnAtPosition(Vector3 position)
        {
            var colliders = Physics.OverlapSphere(position, 1f);
            foreach (var collider in colliders)
            {
                if (collider.CompareTag("Obstacle"))
                {
                    return false;
                }
            }

            return true;
        }

        private Vector3 GetPositionForItem()
        {
            while (true)
            {
                int spawnX = UnityEngine.Random.Range(-_spawnRange, _spawnRange);
                int spawnY = UnityEngine.Random.Range(-_spawnRange, _spawnRange);

                Vector3 randomPosition = new Vector3(spawnX, 1, spawnY);
                if (!IsAbleToSpawnAtPosition(randomPosition))
                {
                    continue;
                }

                return randomPosition;
            }
        }
    }
}