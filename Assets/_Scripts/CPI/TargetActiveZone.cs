using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetActiveZone : MonoBehaviour
{
    [SerializeField] private bool active = true;

    private void OnTriggerEnter2D(Collider2D other)
    {
        other.GetComponent<UnitController>().SetNotTargetable(!active);
    }
}