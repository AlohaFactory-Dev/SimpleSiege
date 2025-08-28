using UniRx;
using UnityEngine;
using Zenject;

namespace _Scripts.Map
{
    public class SpawnableZoneController : MonoBehaviour
    {
        [Inject] private BuildingManager _buildingManager;
        private float _defaultHeight;

        public void Init()
        {
            _buildingManager.OnNearestEnemyBuildingDestroyed.Subscribe(OnNearestEnemyBuildingDestroyed).AddTo(this);
            _defaultHeight = -transform.position.y;
        }

        private void OnNearestEnemyBuildingDestroyed(Vector2 position)
        {
            if (position.y <= _defaultHeight)
            {
                transform.localScale = new Vector2(transform.localScale.x, _defaultHeight + position.y - 3);
            }
        }
    }
}