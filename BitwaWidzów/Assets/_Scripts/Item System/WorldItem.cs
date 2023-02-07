using Battle;
using System.Collections;
using UnityEngine;
using DG.Tweening;

public class WorldItem : MonoBehaviour
{
    [SerializeField] private ItemData _data;

    private Vector3 _startingPosition;

    private void Start()
    {
        _startingPosition = transform.position;

        StartTween();
    }

    private void StartTween()
    {
        transform.DOMoveY(_startingPosition.y + 0.25f, 0.25f).OnComplete(() =>
        {
            transform.DOMoveY(_startingPosition.y - 0.25f, 0.25f).OnComplete(() =>
            {
                StartTween();
            });
        });
    }

    public void Pickup(Unit unit)
    {
        if (!gameObject.activeSelf)
        {
            return;
        }

        gameObject.SetActive(false);

        _data.UseItem(unit);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Unit unit))
        {
            Pickup(unit);
        }
    }
}