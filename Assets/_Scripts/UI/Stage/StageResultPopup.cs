using Aloha.Coconut.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageResultPopup : UISlice
{
    [SerializeField] private GameObject winUI;
    [SerializeField] private GameObject loseUI;
    [SerializeField] private TextMeshProUGUI playTimeText;
    [SerializeField] private TextMeshProUGUI stageNameText;
    [SerializeField] private Button restartButton;
    private bool _isInitialized = false;

    public class Args : UIOpenArgs
    {
        public TeamType winner;
    }

    protected override void Open(UIOpenArgs args)
    {
        Init();
        var resultArgs = args as Args;
        if (resultArgs == null) return;

        float playTime = StageConainer.Get<StageManager>().EndTime;
        int minutes = (int)(playTime / 60);
        int seconds = (int)(playTime % 60);
        stageNameText.text = $"스테이지 : {StageConainer.Get<StageManager>().CurrentStageTable.stageNumber}";
        playTimeText.text = $"경과 시간 : {minutes:D2}분 {seconds:D2}초";
        // 결과에 따라 UI 업데이트 로직 구현
        if (resultArgs.winner == TeamType.Player)
        {
            // 승리 UI 설정
            winUI.SetActive(true);
            loseUI.SetActive(false);
        }
        else
        {
            // 패배 UI 설정
            winUI.SetActive(false);
            loseUI.SetActive(true);
        }
    }

    private void Init()
    {
        if (_isInitialized) return;
        _isInitialized = true;
        restartButton.onClick.AddListener(OnRestartButtonClicked);
    }

    private void OnRestartButtonClicked()
    {
        CloseView();
        GlobalConainer.Get<GameManager>().ReLoadLobby();
    }
}