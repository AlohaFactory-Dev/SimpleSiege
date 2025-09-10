using System;
using TMPro;
using UnityEngine;

public class KingAddController : MonoBehaviour
{
    [SerializeField] private TextMeshPro addUnitText;
    [SerializeField] private TextMeshPro hpText;
    [SerializeField] private int value = 1;
    [SerializeField] private int hp;
    private int _currentHp;
    private Rigidbody2D _rigidbody;
    private float _speed = 1f;

    public void Init(float speed)
    {
        _speed = speed;
        addUnitText.text = "+" + value;
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        addUnitText.text = "+" + value;
        _currentHp = hp;
        hpText.text = _currentHp.ToString();

        _rigidbody = GetComponent<Rigidbody2D>();
    }

    // private void FixedUpdate()
    // {
    //     _rigidbody.MovePosition(_rigidbody.position + Vector2.down * (_speed * Time.deltaTime));
    // }

    private void OnTriggerEnter2D(Collider2D other)
    {
        _currentHp--;
        hpText.text = _currentHp.ToString();

        other.GetComponent<UnitController>().ForceRelease();
        if (_currentHp <= 0)
        {
            StageConainer.Get<KingGroupController>().AddKing(value);
            Destroy(gameObject);
        }
    }
}