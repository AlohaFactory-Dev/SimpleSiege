using Aloha.Coconut;
using FactorySystem;
using UniRx;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(BuildingAnimationSystem))]
public class Building : MonoBehaviour, ITarget
{
    private BuildingManager _buildingManager;
    [SerializeField] private string id;
    public Transform Transform => transform;
    public TeamType TeamType => BuildingTable.teamType;
    public TargetGroup Group => TargetGroup.Building;
    public Collider2D Collider2D => _collider2D;
    private Collider2D _collider2D;

    public Transform DamageEffectPoint => transform;


    public bool IsUntargetable
    {
        get
        {
            if (!_hasHpSystem || IsDestroyed)
            {
                return true;
            }

            return TableManager.IsMagicNumber(MaxHp) || _hpSystem.IsDead;
        }
    }

    public IReadOnlyReactiveProperty<int> MaxHp { get; private set; }
    protected BuildingTable BuildingTable { get; private set; }
    private HpSystem _hpSystem;
    private BuildingAnimationSystem _animationSystem;
    private BuildingAnimationEventHandler _animationEventHandler;
    private bool _hasHpSystem;
    protected bool IsDestroyed;

    public void Init(BuildingManager buildingManager)
    {
        _buildingManager = buildingManager;
        BuildingTable = TableListContainer.Get<BuildingTableLis>().GetBuildingTable(id);
        _hpSystem = GetComponentInChildren<HpSystem>();
        _animationSystem = GetComponent<BuildingAnimationSystem>();
        _collider2D = GetComponent<Collider2D>();
        _animationEventHandler = _animationSystem.GetComponentInChildren<BuildingAnimationEventHandler>();
        MaxHp = new ReadOnlyReactiveProperty<int>(new ReactiveProperty<int>(BuildingTable.maxHp));
        _hasHpSystem = _hpSystem != null;
        if (_hasHpSystem)
            _hpSystem.Init(MaxHp, BuildingTable.teamType, BuildingTable.maxHp);
        _animationSystem.Init();
        _animationEventHandler.Init(Remove);
        CustomInit();
    }

    protected virtual void CustomInit()
    {
    }

    public virtual void TakeDamage(ICaster caster, int damage)
    {
        if (IsDestroyed) return;
        if (_hpSystem.TakeDamage(damage))
        {
            _collider2D.enabled = false;
            IsDestroyed = true;
            DestroyBuilding();
        }
        else
        {
            _animationSystem.TakeDamage();
        }
    }

    protected virtual void DestroyBuilding()
    {
        _buildingManager.RemoveBuilding(this);
        _animationSystem.PlayDestroyAnimation();
    }


    protected virtual void Remove()
    {
        Destroy(gameObject);
    }
}