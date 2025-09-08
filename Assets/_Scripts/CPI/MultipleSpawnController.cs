using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class MultipleSpawnController : MonoBehaviour
{
    [SerializeField] private int value;
    [Inject] private UnitManager _unitManager;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            UnitController unitController = other.GetComponent<UnitController>();
            ApplyOperation(unitController);
        }
    }

    private void ApplyOperation(UnitController unitController)
    {
        Vector2 spawnPoint = unitController.transform.position;
        _unitManager.SpawnUnit(spawnPoint, unitController.UnitTable.id, value);
    }
}