using UnityEngine;

public class KingAddController : MonoBehaviour
{
    [SerializeField] private GameObject addUnitEffect;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<KingGroupController>(out var kingGroupController))
        {
            Instantiate(addUnitEffect, other.transform.position, Quaternion.identity);
            kingGroupController.GetComponent<KingGroupController>().AddKing();
            Destroy(gameObject);
        }


        if (other.CompareTag("Player"))
        {
        }
    }
}