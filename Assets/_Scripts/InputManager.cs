using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace _Scripts
{
    public class InputManager : MonoBehaviour
    {
        [Inject] private CardSelectionManager _cardSelectionManager;

        [InfoBox("연속 생성이 시작 되기 전에 누르고 있어야 하는 시간입니다.")]
        [SerializeField]
        private float holdTime = 0.5f; // 버튼을 누르고 있어야 하는 시간

        [InfoBox("연속 생성 간격입니다. 이 시간마다 유닛이 생성됩니다.")]
        [SerializeField]
        private float spawnInterval = 0.5f; // 버튼을 누르고 있어야 하는 시간

        private bool _isPointerDown;
        private Coroutine _spawnCoroutine;
        private Camera _camera;

        private int _spawnableLayerMask;

        public void Init()
        {
            _camera = Camera.main;
            _spawnableLayerMask = 1 << LayerMask.NameToLayer("SpawnableZone");
        }

        private void Update()
        {
            // 마우스 클릭 또는 터치 입력 처리
            if (Input.GetMouseButtonDown(0))
            {
                OnPointerDown();
            }
            else if (Input.GetMouseButtonUp(0))
            {
                OnPointerUp();
            }
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

            _cardSelectionManager.OffSelectedCard();
        }

        private IEnumerator HandleHold()
        {
            var selectedCard = _cardSelectionManager.SelectedCard;
            if (!selectedCard) yield break;


            if (selectedCard.cardTable.cardType == CardType.Unit)
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
            else if (selectedCard.cardTable.cardType == CardType.Spell)
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

            if (selectedCard.cardTable.cardType == CardType.Unit)
            {
                // 마우스 위치에 SpawnableArea Collider가 있으면 유닛 소환
                Collider2D hit = Physics2D.OverlapPoint(worldPos, _spawnableLayerMask);
                if (hit)
                {
                    _cardSelectionManager.UseSelectedCard(worldPos);
                }

                return;
            }

            _cardSelectionManager.UseSelectedCard(worldPos);
        }
    }
}