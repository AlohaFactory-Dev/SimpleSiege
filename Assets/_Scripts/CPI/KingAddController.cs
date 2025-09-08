using UnityEngine;

public class KingAddController : MonoBehaviour
{
    [SerializeField] private GameObject addUnitEffect;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Instantiate(addUnitEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}