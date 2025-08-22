using FactorySystem;
using UnityEngine;

public class UnitManager
{
    private FactoryManager _factoryManager;

    public UnitManager(FactoryManager factoryManager)
    {
        _factoryManager = factoryManager;
    }

    public void SpawnUnit(Vector2 initialPosition, string unitId)
    {
    }
}