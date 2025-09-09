using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class KingGroupController : MonoBehaviour
{
    private int spawnIndex = 0;
    [SerializeField] private string[] spawnUnitIds;
    [SerializeField] private float spawnInterval = 5f;
    private float _spawnTimer = 0f;
    [SerializeField] private Transform[] kingPoints;
    [Inject] private UnitManager _unitManager;
    [Inject] StageManager _stageManager;
    private Camera _camera;
    private int _kingCount = 0;
    private List<KingSkill> _kingSkills = new List<KingSkill>();

    private IEnumerator Start()
    {
        yield return new WaitUntil(() => _stageManager.isInit);
        _camera = Camera.main;
        AddKing(1);
        _stageManager.CameraController.enabled = false;
    }

    public void UpgradeSpawn()
    {
        spawnIndex++;
        if (spawnIndex >= spawnUnitIds.Length)
        {
            spawnIndex = spawnUnitIds.Length - 1;
        }
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

        if (_spawnTimer >= spawnInterval)
        {
            _spawnTimer = 0f;
            foreach (var skill in _kingSkills)
            {
                skill.GroupSpawnSkill(spawnUnitIds[spawnIndex]);
            }
        }

        _spawnTimer += Time.deltaTime;
    }

    public void AddKing(int value)
    {
        for (int i = 0; i < value; i++)
        {
            if (kingPoints.Length > _kingCount)
            {
                var king = _unitManager.SpawnUnit(kingPoints[_kingCount].position, "P_King", 1, false, false)[0];
                king.transform.parent = transform;
                king.Collider2D.enabled = false;
                var skill = king.GetComponent<KingSkill>();
                skill.SetGroup();
                _kingSkills.Add(skill);
                _kingCount++;
            }
            else
            {
                break;
            }
        }
    }
}