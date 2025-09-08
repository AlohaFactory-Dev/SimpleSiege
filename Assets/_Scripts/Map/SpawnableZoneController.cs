using UniRx;
using UnityEngine;
using Zenject;

public class SpawnableZoneController : MonoBehaviour
{
    [Inject] private CardSelectionManager _cardSelectionManager;
    [SerializeField] private Transform topObj;
    [SerializeField] private Transform zoneObj;
    [SerializeField] private float distance;
    private BuildingManager _buildingManager;
    private float _defaultHeight;
    private Animator _animator;

    public void Init(BuildingManager buildingManager)
    {
        _animator = GetComponent<Animator>();
        _buildingManager = buildingManager;
        _cardSelectionManager.IsCardSelected.Subscribe(card =>
        {
            if (card)
                _animator.SetTrigger("Show");
            else
                _animator.SetTrigger("Hide");
        }).AddTo(this);
        _buildingManager.OnNearestEnemyBuildingDestroyed.Subscribe(OnNearestEnemyBuildingDestroyed).AddTo(this);
        _defaultHeight = -zoneObj.position.y;
    }


    private void OnNearestEnemyBuildingDestroyed(Vector2 position)
    {
        topObj.position = new Vector2(0, position.y - distance);
        zoneObj.localScale = new Vector2(zoneObj.localScale.x, _defaultHeight + position.y - distance);
    }
}