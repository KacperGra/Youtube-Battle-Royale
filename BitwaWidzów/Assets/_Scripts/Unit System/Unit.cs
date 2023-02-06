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

        [Header("Materials")]
        [SerializeField] private float _damageMaterialTime = 0.1f;
        [SerializeField] private Material _defaultMaterial;
        [SerializeField] private Material _damageMaterial;

        private Coroutine _materialCoroutine;
        private HealthSystem _healthSystem;
        private readonly LevelSystem _levelSystem = new();

        public UnitEquipment Equipment => _equipment;
        public HealthSystem HealthSystem => _healthSystem;
        public LevelSystem LevelSystem => _levelSystem;
        public Animator Animator => _animator;

        private void Awake()
        {
            _healthSystem = new HealthSystem(_health);
            _unityUI.Initialize(this);
            _unitAI.Initialize(this);

            HandleLevelChanged();

            _healthSystem.OnHealthChanged += HandleHealthChanged;
            _levelSystem.OnLevelChanged += HandleLevelChanged;
            _unitAI.OnTargetKilled += HandleTargetKilled;
            _equipment.OnArmorEquiped += OnArmorEquiped;
        }

        private void OnArmorEquiped()
        {
            _healthSystem.MaxHealth = CalculateMaxHealth();
        }

        private void Update()
        {
            var cameraPosition = Camera.main.transform.position;
            float distance = Vector3.Distance(cameraPosition, transform.position);

            _unityUI.gameObject.SetActive(distance < 30f);
        }

        private void HandleLevelChanged()
        {
            _healthSystem.MaxHealth = CalculateMaxHealth();
            _healthSystem.Health = _healthSystem.MaxHealth;

            _unitAI.CurrentDamage = _unitAI.BaseDamage * _levelSystem.Level;
        }

        private void HandleHealthChanged()
        {
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

        private int CalculateMaxHealth()
        {
            return _health * _levelSystem.Level + _equipment.Health;
        }

        public void TakeDamage(int damage)
        {
            int fixedDamage = damage - _equipment.Armor;
            if (fixedDamage < 0)
            {
                fixedDamage = 0;
            }

            _healthSystem.Health -= fixedDamage;

            if (_healthSystem.Health <= 0)
            {
                gameObject.SetActive(false);
            }
        }
    }
}