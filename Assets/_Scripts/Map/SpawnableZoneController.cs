using UniRx;
using UnityEngine;
using Zenject;

public class SpawnableZoneController : MonoBehaviour
{
    [SerializeField] private float distance;
    private BuildingManager _buildingManager;
    private float _defaultHeight;

    public void Init(BuildingManager buildingManager)
    {
        _buildingManager = buildingManager;
        _buildingManager.OnNearestEnemyBuildingDestroyed.Subscribe(OnNearestEnemyBuildingDestroyed).AddTo(this);
        _defaultHeight = -transform.position.y;
    }

    private void OnNearestEnemyBuildingDestroyed(Vector2 position)
    {
        transform.localScale = new Vector2(transform.localScale.x, _defaultHeight + position.y - distance);
    }
}