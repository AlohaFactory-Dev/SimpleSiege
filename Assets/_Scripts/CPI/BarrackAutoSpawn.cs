using System;
using UnityEngine;

public class BarrackAutoSpawn : MonoBehaviour
{
    [SerializeField] private float autoSpawnInterval;
    private float _timer;
    private BarrackBuilding _barrackBuilding;

    private void Start()
    {
        _barrackBuilding = GetComponent<BarrackBuilding>();
    }

    private void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= autoSpawnInterval)
        {
            _timer = 0f;
            _barrackBuilding.AutoSpawn();
        }
    }
}