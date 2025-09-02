using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageUI : MonoBehaviour
{
    [SerializeField] private CardContainer cardContainer;

    public void Init()
    {
        cardContainer.Init();
    }
}