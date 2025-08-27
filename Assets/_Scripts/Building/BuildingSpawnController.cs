using UnityEngine;
using Zenject;

public class BuildingSpawnController : MonoBehaviour
{
    public void Init()
    {
        var buildings = GetComponentsInChildren<Building>(true);
        foreach (var building in buildings)
        {
            building.Init();
        }
    }
}