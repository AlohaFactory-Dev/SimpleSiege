using System;
using System.Collections;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public enum FireType
{
    Normal,
    Howitzer
}

public enum AttackObjectSpeedType
{
    Speed, //속도
    Time //시간
}

[RequireComponent(typeof(Rigidbody2D), typeof(RecycleObject))]
public class AttackObject : MonoBehaviour
{
    [InfoBox("이 필드는 곡사(FireType.Howitzer)일 때만 사용됩니다.")]
    [BoxGroup("Howitzer Settings")]
    [SerializeField]
    private AnimationCurve howitzerCurve;

    [BoxGroup("Howitzer Settings")]
    [SerializeField]
    private float howitzerHeight;

    [BoxGroup("Normal")] [SerializeField] protected AnimationCurve normalCurve;

    [SerializeField] private Transform mainObject;
    [SerializeField] private GameObject offObject;
    protected RecycleObject _recycleObject;
    private AttackObjectTable _table;
    protected AttackObjectTable Table => _table;
    protected Vector2 TargetPosition;
    protected Action _onHit;
    private Sequence _sequence;
    private FireType FireType => _table.fireType;
    private TrailRenderer _trailRenderer;
    private ParticleSystem[] _particleSystem;
    private Vector3 _previousPosition;
    private float releaseDelay = 2f;
    bool _isInitialized;

    public void Init(Vector2 position, Action onHit, AttackObjectTable table, Vector2 targetPosition, bool autoFire = true)
    {
        TargetPosition = targetPosition;
        GetComponents();

        if (_trailRenderer)
        {
            _trailRenderer.Clear();
            _trailRenderer.enabled = false;
            transform.position = position; // 위치 먼저 할당
            _trailRenderer.enabled = true;
        }
        else if (_particleSystem.Length > 0)
        {
            foreach (var ps in _particleSystem)
                ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            transform.position = position; // 위치 먼저 할당

            foreach (var ps in _particleSystem) ps.Play();
        }
        else
        {
            transform.position = position;
        }

        offObject.SetActive(true);
        mainObject.transform.localPosition = Vector3.zero;
        _table = table;
        mainObject.localScale = Vector3.one * _table.scale;
        _onHit = onHit;
        if (!autoFire) return;
        if (table.delayRandomMin == 0)
        {
            Fire(targetPosition);
        }
        else
        {
            offObject.SetActive(false);
            StartCoroutine(FireDelay(Random.Range(table.delayRandomMin, table.delayRandomMax), targetPosition));
        }
    }

    private void GetComponents()
    {
        if (_isInitialized) return;
        _recycleObject = GetComponent<RecycleObject>();
        _trailRenderer = GetComponentInChildren<TrailRenderer>();
        _particleSystem = GetComponentsInChildren<ParticleSystem>();
        _isInitialized = true;
    }


    private IEnumerator FireDelay(float delay, Vector2 targetPosition)
    {
        yield return new WaitForSeconds(delay);
        offObject.SetActive(true);

        Fire(targetPosition);
    }


    private void Fire(Vector2 targetPosition)
    {
        if (_table.onLookat)
        {
            var dir = targetPosition - (Vector2)mainObject.position;
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            mainObject.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        _previousPosition = mainObject.position;

        _sequence = DOTween.Sequence();
        if (_table.speedType == AttackObjectSpeedType.Speed)
        {
            var speedValue = Vector2.Distance(transform.position, targetPosition) / Random.Range(_table.speedRandomMin, _table.speedRandomMax);
            _sequence.Append(transform.DOMove(targetPosition, speedValue).SetEase(normalCurve));
            if (FireType == FireType.Howitzer)
            {
                var howitzerTween = mainObject.DOLocalMoveY(howitzerHeight, speedValue).SetEase(howitzerCurve);
                if (_table.onLookat)
                {
                    howitzerTween.OnUpdate(UpdateHowitzerRotation);
                }

                _sequence.Join(howitzerTween);
            }
        }
        else
        {
            var speedValue = Random.Range(_table.speedRandomMin, _table.speedRandomMax);
            _sequence.Append(transform.DOMove(targetPosition, speedValue).SetEase(normalCurve));
            if (FireType == FireType.Howitzer)
            {
                var howitzerTween = mainObject.DOLocalMoveY(howitzerHeight, speedValue).SetEase(howitzerCurve);
                if (_table.onLookat)
                {
                    howitzerTween.OnUpdate(UpdateHowitzerRotation);
                }

                _sequence.Join(howitzerTween);
            }
        }

        _sequence.OnComplete(() =>
        {
            _onHit?.Invoke();
            StartCoroutine(ReleaseDelay());
        });
        _sequence.Play();
    }

    protected IEnumerator ReleaseDelay()
    {
        offObject.SetActive(false);
        yield return new WaitForSeconds(releaseDelay);
        _recycleObject.Release();
    }

    private void UpdateHowitzerRotation()
    {
        // 현재 위치와 이전 위치의 차이로 실제 이동 방향 계산
        var currentPosition = mainObject.position;
        var velocity = currentPosition - _previousPosition;

        if (velocity.magnitude > 0.001f)
        {
            var angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
            mainObject.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        _previousPosition = currentPosition;
    }
}