using Aloha.Coconut;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(BuildingAnimationSystem))]
public class Building : MonoBehaviour, ITarget
{
    [Inject] private BuildingManager _buildingManager;
    [SerializeField] private string id;
    public Transform Transform => transform;
    public TeamType TeamType => TeamType.Enemy;
    public TargetGroup Group => TargetGroup.Building;
    public Collider2D Collider2D => _collider2D;
    private Collider2D _collider2D;
    public bool IsDead => _hpSystem.IsDead;
    public bool IsUntargetable => TableManager.IsMagicNumber(MaxHp) || _hpSystem.IsDead;
    public int MaxHp => BuildingTable.maxHp;
    protected BuildingTable BuildingTable { get; private set; }
    private HpSystem _hpSystem;
    private BuildingAnimationSystem _animationSystem;
    private BuildingAnimationEventHandler _animationEventHandler;

    public void Init()
    {
        BuildingTable = TableListContainer.Get<BuildingTableLis>().GetBuildingTable(id);
        _buildingManager.AddBuilding(this);
        _hpSystem = GetComponent<HpSystem>();
        _animationSystem = GetComponent<BuildingAnimationSystem>();
        _collider2D = GetComponent<Collider2D>();
        _animationEventHandler = _animationSystem.GetComponentInChildren<BuildingAnimationEventHandler>();
        if (_hpSystem != null)
            _hpSystem.Init(BuildingTable.maxHp);
        _animationSystem.Init();
        _animationEventHandler.Init(Remove);
        CustomInit();
    }

    protected virtual void CustomInit()
    {
    }

    public virtual void TakeDamage(ICaster caster)
    {
        if (_hpSystem.TakeDamage(caster.EffectValue))
        {
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