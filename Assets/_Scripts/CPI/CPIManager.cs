using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

[InfoBox("CPIManager는 게임 시작 시 카메라와 인풋 매니저를 비활성화합니다.")]
[InfoBox("A : 배치된 유닛들 전투 시작" + "\nC : 카메라 이동")]
public class CPIManager : MonoBehaviour
{
    [Serializable]
    public struct CameraMoving
    {
        public float normalizedPosition;
        public float duration;
    }


    [Inject] StageManager _stageManager;
    [SerializeField] private DeployedUnitController deployedUnitController;

    [InfoBox("카메라 이동 정보 :\n" +
        "normalizedPosition : 0~1, duration : 이동 시간\n" +
        "onSequenceCameraMoving : true면 다음 카메라 이동이 자동으로 시작됨")]
    [SerializeField]
    private CameraMoving[] cameraMoving;

    [SerializeField] private bool onSequenceCameraMoving;
    [SerializeField] private AnimationCurve cameraMoveCurve;
    private int _currentCameraIndex = 0;
    private bool _endCameraMove = false;


    private IEnumerator Start()
    {
        yield return new WaitUntil(() => _stageManager.isInit);
        _stageManager.CameraController.enabled = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            StageConainer.Get<InputRecorder>().AddEvent("StartBattle");
            StartBattle();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            StageConainer.Get<InputRecorder>().AddEvent("MoveCameraSequence");
            MoveCameraSequence();
        }
    }

    public void StartBattle()
    {
        deployedUnitController.StartBattle();
    }

    public void MoveCameraSequence()
    {
        if (_endCameraMove) return;
        if (onSequenceCameraMoving)
        {
            _endCameraMove = true;
            _stageManager.CameraController.PlaySequenceCameraMove(cameraMoving, cameraMoveCurve);
        }
        else
        {
            if (_currentCameraIndex < cameraMoving.Length)
            {
                var camMove = cameraMoving[_currentCameraIndex];
                _stageManager.CameraController.PlayCameraMove(camMove.normalizedPosition, camMove.duration, cameraMoveCurve);
                _currentCameraIndex++;
            }
            else
            {
                _endCameraMove = true;
            }
        }
    }
}