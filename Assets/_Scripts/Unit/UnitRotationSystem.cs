using UnityEngine;

public class UnitRotationSystem : MonoBehaviour
{
    [SerializeField] private Transform obj;

    public void Rotate(Vector3 dir, ref float lastYRotation)
    {
        if (dir.x != 0)
        {
            float yRotation = dir.x > 0 ? 0f : 180f;
            if (!Mathf.Approximately(lastYRotation, yRotation))
            {
                obj.rotation = Quaternion.Euler(0f, yRotation, 0f);
                lastYRotation = yRotation;
            }
        }
    }
}