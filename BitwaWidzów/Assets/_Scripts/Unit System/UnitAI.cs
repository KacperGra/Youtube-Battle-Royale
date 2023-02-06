using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

namespace Battle
{
    public class UnitAI : MonoBehaviour
    {
        public event Action<int> OnTargetKilled;

        private enum State
        {
            Idle,
            Combat,
            Travel,
            ItemTravel,
        }

        [Header("References")]
        [SerializeField] private NavMeshAgent _agent;

        [Header("Movement")]
        [SerializeField] private float _baseSpeed = 5;
        [SerializeField] private float _travelSpeed = 8;
        [SerializeField] private float _walkDistance = 10f;

        [Header("Combat")]
        [SerializeField] private int _damage = 1;
        [SerializeField] private float _attackDelay = 0.5f;
        [SerializeField] private float _combatRange = 1f;
        [SerializeField] private float _searchRange = 3f;

        private int _currentDamage;
        private float _attackTimer = 0f;

        private Unit _unit;
        private State _state = State.Idle;
        private Vector3 _newLocation;
        private Unit _target;
        private WorldItem _itemTarget;

        public NavMeshAgent Agent => _agent;

        public int BaseDamage => _damage;

        public int CurrentDamage
        {
            get => _currentDamage;
            set
            {
                _currentDamage = value;
            }
        }

        public void Initialize(Unit unit)
        {
            _unit = unit;

            FindNewTargetLocation();
        }

        public void Update()
        {
            switch (_state)
            {
                case State.Idle:
                    HandleIdleState();
                    break;

                case State.Combat:
                    HandleCombatState();
                    break;

                case State.Travel:
                    HandleTravelState();
                    break;

                case State.ItemTravel:
                    HandleItemTravelState();
                    break;
            }
        }

        private void HandleItemTravelState()
        {
            if (!_itemTarget.gameObject.activeSelf)
            {
                SetState(State.Idle);
                return;
            }

            _agent.SetDestination(_itemTarget.transform.position);
        }

        private void HandleTravelState()
        {
            _agent.SetDestination(_newLocation);

            float distance = _agent.remainingDistance;
            if (distance < 2f)
            {
                SetState(State.Idle);
            }
        }

        private void HandleIdleState()
        {
            _agent.SetDestination(_newLocation);

            if (TryFindItem())
            {
                return;
            }

            if (TryFindNewTarget())
            {
                return;
            }

            float distance = _agent.remainingDistance;
            if (distance < 2f)
            {
                FindNewTargetLocation();
            }
        }

        private bool TryFindNewTarget()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, _searchRange);

            Unit bestUnit = null;
            float bestDistance = 999f;
            foreach (var collider in colliders)
            {
                if (collider.TryGetComponent(out Unit unit))
                {
                    if (unit == _unit)
                    {
                        continue;
                    }

                    float unitDistance = Vector3.Distance(unit.transform.position, transform.position);
                    if (unitDistance < bestDistance)
                    {
                        bestUnit = unit;
                        bestDistance = unitDistance;
                    }
                }
            }

            if (bestUnit != null)
            {
                _target = bestUnit;
                SetState(State.Combat);
                return true;
            }

            return false;
        }

        private bool TryFindItem()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, _searchRange);

            WorldItem bestItem = null;
            float bestDistance = 999f;
            foreach (var collider in colliders)
            {
                if (collider.TryGetComponent(out WorldItem item))
                {
                    float unitDistance = Vector3.Distance(item.transform.position, transform.position);
                    if (unitDistance < bestDistance)
                    {
                        bestItem = item;
                        bestDistance = unitDistance;
                    }
                }
            }

            if (bestItem != null)
            {
                _itemTarget = bestItem;
                SetState(State.ItemTravel);
                return true;
            }

            return false;
        }

        private void HandleCombatState()
        {
            if (!_target.enabled)
            {
                SetState(State.Idle);
                return;
            }

            var distance = Vector3.Distance(_target.transform.position, transform.position);
            if (distance > _combatRange)
            {
                _agent.SetDestination(_target.transform.position);
                TryFindNewTarget();
                return;
            }

            _agent.ResetPath();

            _attackTimer += Time.deltaTime;
            if (_attackTimer >= _attackDelay)
            {
                _attackTimer -= _attackDelay;

                int damage = UnityEngine.Random.Range(1, _currentDamage) + _unit.Equipment.Damage;

                _target.HealthSystem.Health -= damage;
                _unit.Animator.SetTrigger("Attack");

                if (_target.HealthSystem.Health <= 0)
                {
                    OnTargetKilled?.Invoke(_target.LevelSystem.Level * 50);

                    _target = null;
                    SetState(State.Travel);
                }
            }
        }

        private void FindNewTargetLocation()
        {
            Vector3 bestPosition = transform.position;
            float bestDistanceFromZone = Vector3.Distance(Vector3.zero, bestPosition);
            for (int i = 0; i < 5; ++i)
            {
                var randomPosition = UnityEngine.Random.insideUnitCircle * _walkDistance;
                Vector3 tempLocation = transform.position + new Vector3(randomPosition.x, 0, randomPosition.y);

                float distanceFromZone = Vector3.Distance(Vector3.zero, tempLocation);
                if (distanceFromZone < bestDistanceFromZone)
                {
                    bestPosition = tempLocation;
                    bestDistanceFromZone = distanceFromZone;
                }
            }

            _newLocation = bestPosition;
        }

        private void SetState(State state)
        {
            _state = state;
            switch (_state)
            {
                case State.Idle:
                    _agent.speed = _baseSpeed;
                    break;

                case State.Combat:
                    _agent.speed = _baseSpeed;
                    break;

                case State.Travel:
                    _agent.speed = _travelSpeed;
                    FindNewTargetLocation();
                    break;

                case State.ItemTravel:
                    _agent.speed = _travelSpeed;
                    break;
            }
        }
    }
}