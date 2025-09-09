using System;
using System.Collections;
using UnityEngine;
using Zenject;

public class KingGroupController : MonoBehaviour
{
    [SerializeField] private Transform[] kingPoints;
    [Inject] private UnitManager _unitManager;
    [Inject] StageManager _stageManager;
    private Camera _camera;
    private int _kingCount = 1;

    private IEnumerator Start()
    {
        yield return new WaitUntil(() => _stageManager.isInit);
        _camera = Camera.main;
        AddKing(1);
        _stageManager.CameraController.enabled = false;
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
            if (kingPoints.Length > _kingCount)
            {
                var king = _unitManager.SpawnUnit(kingPoints[_kingCount].position, "P_King", 1, false)[0];
                king.transform.parent = transform;
                king.Collider2D.enabled = false;
                king.GetComponent<KingSkill>().SetGroup();
                _kingCount++;
            }
            else
            {
                break;
            }
        }
    }
}