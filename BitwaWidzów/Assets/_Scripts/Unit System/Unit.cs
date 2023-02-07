using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Battle
{
    public class Unit : MonoBehaviour
    {
        [Header("General")]
        [SerializeField] private UnitAI _unitAI;
        [SerializeField] private UnitUI _unityUI;
        [SerializeField] private UnitEquipment _equipment;
        [SerializeField] private Animator _animator;
        [SerializeField] private MeshRenderer _bodyMesh;
        [SerializeField] private MeshRenderer _handMesh;

        [Header("Stats")]
        [SerializeField] private int _health = 100;
        [SerializeField] private int _armorPerLevel = 1;
        [SerializeField] private float _speedPerLevel = 0.2f;
        [SerializeField] private int _healthRegeneration = 1;

        [Header("Materials")]
        [SerializeField] private float _damageMaterialTime = 0.1f;
        [SerializeField] private Material _defaultMaterial;
        [SerializeField] private Material _damageMaterial;

        private Coroutine _materialCoroutine;
        private HealthSystem _healthSystem;
        private string _nickname;
        private readonly LevelSystem _levelSystem = new();
        private readonly LeaderboardStatistics _leaderboardStatistics = new();

        public string Nickname
        {
            get => _nickname;
            set
            {
                _nickname = value;
            }
        }

        public float Speed => _speedPerLevel * (_levelSystem.Level - 1);

        public UnitAI AI => _unitAI;
        public UnitEquipment Equipment => _equipment;
        public HealthSystem HealthSystem => _healthSystem;
        public LevelSystem LevelSystem => _levelSystem;
        public LeaderboardStatistics LeaderboardStatistics => _leaderboardStatistics;
        public Animator Animator => _animator;

        private void Start()
        {
            _healthSystem = new HealthSystem(_health);
            _unitAI.Initialize(this);

            HandleLevelChanged();
            StartCoroutine(RegenerateHealth());

            _healthSystem.OnHealthChanged += HandleHealthChanged;
            _levelSystem.OnLevelChanged += HandleLevelChanged;
            _unitAI.OnTargetKilled += HandleTargetKilled;
            _equipment.OnArmorEquiped += OnArmorEquiped;

            _unityUI.Initialize(this);
        }

        private void Update()
        {
            var cameraPosition = Camera.main.transform.position;
            float distance = Vector3.Distance(cameraPosition, transform.position);

            _unityUI.gameObject.SetActive(distance < 30f);
        }

        private void OnArmorEquiped()
        {
            _healthSystem.MaxHealth = CalculateMaxHealth();
        }

        private void HandleLevelChanged()
        {
            _healthSystem.MaxHealth = CalculateMaxHealth();
            _healthSystem.Health = _healthSystem.MaxHealth;

            _unitAI.CurrentDamage = _unitAI.BaseDamage * _levelSystem.Level;
        }

        private void HandleHealthChanged()
        {
        }

        private void HandleTargetKilled(int exp)
        {
            _levelSystem.Exp += exp;
        }

        private IEnumerator SetDefaultMaterial()
        {
            yield return new WaitForSeconds(_damageMaterialTime);

            _bodyMesh.material = _defaultMaterial;
            _handMesh.material = _defaultMaterial;
        }

        private IEnumerator RegenerateHealth()
        {
            while (true)
            {
                yield return new WaitForSeconds(1f);

                _healthSystem.Health += _healthRegeneration * _levelSystem.Level;
            }
        }

        private int CalculateMaxHealth()
        {
            return _health * _levelSystem.Level + _equipment.Health;
        }

        public void TakeDamage(int damage, bool ignoreArmor = false)
        {
            int fixedDamage = damage;
            if (!ignoreArmor)
            {
                int randomDecreaseValue = UnityEngine.Random.Range(0, GetArmor());
                fixedDamage -= randomDecreaseValue;
            }

            if (fixedDamage < 0)
            {
                fixedDamage = 0;
            }

            _healthSystem.Health -= fixedDamage;

            if (!gameObject.activeSelf)
            {
                return;
            }

            _bodyMesh.material = _damageMaterial;
            _handMesh.material = _damageMaterial;

            if (_materialCoroutine != null)
            {
                StopCoroutine(_materialCoroutine);
            }
            _materialCoroutine = StartCoroutine(SetDefaultMaterial());

            if (_healthSystem.Health <= 0)
            {
                gameObject.SetActive(false);
            }
        }

        public int GetArmor()
        {
            return _equipment.Armor + _armorPerLevel * (_levelSystem.Level - 1);
        }
    }
}