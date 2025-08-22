using UnityEngine;
using UnityEngine.EventSystems;
using Cinemachine;
using System.Collections.Generic;
using Zenject;

public class CameraController : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera virtualCamera;
    [SerializeField] CinemachineSmoothPath smoothPath;
    [Inject] CardSelectionManager _cardSelectionManager;

    private float _minPathPosition = 0f;
    private float _maxPathPosition = 1f;
    private bool _isDragging = false;
    private float _lastMouseY;
    private float _currentPathPosition = 0f;
    private CinemachineTrackedDolly _trackedDolly;
    private float _pathLength;
    private float _dragStartMouseY; // 드래그 시작 시 마우스 Y
    private float _dragStartPathPosition; // 드래그 시작 시 패스 위치

    private int _lastScreenWidth;
    private int _lastScreenHeight;

    public void Init()
    {
        _trackedDolly = virtualCamera.GetCinemachineComponent<CinemachineTrackedDolly>();
        _trackedDolly.m_PositionUnits = CinemachinePathBase.PositionUnits.Normalized; // 추가
        _currentPathPosition = _trackedDolly.m_PathPosition;
        SetWidthBasedOrthographicSize();

        // Path의 전체 길이 계산
        if (smoothPath != null)
            _pathLength = smoothPath.PathLength;
        else
            _pathLength = 1f;

        _lastScreenWidth = Screen.width;
        _lastScreenHeight = Screen.height;
    }

    private void SetWidthBasedOrthographicSize()
    {
        float referenceWidth = 1080f;
        float referenceHeight = 1920f;
        float referenceOrthoSize = 10f;

        float screenAspect = (float)Screen.width / Screen.height;
        float referenceAspect = referenceWidth / referenceHeight;

        float orthoSize = referenceOrthoSize * (referenceAspect / screenAspect);
        virtualCamera.m_Lens.OrthographicSize = orthoSize;
    }

    void Update()
    {
        // 해상도 변경 감지 및 카메라 사이즈 갱신
        if (Screen.width != _lastScreenWidth || Screen.height != _lastScreenHeight)
        {
            SetWidthBasedOrthographicSize();
            _lastScreenWidth = Screen.width;
            _lastScreenHeight = Screen.height;
        }

        if (Input.GetMouseButtonDown(0) && !_cardSelectionManager.IsCardSelected)
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                PointerEventData pointerData = new PointerEventData(EventSystem.current)
                {
                    position = Input.mousePosition
                };
                List<RaycastResult> results = new List<RaycastResult>();
                EventSystem.current.RaycastAll(pointerData, results);

                if (results.Count <= 0)
                {
                    _isDragging = true;
                    _dragStartMouseY = Input.mousePosition.y;
                    _dragStartPathPosition = _currentPathPosition;
                }
            }
            else
            {
                _isDragging = true;
                _dragStartMouseY = Input.mousePosition.y;
                _dragStartPathPosition = _currentPathPosition;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            _isDragging = false;
        }

        if (_isDragging)
        {
            float currentMouseY = Input.mousePosition.y;
            float mouseDelta = currentMouseY - _dragStartMouseY;

            // 마우스 이동량을 월드 단위로 변환
            float worldDelta = mouseDelta * (virtualCamera.m_Lens.OrthographicSize * 2f) / Screen.height;

            // 패스 상에서 worldDelta만큼 이동하기 위한 패스 위치 계산
            float pathDelta = worldDelta / _pathLength;

            _currentPathPosition = _dragStartPathPosition - pathDelta; // 드래그 방향 반전
            _currentPathPosition = Mathf.Clamp(_currentPathPosition, _minPathPosition, _maxPathPosition);
        }

        // 카메라 실제 위치 갱신
        _trackedDolly.m_PathPosition = _currentPathPosition;
    }
}