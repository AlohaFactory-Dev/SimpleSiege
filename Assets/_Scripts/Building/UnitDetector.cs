using System;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class UnitDetector : MonoBehaviour
{
    private Action<UnitController> _onDetect;

    public void Init(Action<UnitController> onDetect)
    {
        _onDetect = onDetect;
    }

    public void SetRadius(float radius)
    {
        var collider = GetComponent<CircleCollider2D>();
        if (collider != null)
        {
            collider.radius = radius;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<UnitController>(out var target) && !target.IsUntargetable)
        {
            _onDetect?.Invoke(target);
        }
    }

    public void Off()
    {
        gameObject.SetActive(false);
    }
}