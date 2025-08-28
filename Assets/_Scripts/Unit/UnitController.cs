using System.Collections;
using Aloha.Coconut;
using UnityEngine;
using UniRx;
using Zenject;
using FactorySystem;

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
    [Inject] FactoryManager _factoryManager;
    [Inject] UnitManager _unitManager;

    [SerializeField] private Transform damageEffectPoint;
    [SerializeField] private Transform floatingEffectPoint;
    [SerializeField] private UnitState state;

    private UnitTable _unitTable;
    private UnitStatusSystem _statusSystem;
    private Collider2D _collider2D;
    private float _attackTimer;
    private ITarget _target;
    private bool _isInitialized;
    private bool _isWallUnit;
    private int _effectValue;
    private int _maxHp;
    private readonly string _floatingTextId = "FloatingText";

    public UnitTable UnitTable => _unitTable;
    public int EffectValue => _effectValue;
    public int MaxHp => _maxHp;
    public TargetGroup Group => TargetGroup.Unit;
    public bool IsUntargetable => state == UnitState.Dead || state == UnitState.Spawn || _isWallUnit;
    public Rigidbody2D Rigidbody2D { get; private set; }
    public Collider2D Collider2D => _collider2D;
    public Transform Transform => transform;
    public TargetType TargetType => _unitTable.targetType;
    public TeamType TeamType => _unitTable.teamType;
    public string EffectVfxId => _unitTable.effectVfxId;
    public string ProjectTileId => _unitTable.projectTileId;
    public UnitStatusSystem StatusSystem => _statusSystem;
    public AreaType AreaType => _unitTable.areaType;
    public float EffectRange => _unitTable.effectRange;

    public void Spawn(Vector3 position, UnitTable unitTable, bool onAutoMove)
    {
        _isWallUnit = false;
        _unitTable = unitTable;
        Init();

        if (_unitTable.teamType == TeamType.Enemy)
        {
            var stageTable = StageConainer.Get<StageManager>().CurrentStageTable;
            _effectValue = EffectCalculator.CalculateEffectValue(_unitTable.effectValue, _unitTable.effectGrowth, stageTable.enemyAttackPowerLevel);
            _maxHp = EffectCalculator.CalculateEffectValue(_unitTable.maxHp, _unitTable.maxHpGrowth, stageTable.enemyHpLevel);
        }
        else
        {
            _effectValue = _unitTable.effectValue;
            _maxHp = _unitTable.maxHp;
        }

        transform.position = position;
        _statusSystem.Init(this);


        if (onAutoMove)
        {
            state = UnitState.Spawn;
            ChangeState(UnitState.Spawn);
            StartCoroutine(WaitAndMove(_unitTable.idleTimeAfterSpawn));
        }
    }

    private void Init()
    {
        if (_isInitialized) return;
        _isInitialized = true;
        _statusSystem = GetComponentInChildren<UnitStatusSystem>();
        Rigidbody2D = GetComponent<Rigidbody2D>();
        _collider2D = GetComponent<Collider2D>();
        Rigidbody2D.mass = _unitTable.mass;
    }

    private IEnumerator WaitAndMove(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        ChangeState(UnitState.Move);
    }

    public void SetWallUnit()
    {
        _isWallUnit = true;
        ChangeState(UnitState.Siege);
    }

    private void FixedUpdate()
    {
        Rigidbody2D.rotation = 0f;
        Rigidbody2D.angularVelocity = 0f;
        Rigidbody2D.velocity = Vector2.zero;
    }


    public void ChangeState(UnitState newState, ITarget target = null, bool isSiege = false)
    {
        if (state == UnitState.Dead) return;
        state = newState;
        _statusSystem.ApplyState(target, newState, isSiege);
    }

    public void TakeDamage(ICaster caster, int damage)
    {
        if (IsUntargetable) return;

        var floatingText = _factoryManager.FloatingTextFactory.GetText(_floatingTextId);
        floatingText.SetText(damage.ToString());
        floatingText.Play(floatingEffectPoint.position);

        if (_statusSystem.HpSystem.TakeDamage(damage))
        {
            ChangeState(UnitState.Dead);
            _unitManager.RemoveUnit(this);
        }

        if (!TableManager.IsMagicNumber(caster.EffectVfxId))
        {
            var particle = _factoryManager.ParticleFactory.GetParticle(caster.EffectVfxId);
            particle.Init(damageEffectPoint.position);
            particle.Play();
        }
    }

    public void ForceRelease() => _statusSystem.ForceRelease();
}