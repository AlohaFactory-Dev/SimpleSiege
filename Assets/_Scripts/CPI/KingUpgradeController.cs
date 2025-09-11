using System;
using TMPro;
using UnityEngine;

public class KingUpgradeController : MonoBehaviour
{
    [SerializeField] private int hp;
    [SerializeField] private TextMeshPro hpText;
    [SerializeField] private string skinId;
    private int _currentHp;

    private void Start()
    {
        _currentHp = hp;
        hpText.text = _currentHp.ToString();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var unit = other.GetComponent<UnitController>();
        unit.ForceRelease();
        _currentHp--;
        hpText.text = _currentHp.ToString();
        if (_currentHp <= 0)
        {
            StageConainer.Get<KingGroupController>().UpgradeSpawn(skinId);
            Destroy(gameObject);
        }
    }
}