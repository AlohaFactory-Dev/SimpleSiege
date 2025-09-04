using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTextOff : MonoBehaviour
{
    void Start()
    {
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        gameObject.SetActive(false);
    }
}