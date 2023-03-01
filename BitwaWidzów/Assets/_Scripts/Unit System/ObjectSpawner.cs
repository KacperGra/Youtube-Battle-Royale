using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Battle
{
    [Serializable]
    public class ItemToSpawnWrapper
    {
        [SerializeField] private float _chance = 1f;
        [SerializeField] private GameObject _prefab;

        public GameObject Prefab => _prefab;

        public bool IsAbleToSpawn()
        {
            float randomValue = UnityEngine.Random.value;
            return randomValue < _chance;
        }
    }

    public class ObjectSpawner : MonoBehaviour
    {
        [Header("Unit")]
        [SerializeField] private UnitManager _unityManager;
        [SerializeField] private Unit _unitPrefab;
        [SerializeField] private TextAsset _nicknamesText;
        [SerializeField] private int _spawnRange = 100;

        [Header("Items")]
        [SerializeField] private List<ItemToSpawnWrapper> _items;
        [SerializeField] private float _itemSpawnTime = 1f;

        private void Start()
        {
            string text = _nicknamesText.text;
            List<string> _nicknames = text.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).ToList();

            for (int i = 0; i < _nicknames.Count; ++i)
            {
                if (string.IsNullOrWhiteSpace(_nicknames[i]))
                {
                    continue;
                }

                int spawnX = UnityEngine.Random.Range(-_spawnRange, _spawnRange);
                int spawnY = UnityEngine.Random.Range(-_spawnRange, _spawnRange);

                Vector3 randomPosition = new(spawnX, 2, spawnY);
                if (!IsAbleToSpawnAtPosition(randomPosition))
                {
                    --i;
                    continue;
                }

                var unit = Instantiate(_unitPrefab, randomPosition, Quaternion.identity);
                unit.Nickname = _nicknames[i];
                _unityManager.AddUnit(unit);
            }

            StartCoroutine(SpawnRandomItem());
        }

        private IEnumerator SpawnRandomItem()
        {
            while (true)
            {
                yield return new WaitForSeconds(_itemSpawnTime);

                int randomIndex = UnityEngine.Random.Range(0, _items.Count);
                var randomItem = _items[randomIndex];
                if (!randomItem.IsAbleToSpawn())
                {
                    continue;
                }

                Instantiate(randomItem.Prefab, GetPositionForItem(), Quaternion.identity);
            }
        }

        private bool IsAbleToSpawnAtPosition(Vector3 position, bool isItem = false)
        {
            if (isItem && GameManager.Instance.Zone.IsPositionInZone(position))
            {
                return false;
            }

            Collider[] colliders = Physics.OverlapSphere(position, 1f);
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
                if (!IsAbleToSpawnAtPosition(randomPosition, true))
                {
                    continue;
                }

                return randomPosition;
            }
        }
    }
}