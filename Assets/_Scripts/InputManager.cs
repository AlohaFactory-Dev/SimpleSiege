using System;
using System.Collections;
using Aloha.Coconut;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class InputManager : MonoBehaviour
{
    [Inject] private CardSelectionManager _cardSelectionManager;
    private bool _isPointerDown;
    private Coroutine _spawnCoroutine;
    [SerializeField] Camera _camera;

    private int _spawnableLayerMask;

    public void Init()
    {
        _camera = Camera.main;
        _spawnableLayerMask = 1 << LayerMask.NameToLayer("SpawnableZone");
    }

    private void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        // 마우스 입력 처리 (에디터 및 PC)
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            OnPointerDown();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            OnPointerUp();
        }
#elif UNITY_ANDROID || UNITY_IOS
        // 터치 입력 처리 (모바일)
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began &&
            !EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
        {
            OnPointerDown();
        }
        else if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            OnPointerUp();
        }
#endif
    }

    private void OnPointerDown()
    {
        _isPointerDown = true;
        _spawnCoroutine = StartCoroutine(HandleHold());
    }

    private void OnPointerUp()
    {
        _isPointerDown = false;
        if (_spawnCoroutine != null)
        {
            StopCoroutine(_spawnCoroutine);
            _spawnCoroutine = null;
        }
    }

    private IEnumerator HandleHold()
    {
        var selectedCard = _cardSelectionManager.SelectedCard;
        if (!selectedCard) yield break;

        var holdTime = selectedCard.CardTable.holdTime;
        var spawnInterval = selectedCard.CardTable.spawnInterval;

        if (selectedCard.CardTable.cardType == CardType.Unit)
        {
            HandleInput();
            // HoldTime 동안 기다린 후에 연속 생성 시작
            yield return new WaitForSeconds(holdTime);
            while (_isPointerDown)
            {
                HandleInput();
                yield return new WaitForSeconds(spawnInterval);
            }
        }
        else if (selectedCard.CardTable.cardType == CardType.Spell)
        {
            HandleInput();
        }
    }

    private void HandleInput()
    {
        var selectedCard = _cardSelectionManager.SelectedCard;
        if (!selectedCard) return;

        Vector3 worldPos = _camera.ScreenToWorldPoint(Input.mousePosition);
        worldPos.z = 0;

        if (selectedCard.CardTable.cardType == CardType.Unit)
        {
            // 마우스 위치에 SpawnableArea Collider가 있으면 유닛 소환
            Collider2D hit = Physics2D.OverlapPoint(worldPos, _spawnableLayerMask);
            if (hit)
            {
                _cardSelectionManager.UseSelectedCard(worldPos);
            }
            else
            {
                SystemUI.ShowToastMessage(TextTableV2.Get("SystemUI/ToastMessage/NoSpawnableZone"));
            }

            return;
        }

        _cardSelectionManager.UseSelectedCard(worldPos);
    }
}