using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageUI : MonoBehaviour
{
    [SerializeField] private CardContainer cardContainer;
    [SerializeField] private GameObject pausePopup;
    public CardContainer CardContainer => cardContainer;

    public void Init()
    {
        cardContainer.Init();
        OffPausePopup();
    }

    public void OnPausePopup()
    {
        GameManager.Pause();
        pausePopup.SetActive(true);
    }

    public void OffPausePopup()
    {
        GameManager.Resume();
        pausePopup.SetActive(false);
    }

    public void GotoHome()
    {
        GlobalConainer.Get<GameManager>().ReLoadLobby();
    }
}