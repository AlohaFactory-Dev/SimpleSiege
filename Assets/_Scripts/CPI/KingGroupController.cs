using System;
using System.Collections;
using UnityEngine;
using Zenject;

public class KingGroupController : MonoBehaviour
{
    [SerializeField] private Transform[] kingPoints;
    [Inject] private UnitManager _unitManager;
    [Inject] InputManager _inputManager;
    [Inject] StageManager _stageManager;
    private Camera _camera;
    private int _kingCount = 1;

    private IEnumerator Start()
    {
        yield return new WaitUntil(() => _stageManager.isInit);
        _camera = Camera.main;
        var king = _unitManager.SpawnUnit(kingPoints[0].position, "P_King", 1, false)[0];
        king.transform.parent = transform;
        king.Collider2D.enabled = false;
        _stageManager.CameraController.enabled = false;
        _inputManager.enabled = false;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector2 mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);
            Vector3 newPosition = transform.position;
            newPosition.x = mousePosition.x;
            transform.position = newPosition;
        }
    }

    public void AddKing(int value)
    {
        for (int i = 0; i < value; i++)
        {
            if (kingPoints.Length < _kingCount)
            {
                var king = _unitManager.SpawnUnit(kingPoints[_kingCount].position, "P_King", 1, false)[0];
                king.transform.parent = transform;
                king.Collider2D.enabled = false;
                _kingCount++;
            }
            else
            {
                break;
            }
        }
    }
}