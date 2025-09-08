using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTextOff : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.GetComponent<ITarget>();
        if (player != null && player.TeamType == TeamType.Player)
            gameObject.SetActive(false);
    }
}