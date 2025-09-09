using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingAddControllerRail : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private KingAddController kingAddControllerPrefab;
    [SerializeField] private float spawnInterval = 0.5f;
    [SerializeField] private float speed = 2f;

    private IEnumerator Start()
    {
        while (true)
        {
            SpawnKingAddController();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnKingAddController()
    {
        var kingAddController = Instantiate(kingAddControllerPrefab, spawnPoint.position, Quaternion.identity);
        kingAddController.Init(speed); // 속도 설정
    }
}