using System;
using TMPro;
using UnityEngine;

public class KingAddController : MonoBehaviour
{
    [SerializeField] private TextMeshPro addUnitText;
    [SerializeField] private int value = 1;
    private Rigidbody2D _rigidbody;
    private float _speed = 1f;

    public void Init(float speed)
    {
        _speed = speed;
        addUnitText.text = "+" + value;
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        _rigidbody.MovePosition(_rigidbody.position + Vector2.down * (_speed * Time.deltaTime));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<KingGroupController>(out var kingGroupController))
        {
            kingGroupController.GetComponent<KingGroupController>().AddKing(value);
            Destroy(gameObject);
        }
    }
}