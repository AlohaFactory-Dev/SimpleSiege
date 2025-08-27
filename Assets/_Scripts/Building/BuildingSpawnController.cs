using UnityEngine;
using Zenject;

public class BuildingSpawnController : MonoBehaviour
{
    private void Awake()
    {
        var buildings = GetComponentsInChildren<Building>(true);
        foreach (var building in buildings)
        {
            building.Init();
        }
    }
}