using System.Collections;
using System.Collections.Generic;
using Aloha.Coconut;
using UnityEngine;
using UniRx;
using Zenject;
using FactorySystem;
using Spine.Unity;
using UnityEngine.Rendering;

public enum UnitState
{
    Spawn,
    Idle,
    Move,
    Action,
    Dead,
    Siege
}

public class UnitController : MonoBehaviour, ITarget, ICaster
{
    public string id;
    [Inject] UnitManager _unitManager;

    [SerializeField] private Transform damageEffectPoint;
    [SerializeField] private Transform floatingEffectPoint;
    [SerializeField] private UnitState state;

    [SerializeField] private UnitTable _unitTable;
    private UnitStatusSystem _statusSystem;
    private Collider2D _collider2D;
    private float _attackTimer;
    private ITarget _target;
    private bool _isInitialized;
    private bool _isWallUnit;
    private readonly string _floatingTextId = "FloatingText";
    private SkeletonMecanim _skeletonMecanim;

    public Transform DamageEffectPoint => damageEffectPoint;
    public UnitState State => state;
    public UnitTable UnitTable => _unitTable;
    public int EffectValue => _unitUpgradeController.EffectValue;
    public IReadOnlyReactiveProperty<int> MaxHp => _unitUpgradeController.MaxHp;
    public float EffectAbleRange => _unitUpgradeController.EffectAbleRange;
    public IReadOnlyReactiveProperty<float> SightRange => _unitUpgradeController.SightRange;
    public float MoveSpeed => _unitUpgradeController.MoveSpeed;
    public TargetGroup Group => TargetGroup.Unit;
    public bool IsUntargetable => state == UnitState.Dead || state == UnitState.Spawn || _isWallUnit || isNotTargetable;
    public Rigidbody2D Rigidbody2D { get; private set; }
    public Collider2D Collider2D => _collider2D;
    public Transform Transform => transform;
    public TargetType TargetType => _unitTable.targetType;
    public TeamType TeamType => _unitTable.teamType;
    public string EffectVfxId => _unitTable.effectVfxId;
    public string ProjectTileId => _unitTable.projectTileId;
    public UnitStatusSystem StatusSystem => _statusSystem;
    public AreaType AreaType => _unitTable.areaType;
    public float EffectRange => _unitUpgradeController.EffectRange;
    public IReadOnlyReactiveProperty<float> EffectActionSpeed => _unitUpgradeController.EffectActionSpeed;
    public TargetGroup TargetGroup => _unitTable.targetGroup;

    public UnitUpgradeController UnitUpgradeController => _unitUpgradeController;

    [SerializeField] private UnitUpgradeController _unitUpgradeController;
    public bool IsBarrackUnit { get; private set; }
    private bool isNotTargetable;

    public void Spawn(Vector3 position, UnitTable unitTable, bool onAutoMove)
    {
        IsBarrackUnit = false;
        _isWallUnit = false;
        _unitTable = unitTable;
        _unitUpgradeController = new UnitUpgradeController(_unitTable);
        Init();

        transform.position = position;
        _statusSystem.Init(this);


        var cavalry = GetComponentInChildren<CavalrySkill>();
        if (cavalry)
        {
            cavalry.Init(this);
        }

        if (onAutoMove)
        {
            state = UnitState.Spawn;
            ChangeState(UnitState.Spawn);
            StartCoroutine(WaitAndMove(_unitTable.idleTimeAfterSpawn));
        }

        _skeletonMecanim.Update(0f); // 즉시 반영
    }

    private void Init()
    {
        if (_isInitialized) return;
        _isInitialized = true;
        _statusSystem = GetComponentInChildren<UnitStatusSystem>();
        Rigidbody2D = GetComponent<Rigidbody2D>();
        _collider2D = GetComponent<Collider2D>();
        _skeletonMecanim = GetComponentInChildren<SkeletonMecanim>();
        Rigidbody2D.mass = _unitTable.mass;
        gameObject.layer = LayerMask.NameToLayer(UnitTable.teamType == TeamType.Player ? "Player" : "Enemy");
        gameObject.tag = UnitTable.teamType.ToString();
    }

    private IEnumerator WaitAndMove(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        ChangeState(UnitState.Move);
    }

    public void SetWallUnit(string id, float effectAbleRange, Vector2 position)
    {
        ColliderActive(false);
        _isWallUnit = true;
        _unitUpgradeController.ApplyUpgrade(id, UpgradeType.EffectAbleRangeUp, new UpgradeValue(UpgradeValueType.Additive, effectAbleRange));
        transform.position = position;
        ChangeState(UnitState.Siege);
    }

    public void SetNotTargetable(bool isNotTargetable)
    {
        this.isNotTargetable = isNotTargetable;
    }

    public void SetBarrackUnit(Vector2 position)
    {
        IsBarrackUnit = true;
        ColliderActive(false);
        transform.position = position;
    }

    public void OffBarrackUnit()
    {
        IsBarrackUnit = false;
        ColliderActive(true);
        ChangeState(UnitState.Move);
    }

    public void ColliderActive(bool isActive)
    {
        _collider2D.enabled = isActive;
    }


    private void FixedUpdate()
    {
        // Physics2D 최적화: 불필요한 경우 물리 계산 스킵
        if (state == UnitState.Dead || _isWallUnit || IsBarrackUnit)
        {
            // 완전히 정지된 상태에서는 물리 계산 불필요
            if (Rigidbody2D.bodyType != RigidbodyType2D.Static)
            {
                Rigidbody2D.bodyType = RigidbodyType2D.Static;
            }

            return;
        }

        // 활성 상태일 때는 Dynamic으로 설정 (유닛간 밀어내기 위해)
        if (Rigidbody2D.bodyType != RigidbodyType2D.Dynamic)
        {
            Rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
            // 중력 제거
            Rigidbody2D.gravityScale = 0f;
        }


        if (State == UnitState.Action)
        {
            Rigidbody2D.velocity = Vector2.zero;
        }
    }


    public void ChangeState(UnitState newState, ITarget target = null, bool autoMove = false)
    {
        if (autoMove)
        {
            if (state == UnitState.Dead)
                return;
        }

        state = newState;
        _statusSystem.ApplyState(target, newState);
    }

    public void TakeDamage(ICaster caster, int damage)
    {
        if (IsUntargetable) return;

        // var floatingText = _factoryManager.FloatingTextFactory.GetText(_floatingTextId);
        // floatingText.SetText(damage.ToString());
        // floatingText.Play(floatingEffectPoint.position);


        if (_statusSystem.HpSystem.TakeDamage(damage))
        {
            ChangeState(UnitState.Dead);
            _unitManager.RemoveUnit(this);
            return;
        }

        _statusSystem.ApplyHitAnimation();
    }

    public void ForceRelease() => _statusSystem.ForceRelease();
}